using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.FromNode;
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
            //string url = "http://jsonsong.duapp.com/spider/haha";
            string url = "http://localhost:18080/api/haha/getList";
            var result = RestHelper.Get<object>(url);
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod()]
        public void GetTest2()
        {
            //string url = "http://jsonsong.duapp.com/spider/haha";
            string url = "http://localhost:18080/api/haha/getList";
            var paras = new Dictionary<string, string>()
            {
                {"pageSize", "9"}
            };

            var res = HttpWebResponseUtility.CreatePostHttpResponse(url, paras, 5000, "", Encoding.UTF8, null);
            var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            var result = reader.ReadToEnd();

        }

        [TestMethod()]
        public void GetTest3()
        {
            //string url = "http://jsonsong.duapp.com/spider/haha";
            string url = "http://localhost:18080/api/haha/getList";
           
            var res = HttpRestHelper.GetPost(url, new{pageSize=9});
          

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
