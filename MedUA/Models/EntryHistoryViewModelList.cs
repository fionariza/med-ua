namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class EntryHistoryViewModelList
    {
        public IEnumerable<EntryHistoryViewModel> List { get; set; }
        
        public IEnumerable<SelectListItem> ResearchList { get; set; }

        public string PatientId { get; set; }

        public int Page { get; set; }
    }
}