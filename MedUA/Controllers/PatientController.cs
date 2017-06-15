namespace MedUA.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using MedUA.DAL;
    using MedUA.Data;
    using MedUA.Models;
    using MedUA.Resources;

    using Newtonsoft.Json;

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
        public async Task<ActionResult> PatientHistoryView()
        {
            var id = User.Identity.GetUserId();
            return View("PatientHistory",new EntryHistoryViewModelList() { EntryHistory = this.DataProvider.GetListEntries(id), ResearchHistory = await this.DataProvider.GetListResearches(id) });
        }

        [HttpPost]
        public PartialViewResult PatientHistory()
        {
            using (var req = Request.InputStream)
            {
                req.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(req))
                {
                    var json = streamReader.ReadToEnd();
                    var patientHistory = JsonConvert.DeserializeObject<HistoryGetViewModel>(json);
                    var skip = patientHistory.Skip * PageConstants.PageCount;
                    return this.PartialView(
                        "PatientHistoryPartial",
                        this.DataProvider.GetListEntries(
                            User.Identity.GetUserId(),
                            take: patientHistory.Page * PageConstants.PageCount - skip,
                            skip: skip));
                }
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> ResearchHistory(string patientId)
        {
            using (var req = Request.InputStream)
            {
                req.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(req))
                {
                    var json = streamReader.ReadToEnd();
                    var researchHistory = JsonConvert.DeserializeObject<HistoryGetViewModel>(json);
                    var skip = researchHistory.Skip * PageConstants.PageCount;
                    return this.PartialView(
                        "ResearchHistoryPartial",
                        await this.DataProvider.GetListResearches(
                            patientId,
                            take: researchHistory.Page * PageConstants.PageCount - skip,
                            skip: skip));
                }
            }
        }
        [HttpGet]
        public async Task<ActionResult> DoctorProfile(string doctorId)
        {
            var doctor = await DataProvider.FindDoctorByIdAsync(doctorId);
            return this.View(DoctorProfileViewModel.Convert(doctor));

        }

        [HttpPost]
        public async Task<PartialViewResult> CancelAppointment(int appointmentId)
        {
            ResearchHistoryViewModel result = await DataProvider.CancelAppointment(appointmentId, User.Identity.GetUserId());
            return this.PartialView("ResearchHistoryPartial", new[] { result });
        }
    }
}