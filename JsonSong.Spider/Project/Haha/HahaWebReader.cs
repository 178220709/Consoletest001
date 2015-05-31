using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.Spider.SpiderBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonSong.Spider.Core;
using Suijing.Utils;

namespace JsonSong.Spider.Project.Haha
{
    [TestClass]
    public class HahaWebReader : WebTaskReader
    {
        public HahaWebReader()
        {
            _htmlAsyncHelper = HtmlAsyncHelper.CreatWithProxy(-1);
        }

        private readonly HtmlAsyncHelper _htmlAsyncHelper;

        public override async Task<ReadResult> GetHtmlContent(string url)
        {
            var re = new ReadResult(url);
            var root = (await _htmlAsyncHelper.GetDocumentNode(url)).DocumentNode ;
            re.Title = root.QuerySelector("title").InnerText;
            re.Content = root.QuerySelector(".joke-main-content").OuterHtml;
            var divFooterA = root.QuerySelectorAll(".joke-main-misc .fl a").ToArray();
            var zan = ConvertHelper.ConvertStrToInt(divFooterA[0].InnerText);
            var bishi = ConvertHelper.ConvertStrToInt(divFooterA[1].InnerText);
            re.Weight = ((zan + bishi) / 100) * (zan - bishi * 3);
            return re;
        }

        public override async Task FireTaskCallBack(IList<ReadResult> res)
        {
            var manager = SpiderService.Instance;
            foreach (var re in res)
            {
              await  manager.AddNoRepeat(re, 1);
            }
        }



        [TestMethod]
        public static async Task<List<ReadResult>> GetRecommand()
        {
            var urls = new List<string>();
            const string url = "http://www.haha.mx/joke/1660764";

            var httpHelper = HtmlAsyncHelper.CreatWithProxy(0);

            var doc = await httpHelper.GetDocumentNode(url);
            doc.DocumentNode.QuerySelector(".recommand-joke-main-list-thumbnail")
                .QuerySelectorAll(".joke-text.word-wrap")
                .Select(a => a.QuerySelector("a"))
                .Select(a => "http://www.haha.mx" + a.GetAttributeValue("href", ""))
                .ToList().ForEach(urls.Add);

            doc.DocumentNode.QuerySelector(".recommand-joke-main-list-text")
               .QuerySelectorAll("a").Select(a => "http://www.haha.mx" + a.GetAttributeValue("href", ""))
               .ToList().ForEach(urls.Add);
            var reader = new HahaWebReader();
            var factory = new WebTaskFactory(reader);
            return  await factory.StartAndCallBack(urls.Distinct().ToList());
        }

        [TestMethod]
        public void GetHtmlContentTest()
        {
            // 测试文字笑话
            const string url = "http://www.haha.mx/joke/1660764";
            var re =  GetHtmlContent(url);   
        }
        [TestMethod]
        public void Test2()
        {
            // 测试图片笑话
            const string url = "http://www.haha.mx/joke/1661700";
            var re = GetHtmlContent(url);   
        }
    }
}



/*将Flag 转换为string类型
    db.sp_haha.find().forEach(function(x){
x.Flag=x.Flag+"";
db.HahaJoke.save(x)})
*/