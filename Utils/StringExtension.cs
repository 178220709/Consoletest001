using System;
using System.IO;
using System.Net;
using System.Text;

namespace Suijing.Utils
{
    public static class StringExtension
    {
        public static T As<T>(this object o)
        {
            T result = default(T);

            if (o != null)
            {
                try
                {
                    if (o is IConvertible)
                    {
                        result = (T)Convert.ChangeType(o, typeof(T));
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException(string.Format("无法将类型\"{0}\"转换为\"{1}\",值:{2}", o.GetType().FullName, typeof(T).FullName, (o != null) ? o.ToString() : "null"), ex);
                }
            }

            return result;
        }
        /// <summary>
        /// 格式化金额 保留两位小数
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string AsPrice(this decimal price)
        {
            var result = "0.00";

            if (price > 0)
            {
                result = price.ToString("0.00");
            }

            return result;
        } 
       
        public static string AsPrice(this decimal? price)
        {
            if (price.HasValue)
            {
                return price.Value.ToString("0.00");
            }

            return "";
        } 
        
        public static string GetResponse(this string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding encode = Encoding.UTF8;
                StreamReader reader = new StreamReader(stream, encode);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
