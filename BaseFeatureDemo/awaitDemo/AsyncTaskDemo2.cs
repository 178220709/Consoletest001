using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BaseFeatureDemo.Base.ThreadDemo.ThreadBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.awaitDemo
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
            return await Task.Run(() => SyncHelper.DelayLog(3));
        }

        private static Task<string> SyncFun3()
        {
            var task = new Task<string>(o => SyncHelper.DelayLog((int)o), 3);
            return task;
        }
        private async static Task<string> SyncFun4()
        {
            var str =  await Task.Run(() => SyncHelper.DelayLog(3));
            Log("SyncFun4 after await in " + Thread.CurrentThread.ManagedThreadId );
            return str;
        }
        public static readonly Action<string> Log = TestHelper.Log;

        public  static async void Main1()
        {
            Log("the main thread id is " + Thread.CurrentThread.ManagedThreadId  );

            //Log("start1");
            //var str1 = SyncFun();  //阻塞主线程
            //Log("end1");

            //Log("start2");
            //var task = SyncFun2();
            //Log("end2");
            //// var str2 = task.Result;  
            //Log("end22");// Log("end22");马上执行 task里面仍然异步执行

            //Log("start3");
            //var task3 = SyncFun3();
            //Log("end3 :");
            //task3.Start();

            //var awaiter3 = task3.GetAwaiter();
            //awaiter3.OnCompleted(() =>
            //{
            //    var result = awaiter3.GetResult();
            //    Log("task3 result is  :" + result);
            //});

            

            Log("start4");
            var str4 =  await SyncFun4();
            Log("end4");
            //  task4.Start(); 直接start会报异常? 

            Log("task4 result is  :" + str4 + " in " + Thread.CurrentThread.ManagedThreadId);


        }


        public async Task<int> SumPageSizesAsync2(IList<Uri> uris)
        {
            var tasks = uris.Select(uri => new WebClient().DownloadDataTaskAsync(uri));
            var data = await TaskEx.WhenAll(tasks);
            return await TaskEx.Run(() => data.Sum(s => s.Length));
        }



    }
}