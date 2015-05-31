using System;
using System.IO;
using System.Text;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.SystemInfo;
using Newtonsoft.Json;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.sysTools;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.UtilsTest    
{

    [TestClass()]
    public class SysteminfoHelperTest
    {

        [TestMethod()]
        public void Test1()
        {
            var test1 = SysteminfoHelper.GetCpuID();
            var test2 = SysteminfoHelper.GetMemorySize();

        
           string str2 = SysteminfoHelper.GetSystemInfo();


        }


        [TestMethod()]
        public void Test2()
        {

            string path = @"  D:\code\GitCode\HelloCSharp\BaseFeatureTest\bin\Debug\Log\LogError\20150224.txt";
            var str = File.ReadAllText(path, Encoding.UTF8);
            var str2 = File.ReadAllText(path, Encoding.ASCII);


            var str3 = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
         
        }


        [TestMethod()]
        public void Test3()
        {

            var path = PathHelper.GetDBPath("123");

        }
    }
}
