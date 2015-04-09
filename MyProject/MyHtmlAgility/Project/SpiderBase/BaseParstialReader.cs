using System;
using System.Text;
using MyProject.MyHtmlAgility.Core;
using Suijing.Utils.sysTools;

namespace MyProject.MyHtmlAgility.Project.SpiderBase
{
    /// <summary>
    /// 用于分页内容(一篇帖子分成几十个页面)的爬取  
    /// </summary>
    public abstract class BaseParstialReader
    {
        protected int PageId = 1;
        //最长爬取页数 避免无限循环
        protected const int Limit = 80;
        protected string BaseUrl = "";
        protected string CurrentUrl = "";
        protected ReadResult  result  ;
        protected StringBuilder ContentBuilder = new StringBuilder( );

        protected BaseParstialReader( string baseUrl)
        {
            BaseUrl = baseUrl;
            CurrentUrl = baseUrl;
            result = new ReadResult(baseUrl);
        }

      
        protected abstract void GetTitleInfo();
        protected abstract void GetCurrent();
        /// <summary>
        /// 根据当前doc 判断是否已经是最后一页，如果是 返回false 如果不是 ，更新CurrentUrl和doc
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckAndMoveNext();

        public ReadResult OutputResult()
        {
            result.Content = ContentBuilder.ToString();
            return result;
        }

        public void StartReadAll()
        {
            LogHepler.WriteWebReader(string.Format("开始爬取{0}", BaseUrl ));
            try
            {
                GetTitleInfo();
                //PageId 仅用于标记次数，避免死循环 实际页面逻辑判断在CheckAndMoveNext处理 
                do
                {
                     GetCurrent();
                     PageId++;
                } while (CheckAndMoveNext() && PageId <= Limit);
            }
            catch (Exception ex)
            {
                LogHepler.WriteWebReader(BaseUrl+"出现异常:" + ex.Message); 
            }
            LogHepler.WriteWebReader(string.Format("爬取{0}结束", BaseUrl));
        }
    }

  
}
