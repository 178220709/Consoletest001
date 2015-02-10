using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using MongoDB.Driver.Builders;
using MyProject.TestHelper;
using NPOI.SS.Formula.Functions;

namespace MyProject.MyHtmlAgility.Core
{
    [TestClass]
    public abstract class MyHtmlAgilityBase :IMyHtmlAgilityBase
    {
        public string LoginUrl { get; set; }
        public string Url { get; set; }

        public virtual bool Logining()
        {
            return true;
        }

        public WebBrowser ThisBrowser { get; private set; }

        public virtual void InitBrowser()
        {
            ThisBrowser = new WebBrowser();
        }


        public virtual string GetDocHtmlStr(string url, Encoding encoding=null )
        {
            if (  encoding == null)
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



    }
}
