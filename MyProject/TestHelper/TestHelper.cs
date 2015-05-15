#region Using namespace

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;

#endregion

namespace MyProject.TestHelper
{

    public static class TestHelper
    {
        public static void InitCurrentContext()
        {
            if (HttpContext.Current != null) return;
            Thread.GetDomain().SetData(".appPath", "c:\\inetpub\\wwwroot\\webapp\\");
            Thread.GetDomain().SetData(".appVPath", "/");
            TextWriter tw = new StringWriter();
            HttpWorkerRequest wr = new SimpleWorkerRequest("/Home/app", "", tw);
            HttpContext.Current = new HttpContext(wr);
        }

        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void Log(string str)
        {
            var show = DateTime.Now.ToString("HH:mm:ss-fff") + str;
            Console.WriteLine(show);
            Trace.WriteLine(show);
        }
    }
}