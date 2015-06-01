using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using JsonSong.SpiderApp.MyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.Test
{
    [TestClass]
    public class SpiderTest
    {
        [TestMethod]
        public void AddTest1()
        {
            using (var context = new MyDbContext())
            {
                var result = context.Spiders.ToList();
            }
           

        }  
        
        [TestMethod]
        public async Task AddTest2()
        {
            LogHelper.SetConfig();
            var controller = new TaskController();
            await controller.StartHahaTask();


        }

    }

    
}
