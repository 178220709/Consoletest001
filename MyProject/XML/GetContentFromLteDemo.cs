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
using MongoDB.Driver.Linq;
using MyProject.TestHelper;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace  MyProject.MyHtmlAgility.ProjectTest
{
    /// <summary>
    /// 将lteAdmin原html文件 去掉头尾 只取内容 转换成cshtml
    /// </summary>
    [TestClass]
    public class GetContentFromLteDemo
    {
       // private string oldPath = @"C:\workspace\HelloCSharp\MaxthonExtension\TestDemo\AdminLTE\pages";
        private string oldPath = @"D:\code\GitCode\HelloCSharp\MaxthonExtension\TestDemo\AdminLTE\pages";
        


        [TestMethod]
        public void ExtractContent()
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
                aside.QuerySelectorAll("script").ToList().ForEach(a=>a.Remove()); 
                var scripts = htmlDoc.DocumentNode.QuerySelectorAll("script")
                    .Where(a =>  a.GetAttributeValue("src", "") == "" || a.GetAttributeValue("src", "").Contains("js/plugins")).ToList();
                scripts.Where(a =>  a.GetAttributeValue("src", "").Contains("js/plugins")).ToList().ForEach(a =>
                {
                    //"../../js/plugins/xxx" -> ~/Content/libs/AdminLTE/js/plugins/xxx
                    var src = a.GetAttributeValue("src", "");
                    var newsrc = @"~/Content/libs/AdminLTE/" + src.Substring(src.IndexOf("js/plugins"));
                    a.SetAttributeValue("src", newsrc);
                });
                var newContext = aside.InnerHtml + string.Join(" ", scripts.Select(a=>a.OuterHtml));
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

         /// <summary>
         /// 将demo的文件结构转换成json格式,前台根据这个来显示
         /// </summary>
        [TestMethod]
        public void ExtractIndex()
        {

            var files = Directory.GetFiles(oldPath, "*.html", SearchOption.AllDirectories);

            var links = files.Select(a => a.Replace(oldPath, "").Replace(".html", "").TrimStart(new[]{'\\'})).ToList();

          //  var paths = links.Select(a => a.Substring(0, Math.Max(a.IndexOf('\\'), 0))).Distinct();

            var groups = links.GroupBy(a => a.Substring(0, Math.Max(a.IndexOf('\\'), 0)))
                .OrderBy(a=>a.Key)
                .Select(a=>new{path=a.Key,urls=a.GetEnumerator()});
            var str = JsonConvert.SerializeObject(groups);



        }

    }
}
