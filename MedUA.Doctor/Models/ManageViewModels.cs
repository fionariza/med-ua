using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace MedUA.Models
{
    using MedUA.Resources;

    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLongStringFormat", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLongStringFormat", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }
    
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}