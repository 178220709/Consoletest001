using System;
using System.Collections.Generic;
using JsonSong.SpiderApp.Application;
using JsonSong.SpiderApp.Data;
using MyProject.MyHtmlAgility.Project.Haha;
using Omu.ValueInjecter;

namespace JsonSong.SpiderApp.Task
{
    public  class  TaskController
    {

        /// <summary>
        /// 无尽模式 请注意此段代码在线程中执行
        /// </summary>
        public void StartHahaTask()
        {
            var start = 1650764;
            var end = 1750057;
            var list = new List<SpiderEntity>();
            HahaWebReader reader = new HahaWebReader();
            for (int i = start; i < end; i++)
            {
                var url = "http://www.haha.mx/joke/" + i;
                var result = reader.GetHtmlContent(url);
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