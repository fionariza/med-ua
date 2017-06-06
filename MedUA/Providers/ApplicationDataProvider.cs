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
    using MedUA.Resources;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationDataProvider : IApplicationDataProvider
    {
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

        public IList<EntryHistoryViewModel> GetListEntries(string patientId, string doctorId = null, int take = PageConstants.PageCount, int skip = 0)
        {
            var patient = this.context.Patients.Include(x => x.Entries).First(x => x.Id == patientId);
            var count = patient.Entries.Count;
            if (skip >= count)
            {
                return new List<EntryHistoryViewModel>();
            }
            if (take + skip > count)
            {
                take = count - skip;
            }
            var entries = string.IsNullOrEmpty(doctorId) ? patient.Entries : patient.Entries.Where(e => e.Doctor.Id == doctorId);
            return entries
                .OrderByDescending(x=>x.TimeStamp)
                .Skip(skip)
                .Take(take)
                .Select(EntryHistoryViewModel.Convert).ToList();

        }

        public Entry SaveEntry(EntryHistoryViewModel model)
        {
            try
            {
                var patientUser = this.context.Patients.Find(model.PatientId);
                var doctorUser = this.context.Doctors.Find(model.DoctorId);
                if (patientUser == null || doctorUser == null)
                    return null;
                //var researches = model.ResearchIds != null && model.ResearchIds.Any()
                //                     ? model.ResearchIds.Select(researchId => context.Researches.ToList().First(x => x.Id == int.Parse(researchId))).ToList()
                //                     : null;
                var entry = EntryHistoryViewModel.ConvertBack(model, doctorUser, patientUser);
                this.context.Entries.Add(entry);
                this.context.SaveChanges();
                return entry;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
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
            return Task<List<PatientSearchResultViewModel>>.Factory.StartNew(() =>
            {
                var splilter = new SearchStringSpliter(new SurnameNamePatronimicRetriever());
                Func<PatientUser, bool> searchPattern = null;
                string medicalCode = null;
                if (!splilter.Split(patientSearch.SearchString, out searchPattern, out medicalCode))
                {
                    return null;
                }
                return this.context.Patients.Include(d => d.Doctors)
                        .Where(searchPattern.Invoke)
                        .Select(p => PatientSearchResultViewModel.Convert(p, GetStatus(doctorId, p), medicalCode))
                        .ToList();
            });
        }

        private string GetStatus(ApplicationUser doctor, PatientUser patientUser)
        {
            if (patientUser.Doctors.Any(x => x.Id == doctor.Id))
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