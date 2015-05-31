using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsonSong.Spider.Project.Haha;
using JsonSong.SpiderApp.Application;
using JsonSong.SpiderApp.Data;
using Omu.ValueInjecter;

namespace JsonSong.SpiderApp.MyTask
{
    public  class  TaskController
    {
        /// <summary>
        /// 无尽模式 请注意此段代码在线程中执行
        /// </summary>
        public async Task StartHahaTask()
        {
            var start = 1650764;
            var end = 1750057;
            var list = new List<SpiderEntity>();
            var reader = new HahaWebReader();
            for (int i = start; i < end; i++)
            {
                var url = "http://www.haha.mx/joke/" + i;
                var result = await reader.GetHtmlContent(url);
                if (result.Weight < 100) continue;
                var en = new SpiderEntity()
                {
                    AddedTime = DateTime.Now,
                    TypeId = 1
                };
                en.InjectFrom();
                SpiderManager.Add(en);
            }
        }

    }
}