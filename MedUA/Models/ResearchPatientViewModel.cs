namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using MedUA.Resources;

    public class ResearchPatientViewModel
    {
        public ResearchPickViewModel ResearchPickViewModel { get; set; }
        public IEnumerable<SelectListItem> Regions { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "SettlementScope")]
        public string RegionId { get; set; }
        
    }
}