using System;
using System.Configuration;
using Owin;
using Suijing.Utils.sysTools;

//[assembly: OwinStartup(typeof(JsonSong.ManagerUI.Startup))]
namespace JsonSong.ManagerUI
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["conn"];
          
        }
    }
    //public class HangfireConfig
    //{
      
    //    public static void Configuration(IAppBuilder app)
    //    {
    //        app.UseHangfire(config =>
    //        {
    //            config.UseSqlServerStorage("Data Source=<connectionstring>; Initial Catalog=HangFire; Trusted_Connection=true;");
    //            config.UseServer();
    //        });
    //    }

    //}
}