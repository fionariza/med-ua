namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using MedUA.Resources;

    public class ResearchesPartialViewModel
    {
        [Display(ResourceType = typeof(Resource), Name = "HospitalName")]
        public string HospitalName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HospitalAddress")]
        public string HospitalAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ResearchName")]
        public string ResearchName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Price")]
        public string Price { get; set; }
        public IEnumerable<SelectListItem> Dates { get; set; }
        public IEnumerable<SelectListItem> Times { get; set; }
        public int ResearchHospitalId { get; set; }
    }
}