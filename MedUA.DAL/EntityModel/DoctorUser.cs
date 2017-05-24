namespace MedUA.DAL
{
    using System.Collections.Generic;

    public class DoctorUser : ApplicationUser
    {
        public string Position
        {
            get;
            set;
        }
        
        public Hospital CurrentHospital { get; set; }
        public ICollection<PatientUser> PatientUsers { get; set; }
    }
}
