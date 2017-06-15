namespace MedUA.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using MedUA.DAL;
    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;
    using MedUA.Models;
    using MedUA.Providers;
    using MedUA.Resources;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationDataProvider : IApplicationDataProvider
    {
        private readonly ApplicationDbContext context;

        private ResearchContextWrapper researchContextWrapper;

        public ApplicationDataProvider(ApplicationDbContext context)
        {
            this.context = context;
            this.researchContextWrapper = new ResearchContextWrapper(context);
        }

        public static IApplicationDataProvider Create(IdentityFactoryOptions<IApplicationDataProvider> options, IOwinContext context)
        {
            return new ApplicationDataProvider(context.Get<ApplicationDbContext>());
        }

        public async Task<IList<PatientListViewModel>> GetPatientListViewModelAsync(string doctorId, PatientListFilteredModel filteredModel = null)
        {
            var comparator = new CompareFunctionCreator();
            if (filteredModel == null || AllPropsAreNulls(filteredModel))
            {
                return await GetAllPatients(doctorId);
            }
            var entryFunc = comparator.EntryFunc(filteredModel.Diagnosis, filteredModel.YearOfVisit, filteredModel.MonthOfVisit, filteredModel.DayOfVisit, doctorId);
            var patientFunc = comparator.PatientFunc(filteredModel.YearOfBirth, filteredModel.MonthOfBirth, filteredModel.DayOfBirth);
            if (entryFunc != null)
            {
                this.context.Patients.Include(x => x.Entries);
            }
            IEnumerable<PatientUser> patients = (await this.context.Doctors.Include(x => x.PatientUsers).FirstAsync(x => x.Id == doctorId)).PatientUsers.ToList();
            if (patientFunc != null)
            {
                patients = patients.Where(patientFunc);
            }
            if (entryFunc != null)
            {
                patients = patients.Where(p => p.Entries.Any(entryFunc));
            }

            return patients.Select(x => PatientListViewModel.Convert(x, doctorId)).ToList();
        }


        private async Task<IList<PatientListViewModel>> GetAllPatients(string doctorId)
        {
            var doctor = await this.context.Doctors.Include(x => x.PatientUsers).FirstAsync(x => x.Id == doctorId);
            return doctor.PatientUsers.Select(x => PatientListViewModel.Convert(x, doctorId)).ToList();
        }

        private bool AllPropsAreNulls(PatientListFilteredModel model)
        {
            return string.IsNullOrEmpty(model.Diagnosis?.Trim())
                && model.YearOfBirth == null
                && model.DayOfBirth == null
                && model.MonthOfBirth == null
                && model.YearOfVisit == null
                && model.MonthOfVisit == null
                && model.DayOfVisit == null;
        }
        private bool FilterEntryOptionsIsAllNull(string diagnosis, DateTime? visit)
        {
            return string.IsNullOrEmpty(diagnosis) || visit == null;
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
                .OrderByDescending(x => x.TimeStamp)
                .Skip(skip)
                .Take(take)
                .Select(EntryHistoryViewModel.Convert).ToList();

        }

        public async Task<IEnumerable<ResearchHistoryViewModel>> GetListResearches(string patientId, int take = PageConstants.PageCount, int skip = 0)
        {
            var appointments =
                await this.context.Appointments.Include(x => x.HospitalResearch.Research)
                    .Include(x => x.HospitalResearch.Hospital)
                    .Where(x => x.PatientUser.Id == patientId)
                    .OrderByDescending(x => x.Appointment)
                    .ToListAsync();

            var count = appointments.Count();
            if (skip >= count)
            {
                return new List<ResearchHistoryViewModel>();
            }
            if (take + skip > count)
            {
                take = count - skip;
            }

            return appointments
                .Skip(skip)
                .Take(take)
                .Select(a => ResearchHistoryViewModel.Covert(a, a.HospitalResearch));
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

        public Task<IEnumerable<SelectListItem>> GetResearches(ResearchSettlementScope scope, string doctorId)
        {
            return Task<IEnumerable<SelectListItem>>.Factory.StartNew(() =>
            {
                return this.researchContextWrapper.GetResearches(scope, doctorId);
            });
        }

        public Task<IEnumerable<ResearchesPartialViewModel>> GetResearches(ResearchSettlementScope scope, string doctorId, int researchId)
        {
            return Task<IEnumerable<ResearchesPartialViewModel>>.Factory.StartNew(
                () =>
                    {
                        return this.researchContextWrapper.GetResearches(
                            scope,
                            doctorId,
                            researchId);
                    });
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
        public IEnumerable<SelectListItem> GetRegions(string doctorId)
        {
            var doctorUser = this.context.Doctors.Include(d => d.CurrentHospital).Include(d => d.CurrentHospital.Region).Include(d => d.CurrentHospital.Region.Oblast).First(x => x.Id == doctorId);
            yield return new SelectListItem() { Value = ((int)ResearchSettlementScope.Settlement).ToString(), Text = doctorUser.CurrentHospital.SettlementName, Selected = true };
            yield return new SelectListItem() { Value = ((int)ResearchSettlementScope.Region).ToString(), Text = $"{Resource.Region} {doctorUser.CurrentHospital.Region.Name}" };
            yield return new SelectListItem() { Value = ((int)ResearchSettlementScope.Oblast).ToString(), Text = $"{Resource.Oblast} {doctorUser.CurrentHospital.Region.Oblast.Name}" };
            yield return new SelectListItem() { Value = ((int)ResearchSettlementScope.Country).ToString(), Text = Resource.CountryScope };
        }

        public async Task<ResearchHistoryViewModel> SaveResearchAppointment(int hospitalResearchId, string patientId, DateTime resultDateTime)
        {
            var research = await this.context.HospitalResearches.Include(x => x.Hospital).Include(x => x.Research).Include(x => x.Appointments).FirstAsync(x => x.Id == hospitalResearchId);
            var patient = await this.context.Patients.FirstAsync(x => x.Id == patientId);
            var appointment = new PatientAppointment() { Appointment = resultDateTime, PatientUser = patient };
            research.Appointments.Add(appointment);
            await this.context.SaveChangesAsync();
            return ResearchHistoryViewModel.Covert(appointment, research);
        }
        public async Task<ResearchHistoryViewModel> CancelAppointment(int appointmentId, string patientId)
        {
            var appointment = await this.context.Appointments.Include(x => x.HospitalResearch.Research).Include(x => x.HospitalResearch.Hospital).Include(x => x.PatientUser).Where(p => p.PatientUser.Id == patientId).FirstAsync(a => a.Id == appointmentId);
            appointment.Status = Status.Cancelled;
            await this.context.SaveChangesAsync();
            return ResearchHistoryViewModel.Covert(appointment, appointment.HospitalResearch);
        }

        public Task<IEnumerable<SelectListItem>> GetAvailiableTimes(string date, int researchId)
        {
            return Task<IEnumerable<SelectListItem>>.Factory.StartNew(
                () =>
                    {
                        return this.researchContextWrapper.GetAvailiableTimes(date, researchId);
                    });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
                this.researchContextWrapper?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }


    }
}