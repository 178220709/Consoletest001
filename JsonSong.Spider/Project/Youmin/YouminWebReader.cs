using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.Spider.SpiderBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonSong.Spider.Core;


namespace JsonSong.Spider.Project.Youmin
{
    [TestClass]
    public class YouminWebReader :WebTaskReader
    {
        public override ReadResult GetHtmlContent(string url )
        {
            var parstialReader = new YouminPartialReader(url);
            parstialReader.StartReadAll();

            return parstialReader.OutputResult();
        }

        public override async Task FireTaskCallBack(IList<ReadResult> res)
        {
            var manager = SpiderService.Instance;
            foreach (var re in res)
            {
              await  manager.AddNoRepeat(re, 2);
            }
        }

        internal static string GetFlagFromUrl(string url)
        {
            var flag = "";
            var tags = url.Split('/');
            var tag2 = tags.Skip(tags.Length - 2).Take(2).ToArray();
            return tag2[0] + tag2[1].TrimEnd(".shtml".ToArray());
        }

        [TestMethod]
        public static async Task<List<ReadResult>> GetRecommand()
        {
            var urls = new List<string>();
            const string url = "http://www.gamersky.com/ent/";


            var doc = NormalHtmlHelper.GetDocumentNode(url);

              doc.DocumentNode.QuerySelectorAll(".Lpic").ToList()
                  .ForEach(ul => ul.QuerySelectorAll("li .t2 a")
                  .AsParallel()
                  .ForAll(a => urls.Add(a.GetAttributeValue("href", ""))));


            var urlsValid = urls.Where(a => !SpiderService.Instance.ExistUrl(a)).ToList();
            //如果系统中已经有了 则不会去爬取
          
            var reader = new YouminWebReader();
            var factory = new WebTaskFactory(reader);
       //  return  null;
            return await factory.StartAndCallBack(urlsValid.Distinct().ToList());

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
        public async Task Test3()
        {
            var list = await GetRecommand();
        }
    }
}
