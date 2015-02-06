using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace MyProject.MyHtmlAgility.Core
{
    public class PageResult
    {
        public string strPageContent;
        public string strVisitUrl;
    }

    public struct CrawlerItem
    {
        public string strUrl;
        public string strRefer;
 
    }
    public class WebDownloader
    {
        private readonly Stack<CrawlerItem> m_Stack = new Stack<CrawlerItem>();

        public string GetPageByHttpWebRequest(string url, Encoding encoding)
        {

            string result = null;
   
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
                request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";


                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), encoding);
                result = reader.ReadToEnd();
                
            }
            catch (Exception ex)
            {
                result = "";
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
                
            }
            return result;
        }
        public void AddUrlQueue(string strUrl)
        {
            CrawlerItem cI = new CrawlerItem();
            cI.strUrl = strUrl;
            cI.strRefer = strUrl;
           
            m_Stack.Push(cI);
        }
        public void ClearQueue()
        {

            m_Stack.Clear();
        }

        public PageResult ProcessQueue(Encoding encoding)
        {
            PageResult pr = new PageResult();
            if (m_Stack.Count == 0)
            {
                return null;
            }
            else 
            {
                CrawlerItem cI = m_Stack.Pop();


                string strContent = GetPageByHttpWebRequest(cI.strUrl, encoding);
               
                pr.strPageContent = strContent;
                pr.strVisitUrl = cI.strUrl;
                return pr;
            }
        }
        private void GetMainContent()
        {
            string strContent
                = GetPageByHttpWebRequest("", Encoding.UTF8);
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };

            htmlDoc.LoadHtml(strContent);

            IEnumerable<HtmlNode> NodesMainContent = htmlDoc.DocumentNode.QuerySelectorAll("");

            var titleNode = htmlDoc.DocumentNode.QuerySelector("title");
            var n1 = titleNode.ElementsAfterSelf();
            var n2 = titleNode.NodesAfterSelf();
            if (NodesMainContent.Count() > 0)
            {
                var r1 = NodesMainContent.ToArray()[0].OuterHtml;
              
            }
        }
        /*
站点		--->  CSS路径
"Cnblogs"	---> "div#cnblogs_post_body"
"Csdn"		---> "div#article_content.article_content"
"51CTO"		---> "div.showContent"
"Iteye"		---> "div#blog_content.blog_content"
"ItPub"		---> "div.Blog_wz1"
"ChinaUnix" ---> "div.Blog_wz1"
  */
    }
}
