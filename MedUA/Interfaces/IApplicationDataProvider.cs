using System.Collections.Generic;
using MedUA.Models;

namespace MedUA.Data
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using MedUA.DAL;
    using MedUA.Helpers;
    using MedUA.Resources;

    public interface IApplicationDataProvider : IDisposable
    {
        IList<EntryHistoryViewModel> GetListEntries(string patientId, string doctorId = null, int take = PageConstants.PageCount, int skip = 0);
        Task<IList<PatientListViewModel>> GetPatientListViewModelAsync(string doctorId, PatientListFilteredModel filteredModel = null);
        Entry SaveEntry(EntryHistoryViewModel model);
        Task<IEnumerable<SelectListItem>> GetResearches(ResearchSettlementScope scope, string doctorId);
        Task<DoctorUser> FindDoctorByIdAsync(string id);

        Task<bool> AddPatient(string doctorId, string patientId);

        Task<List<PatientSearchResultViewModel>> GetPatientsSearch(ApplicationUser doctorUser, PatientSearchViewModel patientSearch);

        IEnumerable<SelectListItem> GetRegions(string doctorId);

        Task<IEnumerable<ResearchesPartialViewModel>> GetResearches(ResearchSettlementScope scope, string doctorId, int researchId);

        Task<IEnumerable<SelectListItem>> GetAvailiableTimes(string date, int researchId);

        Task<ResearchHistoryViewModel> SaveResearchAppointment(int hospitalResearchId, string patientId, DateTime resultDateTime);
        
        Task<IEnumerable<ResearchHistoryViewModel>> GetListResearches(string patientId, int take = PageConstants.PageCount, int skip = 0);
        Task<ResearchHistoryViewModel> CancelAppointment(int appointmentId, string patientId);
    }
}