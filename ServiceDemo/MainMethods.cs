using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.IO;
using PriceIndex.BackService;


namespace PriceIndex.BackService
{
    public static class MainMethods
    {
        //是否执行生成今日数据操作，0：未执行，1：已执行
        public static int isRun = 0;
        public static log4net.ILog _Log;
        static MainMethods()
        {
            _Log = log4net.LogManager.GetLogger("MyLogger");
            _Log.Info( " 日志组建初始化完成  "); 
        }

        public static void CreatTodayData()
        {
            _Log.Info(" service 逻辑执行一次 : 距离下次更新时间为：" + ConfigurationManager.AppSettings["timeForMinute"]); 
           
        }
    }
}
