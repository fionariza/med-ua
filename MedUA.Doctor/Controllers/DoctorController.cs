namespace MedUA.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebSockets;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using MedUA.DAL;
    using MedUA.Data;

    using Microsoft.Ajax.Utilities;

    using Models;

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
        public PartialViewResult PatientHistoryCurrentDoctor(string patientId)
        {
            return this.PartialView("PatientHistoryPartial", this.DataProvider.GetListEntries(patientId, User.Identity.GetUserId()));
        }

        [HttpPost]
        public PartialViewResult PatientHistory(string patientId)
        {
            return this.PartialView("PatientHistoryPartial", this.DataProvider.GetListEntries(patientId));
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
                if (DataProvider.SaveEntry(model))
                {
                    return this.PartialView("NewEntryPartial", null);
                }
            }
            var partialView = this.PartialView("NewEntryPartial", model);
            partialView.ViewData = this.ViewData;
            partialView.ViewBag.ResearchesList = DataProvider.GetResearchProvider().GetResearches(model.ResearchIds);
            return partialView;
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