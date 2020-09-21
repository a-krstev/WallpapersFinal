using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wallpaper.Startup))]
namespace Wallpaper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
