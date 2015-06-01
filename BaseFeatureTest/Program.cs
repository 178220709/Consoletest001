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

using MyProject.HotelRecord.Manager;
using JsonSong.Spider.Project.Haha;
using Omu.ValueInjecter;
using Suijing.Utils;
using Suijing.Utils.sysTools;

namespace BaseFeatureTest
{
    [TestClass]
    public class Program
    {
        public static void Main(string[] args)
        {

            LogHelper.SetConfig();
           HahaWebReader.GetRecommand();
        }
    }
 
   
}
