using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Consoletest001
{
    /// <summary>
    /// 本地文件日志记录类
    /// 创建人:李圣虎
    /// 创建时间:2009-11-11
    /// 高效率多线程写系统本地文件日志
    /// </summary>
    public class Loger:IDisposable
    {

        #region 私有变量
        private  Queue<Msg> msgs;
        private readonly string _path;
        private volatile bool _state;
        private  LogType _type;
        private DateTime _timeSign;
        private StreamWriter _writer;
        #endregion
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logPath"></param>
        public Loger(string logPath):this(logPath,LogType.Daily)
        {    
        }
        public Loger()
            : this(System.Windows.Forms.Application.StartupPath + @"\LOG", LogType.Daily) 
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="logType"></param>
        public Loger(string logPath, LogType logType)
        {
            _state = true;
            _path = logPath;
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            _type = logType;
            msgs = new Queue<Msg>();
            Thread thread = new Thread(work);
            thread.Start();
            this.Write(new Msg("------", MsgType.Unknown));
        }
		#region 私有函数
		private void work()
        {
            while (true)
            {
                if (msgs.Count > 0)
                {
                    Msg msg = null;
                    lock (msgs)
                    {
                        msg = msgs.Dequeue();
                    }
                    if (msg != null)
                    {
                        FileWrite(msg);
                    }
                }
                else
                {
                    if (_state)
                    {
                        Thread.Sleep(50);
                    }
                    else
                    {
                        FileClose();
                        return;
                    }
                }
            }
        }
        private string GetFilename()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (_type)
            {
                case LogType.Hour:
                    _timeSign = new DateTime(now.Year, now.Month, now.Day, now.Hour,0, 0);
                    _timeSign = _timeSign.AddHours(1);
                    format = "yyyyMMddHH'.log'";
                    break;
                case LogType.Daily:
                    _timeSign = new DateTime(now.Year, now.Month, now.Day);
                    _timeSign = _timeSign.AddDays(1);
                    format = "yyyyMMdd'.log'";
                    break;
                case LogType.Weekly:
                    _timeSign = new DateTime(now.Year, now.Month, now.Day);
                    _timeSign = _timeSign.AddDays(7);
                    format = "yyyyMMdd'.log'";
                    break;
                case LogType.Monthly:
                    _timeSign = new DateTime(now.Year, now.Month, 1);
                    _timeSign = _timeSign.AddMonths(1);
                    format = "yyyyMM'.log'";
                    break;
                case LogType.Annually:
                    _timeSign = new DateTime(now.Year, 1, 1);
                    _timeSign = _timeSign.AddYears(1);
                    format = "yyyy'.log'";
                    break;
            }
            return now.ToString(format);
        }
        private void FileWrite(Msg msg)
        {
            try
            {
                if (_writer == null)
                {
                    FileOpen();
                }
                else
                {
                    if (DateTime.Now >= _timeSign)
                    {
                        FileClose();
                        FileOpen();
                    }
                    _writer.Write(msg.Datetime);
                    _writer.Write('\t');
                    _writer.Write(msg.Type);
                    _writer.Write('\t');
                    _writer.WriteLine(msg.Text);
                    _writer.Flush();
                }
            }
            catch  
            {
                throw;
            }
        }
        private void FileOpen()
        {
            string pathYear = DateTime.Now.ToString("yyyy-MM");
            string pathDay = DateTime.Now.ToString("yyyy-MM-dd");
            string pathDate = Path.Combine(pathYear,pathDay);
            string pathNow = Path.Combine(_path,pathDate);
            if (!Directory.Exists(pathNow))
            {
                Directory.CreateDirectory(pathNow);
            }
            _writer = new StreamWriter(Path.Combine(pathNow ,GetFilename()), true, Encoding.UTF8);
			_writer.AutoFlush = true;
        }
        private void FileClose()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
		}
		#endregion

		#region 公有函数
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg"></param>
		public void Write(Msg msg)
        {
            if (msg != null)
            {
                lock (msgs)
                {
                    msgs.Enqueue(msg);
                }
            }
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public void Write(string text, MsgType type)
        {
            Write(new Msg(text, type));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="text"></param>
        /// <param name="type"></param>
        public void Write(DateTime dt, string text, MsgType type)
        {
            Write(new Msg(dt, text, type));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="e"></param>
        /// <param name="type"></param>
        public void Write(Exception e, MsgType type)
        {
            Write(new Msg(e.Message, type));
        }
        /// <summary>
        /// 注销
        /// </summary>
        public void Dispose()
        {
            _state = false;
		}
		#endregion
	} 
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 每小时记录一个文件
        /// </summary>
        Hour,
        /// <summary>
        /// 每天时记录一个文件
        /// </summary>
        Daily,
        /// <summary>
        /// 每周时记录一个文件
        /// </summary>
        Weekly,
        /// <summary>
        /// 每月时记录一个文件
        /// </summary>
        Monthly,
        /// <summary>
        /// 每年时记录一个文件
        /// </summary>
        Annually
    }
    /// <summary>
    /// 日志消息类
    /// </summary>
    public class Msg
    {
        #region 变量
        private DateTime datetime;
        private string text;
        private MsgType type;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Msg(): this("", MsgType.Unknown)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        public Msg(string t, MsgType p): this(DateTime.Now, t, p)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="t"></param>
        /// <param name="p"></param>
        public Msg(DateTime dt, string t, MsgType p)
        {
            datetime = dt;
            type = p;
            text = t;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion

        #region 重载方法
        /// <summary>
        /// 重载
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return datetime.ToString() + "\t" + text + "\n";
        }
        #endregion
    }
    /// <summary>
    /// 日志消息类型枚举
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown,
        /// <summary>
        /// 消息类型
        /// </summary>
        Information,
        /// <summary>
        /// 警告类型
        /// </summary>
        Warning,
        /// <summary>
        /// 错误类型
        /// </summary>
        Error,
        /// <summary>
        /// 成功类型
        /// </summary>
        Success
    }
}
