namespace MedUA.Models
{
    using MedUA.DAL.EntityModel;
    using MedUA.Helpers;

    public class ResearchHistoryViewModel
    {
        public int PatientAppointmentId { get; set; }
        public string ResearchName { get; set; }

        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string StatusString { get; set; }
        public Status Status { get; set; }
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