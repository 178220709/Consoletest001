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



    public class WebTaskFactory
    {
        public int ThreadSize = 5;
        public WebTaskReader _reader;

        public WebTaskFactory(WebTaskReader reader)
        {
            _reader = reader;
        }


        public void StartAndCallBack(IList<string> urls)
        {
            if (!urls.Any())
                return;

            LogHepler.WriteWebReader(string.Format("开始爬取{0}条数据:{1} ...", urls.Count, string.Join("", urls.Take(3))));
            try
            {
                var res = urls.Select(a => _reader.GetHtmlContent(a))
                    .Where(a => !string.IsNullOrWhiteSpace(a.Content))
                    .ToList();
                _reader.FireTaskCallBack(res);
            }
            catch (Exception ex)
            {
                LogHepler.WriteWebReader("出现异常:" + ex.Message); 
                return;
            }
            LogHepler.WriteWebReader("成功爬取并执行完毕");
        }

        private IList<ReadResult> GetHtmlContents(IList<string> urls)
        {
            if (!urls.Any())
                return null;
            return urls.Select(a => _reader.GetHtmlContent(a)).Where(a=>!string.IsNullOrWhiteSpace(a.Content)).ToList();
        }
    }

    /// <summary>
    /// 用于承载解析url的结果集
    /// </summary>
    public class ReadResult
    {

        public ReadResult(string url)
        {
            Url = url;
            Date = DateTime.Now;
        }
        public string  Url { get; set; }
        public string  Content { get; set; }
        public string  StyleStr { get; set; }
        public DateTime  Date { get; set; }
        public dynamic  Extend { get; set; }

        public int  Weight { get; set; }
    }


    /// <summary>
    /// url解析器 输入url 返回需要的文本
    /// </summary>
    public abstract class WebTaskReader : IWebTaskReader, IWebTaskCallBack
    {
        public abstract ReadResult GetHtmlContent(string url);
        public abstract void FireTaskCallBack(IList<ReadResult> res);
    }

    public interface IWebTaskCallBack
    {
        void FireTaskCallBack(IList<ReadResult> res);
    }

    public interface IWebTaskReader
    {
        ReadResult GetHtmlContent(string url);
    }
}
