using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RADAR.Startup))]
namespace RADAR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
