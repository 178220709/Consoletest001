using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonSong.Spider.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils.sysTools;

namespace BaseFeatureDemo.ProxyDemo
{
    public class ProxyDemo
    {
        // const string url = "http://www.t66y.com/";
        //const string url = "www.baidu.com";
        private const string url = "www.t66y.com";

        public static async Task Main1()
        {
            var re = await TestProxy(null);
            Thread.Sleep(20000);
        }

        private static async Task<string> TestProxy(IWebProxy proxy)
        {
            const string _url = "http://www.baidu.com/";
          //  const string _url = "http://ip.qq.com/";
            // const string _url = "https://www.youtube.com/";
            var factory = HtmlAsyncHelper.CreatWithProxy(0);
            return await factory.GetDocHtmlStr(_url, Encoding.GetEncoding("gbk"));
        }


    }

    [TestClass]
    public class ProxyDemo2
    {
        [TestMethod]
        public async Task Main1Test()
        {
            const string _url = "http://www.baidu.com/";
            var factory = HtmlAsyncHelper.CreatWithProxy(1);
            try
            {
                var str = await factory.GetDocHtmlStr(_url, Encoding.GetEncoding("utf-8"));
            }
            catch (Exception ex)
            {

                var str = ex;
            }
            

            Thread.Sleep(30000);
        }
    }


}