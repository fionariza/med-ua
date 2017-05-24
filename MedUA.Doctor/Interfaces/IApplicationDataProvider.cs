using System.Collections.Generic;
using MedUA.Models;

namespace MedUA.Data
{
    using System;
    using System.Threading.Tasks;

    using MedUA.DAL;

    public interface IApplicationDataProvider : IDisposable
    {
        IList<EntryHistoryViewModel> GetListEntries(string patientId);
        IList<EntryHistoryViewModel> GetListEntries(string patientId, string doctorId);
        Task<IList<PatientListViewModel>> GetPatientListViewModelAsync(string doctorId);
        bool SaveEntry(EntryHistoryViewModel model);
        IResearchProvider GetResearchProvider();
        Task<DoctorUser> FindDoctorByIdAsync(string id);

        Task<bool> AddPatient(string doctorId, string patientId);

        Task<List<PatientSearchResultViewModel>> GetPatientsSearch(ApplicationUser doctorUser, PatientSearchViewModel patientSearch);

    }
}