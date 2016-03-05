using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AdminTool.Startup))]
namespace AdminTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
