using System;
using Hangfire;
using Hangfire.MemoryStorage;
using JsonSong.SpiderApp.MyTask;
using LiteDbLog.Facade;
using LiteDbLog.LiteDBLog;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;


namespace JsonSong.Front
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();

            GlobalConfiguration.Configuration.UseMemoryStorage();
            //  GlobalConfiguration.Configuration.UseSqlServerStorage("conn");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            int min = ConfigHelper.GetConfigInt(MyConstants.AppSettingKey.KEY_CronMin);

            RecurringJob.AddOrUpdate(() => MyTaskFactory.StartTask(), Cron.Hourly(min));
            DBLogInstances.Spider.Log("UseMemoryStorage is start", level: DBLogLevelEnum.Debugger);
            DBLogInstances.Spider.Log("Environment.CurrentDirectory " + Environment.CurrentDirectory, level: DBLogLevelEnum.Debugger);

        }
    }
}