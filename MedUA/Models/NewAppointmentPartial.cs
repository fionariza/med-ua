namespace MedUA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class NewAppointmentPartial
    {
        public string PatientId { get; set; }
        public int HospitalResearchId { get; set; }
        public string Date { get; set; }
        public IEnumerable<SelectListItem> Dates { get; set; }
        public string Time { get; set; }
        public IEnumerable<SelectListItem> Times { get; set; }
    }
}