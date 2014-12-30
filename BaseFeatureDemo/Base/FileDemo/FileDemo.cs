using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.FileDemo
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

        private const string CheckStr = "XXX";
        private const string newPath = "XXX";
        [TestMethod]
        public  void Main()
        {
            var path = "";//路径
            var dirInfo = new DirectoryInfo(path);
            var list =  dirInfo.GetFiles("*.txt", SearchOption.AllDirectories);
            
            foreach (var file in list)
            {
                if (CheckTxt(file))
                {
                    //检查newPath里面是否已有同名文件 需要循环判断
                   // 如果有 则在后面加上（i）
                    var newFileName = file.Name;
                    int i = 0;
                    while (File.Exists(Path.Combine(newPath, newFileName)))
                    {
                        newFileName = Path.GetFileNameWithoutExtension(file.FullName) + string.Format("({0})", i) +
                                      Path.GetExtension(file.FullName);
                    }
                    file.MoveTo(Path.Combine(newPath,newFileName));
                }
            }
        }

        private static bool CheckTxt(FileSystemInfo file)
        {
            return File.ReadAllText(file.FullName).Contains(CheckStr);
        }
    }



}
