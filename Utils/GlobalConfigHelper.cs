#if (Debug && Trace)
#define DebugAndTrace
#else
#define Debug
#endif
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Hosting;


namespace Suijing.Utils
{
    public static  class GlobalConfigHelper
    {
        private static string Path = "";
        static GlobalConfigHelper()
        {
            InitDebug();
        }

        [Conditional("DEBUG")]
        public static void InitDebug()
        {
            if (HttpContext.Current==null)
            {
                Thread.GetDomain().SetData(".appPath", "c:\\inetpub\\wwwroot\\webapp\\");
                Thread.GetDomain().SetData(".appVPath", "/");
                TextWriter tw = new StringWriter();
                HttpWorkerRequest wr = new SimpleWorkerRequest
                ("/Home/app", "", tw);
                HttpContext.Current = new HttpContext(wr);
            }
        }

        public static string GetProjectPath()
        {
            var runPath = Environment.CurrentDirectory;
            const string ProjectName = "HelloCSharp";
            var index = runPath.IndexOf(ProjectName, System.StringComparison.Ordinal);
            var path = runPath.Substring(0, index + ProjectName.Length) + "\\";
            return path;
        }

    }
}
