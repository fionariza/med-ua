using System.Collections.Generic;
using MedUA.Models;

namespace MedUA.Data
{
    using System;
    using System.Threading.Tasks;

    using MedUA.DAL;
    using MedUA.Resources;

    public interface IApplicationDataProvider : IDisposable
    {
        IList<EntryHistoryViewModel> GetListEntries(string patientId, string doctorId = null, int take = PageConstants.PageCount, int skip = 0);
        Task<IList<PatientListViewModel>> GetPatientListViewModelAsync(string doctorId);
        Entry SaveEntry(EntryHistoryViewModel model);
        IResearchProvider GetResearchProvider();
        Task<DoctorUser> FindDoctorByIdAsync(string id);

        Task<bool> AddPatient(string doctorId, string patientId);

        Task<List<PatientSearchResultViewModel>> GetPatientsSearch(ApplicationUser doctorUser, PatientSearchViewModel patientSearch);

    }
}