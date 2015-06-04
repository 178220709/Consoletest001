using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using JsonSong.SpiderApp.MyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;

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

    }


}
