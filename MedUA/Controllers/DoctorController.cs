namespace MedUA.Controllers
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using MedUA.DAL;
    using MedUA.Data;
    using MedUA.Resources;

    using Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [Authorize(Roles = Roles.Doctor)]
    public class DoctorController : BaseApplicationUserController
    {

        public DoctorController()
        {
        }

        public DoctorController(ApplicationUserManager userManager, SignInManager signInManager, IApplicationDataProvider dataProvider)
            : base(userManager, signInManager, dataProvider)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var id = User.Identity.GetUserId();
            var doctor = await DataProvider.FindDoctorByIdAsync(id);
            return View(DoctorProfileViewModel.Convert(doctor));
        }

        [HttpGet]
        public async Task<ActionResult> ListPatients()
        {
            var model = await DataProvider.GetPatientListViewModelAsync(User.Identity.GetUserId());
            return this.View(model);
        }


        [HttpGet]
        public ActionResult Details(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return this.View("Error");
            }
            var listEntries = this.DataProvider.GetListEntries(patientId, this.User.Identity.GetUserId());
            var researchList = DataProvider.GetResearchProvider().GetResearches();
            return this.View("PatientHistory", new EntryHistoryViewModelList() { List = listEntries, PatientId = patientId, ResearchList = researchList });
        }

        [HttpPost]
        public PartialViewResult PatientHistory(string patientId)
        {
            using (var req = Request.InputStream)
            {
                req.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(req))
                {
                    var json = streamReader.ReadToEnd();
                    var patientHistory = JsonConvert.DeserializeObject<PatientHistoryGetViewModel>(json);
                    var skip = patientHistory.Skip * PageConstants.PageCount;
                    return this.PartialView(
                        "PatientHistoryPartial",
                        this.DataProvider.GetListEntries(
                            patientId,
                            doctorId: patientHistory.FilterDoctor ? User.Identity.GetUserId() : null,
                            take: patientHistory.Page * PageConstants.PageCount - skip,
                            skip: skip));
                }
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(PatientSearchViewModel model)
        {
            var search = await DataProvider.GetPatientsSearch(await UserManager.FindByIdAsync(User.Identity.GetUserId()), model);
            return this.PartialView("SearchResultsPartial", search);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewEntry(EntryHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DoctorId = User.Identity.GetUserId();
                var entry = DataProvider.SaveEntry(model);
                if (entry != null)
                {
                    return this.PartialView("PatientHistoryPartial", new [] { EntryHistoryViewModel.Convert(entry) });
                }
            }

            return null;
        }

        [HttpGet]
        public async Task<ActionResult> DoctorProfile(string doctorid)
        {
            var doctor = await DataProvider.FindDoctorByIdAsync(doctorid);
            return this.View("DoctorProfile", DoctorProfileViewModel.Convert(doctor));
        }

        [HttpPost]
        public async Task<ActionResult> AddPatient(AddPatientViewModel patient)
        {
            var isUserAdded = await DataProvider.AddPatient(User.Identity.GetUserId(), patient.Id);
            if (!isUserAdded)
            {
                return this.View("Error");
            }
            return this.RedirectToAction("ListPatients");
        }
    }
}