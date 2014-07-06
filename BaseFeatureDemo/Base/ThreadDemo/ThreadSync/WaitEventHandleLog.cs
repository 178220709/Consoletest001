using System;
using System.Collections;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadSync
{
    internal class WaitEventHandleLog : IDisposable
    {
        private volatile bool _shouldStop; //用于控制线程正常结束的标志
        private const int _numberOfConsumer = 5; //消费者的数目
        //容器，一个只能容纳一块糖的糖盒子。PS：现在MS已经不推荐使用ArrayList，
        //支持泛型的List才是应该在程序中使用的，我这里偷懒，不想再去写一个Candy类了。
        private Queue _messageQueue;
        private EventWaitHandle _evntWtHndlProduced; //生产完成的事件，ManualReset，用于通知所有消费者生产完成
        private EventWaitHandle _evntWtHndlConsumeds; //消费完成的事件，AutoReset，每一个消费线程对应一个事件，用于通知生产者有消费动作完成

        /// <summary>
        /// 用于结束Produce()和Consume()在辅助线程中的执行
        /// </summary>
        public void StopThread()
        {
            _shouldStop = true;
            //叫醒阻塞中的消费者，让他们看到线程结束标志
            if (_evntWtHndlProduced != null)
            {
                _evntWtHndlProduced.Set();
            }

            //叫醒阻塞中的生产者，让他看到线程结束标志
            if (_evntWtHndlConsumeds != null)
            {
                
                    if (_evntWtHndlConsumeds != null)
                    {
                        _evntWtHndlConsumeds.Set();
                    }
                    
                
            }
        }

        /// <summary>
        /// 生产者的方法
        /// </summary>
        public void Produce()
        {
            if (_messageQueue == null)
            {
                Console.WriteLine("生产者：糖罐在哪里？！");
            }
            else if (_evntWtHndlConsumeds == null)
            {
                Console.WriteLine("生产者：消费者们在哪里？！");
            }
            else if (_evntWtHndlProduced == null) //这个事件用于唤醒所有消费者，因此象个喇叭
            {
                Console.WriteLine("生产者：喇叭坏啦，没办法通知消费者！");
            }
            else
            {
                //逐一检查消费者是否到位
          
                    if (_evntWtHndlConsumeds == null)
                    {
                        Console.WriteLine("生产者：消费者在哪里？！");
                        return;
                    }
                    else
                    {
                        //什么也不做
                    }
    
                while (!_shouldStop)
                {
                    lock (_messageQueue)
                    {
                        if (_messageQueue.Count > 0)
                        {
                            //通知消费者生产已完成
                            _evntWtHndlProduced.Set();
                            //只要有消费者吃完糖，就开始生产
                            WaitHandle.SignalAndWait(_evntWtHndlProduced, _evntWtHndlConsumeds);
                            Thread.Sleep(2000);
                        }

                    }
                    
                   
                }
                
                Console.WriteLine("生产者：下班啦！");
            }
        }

        public void addMessage(string str)
        {
            lock (_messageQueue)
            {
                _messageQueue.Enqueue(str);
            }
                //     this._evntWtHndlConsumeds.Reset();
            _evntWtHndlConsumeds.Set();
            _evntWtHndlConsumeds.Set();
        }

        /// <summary>
        /// 消费者的方法
        /// </summary>
        /// <param name="consumerIndex">消费者序号，用于表明使用哪个_EvntWtHndlConsumed成员</param>
        public void Consume()
        {
            int index = 0;
            if (_messageQueue == null)
            {
                Console.WriteLine("消费者{0}：糖罐在哪里？！", index);
            }
            else if (_evntWtHndlProduced == null)
            {
                Console.WriteLine("消费者{0}：生产者在哪里？！", index);
            }
            else if (_evntWtHndlConsumeds == null || _evntWtHndlConsumeds == null)
            {
                Console.WriteLine("消费者{0}：电话坏啦，没办法通知生产者！", index); //由于每个消费者都有一个专属事件通知生产者，因此相当于电话
            }
            else
            {
                while (!_shouldStop || _messageQueue.Count > 0)
                    //即便看到结束标致也应该把容器中的所有资源处理完毕再退出，否则容器中的资源可能就此丢失。需要指出_candybox.Count是有可能读到脏数据的
                {
                    Console.WriteLine("我被唤醒");
                    lock (_messageQueue)
                    {
                        while (_messageQueue.Count > 0)
                        {
                            Console.WriteLine(_messageQueue.Dequeue());
                        }
                    }
                    _evntWtHndlConsumeds.WaitOne();
                    
                }
            }
        }

        /// <summary>
        /// 初始化所需的各EventWaitHandle和糖罐等
        /// </summary>
        public void Initialize()
        {
            if (_messageQueue == null)
            {
                _messageQueue = new Queue();//按有多少消费者最多生产多少糖的标准初始化糖罐大小
            }
            else
            {
                //什么也不做
            }
            if (_evntWtHndlProduced == null)
            {
                _evntWtHndlProduced = new EventWaitHandle(false, EventResetMode.ManualReset);
            }
            else
            {
                //什么也不做
            }
            if (_evntWtHndlConsumeds == null)
            {
                _evntWtHndlConsumeds = new EventWaitHandle(false,EventResetMode.AutoReset);
            }
            else
            {
                //什么也不做
            }
        }

        public static void MainAlogsdfsdfS()
        {
            WaitEventHandleLog ss = new WaitEventHandleLog();
            try
            {
                ss.Initialize();
                //Start threads. 
                Console.WriteLine("开始启动线程，输入回车终止生产者和消费者的工作……\r\n******************************************");
                ss.addMessage("我的第0条消息");
              //  Thread thdProduce = new Thread(ss.Produce);
           //     thdProduce.Start();
                Thread thdConsume =  new Thread(ss.Consume);
                 thdConsume.Start();

                 ss.addMessage("我的第一条消息");
                 Console.ReadLine();
                ss.addMessage("我的第2条消息");
                Console.ReadLine();
                ss.addMessage("我的第3条消息");
                Console.ReadLine();
                ss.addMessage("我的第4条消息");
                ss._messageQueue.Enqueue("我的第4_2条消息");
                Console.ReadLine();
                ss.addMessage("我的第一5条消息");
                Console.ReadLine(); //通过IO阻塞主线程，等待辅助线程演示直到收到一个回车
                ss.StopThread(); //正常且优雅的结束生产者和消费者线程
             //   thdProduce.Join();
              
                    thdConsume.Join();
                
                Console.WriteLine("******************************************\r\n输入回车结束！");
                Console.ReadLine();
            }
            finally
            {
                ss.Dispose();
            }
            
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_messageQueue != null)
            {
                _messageQueue.Clear();
                _messageQueue = null;
            }
            else
            {
                //什么也不做
            }
            if (_evntWtHndlProduced != null)
            {
                _evntWtHndlProduced.Set();
                _evntWtHndlProduced.Close();
                _evntWtHndlProduced = null;
            }
            else
            {
                //什么也不做
            }
            if (_evntWtHndlConsumeds != null)
            {

                if (_evntWtHndlConsumeds != null)
                {
                    _evntWtHndlConsumeds.Set();
                    _evntWtHndlConsumeds.Close();
                    _evntWtHndlConsumeds = null;


                }
                _evntWtHndlConsumeds = null;
            }
            else
            {
                //什么也不做
            }
            
        }

        #endregion
    }
}