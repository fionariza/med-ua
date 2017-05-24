namespace MedUA.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using MedUA.DAL;
    using MedUA.Data;
    using MedUA.Models;

    [Authorize(Roles = Roles.Patient)]
    public class PatientController : BaseApplicationUserController
    {
        public PatientController()
        {
        }

        public PatientController(ApplicationUserManager userManager, SignInManager signInManager, IApplicationDataProvider dataProvider)
            : base(userManager, signInManager, dataProvider)
        {
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var patient = await UserManager.FindByIdAsync(User.Identity.GetUserId()) as PatientUser;
            return this.View(PatientProfileViewModel.Convert(patient));
        }

        [HttpGet]
        public ActionResult PatientHistory()
        {
            var id = User.Identity.GetUserId();
            return View(new EntryHistoryViewModelList() { List = this.DataProvider.GetListEntries(id) });
        }

        [HttpGet]
        public async Task<ActionResult> DoctorProfile(string doctorId)
        {
            var doctor = await DataProvider.FindDoctorByIdAsync(doctorId);
            return this.View(DoctorProfileViewModel.Convert(doctor));

        }
    }
}