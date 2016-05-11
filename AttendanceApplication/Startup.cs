using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttendanceApplication.Startup))]
namespace AttendanceApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
