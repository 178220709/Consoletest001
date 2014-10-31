using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.Enum
{
    [TestClass]
    public class EnumDemo
    {
        [TestMethod]
        public static void Main1()
        {
           
        }

        [TestMethod]
        public void Test1()
        {
            EnumTest e1 = EnumTest.Pro3;
            var what1 = EnumTest.Pro1 | EnumTest.Pro2;
            bool re = (e1 == what1);

        }
    }

    [Flags]
    public enum EnumTest
    {
        Pro1,
        Pro2,
        Pro3,
        Pro4,
    }
   
}
