using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using MyProject.TestHelper;
using NPOI.SS.Formula.Functions;

namespace  MyProject.MyHtmlAgility.ProjectTest
{
    /// <summary>
    /// 将lteAdmin原html文件 去掉头尾 只取内容 转换成cshtml
    /// </summary>
    [TestClass]
    public class GetContentFromLteDemo
    {
        private string oldPath = @"C:\workspace\HelloCSharp\MaxthonExtension\TestDemo\AdminLTE\pages";
        


        [TestMethod]
        public void Main1()
        {
            var files = Directory.GetFiles(oldPath, "*.html", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var oldContext = File.ReadAllText(file);
                var htmlDoc = new HtmlAgilityPack.HtmlDocument
                {
                    OptionAddDebuggingAttributes = false,
                    OptionAutoCloseOnEnd = true,
                    OptionFixNestedTags = true,
                    OptionReadEncoding = true
                };
                htmlDoc.LoadHtml(oldContext);
                var aside = htmlDoc.DocumentNode.QuerySelector("body aside[class=right-side]") ??
                            htmlDoc.DocumentNode.QuerySelector("body");

                var newContext  =  aside.InnerHtml;
                var newPath = file.Replace("\\pages", "\\views").Replace(".html",".cshtml");
                var newDir  = Path.GetDirectoryName(newPath);
                  if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }

                using (var ws = File.CreateText(newPath))
                {
                    ws.Write(newContext);
                    ws.Flush();
                }
            }


        }
      
    }
}
