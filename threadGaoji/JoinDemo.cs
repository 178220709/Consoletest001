using System;
using System.Threading;

namespace Consoletest001
{
    class JoinDemo
    {
     public     static void MainWERASG()
        {
            Thread.CurrentThread.Name = "MainThread";

            Thread newThread = new Thread(new ThreadStart(ThreadFuncOne));
            newThread.Name = "NewThread";

          newThread.Start();
            for (int j = 0; j < 100; j++)
            {
                if (j == 30)
                {
              //      newThread.Start();
                    newThread.Join();
                }
                else
                {
                Console.WriteLine(Thread.CurrentThread.Name + "   j =  " + j);
                }
            }
            Console.Read();

        }
        private static void ThreadFuncOne()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(Thread.CurrentThread.Name + "   i =  " + i);
            }
            Console.WriteLine(Thread.CurrentThread.Name + " has finished");
        }
    }

}
