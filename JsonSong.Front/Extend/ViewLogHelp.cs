using System.IO;
using System.Net.Http;
using System.Security.Cryptography;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JsonSong.Front.Extend
{
    public enum LogEnum
    {
        LogDebugger,
        LogError,
        LogWebReader
    }

    public  class ViewLogHelp
    {
        private static readonly string _logPath = System.Web.HttpContext.Current.Server.MapPath("~/log");
        public static IList<string>  GetLogFiles()
        {
           // string path = Path.Combine(_logPath, logEnum.ToString());
            var files = Directory.GetFiles(_logPath,"*.txt",SearchOption.AllDirectories);
            return files.ToList();
        }


        public static  string GetrFileContent(string path , string name)
        {
            return File.ReadAllText(Path.Combine(_logPath, path, name), Encoding.GetEncoding("gbk"));
        }

        public static string GetrFileContent(string fullName)
        {
            return File.ReadAllText(fullName, Encoding.GetEncoding("gbk"));
        }
    }
}
