#region Using namespace

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.MyHtmlAgility.Core;

#endregion

namespace MyProject.MyHtmlAgility.ProjectTest
{
    [TestClass]
    public class HttpWebResponseUtilityTest
    {

        [TestMethod]
        public void GetTest()
        {
           
        } 
        
        [TestMethod]
        public void PostTest()
        {
            string loginUrl = "https://passport.baidu.com/?login";
            string userName = "178220709";
            string password = "2123104821";
            string tagUrl = "http://cang.baidu.com/" + userName + "/tags";
            Encoding encoding = Encoding.GetEncoding("gb2312");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tpl", "fa");
            parameters.Add("tpl_reg", "fa");
            parameters.Add("u", tagUrl);
            parameters.Add("psp_tt", "0");
            parameters.Add("username", userName);
            parameters.Add("password", password);
            parameters.Add("mem_pass", "1");
            HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, encoding, null);
            string cookieString = response.Headers["Set-Cookie"];
            var rcs = response.Cookies;


            CookieCollection cookies = cookieString.GetCookieCollection(".baidu.com");//如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  
             var response2 = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);
             var stream =    response2.GetResponseStream();
             string sHtml = new StreamReader(stream, System.Text.Encoding.Default).ReadToEnd();
        }  
        
        [TestMethod]
        public void PostTest2()
        {
            string loginUrl = "https://mylogin.51job.com/10689274243771471409/my/My_Pmc.php";
            string userName = "sj178220709";
            string password = "212310482sj";
            string tagUrl = "https://mylogin.51job.com/10689274243771471409/my/My_Pmc.php";
            Encoding encoding = Encoding.GetEncoding("gb2312");

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tpl", "fa");
            parameters.Add("tpl_reg", "fa");
            parameters.Add("u", tagUrl);
            parameters.Add("psp_tt", "0");
            parameters.Add("username", userName);
            parameters.Add("password", password);
            parameters.Add("mem_pass", "1");
            HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, encoding, null);
            var rcs = response.Cookies;
            string sHtml1 = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default).ReadToEnd();
         

            string cookieString = "";
             cookieString = "guid=13950639023725080039; guide=passflag%3D1; nolife=fromdomain%3Dwww; slife=compfans%3D1%257C0%257C0%257C0%26%7C%26fanstuijiancomp%3D1; ps=us%3DWmdVNgN%252FVGZWP1wuUDQBNwc8CyRSZVM%252FBjcFKw80VmJdZVc1D2VTZFUyDWNXO1xvVmYGNwAzUntSZwIhWzkDYVow%26%7C%26nv_3%3D1; 51job=cuid%3D65790564%26%7C%26cusername%3Dsj178220709%26%7C%26cpassword%3D%26%7C%26ccry%3D.02LMVssg9UMs%26%7C%26cresumeid%3D96250790%26%7C%26cresumeids%3D.0bzNWEnvlbWw%257C%26%7C%26cname%3D%25CB%25CE%25BD%25A1%26%7C%26cemail%3Dsj178220709%2540163.com%26%7C%26cemailstatus%3D3%26%7C%26cnickname%3D%26%7C%26cenglish%3D0%26%7C%26cautologin%3D1%26%7C%26sex%3D0%26%7C%26cconfirmkey%3Dsjnu5Csy0AVOo%26%7C%26cnamekey%3DsjBvlLP9DYUDY";
            CookieCollection cookies = cookieString.GetCookieCollection(".51job.com");//如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  
             var response2 = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);
             var stream =    response2.GetResponseStream();
             string sHtml = new StreamReader(stream, System.Text.Encoding.Default).ReadToEnd();
        }

    }
}