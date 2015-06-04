using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Suijing.Utils.Constants;

namespace Suijing.Utils.ConfigTools
{
    public static  class JSConfigHelper
    {
        public static string JSConfigPath = "~/App_Data/jsConfigs.js";

        public static string GetJsConfigPath()
        {
            return PathHelper.GetRelativePath(JSConfigPath);
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
                configObject = HttpContext.Current.Cache[MyConstants.CacheKey.KEY_JS_CONFIG] as JObject;
            }
          
            if (configObject == null)
            {
                var configFilePath = GetJsConfigPath();
                var configJson = File.ReadAllText(configFilePath);
                configJson = configJson.Trim();
                configJson = configJson.Replace("var $$sc =", string.Empty);
                configJson = configJson.TrimEnd(';');

                configObject = JsonConvert.DeserializeObject(configJson) as JObject;
                if (HttpContext.Current!=null)
                {
                    HttpContext.Current.Cache[MyConstants.CacheKey.KEY_JS_CONFIG] = configObject; 
                }
            }
            result = configObject[key].ToObject<T>();
            return result;
        }
    }
}
