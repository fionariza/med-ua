namespace MedUA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using MedUA.DAL;
    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;
    using MedUA.Resources;



    public class PatientProfileViewModel
    {
        [Display(ResourceType = typeof(Resource), Name = ("PatientProfileViewModelDoctorList"))]
        public List<DoctorSignViewModel> DoctorSignList { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("BloodType"))]
        public string BloodType { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Name"))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Surname"))]
        public string Surname { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Patronimic"))]
        public string Patronimic { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("MedicalCode"))]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("DateOfBirth"))]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("TimeOfBirth"))]
        public string TimeOfBirth { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("PlaceOfBirth"))]
        public string PlaceOfBirth { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Email"))]
        public string Email { get; set; }

        public string ImageSrc { get; set; }

        public static PatientProfileViewModel Convert(PatientUser user)
        {
            return new PatientProfileViewModel()
                   {
                       DoctorSignList = user.Doctors.Select(DoctorSignViewModel.Convert).ToList(),
                       BloodType = BloodTypeSelector.GetName(user.BloodType),
                       Name = user.Name,
                       Surname = user.Surname,
                       Patronimic = user.Partonimic,
                       Code = user.Code,
                       DateOfBirth = user.DateOfBirth.ToString("d"),
                       TimeOfBirth = user.DateOfBirth.ToString("t"),
                       PlaceOfBirth = user.PlaceOfBirth,
                       Email = user.Email,
                       ImageSrc = user.MaleFemale == MaleFemale.Female ? "/Images/patient-woman.jpg" : "/Images/patient-man.jpg",
            };
        }
        
    }
}