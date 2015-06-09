using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonSong.Spider.Core;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using JsonSong.SpiderApp.MyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.Test
{
    [TestClass]
    public class TaskTest
    {

        private Dictionary<string, string> paras = new Dictionary<string, string>()
        {
            {"pageIndex", "1"},
            {"pageSize", "10"},
            {"cnName", "spider"},
        };


        [TestMethod]
        public async Task AddTest1()
        {
            var list = JSConfigHelper.GetJsConfigAs<IList<string>>(MyConstants.JSSettingKey.KEY_YouminTagUrls);
            await MyTaskFactory.SpiderTagPage();
        }


        [TestMethod]
        public async Task DownloadImgTest1()
        {
            LogHelper.SetConfig();
            await JsonSong.Spider.Application.SpiderYouminImageApp.StartSync();
        }
        [TestMethod]
        public void DownloadImgTest2()
        {
            var list = SpiderImgMapDao.Instance.GetCon().FindAll().ToList();
            var total = list.Sum(a => a.MapItems.Count());
            Thread.Sleep(1000 * 3600 * 2);
        }

        [TestMethod]
        public async Task DownloadImgTest3()
        {
            const string url = "http://ww4.sinaimg.cn/mw690/6adc108fjw1esnoq7ht8bg208c04lh19.gif";
            var name = await HtmlAsyncHelper.CreatWithProxy(1).DownloadImageGetFileName(url);

            Thread.Sleep(1000 * 3600 * 2);
        }

    }


}
