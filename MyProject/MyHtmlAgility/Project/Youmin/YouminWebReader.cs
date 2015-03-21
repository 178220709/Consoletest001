using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fizzler.Systems.HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.MyHtmlAgility.Core;
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
                    en.Flag = ConvertHelper.ConvertStrToInt(re.Url.Substring(re.Url.LastIndexOf('/')+1));
                    en.InjectFrom(re);
                    manager.AddEdit(en);
                }
            }
            catch (Exception)
            {
                return;
            }
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
         return  null;
         return  factory.StartAndCallBack(urls.Distinct().ToList());

        }

        [TestMethod]
        public void Test1()
        {
            // 测试
            string str = "http://www.gamersky.com/ent/201503/529106.shtml";
            GetHtmlContent(str);
        }
        [TestMethod]
        public void Test2()
        {
          
        }
    }
}
