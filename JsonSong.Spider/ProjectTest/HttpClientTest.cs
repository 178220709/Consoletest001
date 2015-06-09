using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using JsonSong.Spider.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace  JsonSong.Spider.ProjectTest
{
 
    [TestClass]
    public class HttpClientTest
    {
        private const string url = "http://www.zhihu.com/";
        [TestMethod]
        public void FixFlag()
        {
            //保存cookie 模拟登陆请求
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseCookies = true;
            var uri = new Uri(url);
            handler.CookieContainer.SetCookies(uri, @" z_c0=QUFBQVBZb2VBQUFYQUFBQVlRSlZUY3NlbVZYZV9JVF81ZGVGTzFyU005SDBHWTVRWkhfRlFnPT0=|1433506251|43144798d3c15dc64e1ba613bbcac1e1218d8b09");
            HttpClient client = new HttpClient(handler);
        
            var result = client.GetStringAsync(uri);
            Console.WriteLine(result.Result);

            //z_c0  "QUFBQVBZb2VBQUFYQUFBQVlRSlZUY3NlbVZYZV9JVF81ZGVGTzFyU005SDBHWTVRWkhfRlFnPT0=|1433506251|43144798d3c15dc64e1ba613bbcac1e1218d8b09"

            var getCookies = handler.CookieContainer.GetCookies(uri);
            Console.WriteLine("获取到的cookie数量：" + getCookies.Count);
            Console.WriteLine("获取到的cookie：");
            for (int i = 0; i < getCookies.Count; i++)
            {
                Console.WriteLine(getCookies[i].Name + ":" + getCookies[i].Value);
            }
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine(handler.CookieContainer.PerDomainCapacity);


            Console.ReadKey();
        }

        private const string img1 = "http://img1.gamersky.com/image2014/11/20141128ge_5/image006.jpg";
        [TestMethod]
        public void DownImgTest()
        {
           
        }
    }
}
