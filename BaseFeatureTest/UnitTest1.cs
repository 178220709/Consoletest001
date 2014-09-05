using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using BaseFeatureDemo.MyGame;
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

            var str = MyConstants.Globe2;

        }
    }
    public static class MyConstants
    {
        public static string Globe2 = "icon-globe";
        public static class Bootstrap
        {

            public static class Icon
            {
                public const string Globe = "icon-globe";
                public const string User = "icon-globe";
                public const string Card = "icon-credit-card";


            }
        }
    }
   
}
