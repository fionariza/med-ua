namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class EntryHistoryViewModelList
    {
        public IEnumerable<EntryHistoryViewModel> EntryHistory { get; set; }
        
        public IEnumerable<SelectListItem> ResearchList { get; set; }

        public string PatientId { get; set; }
        
        public IEnumerable<SelectListItem> Regions { get; set; }
        public IEnumerable<ResearchHistoryViewModel> ResearchHistory { get; set; }
    }
}