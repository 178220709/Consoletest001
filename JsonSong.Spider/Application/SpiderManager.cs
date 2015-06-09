using System;
using System.Linq;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.Spider.Core;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.Spider.DataAccess.Entity;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;

namespace JsonSong.Spider.Application
{
    public class SpiderYouminImageApp
    {

        public static async Task StartSync()
        {
            var dir = ConfigHelper.GetConfigString(MyConstants.AppSettingKey.KEY_SpiderImgDir);
            var list = SpiderLiteDao.Instance.GetCon().Find(a => a.TypeId == 2).Skip(0).Take(500).ToList();
            var helper = HtmlAsyncHelper.CreatWithProxy(-1);

            foreach (var page in list)
            {
                var map = new SpiderImgMapEntity()
                {
                    Url = page.Url
                };

                var root = NormalHtmlHelper.GetDocumentNode(page.Content).DocumentNode;
                var imgUrls = root.QuerySelectorAll("img")
                    .Select(img => img.GetAttributeValue("src", ""))
                    .Where(a => !string.IsNullOrWhiteSpace(a))
                    .ToList(); //all img's src
                var tasks = imgUrls.Select(imgurl => helper.DownloadImageGetFileName(imgurl, dir)).ToList();


                var result = await Task.WhenAll(tasks);
                map.MapItems = result.Select((imgFileName, index) => new ImgMapItem
                {
                    ImgFileName = imgFileName,
                    ImgUrl = imgUrls[index]
                }).ToList();
                // save img new file name map to db
                SpiderImgMapDao.Instance.AddNoRepeat(map);
            }
        }




    }
}