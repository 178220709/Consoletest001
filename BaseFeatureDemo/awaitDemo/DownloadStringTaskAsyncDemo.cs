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
        [TestMethod]
        public static async Task Main1()
        {
            Console.WriteLine(await DownloadStringTaskAsync());
        }

        [TestMethod]
        public static void Main2()
        {
            DownloadStringAsync();
        }
        private static async Task<string> DownloadStringTaskAsync()
        {
            const string url = "http://t0.jsonsong.com/";
            using (var wc = new WebClient())
            {
                return await wc.DownloadStringTaskAsync(url);
            }
        }

        private static void DownloadStringAsync()
        {
            const string url = "http://t0.jsonsong.com/";
            using (var wc = new WebClient())
            {
                wc.DownloadStringAsync(new Uri(url), new object());
                wc.DownloadStringCompleted += (sender, args) => Console.WriteLine(args.Result);
            }
        }

        private static async Task<string> DownloadStringAsync2()
        {
            const string url = "http://t0.jsonsong.com/";
            using (var wc = new HttpClient())
            {
                var res = await wc.GetAsync(url);
                return await res.Content.ReadAsStringAsync();
            }
        }

    }
}