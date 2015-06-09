using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.sysTools;


namespace JsonSong.Spider.Core
{
    public class HtmlAsyncHelper
    {
        protected HttpClient Client;

        protected static WebClient NewWebClient
        {
            get { return new WebClient()
            {
                Proxy = GetProxy(1)
            }; }
        }
        public HtmlAsyncHelper(IWebProxy proxy = null)
        {
            if (proxy != null)
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
        /// <summary>
        /// 创建HtmlAsyncHelper的使用代理的实例
        /// -1 noProxy 0 SystemWebProxy 1 pacSs 2 pacLocal
        /// </summary>
        /// <returns></returns>
        public static HtmlAsyncHelper CreatWithProxy(int index)
        {
            var proxy = HtmlAsyncHelper.GetProxy(index);
            return new HtmlAsyncHelper(proxy);
        }


        private static IWebProxy GetProxy(int index)
        {
            switch (index)
            {
                case -1: return null;
                case 0: return WebRequest.GetSystemWebProxy();
                case 1: return new WebProxy(Path.Combine(PathHelper.GetPacUrl(), "pacSs.js"));
                case 2: return new WebProxy(Path.Combine(PathHelper.GetPacUrl(), "pacLocal.js"));
                default: return WebRequest.GetSystemWebProxy();
            }
        }

        public async Task<string> GetDocHtmlStr(string url)
        {
            return await GetDocHtmlStr(url, Encoding.UTF8);
        }
        public async Task<string> GetDocHtmlStr(string url, Encoding encoding)
        {
            if (encoding == null)
            {
                return await Client.GetStringAsync(url);
            }
            else
            {
                var buffer = await Client.GetByteArrayAsync(url);
                //Encoding.GetEncoding("gb2312").GetString(buffer, 0, buffer.Length);
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

        public async Task<string> DownloadImageGetFileName(string url, string savePath = ""  )
        {
            savePath = string.IsNullOrWhiteSpace(savePath) ? PathHelper.GetRelativePath("~/download/imagesV") : savePath;
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            var guid = Guid.NewGuid().ToString();
            var fileName = guid + "." + PathHelper.GetFileEx(url, "jpg");
            try
            {
             // wc.DownloadFileAsync(new Uri(url), Path.Combine(savePath, fileName));
                await NewWebClient.DownloadFileTaskAsync(new Uri(url), Path.Combine(savePath, fileName));
              return fileName;
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("download {0} is error", url),ex);
                return "";
            }
        }



        public static void LoginCnblogs()
        {
            var httpClient = new HttpClient {MaxResponseContentBufferSize = 256000};
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            String url = "http://passport.cnblogs.com/login.aspx";
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            String username = "hi_amos";
            String password = "密码";

            do
            {
                String __EVENTVALIDATION = new Regex("id=\"__EVENTVALIDATION\" value=\"(.*?)\"").Match(result).Groups[1].Value;
                String __VIEWSTATE = new Regex("id=\"__VIEWSTATE\" value=\"(.*?)\"").Match(result).Groups[1].Value;
                String LBD_VCID_c_login_logincaptcha = new Regex("id=\"LBD_VCID_c_login_logincaptcha\" value=\"(.*?)\"").Match(result).Groups[1].Value;

                //图片验证码
                url = "http://passport.cnblogs.com" + new Regex("id=\"c_login_logincaptcha_CaptchaImage\" src=\"(.*?)\"").Match(result).Groups[1].Value;
                response = httpClient.GetAsync(new Uri(url)).Result;
                // Write("amosli.png", response.Content.ReadAsByteArrayAsync().Result);

                Console.WriteLine("输入图片验证码：");
                String imgCode = "wupve";//验证码写到本地了，需要手动填写
                imgCode = Console.ReadLine();

                //开始登录
                url = "http://passport.cnblogs.com/login.aspx";
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("__EVENTTARGET", ""));
                paramList.Add(new KeyValuePair<string, string>("__EVENTARGUMENT", ""));
                paramList.Add(new KeyValuePair<string, string>("__VIEWSTATE", __VIEWSTATE));
                paramList.Add(new KeyValuePair<string, string>("__EVENTVALIDATION", __EVENTVALIDATION));
                paramList.Add(new KeyValuePair<string, string>("tbUserName", username));
                paramList.Add(new KeyValuePair<string, string>("tbPassword", password));
                paramList.Add(new KeyValuePair<string, string>("LBD_VCID_c_login_logincaptcha", LBD_VCID_c_login_logincaptcha));
                paramList.Add(new KeyValuePair<string, string>("LBD_BackWorkaround_c_login_logincaptcha", "1"));
                paramList.Add(new KeyValuePair<string, string>("CaptchaCodeTextBox", imgCode));
                paramList.Add(new KeyValuePair<string, string>("btnLogin", "登  录"));
                paramList.Add(new KeyValuePair<string, string>("txtReturnUrl", "http://home.cnblogs.com/"));
                response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList)).Result;
                result = response.Content.ReadAsStringAsync().Result;
                //   Write("myCnblogs.html", result);
            } while (result.Contains("验证码错误，麻烦您重新输入"));

            Console.WriteLine("登录成功！");

            //用完要记得释放
            httpClient.Dispose();
        }





    }
}