namespace MedUA.DAL.Migrations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using MedUA.DAL;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    class PatientUserBuilder : UserBuilder<PatientUser>
    {

        private readonly ApplicationDbContext context;

        private ApplicationUserManager patientUserManager;

        public PatientUserBuilder(ApplicationDbContext context, ApplicationUserManager userManager)
        {
            this.context = context;
            this.patientUserManager = userManager;
        }

        public override PatientUser AddOrUpdate(PatientUser updatedUser)
        {
            var user = base.AddOrUpdate(updatedUser);
            user.Doctors = user.Doctors ?? new List<DoctorUser>();
            foreach (var doctor in user.Doctors)
            {
                if (user.Doctors.All(x => x.Email != doctor.Email))
                {
                    user.Doctors.Add(doctor);
                }
            }
            user.Entries = user.Entries ?? new List<Entry>();
            foreach (var entry in user.Entries)
            {
                if (user.Entries.All(x=>x.TimeStamp != entry.TimeStamp))
                {
                    user.Entries.Add(entry);
                }
            }

            this.context.SaveChanges();
            this.patientUserManager.AddToRole(user.Id, Roles.Patient);
            this.context.SaveChanges();
            return user;
        }

        public override PatientUser CreateUser(PatientUser user)
        {
            var identityMessage = this.patientUserManager.Create(user);
            return user;
        }

        public override PatientUser GetUser(string email)
        {
            return this.patientUserManager.FindByEmail(email) as PatientUser;
        }

        public override void UpdateProperty(PropertyInfo propertyInfo, PatientUser updatedUser, PatientUser returnUser)
        {
            switch (propertyInfo.Name)
            {
                case nameof(PatientUser.Doctors):
                case nameof(PatientUser.Entries):
                    break;
                default:
                    base.UpdateProperty(propertyInfo, updatedUser, returnUser);
                    break;
            }

        }
    }
}
