using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSong.Spider.Project.Haha;
using JsonSong.Spider.Project.Youmin;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.MyTask
{
    public static class MyTaskFactory
    {
        public static async void StartTask()
        {
            LogHelper.WriteWebReader("TaskFactory.SpiderTask is call");
            try
            {
                var list1 = await HahaWebReader.GetRecommand();
             //   var list2 = await YouminWebReader.GetRecommand();
                await SpiderTagPage();
            }
            catch (Exception ex)
            {
                LogHelper.Error("TaskFactory.SpiderTask", ex);
            }
        }

        public static async Task SpiderTagPage()
        {
            LogHelper.Info("TaskFactory.SpiderTagPage is start");
            var list = JSConfigHelper.GetJsConfigAs<IList<string>>(MyConstants.JSSettingKey.KEY_YouminTagUrls);
            if (!list.HasValue())
            {
                return;
            }

            LogHelper.Info(string.Format("SpiderTagPage start,count:{0},urls:{1}", list.Count, string.Join("~~", list.Take(3))));
            await YouminWebReader.SpiderAll(list);
        }
    }
}
