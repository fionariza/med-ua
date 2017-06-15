namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ResearchPatientViewModel
    {
        public ResearchPickViewModel ResearchPickViewModel { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }
        public string RegionId { get; set; }
    }
}