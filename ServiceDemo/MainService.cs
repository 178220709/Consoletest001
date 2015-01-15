using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using PriceIndex.BackService;

namespace PriceIndex.BackService
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string timerInterval = Globals.timeForMinute.ToString();
                this.TimeForminute.Interval = double.Parse(timerInterval) * 1000 * 60;
            }
            catch
            {
                this.TimeForminute.Interval = 30 * 60 * 1000;//默认间隔30　分钟
            }
            this.TimeForminute.Start();
        }

        protected override void OnStop()
        {
            this.TimeForminute.Stop();
        }

        private void TimeForminute_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MainMethods.CreatTodayData();
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("MyLogger");
                log.Error(ex.Message, ex);
                throw;           
            }
        }
    }
}
