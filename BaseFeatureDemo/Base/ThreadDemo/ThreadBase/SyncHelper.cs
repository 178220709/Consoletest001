using System;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadBase
{
    public class SyncHelper
    {
        public static string DelayLog(int second)
        {
            Thread.Sleep(second*1000);
            var str = string.Format("Delay{0} is Return", second);
            Console.WriteLine(str);
            return str ;
        }

        public static void DelayAction(int second,Action action)
        {
            Thread.Sleep(second * 1000);
            action();
        }
    }
}