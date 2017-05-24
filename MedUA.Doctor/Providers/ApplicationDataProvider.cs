namespace MedUA.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using MedUA.DAL;
    using MedUA.Helpers;
    using MedUA.Models;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationDataProvider : IApplicationDataProvider
    {
        private const int CodeLength = 18;
        private readonly ApplicationDbContext context;

        public ApplicationDataProvider(ApplicationDbContext context)
        {
            this.context = context;
        }

        public static IApplicationDataProvider Create(IdentityFactoryOptions<IApplicationDataProvider> options, IOwinContext context)
        {
            return new ApplicationDataProvider(context.Get<ApplicationDbContext>());
        }

        public Task<IList<PatientListViewModel>> GetPatientListViewModelAsync(string doctorId)
        {
            return Task<IList<PatientListViewModel>>.Factory.StartNew(
                () =>
                    {
                        var doctor = this.context.Doctors.Include(x => x.PatientUsers).First(x => x.Id == doctorId);
                        return doctor.PatientUsers.Select(x => PatientListViewModel.Convert(x, doctor.Id)).ToList();
                    });

        }

        public IList<EntryHistoryViewModel> GetListEntries(string patientId)
        {
            var patient = this.context.Patients.Include(x => x.Entries).First(x => x.Id == patientId);
            return patient.Entries.Select(EntryHistoryViewModel.Convert).ToList();

        }

        public IList<EntryHistoryViewModel> GetListEntries(string patientId, string doctorId)
        {
            var patient = this.context.Patients.Include(x => x.Entries).First(x => x.Id == patientId);
            return patient.Entries.Where(x => x.Doctor.Id == doctorId).Select(EntryHistoryViewModel.Convert).ToList();
        }

        public bool SaveEntry(EntryHistoryViewModel model)
        {
            try
            {
                var patientUser = this.context.Patients.Find(model.PatientId);
                var doctorUser = this.context.Doctors.Find(model.DoctorId);
                if (patientUser == null || doctorUser == null)
                    return false;
                var researches = model.ResearchIds != null && model.ResearchIds.Any()
                                     ? model.ResearchIds.Select(researchId => context.Researches.ToList().First(x => x.Id == int.Parse(researchId))).ToList()
                                     : null;
                this.context.Entries.Add(EntryHistoryViewModel.ConvertBack(model, researches, doctorUser, patientUser));
                this.context.SaveChanges();
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }

        public IResearchProvider GetResearchProvider()
        {
            return new ResearchProvider(this.context.Researches);
        }

        public Task<DoctorUser> FindDoctorByIdAsync(string id)
        {
            return this.context.Doctors.Include(x => x.CurrentHospital).Include(x => x.PatientUsers).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<PatientSearchResultViewModel>> GetPatientsSearch(ApplicationUser doctorId, PatientSearchViewModel patientSearch)
        {
            return Task<List<PatientSearchResultViewModel>>.Factory.StartNew(
                (() =>
                    {
                        var searchString = patientSearch.SearchString?.Trim();
                        if (string.IsNullOrEmpty(searchString))
                        {
                            return null;
                        }

                        var intRegex = new Regex("^[0-9]+$");
                        if (intRegex.IsMatch(searchString) && patientSearch.SearchString.Length == CodeLength)
                        {
                            return
                                this.context.Patients.Where(p => p.Code == searchString)
                                    .ToList()
                                    .Select(
                                        p =>PatientSearchResultViewModel.Convert(
                                                p,
                                                GetStatus(doctorId, p),
                                                searchString))
                                    .ToList();
                        }
                        var func = new SurnameNamePatronimicRetriever().RetrieveFunc(searchString);
                        if (func == null)
                        {
                            return null;
                        }
                        return
                            this.context.Patients.Include(p => p.Doctors)
                                .ToList()
                                .Where(func.Invoke)
                                .Select(
                                    p =>
                                        PatientSearchResultViewModel.Convert(
                                            p,
                                            GetStatus(doctorId, p)))
                                .ToList();
                    }));
        }

        private string GetStatus(ApplicationUser doctor, PatientUser patientUser)
        {
            if (patientUser.Doctors.Any(x=>x.Id == doctor.Id))
            {
                return Resources.Resource.SearchResultsPartialAdded;
            }
            return doctor.Code == patientUser.Code ? Resources.Resource.SearchResultsPartialItsYou : null;
        }

        public async Task<bool> AddPatient(string doctorId, string patientId)
        {
            try
            {
                var doctorUser = await this.FindDoctorByIdAsync(doctorId);
                if (doctorUser == null)
                {
                    return false;
                }

                var patientUser = await this.context.Patients.FirstOrDefaultAsync(x => x.Id == patientId);
                if (patientUser == null)
                {
                    return false;
                }

                doctorUser.PatientUsers.Add(patientUser);

                this.context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }



    }
}