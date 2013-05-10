using System;

namespace Consoletest001.XML.Utility
{
    /// <summary>
    /// 时间、日期处理公共类
    /// </summary>
    public static class TimeManager
    {
        private static string _todayNumber;

        /// <summary>
        /// 当前时间的yyyymmdd
        /// </summary>
        public static string TodayNumber
        {
            get
            {
                DateTime dt = DateTime.Now;
                string str = dt.Year.ToString("0000") +
                             dt.Month.ToString("00") +
                             dt.Day.ToString("00");
                return str;
            }
        }

        /// <summary>
        /// 将一个时间格式化为yyyymmdd的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string TransDateTimeToTimeNumber(DateTime dt)
        {
            string str = dt.Year.ToString("0000") +
                         dt.Month.ToString("00") +
                         dt.Day.ToString("00");
            return str;
        }
    }
}