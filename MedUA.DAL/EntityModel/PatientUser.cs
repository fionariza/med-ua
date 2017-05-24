namespace MedUA.DAL
{
    using System;
    using System.Collections.Generic;
        public class PatientUser : ApplicationUser
    {
        public double WeightAtBirth
        {
            get;
            set;
        }

        public BloodType BloodType
        {
            get;
            set;
        }
        public virtual ICollection<DoctorUser> Doctors { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }
    }
}