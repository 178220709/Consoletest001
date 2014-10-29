using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace BaseFeatureDemo.Base.Reflect
{
    public sealed class SealedApplication
    {
        internal static string WindowsFormsVersion
        {
            get
            {
                return "WindowsForms10";
            }
        }
    }


    public static class RunTest
    {
        public static void Main1()
        {
            Func<string> myReturn = () =>
            {
                return "this is my string";
            };


            var info = typeof(SealedApplication).GetProperty("WindowsFormsVersion");
          
          
        }
    }
}
