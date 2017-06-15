namespace MedUA.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    using Microsoft.AspNet.Identity;

    using MedUA.DAL;
    using MedUA.Data;
    using MedUA.Helpers;
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

        [HttpPost]
        public async Task<PartialViewResult> ListPatients(PatientListFilteredModel filteredModel)
        {
            var model = await DataProvider.GetPatientListViewModelAsync(User.Identity.GetUserId(), filteredModel);
            return this.PartialView("ListPatientsPartial", model);
        }


        [HttpGet]
        public async Task<ActionResult> Details(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return this.View("Error");
            }
            var doctorId = this.User.Identity.GetUserId();
            var patientUser = await this.UserManager.FindByIdAsync(patientId);
            var listEntries = this.DataProvider.GetListEntries(patientId, doctorId);
            var researchList = await DataProvider.GetResearches(ResearchSettlementScope.Settlement, this.User.Identity.GetUserId());
            var researchHistory = await DataProvider.GetListResearches(patientId);
            var regions = DataProvider.GetRegions(doctorId);
            return this.View("PatientHistory", new EntryHistoryViewModelList() { EntryHistory = listEntries, PatientId = patientId, PatientSurnameName = $"{patientUser.Surname} {patientUser.Name}", ResearchList = researchList, Regions = regions, ResearchHistory = researchHistory });
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
                    var patientHistory = JsonConvert.DeserializeObject<HistoryGetViewModel>(json);
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
                    return this.PartialView("PatientHistoryPartial", new[] { EntryHistoryViewModel.Convert(entry) });
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

        [HttpPost]
        public async Task<PartialViewResult> LoadResearchPickPartial(string patientId, int scope)
        {
            var scopeModel = (ResearchSettlementScope)scope;
            var researches = new ResearchPickViewModel() { PatientId = patientId, ResearchesList = await DataProvider.GetResearches(scopeModel, this.User.Identity.GetUserId()) };
            return this.PartialView("ResearchPickPartial", researches);
        }

        [HttpPost]
        public async Task<PartialViewResult> LoadResearches(string patientId, int scope, int research)
        {
            var scopeModel = (ResearchSettlementScope)scope;
            var researches = await DataProvider.GetResearches(scopeModel, this.User.Identity.GetUserId(), research);
            return this.PartialView("ResearchesPartial", new ResearchesPartialViewModelList(researches) { PatientId = patientId });
        }

        [HttpPost]
        public async Task<PartialViewResult> SaveResearchAppointment(NewAppointmentPartial newAppointmentPartial)
        {
            var resultDate = DateTime.Parse(newAppointmentPartial.Date);
            var resultTime = DateTime.Parse(newAppointmentPartial.Time);
            var resultDateTime = new DateTime(resultDate.Year, resultDate.Month, resultDate.Day, resultTime.Hour, resultTime.Minute, resultTime.Second);
            var result = await this.DataProvider.SaveResearchAppointment(newAppointmentPartial.HospitalResearchId, newAppointmentPartial.PatientId, resultDateTime);
            return this.PartialView("ResearchHistoryPartial", new[] { result });
        }

        public async Task<ActionResult> GetTimes(string date, int id)
        {
            var times = await DataProvider.GetAvailiableTimes(date, id);
            return Json(times, JsonRequestBehavior.AllowGet);
        }


    }
}