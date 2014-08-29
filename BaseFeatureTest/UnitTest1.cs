using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaseFeatureDemo.MyGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvcDemo.Extend;
using Omu.ValueInjecter;

namespace BaseFeatureTest
{
    [TestClass]
    public class MyTest1
    {
        public class TargeClass
        {
            public int? Id { get; set; }
        }

        [TestMethod]
        public  void Test1()
        {
            
        }

        public static void Main(string[] args)
        {

            var str = MyConstants.Globe2;

        }
    }
    public static class MyConstants
    {
        public static string Globe2 = "icon-globe";
        public static class Bootstrap
        {

            public static class Icon
            {
                public const string Globe = "icon-globe";
                public const string User = "icon-globe";
                public const string Card = "icon-credit-card";


            }
        }
    }
   
}
