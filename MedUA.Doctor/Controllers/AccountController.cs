using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MedUA.Models;
using MedUA.DAL;

namespace MedUA.Controllers
{
    using System;

    using MedUA.Resources;

    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
            : base()
        {
        }

        public AccountController(ApplicationUserManager userManager, SignInManager signInManager)
            : base(userManager, signInManager)
        {
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View();
        }

        //
        // GET: /PatientRegister/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /PatientRegister/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return await RedirectToRoleController((await UserManager.FindByEmailAsync(model.Email)).Id, returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resource.DoctorRegisterControllerLoginInvalidAttempt);
                    return View(model);
            }
        }

        //
        // GET: /PatientRegister/Register
        [AllowAnonymous]
        public ActionResult Register(string userId)
        {
            if (userId == null)
            {
                return this.View("Error");
            }
            return View(new RegisterViewModel() { Id = userId });
        }

        //
        // POST: /PatientRegister/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                Func<bool> emailInvalid = () => string.Compare(model.Email.Trim(), user.Email, StringComparison.OrdinalIgnoreCase) != 0;
                Func<bool> codeInvalid = () => string.Compare(model.Code.Trim(), user.Code, StringComparison.OrdinalIgnoreCase) != 0;
                if (emailInvalid.Invoke() || codeInvalid.Invoke())
                {
                    ModelState.AddModelError("", Resource.AccountRegisterCodeOrEmailIsNotValid);
                }
                else
                {
                    var result = await UserManager.AddPasswordAsync(user.Id, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return await RedirectToRoleController(user.Id);
                    }
                    ModelState.AddModelError("Password", Resource.PasswordErrorFormat);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterEnterMail(EnterMailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                var link = user == null ? null : await GetCallbackUrl(user);
                return this.View("ConfirmLink", new ConfirmLinkModel() { Link = link });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private async Task<string> GetCallbackUrl(ApplicationUser user)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            var htmlCallbackUrl = "<a href=\"" + callbackUrl + "\">" + Resource.ConfirmEmailMessageLinkText + "</a>";
            return string.Format(Resource.ConfirmEmailMessageStringFormat, htmlCallbackUrl);
        }

        [AllowAnonymous]
        public ActionResult RegisterEnterMail()
        {
            return View();
        }

        //
        // GET: /PatientRegister/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return RedirectToAction("Register", new { userId = userId });
            }
            return View("Error");
        }

        //
        // GET: /PatientRegister/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /PatientRegister/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "Account", new { userId = user.Id });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /PatientRegister/ForgotPasswordConfirmation
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPasswordConfirmation(string userId)
        {
            if (userId != null)
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(userId);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
                var htmlCallbackUrl = "<a href=\"" + callbackUrl + "\">" + Resource.ForgotPasswordConfirmationLink + "</a>";
                ViewBag.Message = string.Format(Resource.ForgotPasswordConfirmationMessageStringFormat, htmlCallbackUrl);
            }
            return View();
        }

        //
        // GET: /PatientRegister/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /PatientRegister/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            foreach (var resultError in result.Errors)
            {
                this.ModelState.AddModelError("", resultError.StartsWith("Passwords must have") ? Resource.PasswordErrorFormat : resultError);
            }
            return View();
        }

        //
        // GET: /PatientRegister/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /PatientRegister/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        private async Task<ActionResult> RedirectToRoleController(string userId, string returnUrl = null)
        {
            if (returnUrl != null)
            {
                return RedirectToLocal(returnUrl);
            }
            if (await UserManager.IsInRoleAsync(userId, Roles.Patient))
            {
                return RedirectToAction("Index", "Patient");
            }
            if (await UserManager.IsInRoleAsync(userId, Roles.Doctor))
            {
                return RedirectToAction("Index", "Doctor");
            }
            return this.RedirectToAction("Index", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login");
        }

    }
}