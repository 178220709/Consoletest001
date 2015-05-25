using System;

namespace Suijing.Utils.Utility
{
    public static class DataConvertUtil
    {
        public static string SafeToString(this object obj)
        {
            string val = string.Empty;

            if (obj != null)
            {
                val = obj.ToString().Trim();
            }

            return val;
        }

        /// <summary>
        /// 将字符串转成int 型。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int SafeParseInt(this object obj, int defaultValue = 0)
        {
            if (null == obj)
            {
                return defaultValue;
            }
            int.TryParse(obj.ToString(), out defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转成long 型。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long SafeParseLong(this object obj, long defaultValue = 0L)
        {
            if (null == obj)
            {
                return defaultValue;
            }
            long.TryParse(obj.ToString(), out defaultValue);
            return defaultValue;
        }

        public static double SafeParseDouble(this object obj, double defaultValue = 0D)
        {
            if (null == obj)
            {
                return defaultValue;
            }
            double.TryParse(obj.ToString(), out defaultValue);
            return defaultValue;
        }


        public static float SafeParseFloat(this object obj, float defaultValue = 0F)
        {
            if (null == obj)
            {
                return defaultValue;
            }
            float.TryParse(obj.ToString(), out defaultValue);
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转成decimal 型。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal SafeParseDecimal(this object obj, decimal defaultValue = 0)
        {
            if (null == obj)
            {
                return 0;
            }
            decimal.TryParse(obj.ToString(), out defaultValue);
            return defaultValue;

        }

        public static bool SafeParseBoolean(this object obj)
        {
            return Convert.IsDBNull(obj) ? false : Convert.ToBoolean(obj);
        }


        public static long LongTryParse(this string pram)
        {
            long result = 0;
            long.TryParse(pram, out result);

            return result;
        }



    }

}
