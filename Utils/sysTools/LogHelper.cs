using System;
using System.IO;

namespace Suijing.Utils.sysTools
{
    public class LogHelper
    {
        private static readonly log4net.ILog logdebugger = log4net.LogManager.GetLogger("logdebugger");

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        private static readonly log4net.ILog logWebReader = log4net.LogManager.GetLogger("logWebReader");

        public static void SetConfig()
        {
            const string TestPath = @"D:\code\GitCode\HelloCSharp\JsonSong.ManagerUI\WEB.config";
            if (File.Exists(TestPath))
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(TestPath));
                return;
            }
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }

        public static void Info(string info)
        {
            if (logdebugger.IsDebugEnabled)
            {
                logdebugger.Debug(info);
            }
        }

        public static void Error(string info, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }

        public static void WriteWebReader(string info)
        {
            if (logWebReader.IsDebugEnabled)
            {
                logWebReader.Debug(info);
            }
        }

       
        private void ChangeLog4netLogFileName(log4net.ILog iLog, string fileName)
        {
            var logImpl = iLog as log4net.Core.LogImpl;
            if (logImpl != null)
            {
                log4net.Appender.AppenderCollection ac = ((log4net.Repository.Hierarchy.Logger)logImpl.Logger).Appenders;
                for (int i = 0; i < ac.Count; i++)
                {    //这里我只对RollingFileAppender类型做修改
                    log4net.Appender.RollingFileAppender rfa = ac[i] as log4net.Appender.RollingFileAppender;
                    if (rfa != null)
                    {
                        rfa.File = fileName;
                        if (!System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Create(fileName);
                        }
                        //更新Writer属性
                        rfa.Writer = new System.IO.StreamWriter(rfa.File, rfa.AppendToFile, rfa.Encoding);
                    }
                }
            }
        }



    }
}
