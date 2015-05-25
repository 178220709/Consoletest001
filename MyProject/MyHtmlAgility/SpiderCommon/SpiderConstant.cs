using System.Collections.Generic;
using JsonSong.Spider.Project.Haha;
using JsonSong.Spider.Project.Youmin;
using MyProject.MyHtmlAgility.SpiderBase;

namespace MyProject.MyHtmlAgility.SpiderCommon
{
    /// <summary>
    /// 同步远程的数据
    /// </summary>
    public static class SpiderConstant
    {
        
        public const string RemoteUrl = "http://jsonsong.duapp.com/api/spider/GetSpiderList";


        public static Dictionary<int, string> CnNameDictionary = new Dictionary<int, string>()
        {
             {1, "sp_haha"},
             {2, "sp_youmin"}
        };
    }
}