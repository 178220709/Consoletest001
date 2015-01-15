using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace PriceIndex.BackService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            MainMethods.CreatTodayData();

            //var servicesToRun = new ServiceBase[] 
            //{ 
            //    new MainService() 
            //};
            //ServiceBase.Run(servicesToRun);
        }
    }
}
