namespace MedUA.DAL.Migrations
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNet.Identity.EntityFramework;
    public abstract class UserBuilder<TUser>  where TUser : IdentityUser, new()
    {
        public virtual TUser AddOrUpdate(TUser updatedUser)
        {
            var user = GetUser(updatedUser.Email);
            if (user == null)
            {
                return this.CreateUser(updatedUser);
            }
            var properties = updatedUser.GetType().GetProperties().Where(x=>!x.PropertyType.Name.StartsWith(nameof(ICollection)) && x.Name != "Id");
            foreach (var propertyInfo in properties)
            {
                UpdateProperty(propertyInfo, updatedUser, user);
            }
            return user;
        }

        public virtual void UpdateProperty(PropertyInfo propertyInfo, TUser updatedUser, TUser returnUser)
        {
            var value = propertyInfo.GetValue(updatedUser, null);
            if (value != null)
            {
                returnUser.GetType().GetProperties().First(x => x.Name == propertyInfo.Name).SetValue(returnUser, value);
            }
        }
        public abstract TUser CreateUser(TUser user);

        public abstract TUser GetUser(string email);
    }

    public class UserComparer : IEqualityComparer<PropertyInfo>
    {
        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            return x.PropertyType.Name == y.PropertyType.Name;
        }

        public int GetHashCode(PropertyInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
