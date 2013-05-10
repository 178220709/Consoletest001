using System;

namespace Consoletest001.XML.Utility
{
    /// <summary>
    /// ʱ�䡢���ڴ�������
    /// </summary>
    public static class TimeManager
    {
        private static string _todayNumber;

        /// <summary>
        /// ��ǰʱ���yyyymmdd
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
        /// ��һ��ʱ���ʽ��Ϊyyyymmdd���ַ���
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