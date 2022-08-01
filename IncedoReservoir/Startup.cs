using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncedoReservoir.Startup))]
namespace IncedoReservoir
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
