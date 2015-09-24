using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using JsonSong.BaseDao.LiteDb;
using JsonSong.Spider.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace  JsonSong.Spider.Proxy
{
    public class ProxyFactory
    {
        public static async Task<IList<ProxyEntity>> GetProxyList()
        {
            var url = "http://www.xicidaili.com/";
            var help = HtmlAsyncHelper.CreatWithProxy(-1);
            var doc = await help.GetDocumentNode(url);
            //doc.DocumentNode.QuerySelector()

            return null;
        }
    }




    public class ProxyEntity :BaseLiteEntity
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Description { get; set; }
        public DateTime LastCheck { get; set; }
        public bool  Valid { get; set; }


    }
}
