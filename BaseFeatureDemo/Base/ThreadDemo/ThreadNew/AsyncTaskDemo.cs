using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadNew
{
    [TestClass]
    public class AsyncTaskDemo
    {

        private static Task<MyResult> startDownload(string url)
        {
            var ttt = new TaskCompletionSource<MyResult>();
       

            var task = new Task<MyResult>(() => new MyResult());
            return task;
        }

        private static void finishDownload(Task<MyResult> task)
        {
            var re = task.Result;


        }

        [TestMethod]
        public static void Main1()
        {
            var urls = Enumerable.Range(0, 10).Select(a => a + "");
            urls.RunAsync(startDownload, finishDownload);
        }

        private static Task<byte[]> startDownload2(string url)
        {
            var tcs = new TaskCompletionSource<byte[]>(url);
            var wc = new WebClient();
            wc.DownloadDataCompleted += (sender, e) =>
            {
                if (e.UserState == tcs)
                {
                    if (e.Cancelled)
                        tcs.TrySetCanceled();
                    else if (e.Error != null)
                        tcs.TrySetException(e.Error);
                    else
                        tcs.TrySetResult(e.Result);
                }
            };
            wc.DownloadDataAsync(new Uri(url), tcs);
            return tcs.Task;
        }



    }

    internal class MyResult
    {
        public string  Content { get; set; }
    }


    public static class AsyncEx
    {
        public static void RunAsync<T, TResult>(this IEnumerable<T> taskParms,
Func<T, Task<TResult>> taskStarter, Action<Task<TResult>> taskFinisher)
        {
            taskParms.Select(parm => taskStarter(parm)).
            AsParallel().
            ForAll(t => t.ContinueWith(t2 => taskFinisher(t2)));
        }
    }



}