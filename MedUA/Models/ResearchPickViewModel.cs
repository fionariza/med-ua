namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ResearchPickViewModel
    {
        public IEnumerable<SelectListItem> ResearchesList { get; set; }
        public string ResearchId { get; set; }

        public string PatientId { get; set; }
    }
}