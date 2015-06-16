#if (Debug && Trace)
#define DebugAndTrace
#else 
#define Debug
#endif

using System;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.CLR
{
    [TestClass]
    public class StringDemo
    {

        private const string faceStr = "༼ ༎ຶ ෴ ༎ຶ༽ ";
        public static void Main1()
        {
            string str = "";
        }

        [TestMethod]
        public void Test1()
        {
            var str0 = DefaultNullableTest(null);
            var str1 = DefaultNullableTest();
            var str2 = DefaultNullableTest("");
            var str3 = DefaultNullableTest("test");

            Assert.AreEqual(str0, str3);
            Assert.AreNotEqual(str1, str3);
            Assert.AreNotEqual(str2, str3);

        }


        private static string DefaultNullableTest(string str = "")
        {
            return str ?? "test";
        }

    }
}