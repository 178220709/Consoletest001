using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;

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
            var request1 = new WebTestRequest("http://www.baidu.com/");
            request1.ValidateResponse += request1_ValidateResponse;

            var web = new HtmlWeb();
            var doc = web.Load("http://www.baidu.com/");


        }
    }
}
