namespace MedUA.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;

    using MedUA.DAL;
    using MedUA.Resources;

    public class PatientListViewModel
    {
        public string Id
        {
            get; set;

        }

        [Display(ResourceType = typeof(Resource), Name = ("Name"))]
        public string Name
        {
            get;
            set;
        }
        
        [Display(ResourceType = typeof(Resource), Name = ("Surname"))]
        public string Surname
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = ("Patronimic"))]
        public string Partonimic
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = ("PlaceOfBirth"))]
        public string PlaceOfBirth
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = "DateOfBirth")]
        public string DateOfBirth
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = ("MedicalCode"))]
        public string Code
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = ("Email"))]
        public string Email
        {
            get;
            set;
        }

        [Display(ResourceType = typeof(Resource), Name = ("PatientListViewModelLastDateVisit"))]
        public string LastDateVisit
        {
            get;
            set;
        }

        public static PatientListViewModel Convert(PatientUser arg, string doctorId)
        {
            return new PatientListViewModel()
            {
                Id = arg.Id,
                Code = arg.Code,
                DateOfBirth = arg.DateOfBirth.ToString("d", CultureInfo.CurrentCulture),
                Email = arg.Email,
                Name = arg.Name,
                Partonimic = arg.Partonimic,
                PlaceOfBirth = arg.PlaceOfBirth,
                Surname = arg.Surname,
                LastDateVisit = arg.Entries.LastOrDefault(x => x.Doctor.Id == doctorId)?.TimeStamp.ToString("g") ?? string.Empty
            };
        }
    }
}