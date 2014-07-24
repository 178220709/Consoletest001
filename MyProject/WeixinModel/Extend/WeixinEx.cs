using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MyProject.WeixinModel.Model;

namespace MyProject.WeixinModel.Extend
{
    public  static  class WeixinEx 
    {
        public static bool CheckSignature(this CheckModel model)
        {
            var result = false;
            try
            {
                var list = new List<string> { model.timestamp, model.nonce, WeixinConstants.MyToken };
                list = list.OrderBy(t => t).ToList();
                var psw = list[0] + list[1] + list[2];
                //   var hashStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(psw, "SHA1");
                var hashStr = psw.ToHashStr();
                result = model.signature.Equals(hashStr, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
            return result;
        }

        public static string ToHashStr(this string str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            return BitConverter.ToString(dataHashed).Replace("-", "");

        }

        public static DateTime ToDateTime(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        public static string ToTimeInt(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (time - startTime).TotalSeconds.ToString();
        }

        public static string ToXmlStr(this ReceiveBase model)
        {
            return "xml";
        }

       
    }
}
