using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Suijing.Utils.sysTools;

namespace MyProject.MyHtmlAgility.Core
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
                //count 仅用于标记次数 仅用于避免死循环 实际页面逻辑判断在PageId处理 
                int count = 0;
                do
                {
                     GetCurrent();
                     count++;
                } while ( CheckAndMoveNext() && count<Limit);

            }
            catch (Exception ex)
            {
                LogHepler.WriteWebReader(BaseUrl+"出现异常:" + ex.Message); 
            }
            LogHepler.WriteWebReader(string.Format("爬取{0}结束", BaseUrl));
        }
    }

  
}
