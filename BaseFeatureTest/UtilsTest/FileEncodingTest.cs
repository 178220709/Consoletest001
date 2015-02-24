using System;
using System.IO;
using System.Text;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.UtilsTest    
{

    [TestClass()]
    public class FileEncodingTest
    {

        [TestMethod()]
        public void Test1()
        {
            string path = @"D:\code\GitCode\HelloCSharp\MyMvcDemo\Log\LogDebugger\20141120.txt";
            var str = File.ReadAllText(path, Encoding.UTF8);
            var str2 = File.ReadAllText(path, Encoding.ASCII);
            var str3 = File.ReadAllText(path, Encoding.GetEncoding("gbk"));

        }


        [TestMethod()]
        public void Test2()
        {
           
        }
    }
}
