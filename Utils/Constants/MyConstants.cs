namespace Suijing.Utils.Constants
{
    public static class MyConstants
    {
        public static string Globe2 = "icon-globe";
        public static class Bootstrap
        {
           
            public static class Icon
            {
                public const string Globe = "icon-globe";
                public const string User = "icon-globe";
                public const string Card = "icon-credit-card"; 
            }
        }   
        
        
        public static class CacheKey
        {
            public const string KEY_JS_CONFIG = "KEY_JS_CONFIG";
        }
        public static class JSSettingKey
        {
           
            /// <summary>
            /// YouminTagUrls 用于任务的动态获取
            /// </summary>
            public static string KEY_YouminTagUrls = "YouminTagUrls";
        }

        public static class AppSettingKey
        {
            /// <summary>
            /// PacDir的路径,目前只有本地能用
            /// </summary>
            public static string KEY_PacDir= "PacDir";
            /// <summary>
            /// 是否在启动阶段开启爬虫任务(试用hangfire管理)
            /// </summary>
            public static string KEY_StartSpider = "StartSpider";
            /// <summary>
            /// JSConfig是否需要缓存
            /// </summary>
            public static string KEY_JSConfigCache = "JSConfigCache";
            /// <summary>
            /// 每小时第几分钟
            /// </summary>
            public static string KEY_CronMin = "CronMin";
            /// <summary>
            /// SpiderImgDir path 
            /// </summary>
            public static string KEY_SpiderImgDir = "SpiderImgDir";
        }
    }
}
