using System;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadBase
{
    public class ResumeDemo
    {
        private Thread newThread;


        public static void Main4()
        {
            ResumeDemo resume = new ResumeDemo();
            Console.WriteLine("is start");
            resume.StartThread();
            Thread.Sleep(600);
            Console.WriteLine("is suspend");
            resume.SuspendThread();
            Thread.Sleep(3600);
            Console.WriteLine("is resume");
            resume.ResumeThread();
            Thread.Sleep(1600);
        }

        private void StartThread()
        {
            newThread = new Thread(new ThreadStart(GenerateNum));
            newThread.Name = "Prime   Number   SemaphoreExample ";
            newThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            newThread.Start();
        }

        private void GenerateNum()
        {
            int i = 0;
            while (true)
            {
                Console.Write("@" + i + "@");
                Thread.Sleep(50);
                i++;
            }
        }

        private void ResumeThread()
        {
            if (newThread.ThreadState == System.Threading.ThreadState.Suspended ||
                newThread.ThreadState == System.Threading.ThreadState.SuspendRequested)
            {
                try
                {
                    newThread.Resume();
                }
                catch (ThreadStateException ex)
                {
                    Console.WriteLine("ResumeThread出现异常" + ex.Message);
                }
            }
        }

        public void SuspendThread()
        {
            try
            {
                Console.WriteLine("线程状态" + newThread.ThreadState);
               // if (newThread.ThreadState.CompareTo(System.Threading.ThreadState.Running) ==0 )

                     if (newThread.ThreadState == ThreadState.Running || newThread.ThreadState == ThreadState.WaitSleepJoin)
                    
                {
                    newThread.Suspend();
                }
                
            }
            catch (ThreadStateException   ex)
            {
                Console.WriteLine("SuspendThread出现异常" + ex.Message);
            }
        }
    }
}