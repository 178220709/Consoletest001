using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.awaitDemo
{
    [TestClass]
    public class DownloadStringTaskAsyncDemo
    {
       // const string url = "http://www.baidu.com";
        const string url = "http://localhost/";
        [TestMethod]
        public static async Task Main1()
        {
            // Console.WriteLine(await DownloadStringTaskAsync());
            DownloadStringTaskAsyncDemo demo = new DownloadStringTaskAsyncDemo();

          await  demo.Main2();
            Console.ReadLine();
        }

        [TestMethod]
        public async Task Main2()
        {
            await Task.Run(async () =>
             {
                 var re = await DownloadStringTaskAsync();
                 Console.WriteLine(re);
             });
            Console.ReadLine();
        }
        private static async Task<string> DownloadStringTaskAsync()
        {
            string str = "";
            using (var wc = new WebClient())
            {
                str = await wc.DownloadStringTaskAsync(url);
            }
            return str;
        }

        private static void DownloadStringAsync()
        {

            using (var wc = new WebClient())
            {
                wc.DownloadStringAsync(new Uri(url), new object());
                wc.DownloadStringCompleted += (sender, args) => Console.WriteLine(args.Result);
            }
        }

        private static async Task<string> DownloadStringAsync2()
        {

            using (var wc = new HttpClient())
            {
                var res = await wc.GetAsync(url);
                return await res.Content.ReadAsStringAsync();
            }
        }

    }
}