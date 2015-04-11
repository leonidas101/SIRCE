using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(STPS.SIRCE.Web.Startup))]
namespace STPS.SIRCE.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
