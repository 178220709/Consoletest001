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

namespace JsonSong.Spider.Core
{
    public class WebTaskFactory
    {
        public int ThreadSize = 5;
        public WebTaskReader _reader;

        public WebTaskFactory(WebTaskReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// 根据传过来的urls集合,并行抓取url对应的内容,抓取方法由WebTaskReader子类实现
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public List<ReadResult> StartAndCallBack(IList<string> urls)
        {
            if (!urls.Any())
                return null;

            LogHepler.WriteWebReader(string.Format("开始爬取{0}条数据:\n {1} ...", urls.Count, string.Join("\n", urls.Take(3))));
            try
            {
                var res = urls.AsParallel().Select(a => _reader.GetHtmlContent(a))
                    .Where(a => !string.IsNullOrWhiteSpace(a.Content))
                    .ToList();
                _reader.FireTaskCallBack(res);
                LogHepler.WriteWebReader("成功爬取并执行完毕");
                return res;
            }
            catch (Exception ex)
            {
                LogHepler.WriteWebReader("出现异常:" + ex.Message); 
                return null;
            }
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
        }
        public string  Url { get; set; }
        public string  Title { get; set; }
        public string  Content { get; set; }
        public string  StyleStr { get; set; }
       
        public dynamic  Extend { get; set; }

        public int  Weight { get; set; }
    }


    /// <summary>
    /// url解析器 输入url 返回需要的文本
    /// </summary>
    public abstract class WebTaskReader : IWebTaskReader, IWebTaskCallBack
    {
        /// <summary>
        /// 通过url 获取内容 子类Reader去具体实现 由工厂类调用
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public abstract ReadResult GetHtmlContent(string url);
        /// <summary>
        /// 工厂类执行获取任务后,会调用 子类Reader实现的回调  
        /// </summary>
        /// <param name="res"></param>
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
