#if (Debug && Trace)
#define DebugAndTrace
#else
#define Debug
#endif
using System;
using System.Diagnostics;

namespace Suijing.Utils
{
    public static  class ConfigHelper
    {
        private static string Path = "";
        static ConfigHelper()
        {
            InitDebugPath();
        }

        [Conditional("DEBUG")]
        static void InitDebugPath()
        {
            const string path = @"D:\code\GitCode\HelloCSharp\MyMvcDemo\App_Data\jsConfigs.js";


            Path = path;
        }
    }
}
