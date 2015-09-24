using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.Spider.Core;

namespace JsonSong.TaskFactory
{
    /// <summary>
    /// 检查搬瓦工4美元一年的vps是否到货
    /// </summary>
    public class BandwagonMonitor : IMyMonitor
    {
        private const string Url = "https://bandwagonhost.com/cart.php?a=add&pid=19";
        private const string Selecter = ".contentlt-wide .whmcscontainer #whmcsorderfrm";
        private const string Key = "Out of Stock";


        public void TaskStart()
        {
            if (Check())
            {
                Notify();
            }
        }

        public bool Check()
        {
            var helper = HtmlAsyncHelper.CreatWithProxy(-1);
            var doc = helper.GetDocumentNode(Url).Result;
            var html = doc.DocumentNode.QuerySelector(Selecter).InnerHtml;
            return !html.Contains(Key);
        }

        public void Notify()
        {
            
        }
    }
}
