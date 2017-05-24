namespace MedUA.DAL
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MedUAConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            var context =  new ApplicationDbContext();
            return context;
        }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<DoctorUser> Doctors { get; set; }
        public DbSet<PatientUser> Patients { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public DbSet<Research> Researches { get; set; }
    }
}
