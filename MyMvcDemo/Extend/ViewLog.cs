using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using MyMvcDemo.Models;
using MyProject.WeixinModel.Model;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyMvcDemo.Extend
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
        public static IList<string>  GetDebuggerFiles()
        {
           // string path = Path.Combine(_logPath, logEnum.ToString());
            var files = Directory.GetFiles(_logPath);
            return files.ToList();
        }


        public static  string GetrFileContent(string path)
        {
           return File.ReadAllText(path);
        }

    }
}
