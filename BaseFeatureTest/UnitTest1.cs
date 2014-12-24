using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using BaseFeatureDemo.MyGame;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvcDemo.Extend;
using Omu.ValueInjecter;
using Suijing.Utils;

namespace BaseFeatureTest
{
    [TestClass]
    public class MyTest1
    {
        public class TargeClass
        {
            public int? Id { get; set; }
        }

        [TestMethod]
        public  void Test1()
        {
            //Thread.GetDomain().SetData(".appPath", "c:\\inetpub\\wwwroot\\webapp\\");
            //Thread.GetDomain().SetData(".appVPath", "/");
            //TextWriter tw = new StringWriter();
            //HttpWorkerRequest wr = new SimpleWorkerRequest
            //("/Home/app", "", tw);
            //HttpContext.Current = new HttpContext(wr);

            var str = ConfigHelper.GetJsConfigAs<string>("cookie51job");
        }

        public static void Main(string[] args)
        {
           var ll= log4net.Config.XmlConfigurator.Configure();

            var loginfo = log4net.LogManager.GetLogger("loginfo2");
            var b1 = LogManager.Exists("loginfo2");
            var logerror = log4net.LogManager.GetLogger("logerror");
            if (logerror.IsErrorEnabled)
            {
                logerror.Error("this is my");
            }

        }
    }
 
   
}
