
using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using MyProject.TestHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suijing.Utils;

namespace JsonSong.ManagerUI.Extend
{
    public static  class ConfigHelper
    {
        static ConfigHelper()
        {
            TestHelper.InitCurrentContext();
        }

        public static string JSConfigPath = "~/App_Data/jsConfigs.js";

        public static string GetJsConfigPath()
        {
            return GlobalConfigHelper.GetProjectPath() + "JsonSong.ManagerUI/App_Data/jsConfigs.js";
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

            var configObject = HttpContext.Current.Cache[Suijing.Utils.Constants.MyConstants.CacheKey.KEY_JS_CONFIG] as JObject;

            if (configObject == null)
            {
                var configFilePath = GetJsConfigPath();
                var configJson = File.ReadAllText(configFilePath);
                configJson = configJson.Trim();
                configJson = configJson.Replace("var $$sc =", string.Empty);
                configJson = configJson.TrimEnd(';');

                configObject = JsonConvert.DeserializeObject(configJson) as JObject;
                HttpContext.Current.Cache[Suijing.Utils.Constants.MyConstants.CacheKey.KEY_JS_CONFIG] = configObject;
            }

            result = (T)Convert.ChangeType(((JValue)configObject[key]).Value, typeof(T));

            return result;
        }
    }
}
