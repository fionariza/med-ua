namespace MedUA.DAL.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class HospitalResearch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Research Research { get; set; }

        public double Price { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public ICollection<PatientAppointment> Appointments { get; set; }
        public Hospital Hospital { get; set; }
       
    }
}
