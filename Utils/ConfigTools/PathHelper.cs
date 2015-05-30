using System;
using System.Configuration;
using Suijing.Utils.WebTools;

namespace Suijing.Utils.ConfigTools
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class PathHelper
    {
        const string ProjectName = "HelloCSharp";
       
        public static string GetDBPath(string name)
        {
            var path = System.IO.Path.GetFullPath(
               System.IO.Directory.GetCurrentDirectory() + string.Format("../../../../dbFile/{0}.db",name) );
             
            return path;
        }
    }

}
