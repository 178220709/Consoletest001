using System;
using System.IO;
using System.Text;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Suijing.Utils.sysTools;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.UtilsTest    
{

    [TestClass()]
    public class NetTest
    {

        [TestMethod()]
        public void Test1()
        {
            var url = "www.baidu.com";
            var re = NetHelper.GetPing(url);

        }


    }
}
