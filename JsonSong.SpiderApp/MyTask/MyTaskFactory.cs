using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSong.Spider.Project.Haha;
using JsonSong.Spider.Project.Youmin;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.MyTask
{
    public static class MyTaskFactory
    {
        public static async void SpiderTask()
        {
            LogHelper.WriteWebReader("TaskFactory.SpiderTask is call");
            var list1 = await HahaWebReader.GetRecommand();
            var list2 = await YouminWebReader.GetRecommand();
        }
    }
}
