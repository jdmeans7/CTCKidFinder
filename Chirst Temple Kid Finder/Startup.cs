using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Chirst_Temple_Kid_Finder.Startup))]
namespace Chirst_Temple_Kid_Finder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
