//===============================================================================
//功    能： 数据加密工具类
//作    者：李勇
//创建日期：2005年10月20日
//修改历史
//修 改 人：
//修改日期：
//修改描述：
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaseFeatureDemo.Encrypt
{
    /// <summary>
    /// 数据加密工具类
    /// </summary>
    public class DataProtector
    {
        #region 构造函数
        public DataProtector() { }
        #endregion

        #region 自定义变量
        //密钥
        private const string keyString = "qadbA1mpYXw=";

        //向量
        private const string ivString = "nt+VPT5cb6M=";

        /// <summary>
        /// 密钥配置值-长度必须为8位的字符串
        /// </summary>
        private static string configKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["configKey"]));

        /// <summary>
        /// 向量配置值-长度必须为8位的字符串
        /// </summary>
        private static string configIV = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["configIV"]));
        private static Encoding myEncoding = Encoding.GetEncoding("utf-8");
        #endregion

        #region 加密和解密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="Value">加密前的字符串</param>
        /// <returns>加密后返回的字符串</returns>
        public static string EncryptString(string Value)
        {
            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();

            mCSP.Key = Convert.FromBase64String(keyString);
            mCSP.IV = Convert.FromBase64String(ivString);

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">解密前的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DecryptString(string Value)
        {
            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();
            mCSP.Key = Convert.FromBase64String(keyString);
            mCSP.IV = Convert.FromBase64String(ivString);

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());

        }

        public static string EncryptString(string Value, bool isDynamic)
        {

            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();

            mCSP.Key = isDynamic ? Convert.FromBase64String(configKey) : Convert.FromBase64String(keyString);
            mCSP.IV = isDynamic ? Convert.FromBase64String(configIV) : Convert.FromBase64String(ivString);

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

            byt = Encoding.UTF8.GetBytes(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }


        public static string DecryptString(string Value, bool isDynamic)
        {
            SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();
            mCSP.Key = isDynamic ? Convert.FromBase64String(configKey) : Convert.FromBase64String(keyString);
            mCSP.IV = isDynamic ? Convert.FromBase64String(configIV) : Convert.FromBase64String(ivString);
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

            byt = Convert.FromBase64String(Value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());

        }


        #endregion

        #region base64加密和解密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="value">加密前的字符串</param>
        /// <returns>加密后返回的字符串</returns>  
        public static string EncryptStringToBase64(string value)
        {
            byte[] myByte = myEncoding.GetBytes(value);
            string Base64Str;
            Base64Str = Convert.ToBase64String(myByte);
            return Base64Str;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="Value">解密前的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DecryptStringFromBase64(string value)
        {
            byte[] myByte = Convert.FromBase64String(value);
            string resultStr = myEncoding.GetString(myByte);
            return resultStr;

        }
        #endregion
    }
}
