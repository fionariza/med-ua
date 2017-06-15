namespace MedUA.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using MedUA.Data;
    using MedUA.DAL;
    using MedUA.Helpers;
    using MedUA.Models;

    public class ResearchContextWrapper : IDisposable
    {
        private IResearchProvider researchProvider;

        private ApplicationDbContext context;

        public ResearchContextWrapper(ApplicationDbContext context)
        {
            this.context = context;
            var hospitals = this.context.HospitalResearches.Include(h=>h.Research).Include(x=>x.Appointments).Include(h => h.Hospital).Include(h => h.Hospital.Region).Include(h => h.Hospital.Region.Oblast);
            this.researchProvider = new ResearchProvider(hospitals);
        }
        public IEnumerable<SelectListItem> GetResearches(ResearchSettlementScope scope, string doctorId)
        {
            var doctorUser = this.context.Doctors.Include(d => d.CurrentHospital).Include(d => d.CurrentHospital.Region).Include(d => d.CurrentHospital.Region.Oblast).First(x => x.Id == doctorId);
            return researchProvider.GetResearches(scope, doctorUser.CurrentHospital);
        }

        public IEnumerable<ResearchesPartialViewModel> GetResearches(ResearchSettlementScope scope, string doctorId, int researchId)
        {
            var doctorUser = this.context.Doctors.Include(d => d.CurrentHospital).Include(d => d.CurrentHospital.Region).Include(d => d.CurrentHospital.Region.Oblast).First(x => x.Id == doctorId);
            return researchProvider.GetResearches(scope, doctorUser.CurrentHospital, researchId);
        }

        public IEnumerable<SelectListItem> GetAvailiableTimes(string date, int researchId)
        {
            var research = this.context.HospitalResearches.Include(x => x.Appointments).FirstOrDefault(x => x.Id == researchId);
            return this.researchProvider.GetAvailiableTimes(date, research);
        }

        public void Dispose()
        {
            this.context?.Dispose();
        }
    }
}