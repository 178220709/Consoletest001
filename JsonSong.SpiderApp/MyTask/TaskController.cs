using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.Spider.DataAccess.Entity;
using JsonSong.Spider.Project.Haha;
using JsonSong.SpiderApp.Application;
using JsonSong.SpiderApp.Data;
using Omu.ValueInjecter;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.MyTask
{
    public class TaskController
    {
        /// <summary>
        /// 无尽模式 请注意此段代码在线程中执行
        /// </summary>
        public async Task StartHahaTask()
        {
            var start = 1654376;
            var end = 1750057;
            var list = new List<SpiderEntity>();
            var reader = new HahaWebReader();
            for (int i = start; i < end; i++)
            {
               // var url = "http://www.haha.mx/joke/" + i;
                var url = "http://localhost:4001" + i;
                Task.Run(async () =>
                {
                    LogHelper.WriteWebReader(string.Format("1====> {0} start   ", url));
                    var result = await reader.GetHtmlContent(url);
                  
                   
                    LogHelper.WriteWebReader(string.Format("2====> {0} done   ", url));
                    if (result.Weight < 100) return;
                    SpiderLiteDao.Instance.AddNoRepeat(result, 1);
                    LogHelper.WriteWebReader(string.Format("3====> {0} is insert ", url));
                });
            }
        }

        public async Task StartHahaTask2( )
        {
            var start = 1654376;
            var end = 1750057;
            var list = new List<SpiderEntity>();
            var reader = new HahaWebReader();
            for (int i = start; i < end; i++)
            {
                // var url = "http://www.haha.mx/joke/" + i;
                var url = "http://localhost:4001" + i;
                Task.Run(async () =>
                {
                    LogHelper.WriteWebReader(string.Format("1====> {0} start   ", url));
                    var result = await reader.GetHtmlContent(url);


                    LogHelper.WriteWebReader(string.Format("2====> {0} done   ", url));
                    if (result.Weight < 100) return;
                    SpiderLiteDao.Instance.AddNoRepeat(result, 1);
                    LogHelper.WriteWebReader(string.Format("3====> {0} is insert ", url));
                });
            }
        }

    }
}