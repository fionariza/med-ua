namespace MedUA.Models
{
    using System.ComponentModel.DataAnnotations;

    using MedUA.Resources;

    public class PatientSearchViewModel
    {
        [Display(ResourceType = typeof(Resource),Name = "SearchViewExplanation")]
        public string SearchString { get; set; }
    }
}