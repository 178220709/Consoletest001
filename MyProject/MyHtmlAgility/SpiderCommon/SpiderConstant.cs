using System.Collections.Generic;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.MyHtmlAgility.Project.Youmin;
using MyProject.MyHtmlAgility.SpiderBase;

namespace MyProject.MyHtmlAgility.SpiderCommon
{
    /// <summary>
    /// 同步远程的数据
    /// </summary>
    public static class SpiderConstant
    {
        /// <summary>
        /// 本地 mongo服务的引用字典
        /// </summary>
        public static readonly Dictionary<int, SpiderService> DicServices = new Dictionary<int, SpiderService>()
        {
            {1, HahaJokeService.Instance},
            {2, YouminService.Instance}
        };

        public const string RemoteUrl = "http://jsonsong.duapp.com/api/spider/GetSpiderList";


        public static Dictionary<int, string> CnNameDictionary = new Dictionary<int, string>()
        {
             {1, "sp_haha"},
             {2, "sp_youmin"}
        };
    }
}