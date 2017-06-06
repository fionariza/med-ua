namespace MedUA.Models
{
    using MedUA.DAL;

    public class DoctorSignViewModel
    {
        public string DoctorSign { get; set; }
        public string DoctorId { get; set; }

        public static DoctorSignViewModel Convert(DoctorUser doctor)
        {
            return new DoctorSignViewModel()
            {
                DoctorSign = $"{doctor.Surname} {doctor.Name} ({doctor.Position})",
                DoctorId = doctor.Id
            };
        }
    }
}