using System;
using System.Configuration;
using Hangfire;
using JsonSong.ManagerUI.Extend;
using JsonSong.SpiderApp.MyTask;
using Owin;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;

//[assembly: OwinStartup(typeof(JsonSong.ManagerUI.Startup))]
namespace JsonSong.ManagerUI
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (ConfigHelper.GetConfigBool(MyConstants.AppSettingKey.KEY_StartSpider))
            {
                GlobalConfiguration.Configuration.UseSqlServerStorage("conn");
                app.UseHangfireDashboard();
                app.UseHangfireServer();
                //BackgroundJob.Requeue()
                int min = ConfigHelper.GetConfigInt(MyConstants.AppSettingKey.KEY_CronMin);

                RecurringJob.AddOrUpdate(() => MyTaskFactory.StartTask(), Cron.Hourly(min));
                LogHelper.WriteWebReader("UseSqlServerStorage is start");
                LogHelper.WriteWebReader("Environment.CurrentDirectory " + Environment.CurrentDirectory);
            }
        }







    }

}