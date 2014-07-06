using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.TestHelper;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadNew
{
    [TestClass]
    public class TaskDemo
    {
        [TestMethod]
        public  static void Main1()
        {
            Action<object> printMessage = o =>
            {
                Console.WriteLine(o);
                Thread.Sleep(200);
            };

            Task task1 = new Task(() =>
            {
                Console.WriteLine("Message: Say \"Hello\" from task1");
                Thread.Sleep(300);
            });
            Task task2 = new Task(printMessage,
            "Say \"Hello\" from task2");

            Task task3 = new Task(printMessage,
                "Say \"Hello\" from task3");

            Task task4 = new Task((obj) => Console.WriteLine("Message: " + obj),
                "Say \"Hello\" from task4");

            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();
            Console.Read();
        }

        static void ThreadInvoke(Object param)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Execute in ThreadInvoke");
                //每隔100毫秒，循环一次
                System.Threading.Thread.Sleep(100);
            }
        }


        public static void Main2()
        {
            var loop = 0;
            var task1 = new Task<int>(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    loop += i;
                    Thread.Sleep(5);
                }

                return loop;
            });
            task1.Start();
            var loopResut = task1.Result;
            var task2 = new Task<long>(obj =>
            {
                long res = 0;
                var looptimes = (int)obj;
                for (var i = 0; i < looptimes; i++)
                {
                    res += i;
                    Thread.Sleep(1);
                }
                return res;
            }, loopResut);

            task2.Start();
            var resultTask2 = task2.Result;

            Console.WriteLine("Task1's result:{0}\nTask2's result:{1}",
                loopResut,
                resultTask2);
            Console.ReadKey();
        }

        public static void Main3()
        {
            Random random = new Random();

            Action<object> doAction = (i) =>
            {
                Console.WriteLine( " Thread{0} is start at {1}   ",i, ConsoleTestHelper.GetCurrentTime());
                Thread.Sleep(random.Next(100,800));
                Console.WriteLine(" Thread{0} is over at {1}   ", i, ConsoleTestHelper.GetCurrentTime());
            };

            for (var i = 0; i < 16; i++)
            {
                Task task = new Task(doAction,i);
                Console.WriteLine(" MainThread is open task{0} at {1}   ", i, ConsoleTestHelper.GetCurrentTime());
                task.Start();
            }
            Thread.Sleep(20*1000);

        }


    }
}