using System;
using System.IO;
using System.Text;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Suijing.Utils.sysTools;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.UtilsTest    
{

    [TestClass()]
    public class LogTest
    {

        [TestMethod()]
        public void Test1()
        {
            LogHepler.SetConfig();
            try
            {
                int b = 0;
                int a = 10 / b;
            }
            catch (Exception ex)
            {
                LogHepler.WriteLog("hahaha",ex);
            }
           
        }


        [TestMethod()]
        public void Test2()
        {

            string path = @"  D:\code\GitCode\HelloCSharp\BaseFeatureTest\bin\Debug\Log\LogError\20150224.txt";
            var str = File.ReadAllText(path, Encoding.UTF8);
            var str2 = File.ReadAllText(path, Encoding.ASCII);


            var str3 = File.ReadAllText(path, Encoding.GetEncoding("gbk"));
         
        }
    }
}
