#region Using namespace

using System;

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