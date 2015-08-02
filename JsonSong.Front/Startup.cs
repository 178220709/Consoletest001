using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JsonSong.Front.Startup))]
namespace JsonSong.Front
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
