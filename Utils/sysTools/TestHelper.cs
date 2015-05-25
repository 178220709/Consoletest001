

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;



namespace Suijing.Utils.sysTools
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