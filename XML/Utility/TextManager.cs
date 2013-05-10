using System;

namespace Consoletest001.XML.Utility
{
    /// <summary>
    /// 时间、日期处理公共类
    /// </summary>
    public static class TextManager
    {
       
       /// <summary>
       /// 可以计算string的加法
       /// </summary>
       /// <param name="source">原数字字符</param>
       /// <param name="count">要加上的数</param>
        public static void AddForStr(ref string source ,int count )
        {
            int sourceTemp;
            if (string .IsNullOrEmpty(source))
            {
                source = "";
                return;
            }

            source = Int32.TryParse(source,out sourceTemp) ? (sourceTemp + count).ToString() : count.ToString();
        }

        /// <summary>
        /// 可以计算string的加法
        /// </summary>
        /// <param name="source">原数字字符</param>
        /// <param name="count">要加上的数</param>
        public static void AddForStr(ref string source, string count)
        {
            int sourceTemp;
            if (string.IsNullOrEmpty(source))
            {
                source = "";
                return;
            }

            source = Int32.TryParse(source, out sourceTemp) ? (sourceTemp + Int32.Parse(count)).ToString() : count.ToString();
        }

    }
}