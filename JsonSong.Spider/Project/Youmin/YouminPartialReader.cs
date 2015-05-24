using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using JsonSong.Spider.SpiderBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonSong.Spider.Core;
using MyProject.MyHtmlAgility.SpiderBase;
using Omu.ValueInjecter;
using Suijing.Utils;

namespace MyProject.MyHtmlAgility.Project.Youmin
{
    [TestClass]
    public class YouminPartialReader : BaseParstialReader
    {
        private HtmlNode _doc;


        public YouminPartialReader(string baseUrl) : base(baseUrl)
        {
            _doc = NormalHtmlHelper.GetDocumentNode(this.BaseUrl).DocumentNode;
        }
        // 
        protected override void GetTitleInfo()
        {
            result.Title = _doc.QuerySelector(".tit2.mid h1").InnerText;
        }

        protected override void GetCurrent()
        {
            _doc.QuerySelectorAll(".left_got #gspaging p").ToList()
                .ForEach(a=> ContentBuilder.Append(a.OuterHtml));
        }

        protected override bool CheckAndMoveNext()
        {
            var list = _doc.QuerySelectorAll(".page_css a")
                .Select(a => new {title = a.InnerText, href = a.GetAttributeValue("href", "")});
            var next = list.SingleOrDefault(a => a.title == "下一页");
            if (next==null)
            {
                return false;
            }
            this.CurrentUrl = next.href;
            _doc = NormalHtmlHelper.GetDocumentNode(this.CurrentUrl).DocumentNode;
            return true;
        }
    }
}
