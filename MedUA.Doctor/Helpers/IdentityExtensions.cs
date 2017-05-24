namespace MedUA.Helpers
{
    using System;
    using System.Security.Principal;

    using MedUA.DAL;

    using Microsoft.AspNet.Identity;

    public static class IdentityExtensions
    {
        public static string GetNameAndSurname(this IIdentity identity, ApplicationUserManager userManager)
        {
            if (identity.IsAuthenticated)
            {
                var user = userManager.FindById(identity.GetUserId());
                return string.Format("{0} {1}", user.Surname, user.Name);
            }
            return null;
        }
    }
}