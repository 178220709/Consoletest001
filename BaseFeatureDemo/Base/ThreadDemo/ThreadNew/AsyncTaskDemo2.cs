using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BaseFeatureDemo.Base.ThreadDemo.ThreadBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadNew
{
    [TestClass]
    public class AsyncTaskDemo2
    {
        private static string SyncFun()
        {
           return SyncHelper.DelayLog(3);
        }
        private async static Task<string> SyncFun2()
        {
            return await TaskEx.Run(() => SyncHelper.DelayLog(3));
        }
        private async static Task<string> SyncFun3()
        {
            return await TaskEx.Run(() => SyncHelper.DelayLog(3));
        }
        private static readonly Action<string> Log = TestHelper.Log;

        public static void Main1()
        {
            Log("start1");
            var str1 = SyncFun();
            Log("end1");

            Log("start2");
            var task = SyncFun2();
            Log("end2");
            // var str2 = task.Result;  
            Log("end22");// Log("end22");马上执行 task里面仍然异步执行

            Console.ReadLine();
        }


        public async Task<int> SumPageSizesAsync2(IList<Uri> uris)
        {
            var tasks = uris.Select(uri => new WebClient().DownloadDataTaskAsync(uri));
            var data = await TaskEx.WhenAll(tasks);
            return await TaskEx.Run(() => data.Sum(s => s.Length));
        }



    }
}