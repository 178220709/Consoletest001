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

            if (string.IsNullOrEmpty(_projectPath))
            {
                var p3 = AppDomain.CurrentDomain.BaseDirectory;
                LogHepler.Info("AppDomain.CurrentDomain.BaseDirectory: " + p3);
                _projectPath = CuteProjectPath(p3);
            }


            if (string.IsNullOrEmpty(_projectPath))
            {
                var p1 = HttpRuntime.BinDirectory;
                if (p1.Contains(ProjectName))
                {
                    _projectPath = CuteProjectPath(p1);
                }
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


    }

}
