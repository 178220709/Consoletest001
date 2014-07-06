#region Using namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.WebTesting;

#endregion

namespace MyProject.TestHelper
{

    public   class ConsoleTestHelper
    {
        public static void PrintCurrentTime()
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss fff")); 
        } 
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("hh:mm:ss fff");
        }
    }
}