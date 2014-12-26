using System;
using System.IO;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.UtilsTest    
{

    [TestClass()]
    public class CacheTest
    {

        [TestMethod()]
        public void Test1()
        {
            string key = "key";
            string value = "json";
            DataCache.SetCacheSecond(key, value, 100 * 60, TimeSpan.Zero);
            DataCache.SetCacheSecond(key, value, 100 * 60);

            TimeSpan sp1 = TimeSpan.Zero;
            TimeSpan sp2 = default(TimeSpan);
            TimeSpan sp3 = Cache.NoSlidingExpiration;

            bool re = sp3 == sp2;
        }


        [TestMethod()]
        public void Test2()
        {
           
        }
    }
}
