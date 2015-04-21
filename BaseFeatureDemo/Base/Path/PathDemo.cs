using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.sysTools;

namespace BaseFeatureDemo.Base.Path
{
    [TestClass]
    public class PathDemo
    {
        public static void Main1()
        {
            //得到当前项目的路径 Ps：不是应用程序的路径
             string str1 = System.IO.Directory.GetCurrentDirectory ( );
             string str2 = Assembly.GetExecutingAssembly().Location;

             LogHepler.WriteLog(str1);
             LogHepler.WriteLog(str2);
        }

        [TestMethod]
        public void Main1Test()
        {        
            //得到当前项目的路径 Ps：不是应用程序的路径
            PathDemo.Main1();
        }
    }
}
