using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;


namespace JsonSong.Spider.Core
{
    public static class NormalHtmlHelper
    {
        public static string GetDocHtmlStr(string url, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
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

        public static HtmlDocument GetDocumentNode(string url, Encoding encoding = null)
        {
            if (encoding==null)
            {
                encoding = Encoding.UTF8;
            }
            string strContent = GetDocHtmlStr(url, encoding);
            return GetDocumentNode(strContent);
        }

        public static HtmlDocument GetDocumentNode(string html)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }


    }
}