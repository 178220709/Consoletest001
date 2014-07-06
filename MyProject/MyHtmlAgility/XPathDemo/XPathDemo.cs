#region Using namespace

using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace MyProject.MyHtmlAgility.XPathDemo
{
    [TestClass]
    public class XPathDemo
    {
        public  static string GetemoXML
        {
            get
            {
                return "";
            }

           
        }

        public void main1()
        {
            var web = new HtmlDocument();
            web.LoadHtml(GetemoXML);
            var root = web.DocumentNode;
            var chld = root.SelectNodes("");


        }

    }
}