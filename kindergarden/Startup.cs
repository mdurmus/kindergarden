using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(kindergarden.Startup))]
namespace kindergarden
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}