using System.Collections.Generic;
using System.Text;
using System;
using System.Data;

namespace Consoletest001.NewFolder1
{


    /// <summary>
    ///目的：通用转换类
    ///创建人：wdg
    ///创建日期：
    ///修改描述：
    ///修改人：
    ///修改日期：
    ///备注：
    /// </summary>
    public class ConvertEx2
    {
        private static UnicodeEncoding _unicode = new UnicodeEncoding();

        /// <summary>
        /// bool类型的值转换为Int类型
        /// </summary>
        /// <param name="bValue"></param>
        /// <returns></returns>
        public static int Bool2Int(bool bValue)
        {
            if (bValue == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// bool类型的值转换为Int类型
        /// </summary>
        /// <param name="nValue"> </param>
        /// <returns></returns>
        public static bool Int2Bool(int nValue)
        {
            if (nValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 转换一个字符串到整数。失败返回-9999
        /// </summary>
        /// <param name="sValue">字符串</param>
        /// <returns>返回整数</returns>
        public static int ConvertStr2Int(string sValue)
        {
            try
            {
                return Convert.ToInt32(sValue);
            }
            catch (Exception ex)
            {
                return -9999;
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static bool ToBoolean(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static DateTime ToDateTime(object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch (Exception ex)
            {
                return DateTime.Parse("0001-01-01");
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static Double ToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                return -9999;
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static int ToInt32(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static long ToInt64(object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// object值转换
        /// </summary>
        /// <param name="value"></param>
        public static string ToString(object value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string ToSize(double dataSize)
        {
            string dataSizeInfo = string.Empty;
            if (dataSize / 1024 / 1024 / 1024 > 1)
            {
                dataSizeInfo = Math.Round(dataSize / 1024 / 1024 / 1024, 2) + "G";
            }
            else if (dataSize / 1024 / 1024 > 1)
            {
                dataSizeInfo = Math.Round(dataSize / 1024 / 1024, 2) + "M";
            }
            else if (dataSize / 1024 > 1)
            {
                dataSizeInfo = Math.Round(dataSize / 1024, 2) + "KB";
            }
            else if (dataSize > 1)
            {
                dataSizeInfo = Math.Round(dataSize, 2) + "B";
            }
            else
            {
                dataSizeInfo = Math.Round(dataSize * 8, 2) + "b";
            }

            return dataSizeInfo;
        }


        public static Dictionary<string, DataTable> ToDicStrTable(Dictionary<string, object> dictionary)
        {
            Dictionary<string, DataTable> dicResult = new Dictionary<string, DataTable>();
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                dicResult.Add(keyValuePair.Key, keyValuePair.Value as DataTable);
            }
            return dicResult;
        }
    }
}
