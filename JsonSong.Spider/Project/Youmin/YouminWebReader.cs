using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.Spider.SpiderBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonSong.Spider.Core;


namespace JsonSong.Spider.Project.Youmin
{
    [TestClass]
    public class YouminWebReader : WebTaskReader
    {
        public YouminWebReader()
        {
            _htmlAsyncHelper = HtmlAsyncHelper.CreatWithProxy(-1);
        }

        private readonly HtmlAsyncHelper _htmlAsyncHelper;

        public override async Task<ReadResult> GetHtmlContent(string url)
        {
            var parstialReader = new YouminPartialReader(url, _htmlAsyncHelper);
            await parstialReader.StartReadAll();
            return parstialReader.OutputResult();
        }

        public override void FireTaskCallBack(IList<ReadResult> res)
        {
            res.ToList().ForEach(a => SpiderLiteDao.Instance.AddNoRepeat(a, 2));
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

            var httpHelper = HtmlAsyncHelper.CreatWithProxy(-1);
            var doc = await httpHelper.GetDocumentNode(url);
            doc.DocumentNode.QuerySelectorAll(".Lpic").ToList()
                .ForEach(ul => ul.QuerySelectorAll("li .t2 a")
                .AsParallel()
                .ForAll(a => urls.Add(a.GetAttributeValue("href", ""))));

            var urlsValid = urls.Where(a => !SpiderLiteDao.Instance.ExistUrl(a)).ToList();
            //如果系统中已经有了 则不会去爬取

            var reader = new YouminWebReader();
            var factory = new WebTaskFactory(reader);
            //  return  null;
            return await factory.StartAndCallBack(urlsValid.Distinct().ToList());

        }

        /// <summary>
        /// 从url栏目页面中抓取全部内容
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static async Task SpiderAll(IList<string> pages)
        {
            var httpHelper = HtmlAsyncHelper.CreatWithProxy(0);
            foreach (var listPageUrl in pages)
            {
                var urls = new List<string>();
                var doc = await httpHelper.GetDocumentNode(listPageUrl);
                var sourceUrls = doc.DocumentNode.QuerySelectorAll(".news_list a")
                    .Select(a => a.GetAttributeValue("href", ""));
                var urlsValid = sourceUrls.Where(a => !SpiderLiteDao.Instance.ExistUrl(a)).ToList();
                var factory = new WebTaskFactory(new YouminWebReader());
                await factory.StartAndCallBack(urlsValid.Distinct().ToList());
            }
        }


        [TestMethod]
        public async Task Test1()
        {
            // 测试
            string str = "http://www.gamersky.com/ent/201503/529106.shtml";
            var re = await GetHtmlContent(str);
        }
        [TestMethod]
        public async Task Test2()
        {
            IList<string> list = new[] { "http://www.gamersky.com/ent/cos/" };
            await  SpiderAll(list);
        }

        [TestMethod]
        public async Task Test3()
        {
            var list = await GetRecommand();
        }
    }
}
