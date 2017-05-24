using System.Threading.Tasks;

namespace MedUA.DAL
{
    using System.Security.Claims;

    using Microsoft.AspNet.Identity;

    public static class ApplicationUserExtension
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this ApplicationUser applicationUser, ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
