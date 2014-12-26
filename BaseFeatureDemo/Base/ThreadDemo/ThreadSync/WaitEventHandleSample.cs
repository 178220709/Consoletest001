using System;
using System.Collections;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadSync
{
    internal class WaitEventHandleSample : IDisposable
    {
        private volatile bool _shouldStop; //用于控制线程正常结束的标志
        private const int _numberOfConsumer = 5; //消费者的数目
        //容器，一个只能容纳一块糖的糖盒子。PS：现在MS已经不推荐使用ArrayList，
        //支持泛型的List才是应该在程序中使用的，我这里偷懒，不想再去写一个Candy类了。
        private ArrayList _candyBox;
        private EventWaitHandle _evntWtHndlProduced; //生产完成的事件，ManualReset，用于通知所有消费者生产完成
        private EventWaitHandle[] _evntWtHndlConsumeds; //消费完成的事件，AutoReset，每一个消费线程对应一个事件，用于通知生产者有消费动作完成

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
                for (int i = 0; i < _numberOfConsumer; i++)
                {
                    if (_evntWtHndlConsumeds[i] != null)
                    {
                        _evntWtHndlConsumeds[i].Set();
                    }
                    
                }
            }
        }

        /// <summary>
        /// 生产者的方法
        /// </summary>
        public void Produce()
        {
            if (_candyBox == null)
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
                for (int i = 0; i < _numberOfConsumer; ++i)
                {
                    if (_evntWtHndlConsumeds[i] == null)
                    {
                        Console.WriteLine("生产者：消费者{0}在哪里？！", i);
                        return;
                    }
                    else
                    {
                        //什么也不做
                    }
                    
                }


                while (!_shouldStop)
                {
                    lock (_candyBox)
                    {
                        if (_candyBox.Count < _numberOfConsumer)
                        {
                            int numberOfSugarProduced = 0; //本次一共生产了多少颗糖
                            while (_candyBox.Count < _numberOfConsumer) //一共有多少个消费者就生产多少块糖
                            {
                                //生产一块糖
                                _candyBox.Add("A Candy");
                                ++numberOfSugarProduced;
                            }
                            
                            Console.WriteLine("生产者：这次生产了{0}块糖，罐里现在一共有{1}块糖！", numberOfSugarProduced, _candyBox.Count);
                            Console.WriteLine("生产者：赶快来吃！！");
                        }
                        else //容器是满的
                        {
                            Console.WriteLine("生产者：糖罐是满的！");
                        }
                        
                    }
                    
                    //通知消费者生产已完成
                    _evntWtHndlProduced.Set();
                    //只要有消费者吃完糖，就开始生产
                    WaitHandle.WaitAny(_evntWtHndlConsumeds);
                    Thread.Sleep(2000);
                }
                
                Console.WriteLine("生产者：下班啦！");
            }
        }

        /// <summary>
        /// 消费者的方法
        /// </summary>
        /// <param name="consumerIndex">消费者序号，用于表明使用哪个_EvntWtHndlConsumed成员</param>
        public void Consume(object consumerIndex)
        {
            int index = (int) consumerIndex;
            if (_candyBox == null)
            {
                Console.WriteLine("消费者{0}：糖罐在哪里？！", index);
            }
            else if (_evntWtHndlProduced == null)
            {
                Console.WriteLine("消费者{0}：生产者在哪里？！", index);
            }
            else if (_evntWtHndlConsumeds == null || _evntWtHndlConsumeds[index] == null)
            {
                Console.WriteLine("消费者{0}：电话坏啦，没办法通知生产者！", index); //由于每个消费者都有一个专属事件通知生产者，因此相当于电话
            }
            else
            {
                while (!_shouldStop || _candyBox.Count > 0)
                    //即便看到结束标致也应该把容器中的所有资源处理完毕再退出，否则容器中的资源可能就此丢失。需要指出_candybox.Count是有可能读到脏数据的
                {
                    lock (_candyBox)
                    {
                        if (_candyBox.Count > 0)
                        {
                            if (!_shouldStop)
                            {
                                _candyBox.RemoveAt(0);
                                Console.WriteLine("消费者{0}：吃了1颗糖，还剩{1}颗！！", index, _candyBox.Count);
                                Console.WriteLine("消费者{0}：赶快生产！！", index);
                            }
                            else
                            {
                                Console.WriteLine("消费者{0}：我来把剩下的糖都吃了！", index);
                                while (_candyBox.Count > 0)
                                {
                                    _candyBox.RemoveAt(0);
                                    Console.WriteLine("消费者{0}：吃了1颗糖，还剩{1}颗！！", index, _candyBox.Count);
                                }
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("消费者{0}：糖罐是空的！", index);
                            Console.WriteLine("消费者{0}：赶快生产！！", index);
                        }
                    }
                    WaitHandle.SignalAndWait(_evntWtHndlConsumeds[index], _evntWtHndlProduced);
                    Thread.Sleep((index + 1)*1500);
                }
            }
            Console.WriteLine("消费者{0}：都吃光啦，下次再吃！", index);
        }

        /// <summary>
        /// 初始化所需的各EventWaitHandle和糖罐等
        /// </summary>
        public void Initialize()
        {
            if (_candyBox == null)
            {
                _candyBox = new ArrayList(_numberOfConsumer); //按有多少消费者最多生产多少糖的标准初始化糖罐大小
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
                _evntWtHndlConsumeds = new EventWaitHandle[_numberOfConsumer];
                for (int i = 0; i < _numberOfConsumer; ++i)
                {
                    _evntWtHndlConsumeds[i] = new EventWaitHandle(false, EventResetMode.AutoReset);
                }
            }
            else
            {
                //什么也不做
            }
        }

        public static void MainASDFasdfaDAFHADFS(string[] args)
        {
            WaitEventHandleSample ss = new WaitEventHandleSample();
            try
            {
                ss.Initialize();
                //Start threads. 
                Console.WriteLine("开始启动线程，输入回车终止生产者和消费者的工作……\r\n******************************************");
                Thread thdProduce = new Thread(ss.Produce);
                thdProduce.Start();
                Thread[] thdConsume = new Thread[_numberOfConsumer];
                for (int i = 0; i < _numberOfConsumer; ++i)
                {
                    thdConsume[i] = new Thread(ss.Consume);
                    thdConsume[i].Start(i);
                }
                Console.ReadLine(); //通过IO阻塞主线程，等待辅助线程演示直到收到一个回车
                ss.StopThread(); //正常且优雅的结束生产者和消费者线程
                thdProduce.Join();
                for (int i = 0; i < _numberOfConsumer; ++i)
                {
                    thdConsume[i].Join();
                }
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
            if (_candyBox != null)
            {
                _candyBox.Clear();
                _candyBox = null;
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
                for (int i = 0; i < _numberOfConsumer; ++i)
                {
                    if (_evntWtHndlConsumeds[i] != null)
                    {
                        _evntWtHndlConsumeds[i].Set();
                        _evntWtHndlConsumeds[i].Close();
                        _evntWtHndlConsumeds[i] = null;
                    }
                    
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