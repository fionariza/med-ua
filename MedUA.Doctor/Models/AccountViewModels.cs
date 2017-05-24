using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedUA.Models
{
    using MedUA.Resources;

    using Newtonsoft.Json;
    
    public class ConfirmLinkModel
    {
        public string Link { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof(Resource), Name = "MedicalCode")]
        public string Code { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLongStringFormat", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resource), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resource), Name = "ConfirmPassword")]
        [Compare("Password",  ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLongStringFormat", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resource), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordErrorFormat")]
        [Display(ResourceType = typeof(Resource), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "RequiredErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailAddressInvalidFormat")]
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        public string Email { get; set; }
    }
}
