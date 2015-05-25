using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsonSong.Spider.Core;

namespace BaseFeatureDemo.ProxyDemo
{
    public class ProxyDemo
    {
       // const string url = "http://www.t66y.com/";
        //const string url = "www.baidu.com";
        const string url = "www.t66y.com";
        public static async Task Main1()
        {
            //var proxy1 = WebRequest.GetSystemWebProxy();
            //var result1 = await TestProxy(proxy1);

            var proxy2 = new WebProxy("http://127.0.0.1:1080/pac?t=201505242240223550");
            proxy2.UseDefaultCredentials = true;
            WebRequest.DefaultWebProxy = proxy2;

         //   var result2= await TestProxy(proxy2);
          //  var reurl = proxy2.GetProxy(new Uri(url));

            var hs =  Dns.GetHostAddresses(url);


            var reur2 =  proxy2.GetProxy(new Uri("http://www.baidu.com/"));



         var re =   await TestProxy(proxy2);
        }

        private static async Task<string> TestProxy( IWebProxy proxy)
        {

            //  const string _url = "http://ip.qq.com/";
            const string _url = "https://www.youtube.com/";
            var factory = new HtmlAsyncHelper(proxy);


            return await factory.GetDocHtmlStr(_url, Encoding.GetEncoding("gbk"));
        }
    }
}