namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ResearchesPartialViewModel
    {
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string ResearchName { get; set; }
        public string Price { get; set; }
        public IEnumerable<SelectListItem> Dates { get; set; }
        public IEnumerable<SelectListItem> Times { get; set; }
        public int ResearchHospitalId { get; set; }
    }
}