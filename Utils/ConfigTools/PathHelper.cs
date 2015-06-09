using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Web;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;
using Suijing.Utils.WebTools;

namespace Suijing.Utils.ConfigTools
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class PathHelper
    {
        const string ProjectName = "HelloCSharp";

        private static string _projectPath = "";
        private static string _pacDir = "";

        private static string CuteProjectPath(string path)
        {
            return path.Contains(ProjectName) ?
                path.Substring(0, path.IndexOf(ProjectName, StringComparison.Ordinal) + ProjectName.Length)
                : path;
        }

        public static string GetProjectPath()
        {
            if (string.IsNullOrEmpty(_projectPath))
            {
                if (HttpContext.Current != null)
                {
                    var p1 = HttpContext.Current.Server.MapPath("~");
                    _projectPath = CuteProjectPath(p1);
                }
            }

            //if (string.IsNullOrEmpty(_projectPath))
            //{
            //    var p3 = AppDomain.CurrentDomain.BaseDirectory;
            //    由不同项目调用 会得到不同路径 不适合在这里使用
            //    _projectPath = CuteProjectPath(p3);
            //}

            //单元测试的时候不能使用
            //if (string.IsNullOrEmpty(_projectPath))
            //{
            //    var p1 = HttpRuntime.BinDirectory;
            //    LogHelper.Info("HttpRuntime.BinDirectory: " + p1);
            //    if (p1.Contains(ProjectName))
            //    {
            //        _projectPath = CuteProjectPath(p1);
            //    }
            //    else
            //    {
            //        _projectPath = p1.Replace("bin\\", "");
            //    }
            //    LogHelper.Info("_projectPath: " + _projectPath);
            //}

            if (string.IsNullOrEmpty(_projectPath))
            {
                var p1 = Environment.CurrentDirectory;
                if (p1.Contains(ProjectName))
                {
                    _projectPath = CuteProjectPath(p1);
                }
                else
                {
                    _projectPath = p1.Replace("bin\\", "");
                }
                LogHelper.Info("_projectPath: " + _projectPath);
            }

            if (string.IsNullOrEmpty(_projectPath))
            {
                throw new Exception("the path init has error");
            }
            else
            {
                return _projectPath;
            }


        }

        public static string GetPacUrl()
        {
            if (string.IsNullOrEmpty(_pacDir))
            {
                if (HttpContext.Current != null)
                {
                    var url = HttpContext.Current.Request.Url;
                    _pacDir = url.GetLeftPart(UriPartial.Authority);
                }
            }
            if (string.IsNullOrEmpty(_pacDir))
            {
                _pacDir = ConfigHelper.GetConfigString(MyConstants.AppSettingKey.KEY_PacDir);
            }

            if (string.IsNullOrEmpty(_pacDir))
            {
                _pacDir = "http://localhost:4001/content/pac";
            }

            return _pacDir;
        }

        public static string GetDBPath(string name)
        {
            var projectPath = GetProjectPath();

            var path = System.IO.Path.GetFullPath(projectPath + string.Format("/dbFile/{0}.db", name));

            if (!Directory.GetParent(path).Exists)
            {
                Directory.CreateDirectory(Directory.GetParent(path).FullName);
            }
            return path;
        }

        /// <summary>
        /// 根据相对路径得到文件路径, 单元测试可用 //注意 无http请求时使用
        /// </summary>
        /// <param name="rPath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string rPath)
        {
            var p3 = AppDomain.CurrentDomain.BaseDirectory;
            if (p3.Contains(ProjectName))
            {
                return Path.Combine(GetProjectPath(), "JsonSong.ManagerUI" + rPath.TrimStart('~'));
            }
            else
            {
                var path =Path.Combine(GetProjectPath(), rPath.TrimStart('~'));
               
                return path;
            }
        }


        /// <summary>
        /// 得到文件的后缀
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultEx"></param>
        /// <returns></returns>
        public static string GetFileEx(string name,string defaultEx = "")
        {
            var index = name.LastIndexOf('.');
            var endIndex = name.LastIndexOf('?');
            if (endIndex==-1 || endIndex < index)
            {
                endIndex = name.Length-1;
            }
            if (index ==-1)
            {
                return defaultEx ;
            }
            var subStr = name.Substring(index + 1, endIndex - index);
            if (subStr.Length>3)
            {
                 return defaultEx ;
            }
            return subStr;
        }
    }

}
