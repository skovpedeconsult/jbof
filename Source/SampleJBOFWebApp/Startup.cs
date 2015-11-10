using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleJBOFWebApp.Startup))]
namespace SampleJBOFWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
