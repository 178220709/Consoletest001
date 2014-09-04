using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using MyProject.TestHelper;

namespace MyProject.MyHtmlAgility.Core
{
    [TestClass]
    public class WebTestEx : WebTest
    {

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            var request1 = new WebTestRequest("http://www.baidu.com/");
           
            request1.ValidateResponse += request1_ValidateResponse;
            yield return request1;
        }

        void request1_ValidateResponse(object sender, ValidationEventArgs e)
        {

            //load the response body string as an HtmlAgilityPack.HtmlDocument

            var doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(e.Response.BodyString);

            //locate the "Nav" element

            HtmlNode navNode = doc.GetElementbyId("Nav");

            //pick the first <li> element

            HtmlNode firstNavItemNode = navNode.SelectSingleNode(".//li");

            //validate the first list item in the Nav element says "Windows"

            e.IsValid = firstNavItemNode.InnerText == "Windows";

        }

        [TestMethod]
        public  void main1()
        {
            var web = new HtmlWeb();
            var doc = web.Load("http://www.haha.mx/joke/1306731");
            var text = "//div[@id='content']//div[@class='clearfix mt-15']";

            HtmlNode htmlNode = doc.DocumentNode.SelectSingleNode(text);

            string str = htmlNode.InnerHtml;
            

            var list = htmlNode.ChildNodes.Where(a=>a.NodeType!=HtmlNodeType.Element);

            string viewStateValue = htmlNode.Attributes["value"].Value;
            htmlNode = doc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']");
            string eventValidation = htmlNode.Attributes["value"].Value;
            htmlNode = doc.DocumentNode.SelectSingleNode("//input[@type='submit']");
            string submitName = htmlNode.Attributes["name"].Value;

        }

        [TestMethod]
        public void main2()
        {
            var resultList = new List<string>();
            Func<object, string> getJokeContent = url =>
            {
                var mweb = new HtmlWeb();
                var mdoc = mweb.Load(url.ToString());
                var mtext = "//div[@id='content']//p[@class='text word-wrap']";

                try
                {
                    Debug.WriteLine("this is task{0} ,start on {1}", Task.CurrentId, ConsoleTestHelper.GetCurrentTime());
                    HtmlNode mhtmlNode = mdoc.DocumentNode.SelectSingleNode(mtext);

                    var result = mhtmlNode.InnerHtml;
                    if (!string.IsNullOrEmpty(result))
                    {
                        resultList.Add(result);
                    }
                    var random = new Random();
                    Thread.Sleep(random.Next(100, 800));
                    Debug.WriteLine("this is task{0} ,over on {1}", Task.CurrentId, ConsoleTestHelper.GetCurrentTime());
                    return result;
                }
                catch (Exception)
                {
                    return "";
                } 
            };
            int anchor = 1306731;
         
            for (int i = 0; i < 10; i++)
            {
                var index = anchor + i;
                var url = "http://www.haha.mx/joke/" + index;
                var task = new Task<string>(getJokeContent, url);
                task.Start();
            }

            Thread.Sleep(20000);
            string listStr = string.Join("<br/>", resultList);
        }

        [TestMethod]
        public void Test1()
        {
            const string loginUrl = @"http://my.51job.com/my/My_SignIn.php?url=%2Fmy%2FMy_Pmc.php%3F3547";
            var web = new HtmlWeb();
            var loginDoc = web.Load(loginUrl);
            var head = loginDoc.DocumentNode.SelectSingleNode("//head");
            
            const string url = @"http://my.51job.com/my/65790564/My_Pmc.php";
            const string url2 = @"http://www.dygod.net/html/gndy/";
           
            var doc = web.Load(url);
         


        }
    }
}
