using System;
using System.Collections.Generic;
using System.Linq;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    }

    
}
