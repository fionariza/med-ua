namespace MedUA.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Entry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Complains { get; set; }
        public string Examination { get; set; }
        public string Difficulties { get; set; }
        public string AccordingDiagnosis { get; set; }
        public string Diagnosis { get; set; }
        public string Recomendations { get; set; }
        public string QuestionDiagnosis { get; set; }
        public virtual PatientUser Patient { get; set; }
        public virtual DoctorUser Doctor { get; set; }
    }
}
