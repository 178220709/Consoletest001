﻿using System;
using System.Threading;

namespace BaseFeatureDemo
{
    class MutexTest
    {
        // Create a new Mutex. The creating thread does not own the
        // Mutex.
        private static Mutex mut = new Mutex();
        private const int numIterations = 2;
        private const int numThreads = 5;

      public   static void MainRTDFHSRSRDGSDGD()
        {
            // Create the threads that will use the protected resource.
            for (int i = 0; i < numThreads; i++)
            {
                Thread myThread = new Thread(new ThreadStart(MyThreadProc));
                myThread.Name = String.Format("Thread{0}", i + 1);
                myThread.Start();
            }

            // The main thread exits, but the application continues to
            // run until all foreground threads have exited.
        }

        private static void MyThreadProc()
        {
            for (int i = 0; i < numIterations; i++)
            {
                UseResource();
            }
        }

        // This method represents a resource that must be synchronized
        // so that only one thread at a time can enter.
        private static void UseResource()
        {
            // Wait until it is safe to enter.
            mut.WaitOne();

            Console.WriteLine("{0} has entered the protected area",
                              Thread.CurrentThread.Name);

            // Place code to access non-reentrant resources here.

            // Simulate some work.
            Thread.Sleep(500);

            Console.WriteLine("{0} is leaving the protected area\r\n",
                              Thread.CurrentThread.Name);

            // Release the Mutex.
            mut.ReleaseMutex();
        }
    }
}
