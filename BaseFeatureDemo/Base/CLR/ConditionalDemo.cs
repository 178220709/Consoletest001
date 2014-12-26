#if (Debug && Trace)
#define DebugAndTrace
#else 
#define Debug
#endif

using System;
using System.Diagnostics;

namespace BaseFeatureDemo.Base.CLR
{
    public class ConditionalDemo
    {
        public static void Main1()
        {
            Print1();
            Print2();
            Print22();
            Print3();
        }

        [Conditional("DEBUG")]
        private static void Print1()
        {
            Console.WriteLine("You defined the Debug parameter");
        }

        //定义了Debug或者Trace后才会执行
        //或者的关系
        [Conditional("Debug"), Conditional("Trace")]
        private static void Print2()
        {
            Console.WriteLine("You defined the Debug or Trace parameter");
        } 
        //定义了Debug或者Trace后才会执行
        //或者的关系
        [Conditional("TRACE")]
        private static void Print22()
        {
            Console.WriteLine("You defined the Debug or Trace parameter22222");
        }


        //只有定义了Debug和Trace后才会执行此方法
        [Conditional("DebugAndTrace")]
        private static void Print3()
        {
            Console.WriteLine("You defined the Debug and Trace parameter");
        }
    }
}