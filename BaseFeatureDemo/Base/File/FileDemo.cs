using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.File
{
    [TestClass]
    public class FileDemo
    {
        [TestMethod]
        public static void Main1()
        {
            var runPath = Environment.CurrentDirectory;
            const string ProjectName = "HelloCSharp";
            var index = runPath.IndexOf(ProjectName, System.StringComparison.Ordinal);
            var path = runPath.Substring(0, index + ProjectName.Length)+"\\";
        }  
        
        [TestMethod]
        public  void test()
        {
            var runPath = Environment.CurrentDirectory;
            var path = Directory.GetDirectoryRoot(runPath);

            var domain = AppDomain.CurrentDomain;
            var runtime = domain.BaseDirectory;
          
        }

    }



}
