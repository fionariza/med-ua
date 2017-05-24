using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedUA.Startup))]
namespace MedUA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
