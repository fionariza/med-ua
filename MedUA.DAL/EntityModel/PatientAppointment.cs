namespace MedUA.DAL.EntityModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PatientAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Appointment { get; set; }

        public Status Status { get; set; }

        public PatientUser PatientUser { get; set; }

        public HospitalResearch HospitalResearch { get; set; }
    }
}
