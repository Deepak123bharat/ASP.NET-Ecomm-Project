using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SabjiMandi.Web.Startup))]
namespace SabjiMandi.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
