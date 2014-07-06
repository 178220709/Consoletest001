using System;
using System.Collections;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadSync
{
    public class MonitorSample
    {
        //容器，一个只能容纳一块糖的糖盒子。PS：现在MS已经不推荐使用ArrayList，
        //支持泛型的List才是应该在程序中使用的，我这里偷懒，不想再去写一个Candy类了。
        private ArrayList _candyBox = new ArrayList(1);
        private volatile bool _shouldStop = false; //用于控制线程正常结束的标志

        /// <summary>
        /// 用于结束Produce()和Consume()在辅助线程中的执行
        /// </summary>
        public void StopThread()
        {
            _shouldStop = true;
            //这时候生产者/消费者之一可能因为在阻塞中而没有机会看到结束标志，
            //而另一个线程顺利结束，所以剩下的那个一定长眠不醒，需要我们在这里尝试叫醒它们。
            //不过这并不能确保线程能顺利结束，因为可能我们刚刚发送信号以后，线程才阻塞自己。
            Monitor.Enter(_candyBox);
            try
            {
                Monitor.PulseAll(_candyBox);
            }
            finally
            {
                Monitor.Exit(_candyBox);
            }
        }

        /// <summary>
        /// 生产者的方法
        /// </summary>
        public void Produce()
        {
            while (!_shouldStop)
            {
                Monitor.Enter(_candyBox);
                try
                {
                    if (_candyBox.Count == 0)
                    {
                        _candyBox.Add("A candy");
                        Console.WriteLine("生产者：有糖吃啦！");
                        //唤醒可能现在正在阻塞中的消费者
                        Monitor.Pulse(_candyBox);
                        Console.WriteLine("生产者：赶快来吃！！");
                        //调用Wait方法释放对象上的锁，并使生产者线程状态转为WaitSleepJoin，阻止该线程被CPU调用（跟Sleep一样）
                        //直到消费者线程调用Pulse(_candyBox)使该线程进入到Running状态
                        Monitor.Wait(_candyBox);
                    }
                    else //容器是满的
                    {
                        Console.WriteLine("生产者：糖罐是满的！");
                        //唤醒可能现在正在阻塞中的消费者
                        Monitor.Pulse(_candyBox);
                        //调用Wait方法释放对象上的锁，并使生产者线程状态转为WaitSleepJoin，阻止该线程被CPU调用（跟Sleep一样）
                        //直到消费者线程调用Pulse(_candyBox)使生产者线程重新进入到Running状态，此才语句返回
                        Monitor.Wait(_candyBox);
                    }
                }
                finally
                {
                    Monitor.Exit(_candyBox);
                }
                Thread.Sleep(2000);
            }
            Console.WriteLine("生产者：下班啦！");
        }

        /// <summary>
        /// 消费者的方法
        /// </summary>
        public void Consume()
        {
            //即便看到结束标致也应该把容器中的所有资源处理完毕再退出，否则容器中的资源可能就此丢失
            //不过这里_candyBox.Count是有可能读到脏数据的，好在我们这个例子中只有两个线程所以问题并不突出
            //正式环境中，应该用更好的办法解决这个问题。
            while (!_shouldStop || _candyBox.Count > 0)
            {
                Monitor.Enter(_candyBox);
                try
                {
                    if (_candyBox.Count == 1)
                    {
                        _candyBox.RemoveAt(0);
                        if (!_shouldStop)
                        {
                            Console.WriteLine("消费者：糖已吃完！");
                        }
                        else
                        {
                            Console.WriteLine("消费者：还有糖没吃，马上就完！");
                        }
                        //唤醒可能现在正在阻塞中的生产者
                        Monitor.Pulse(_candyBox);
                        Console.WriteLine("消费者：赶快生产！！");
                        Monitor.Wait(_candyBox);
                    }
                    else
                    {
                        Console.WriteLine("消费者：糖罐是空的！");
                        //唤醒可能现在正在阻塞中的生产者
                        Monitor.Pulse(_candyBox);
                        Monitor.Wait(_candyBox);
                    }
                }
                finally
                {
                    Monitor.Exit(_candyBox);
                }
                Thread.Sleep(2000);
            }
            Console.WriteLine("消费者：都吃光啦，下次再吃！");
        }

        public static void MainASDFBSSVDF(string[] args)
        {
            MonitorSample ss = new MonitorSample();
            Thread thdProduce = new Thread(new ThreadStart(ss.Produce));
            Thread thdConsume = new Thread(new ThreadStart(ss.Consume));
            //Start threads. 
            Console.WriteLine("开始启动线程，输入回车终止生产者和消费者的工作……\r\n******************************************");
            thdProduce.Start();
            Thread.Sleep(2000); //尽量确保生产者先执行
            thdConsume.Start();
            Console.ReadLine(); //通过IO阻塞主线程，等待辅助线程演示直到收到一个回车
            ss.StopThread(); //正常且优雅的结束生产者和消费者线程
            Thread.Sleep(1000); //等待线程结束
            while (thdProduce.ThreadState != ThreadState.Stopped)
            {
                ss.StopThread(); //线程还没有结束有可能是因为它本身是阻塞的，尝试使用StopThread()方法中的PulseAll()唤醒它，让他看到结束标志
                thdProduce.Join(1000); //等待生产这线程结束
            }
            while (thdConsume.ThreadState != ThreadState.Stopped)
            {
                ss.StopThread();
                thdConsume.Join(1000); //等待消费者线程结束
            }
            Console.WriteLine("******************************************\r\n输入回车结束！");
            Console.ReadLine();
        }
    }
}