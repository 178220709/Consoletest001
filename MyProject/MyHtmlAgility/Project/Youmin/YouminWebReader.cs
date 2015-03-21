using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fizzler.Systems.HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.MyHtmlAgility.Core;
using NPOI.SS.Formula.Functions;
using Omu.ValueInjecter;
using Suijing.Utils;

namespace MyProject.MyHtmlAgility.Project.Youmin
{
    [TestClass]
    public class YouminWebReader :WebTaskReader
    {
        public override ReadResult GetHtmlContent(string url )
        {
            var parstialReader = new YouminParstialReader(url);
            parstialReader.StartReadAll();

            return parstialReader.OutputResult();
        }
        public override void FireTaskCallBack(IList<ReadResult> res)
        {
            try
            {
                var manager = YouminService.Instance;
                foreach (var re in res)
                {
                    if (manager.Entities.Any(a => a.Url == re.Url))
                    {
                        continue;
                    }
                    var en = new BaseSpiderEntity();
                    en.Flag = GetFlagFromUrl(re.Url);
                    en.InjectFrom(re);
                  var writeConcernResult =  manager.AddEdit(en);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private string GetFlagFromUrl(string url)
        {
            var flag = "";
            var tags = url.Split('/');
            var tag2 = tags.Skip(tags.Length - 2).Take(2).ToArray();
            return tag2[0] + tag2[1].TrimEnd(".shtml".ToArray());
        }

        [TestMethod]
        public static List<ReadResult> GetRecommand()
        {
            var urls = new List<string>();
            const string url = "http://www.gamersky.com/ent/";


            var doc = NormalHtmlHelper.GetDocumentNode(url);

              doc.DocumentNode.QuerySelectorAll(".Lpic").ToList()
                  .ForEach(ul => ul.QuerySelectorAll("li .t2 a")
                  .AsParallel()
                  .ForAll(a => urls.Add(a.GetAttributeValue("href", ""))));
                  
            var reader = new YouminWebReader();
            var factory = new WebTaskFactory(reader);
       //  return  null;
         return  factory.StartAndCallBack(urls.Distinct().Take(1).ToList());

        }

        [TestMethod]
        public void Test1()
        {
            // 测试
            string str = "http://www.gamersky.com/ent/201503/529106.shtml";
          var re =  GetHtmlContent(str);
        }
        [TestMethod]
        public void Test2()
        {
            string str = "http://www.gamersky.com/ent/201503/529106.shtml";
            var flag = GetFlagFromUrl(str);
        } 
        
        [TestMethod]
        public void Test3()
        {
            var list = GetRecommand();
        }
    }
}
