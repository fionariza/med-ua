namespace MedUA.DAL.Migrations
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class DoctorUserBuilder : UserBuilder<DoctorUser>
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserManager applicationUserManager;

        public DoctorUserBuilder(ApplicationDbContext context, ApplicationUserManager userManager)
        {
            this.context = context;
            this.applicationUserManager = userManager;
        }

        public override DoctorUser AddOrUpdate(DoctorUser updatedUser)
        {
            var user =  base.AddOrUpdate(updatedUser);
            this.context.SaveChanges();
            this.applicationUserManager.AddToRole(user.Id, Roles.Doctor);
            this.context.SaveChanges();
            return user;
        }

        public override void UpdateProperty(PropertyInfo propertyInfo, DoctorUser updatedUser, DoctorUser returnUser)
        {
            if (propertyInfo.Name != nameof(updatedUser.PatientUsers))
            {
                base.UpdateProperty(propertyInfo, updatedUser, returnUser);
            }
        }

        public override DoctorUser CreateUser(DoctorUser user)
        {
            this.applicationUserManager.Create(user);
            return user;
        }

        public override DoctorUser GetUser(string email)
        {
           return this.applicationUserManager.FindByEmail(email) as DoctorUser;
        }
    }
}
