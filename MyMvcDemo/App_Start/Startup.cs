using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire.SqlServer;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Suijing.Utils.sysTools;

//[assembly: OwinStartup(typeof(MyMvcDemo.Startup))]
namespace MyMvcDemo
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var connectionstring = ConfigurationManager.ConnectionStrings["conn"];
            try
            {
                app.UseHangfire(config =>
                {
                    config.UseSqlServerStorage("Data Source=<" + connectionstring + ">; Initial Catalog=HangFire; Trusted_Connection=true;");
                    config.UseServer();
                });

                RecurringJob.AddOrUpdate(() => LogHepler.WriteLog("Hourly is call"), Cron.Minutely);
                LogHepler.WriteLog("Hangfire is start");
            }
            catch (Exception ex)
            {
                LogHepler.WriteLog("Hangfire error :" + ex.Message);
            }
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