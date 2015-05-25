using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace JsonSong.Spider.Core
{
    public class HtmlAsyncHelper
    {
        protected HttpClient Client;
        public HtmlAsyncHelper( IWebProxy proxy=null)
        {
            var proxy1 = WebRequest.GetSystemWebProxy();
            var proxy2 = new WebProxy();

            if (proxy!=null)
            {
                var handler = new HttpClientHandler
                {
                    Proxy = proxy,
                };
                handler.Proxy.Credentials = CredentialCache.DefaultCredentials;
                Client = new HttpClient(handler);
            }
            else
            {
                Client = new HttpClient();
            }
        }

        public async Task<string> GetDocHtmlStr(string url)
        {
            return await GetDocHtmlStr(url,Encoding.UTF8);
        }
        public async Task<string> GetDocHtmlStr(string url, Encoding encoding)
        {
            if (encoding==null)
            {
                return await Client.GetStringAsync(url);
            }
            else
            {
                var buffer = await Client.GetByteArrayAsync(url);
                Encoding.GetEncoding("gb2312").GetString(buffer, 0, buffer.Length);
                return encoding.GetString(buffer, 0, buffer.Length);   
            }
        }

        public async Task<HtmlDocument> GetDocumentNode(string url, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            string strContent = await GetDocHtmlStr(url, encoding);
            var htmlDoc = new HtmlAgilityPack.HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };

            htmlDoc.LoadHtml(strContent);
            return htmlDoc;
        }
    }
}