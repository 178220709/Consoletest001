using System;
using System.Threading;

namespace Consoletest001.thread1
{
    class ThreadPoolTest
    {
        public  static void MainSDEDFgsd()
        {
            Console.WriteLine("Begin in Main");
            //Thread thread = new Thread(new ThreadStart(ThreadInvoke));
            ////启动线程
            //thread.Start();

            bool queueUserWorkItem = ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadInvoke));

            //将当前线程挂起200毫秒
            Thread.Sleep(200);
            Console.WriteLine("End in Main");

             Thread.Sleep(3000);
        }

        static void ThreadInvoke(Object param)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Execute in ThreadInvoke");
                //每隔100毫秒，循环一次
                Thread.Sleep(100);
            }
        }
    }
}