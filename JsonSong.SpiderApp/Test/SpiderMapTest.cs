using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using JsonSong.SpiderApp.MyTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.sysTools;

namespace JsonSong.SpiderApp.Test
{
    [TestClass]
    public class SpiderMapTest
    {
        [TestMethod]
        public void AddTest1()
        {
            var list = SpiderImgMapDao.Instance.GetCon().FindAll().ToList();

        }  
        
        [TestMethod]
        public async Task AddTest2()
        {

            string str = @"f:\usr\LocalUser\qxw1099000260\bin";
            var str2 = str.Replace("bin\\", "");
          
            LogHelper.SetConfig();
            var controller = new TaskController();
            await controller.StartHahaTask();


        }

    }

    
}
