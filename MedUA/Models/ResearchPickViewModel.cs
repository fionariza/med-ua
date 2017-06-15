namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using MedUA.Resources;

    public class ResearchPickViewModel
    {
        public IEnumerable<SelectListItem> ResearchesList { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Research")]
        public string ResearchId { get; set; }

        public string PatientId { get; set; }
    }
}