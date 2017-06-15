namespace MedUA.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using MedUA.DAL.EntityModel;

    public class Hospital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string SettlementName { get; set; }
        public Region Region { get; set; }
        public virtual ICollection<DoctorUser> Doctors { get; set; }
        public virtual ICollection<HospitalResearch> Researches { get; set; }

    }
}
