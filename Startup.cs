using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DreamTeamBlogZ.Startup))]
namespace DreamTeamBlogZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
