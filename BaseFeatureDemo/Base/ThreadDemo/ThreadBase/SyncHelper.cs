using System;
using System.Threading;
using BaseFeatureDemo.Base.Reflect;
using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadBase
{
    public class SyncHelper
    {
        public static string DelayLog(int second)
        {
            Thread.Sleep(second*1000);
            var str = string.Format("Delay{0} is Return in {1}", second,Thread.CurrentThread.ManagedThreadId);
            //TestHelper.Log(str);
            return str ;
        }

        public static void DelayAction(int second,Action action)
        {
            Thread.Sleep(second * 1000);
            action();
        }
    }
}