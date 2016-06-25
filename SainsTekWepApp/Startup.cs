using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SainsTekWepApp.Startup))]
namespace SainsTekWepApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
