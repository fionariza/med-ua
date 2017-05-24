using Microsoft.AspNet.Identity.EntityFramework;

namespace MedUA.DAL
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using MedUA.DAL.EntityModel;

    using Microsoft.AspNet.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string Name
        {
            get;
            set;
        }

        public string Surname
        {
            get;
            set;
        }

        public string Partonimic
        {
            get;
            set;
        }

        public string PlaceOfBirth
        {
            get;
            set;
        }

        public DateTime DateOfBirth
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public MaleFemale MaleFemale
        {
            get;
            set;
        }
    }
}
