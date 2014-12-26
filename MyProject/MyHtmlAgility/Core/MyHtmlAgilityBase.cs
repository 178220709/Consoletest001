using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;
using MyProject.TestHelper;
using NPOI.SS.Formula.Functions;

namespace MyProject.MyHtmlAgility.Core
{
    [TestClass]
    public abstract class MyHtmlAgilityBase 
    {
        public string LoginUrl { get; set; }
        public string Url { get; set; }

        public virtual bool Logining()
        {
            return true;
        }

        public WebBrowser ThisBrowser { get; private set; }

        public virtual void InitBrowser()
        {
            ThisBrowser = new WebBrowser();
           // ThisBrowser.op

        }


   
    }
}
