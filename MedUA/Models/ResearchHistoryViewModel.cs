namespace MedUA.Models
{
    using System.ComponentModel.DataAnnotations;

    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;
    using MedUA.Resources;

    public class ResearchHistoryViewModel
    {
        public int PatientAppointmentId { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Research")]
        public string ResearchName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HospitalName")]
        public string HospitalName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "HospitalAddress")]
        public string HospitalAddress { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Status")]
        public string StatusString { get; set; }

        public Status Status { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Date")]
        public string Date { get; set; }

        public static ResearchHistoryViewModel Covert(PatientAppointment appointment, HospitalResearch research)
        {
            return new ResearchHistoryViewModel()
                   {
                       ResearchName = research.Research.Name,
                       HospitalName = research.Hospital.Name,
                       HospitalAddress = research.Hospital.Address,
                       PatientAppointmentId = appointment.Id,
                       StatusString = StatusGenerator.GetStatus(appointment.Status),
                       Date = appointment.Appointment.ToShowDateTime(),
                       Status = appointment.Status
                   };
        }
    }
}