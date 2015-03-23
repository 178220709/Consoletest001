#if (Debug && Trace)
#define DebugAndTrace
#else 
#define Debug
#endif

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.CLR
{
    [TestClass]
    public class StringDemo
    {
        public static void Main1()
        {
            string str = "";
        }

        [TestMethod]
        public void Test1()
        {


            string str = "12,345.12";



        }

    }
}