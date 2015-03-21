using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.MyHtmlAgility.Core;

namespace MyProject.MyHtmlAgility.ProjectTest
{
    [TestClass]
    public class TestWebReader : WebTaskReader
    {
        public override ReadResult GetHtmlContent(string url)
        {
            var re = new ReadResult(url);
            Random random = new Random();

            string start = DateTime.Now.ToString("mm:ss fff");

            var sleep = random.Next(10, 5000);
            Thread.Sleep(sleep);
            re.Content = string.Format(" {0} start at {1} , end at {2}", url, start, DateTime.Now.ToString("mm:ss fff"));
            return re;
        }

        public override void FireTaskCallBack(IList<ReadResult> res)
        {
            try
            {
                var list = res;
            }
            catch (Exception)
            {
                return;
            }
        }

     

        public static List<ReadResult> GetRecommand()
        {
            var urls = new List<string>();
            urls.AddRange(Enumerable.Range(0, 10).Select(a => "" + a));
            var reader = new TestWebReader();
            var factory = new WebTaskFactory(reader);
            return factory.StartAndCallBack(urls.Distinct().ToList());
        }

        [TestMethod]
        public void Test1()
        {
            var list = GetRecommand();
            string str = "";
            list.ForEach(a => str += a.Content+"\n");
        }
    }

  
}
