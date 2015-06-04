using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suijing.Utils.Constants;
using Suijing.Utils.sysTools;
using Suijing.Utils.WebTools;

namespace Suijing.Utils.ConfigTools
{
    public static  class JSConfigHelper
    {
        public static string JSConfigPath = "~/App_Data/jsConfigs.js";

        public static string GetJsConfigPath()
        {
            if (HttpContext.Current==null)
            {
                const string server = @"f:\usr\LocalUser\qxw1099000260\App_Data\jsConfigs.js";
                if (File.Exists(server))
                {
                    LogHelper.Info("File.Exists(server)");
                    return server;
                }
                else
                {
                    var path = PathHelper.GetRelativePath(JSConfigPath);
                    LogHelper.Info("PathHelper.GetRelativePath(JSConfigPath);" + PathHelper.GetProjectPath() + path);
                    return path;
                }
            }
            return HttpContext.Current.Server.MapPath(JSConfigPath);
        }

        /// <summary>
        /// 取得Config.js内的Json节点，并尝试转换成指定的类型
        /// </summary>
        /// <typeparam name="T">转换成的类型</typeparam>
        /// <param name="key">键名</param>
        /// <returns>值</returns>
        public static T GetJsConfigAs<T>( string key)
        {
            var result = default(T);
            JObject configObject = null;
            if (ConfigHelper.GetConfigBool(MyConstants.AppSettingKey.KEY_JSConfigCache))
            {
                configObject = CacheHelper.GetCache(MyConstants.CacheKey.KEY_JS_CONFIG) as JObject;
            }
          
            if (configObject == null)
            {
                var configFilePath = GetJsConfigPath();
                var configJson = File.ReadAllText(configFilePath);
                configJson = configJson.Trim();
                configJson = configJson.Replace("var $$sc =", string.Empty);
                configJson = configJson.TrimEnd(';');

                configObject = JsonConvert.DeserializeObject(configJson) as JObject;
                CacheHelper.SetCache(MyConstants.CacheKey.KEY_JS_CONFIG, configObject);
            }
            result = configObject[key].ToObject<T>();
            return result;
        }
    }
}
