using System;
using System.Collections.Generic;
using System.Threading;

namespace BaseFeatureDemo
{

    /// <summary>
    /// 线程执行完毕事件的参数，记录了是哪一个线程执行完毕
    /// </summary>
    public class ThreadOverEventArgs : EventArgs
    {
        public  ThreadOverEventArgs(string index)
        {
            _overThreadIndex = index;
        }
        private readonly string _overThreadIndex;
        public string OverThreadIndex
        {
            get { return _overThreadIndex; }
        }
    }

    /// <summary>
    /// 1.3.1 归档数据监控
    /// 创建M条归档线程；数据库中循环读取待归档数据表，
    /// 每次读取N条数据待归档数据，并将N条待归档数据分配到归档线程中
    /// 归档任务监控线程和应用程序有相同的生命周期。
    /// </summary>
    public class ArchiveControler
    {

        # region 初始化监控环境

        private delegate void ThreadOverEventHandler(object serder, ThreadOverEventArgs e);

        private event ThreadOverEventHandler ThreadOver;

        private readonly int _threadCount;
        private readonly int _archiveCount;

        private SortedDictionary<string, Thread> _lstThreads = new SortedDictionary<string, Thread>();
        //用于保存线程运行需要的参数，必须加锁同步
        private Stack<object> _archiveDatas = new Stack<object>();
        //    private ProcessTest.Loger _loger = new ProcessTest.Loger();

        /// <summary>
        /// 归档数据监控 构造函数 
        /// </summary>
        /// <param name="threadCount"> M ： 开辟的线程数量</param>
        /// <param name="archiveCount">N ： 待归档数据数量</param>
        public ArchiveControler(int threadCount, int archiveCount)
        {
            if (threadCount < archiveCount)
            {
                this._threadCount = threadCount;
                this._archiveCount = archiveCount;
            }
            else
            {
                this._threadCount = threadCount;
                this._archiveCount = threadCount;
            }

        }

        public void StartWork()
        {
            InitlstThreads();
            TakeOutDatas();
            ThreadOver += new ThreadOverEventHandler(ThreadIsOver);
            PutDatasToThreads();
        }

        /// <summary>
        /// 创建M个线程
        /// </summary>
        private void InitlstThreads()
        {
            for (int i = 0; i < _threadCount; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DoWork));
                thread.Name = string.Format("thread{0}", i);
                _lstThreads.Add(string.Format("thread{0}", i), thread);
            }
        }

        /// <summary>
        /// 初始化的时候，将数据放入线程并启动
        /// </summary>
        private void PutDatasToThreads()
        {
            for (int i = 0; i < _threadCount; i++)
            {
                _lstThreads[string.Format("thread{0}", i)].Start(_archiveDatas.Pop());
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 线程执行完毕之后执行的函数，和ThreadOver绑定
        /// </summary>
        /// <param name="serder"></param>
        /// <param name="e">记录了是哪一个线程</param>
        private void ThreadIsOver(object serder, ThreadOverEventArgs e)
        {
            lock (_archiveDatas)
            {
                if (_archiveDatas.Count == 0)
                {
                    TakeOutDatas();
                }
                _lstThreads[e.OverThreadIndex] = new Thread(DoWork);
                _lstThreads[e.OverThreadIndex].Name = e.OverThreadIndex;
                _lstThreads[e.OverThreadIndex].Start(_archiveDatas.Pop());
            }
        }

        /// <summary>
        /// 从数据源中取出N条记录，
        /// </summary>
        private void TakeOutDatas()
        {
            //Thread.Sleep(800);
            for (int i = 0; i < _archiveCount; i++)
            {
                _archiveDatas.Push(new object());
            }
        }

        #endregion

        public static int SignLock = 0;
        public static int SignUnLock = 0;
        private static readonly object LockThis = new object();

        /// <summary>
        /// 归档执行函数,此函数体内部所有代码都运行在线程队列的子线程环境中
        /// </summary>
        /// <param name="param">新线程所需参数</param>
        private void DoWork(object param)
        {
            int n = 0;
            if (param is int )
            {
                 n = (int)param;
            }
            

            //执行完毕后，触发线程做完事件，
            ThreadOver(Thread.CurrentThread, new ThreadOverEventArgs(Thread.CurrentThread.Name));

        }

    }
}
