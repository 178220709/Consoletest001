
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using JsonSong.SpiderApp.MyTask;
using Suijing.Utils.sysTools;

namespace JsonSong.ManagerUI
{
    public class PreWarmCache : System.Web.Hosting.IProcessHostPreloadClient
    {

        public void Preload(string[] parameters)
        {
            // Perform initialization and cache loading logic here...\
            LogHelper.Info("机器被唤醒");
            RecurringJob.AddOrUpdate(() => MyTaskFactory.StartTask(), Cron.Hourly);
        }

    }
}