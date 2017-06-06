namespace MedUA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using MedUA.DAL;
    using MedUA.Resources;

    public class PatientSearchResultViewModel
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Surname")]
        public string Surname { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Patronimic")]
        public string Patronimic { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "PlaceOfBirth")]
        public string PlaceOfBirth { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "DateOfBirth")]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "MedicalCode")]
        public string Code { get; set; }
        
        public string Status { get; set; }

        public static PatientSearchResultViewModel Convert(PatientUser arg, string status = null, string medicalCode = null)
        {
            return new PatientSearchResultViewModel()
                   {
                       Id = arg.Id,
                       Surname = arg.Surname,
                       Patronimic = arg.Partonimic,
                       Name = arg.Name,
                       Code = medicalCode,
                       DateOfBirth = arg.DateOfBirth.ToString("d", CultureInfo.CurrentCulture),
                       Status = status,
                       PlaceOfBirth = arg.PlaceOfBirth
                   };
        }
    }
}