using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suijing.Utils.WebTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Suijing.Utils.WebTools.Tests
{
    [TestClass()]
    public class RestHelperTests
    {
        [TestMethod()]
        public void GetTest()
        {
            string url = "http://jsonsong.duapp.com/spider/haha";
            var result = RestHelper.Get<object>(url);
            Assert.IsTrue(result.IsSuccess);
        }
        
        [TestMethod()]
        public void PostTest()
        {
            string url = "http://localhost:3900/hello2/1234679";
            Dictionary<string,string> paras = new Dictionary<string, string>()
            {
               {"mykey1","mykey111"},
               {"mykey2","mykey222"}
            };
            var result = RestHelper.Post<object>(url, paras);
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
