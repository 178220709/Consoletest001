//===============================================================================
//��    �ܣ� ���ݼ��ܹ�����
//��    �ߣ�����
//�������ڣ�2005��10��20��
//�޸���ʷ
//�� �� �ˣ�
//�޸����ڣ�
//�޸�������
//===============================================================================

using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaseFeatureDemo.Encrypt
{
    /// <summary>
    /// ���ݼ��ܹ�����
    /// </summary>
    public class DataProtector
    {
        #region ���캯��
        public DataProtector() { }
        #endregion

        #region �Զ������
        //��Կ
        private const string keyString = "qadbA1mpYXw=";

        //����
        private const string ivString = "nt+VPT5cb6M=";

        /// <summary>
        /// ��Կ����ֵ-���ȱ���Ϊ8λ���ַ���
        /// </summary>
        private static string configKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["configKey"]));

        /// <summary>
        /// ��������ֵ-���ȱ���Ϊ8λ���ַ���
        /// </summary>
        private static string configIV = Convert.ToBase64String(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["configIV"]));
        private static Encoding myEncoding = Encoding.GetEncoding("utf-8");
        #endregion

        #region ���ܺͽ����ַ���
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="Value">����ǰ���ַ���</param>
        /// <returns>���ܺ󷵻ص��ַ���</returns>
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
        /// �����ַ���
        /// </summary>
        /// <param name="Value">����ǰ���ַ���</param>
        /// <returns>���ؽ��ܺ���ַ���</returns>
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

        #region base64���ܺͽ����ַ���
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="value">����ǰ���ַ���</param>
        /// <returns>���ܺ󷵻ص��ַ���</returns>  
        public static string EncryptStringToBase64(string value)
        {
            byte[] myByte = myEncoding.GetBytes(value);
            string Base64Str;
            Base64Str = Convert.ToBase64String(myByte);
            return Base64Str;
        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="Value">����ǰ���ַ���</param>
        /// <returns>���ؽ��ܺ���ַ���</returns>
        public static string DecryptStringFromBase64(string value)
        {
            byte[] myByte = Convert.FromBase64String(value);
            string resultStr = myEncoding.GetString(myByte);
            return resultStr;

        }
        #endregion
    }
}
