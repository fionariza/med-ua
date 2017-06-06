namespace MedUA.Models
{
    using System.ComponentModel.DataAnnotations;

    using MedUA.Resources;

    public class EnterMailViewModel
    {

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        public string Email { get; set; }
    }
}