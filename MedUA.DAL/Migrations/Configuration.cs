namespace MedUA.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    using MedUA.DAL.UserBuilder;

    internal sealed class Configuration : DbMigrationsConfiguration<MedUA.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MedUA.DAL.ApplicationDbContext";
        }
        
        protected override void Seed(ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            new UserGenerator(context).Generate();
        }
        
    }
}
