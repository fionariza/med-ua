namespace MedUA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using MedUA.DAL;
    using MedUA.DAL.EntityModel;
    using MedUA.Resources;

    public class DoctorProfileViewModel
    {
        public string ImageSrc { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Name"))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Surname"))]
        public string Surname { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Patronimic"))]
        public string Patronimic { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("Email"))]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("DoctorProfileViewModelPosition"))]
        public string Position { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("DoctorProfileViewModelHospital"))]
        public string Hospital { get; set; }

        [Display(ResourceType = typeof(Resource), Name = ("DateOfBirth"))]
        public string DateOfBirth { get; set; }

        public static DoctorProfileViewModel Convert(DoctorUser user)
        {
            return new DoctorProfileViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Patronimic = user.Partonimic,
                DateOfBirth = user.DateOfBirth.ToString("d", CultureInfo.CurrentCulture),
                Hospital = $"{user.CurrentHospital.Name}. {user.CurrentHospital.Address}",
                Position = user.Position,
                ImageSrc = user.MaleFemale == MaleFemale.Female ? "/Images/doctor-woman.jpg" : "/Images/doctor-man.jpg",
                Email = user.Email
            };
        }
    }
}