using System;
using System.Collections.Generic;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void AddTest1()
        {
            using (var context = new SpiderDbContext())
            {
                var en = new SpiderEntity()
                {
                    Title = "test",
                    Url = "",
                    Content = "Content",
                    AddedTime = DateTime.Now,
                };
                context.Spiders.Add(en);
                context.SaveChanges() ;
            }
           
        }

    }

    
}
