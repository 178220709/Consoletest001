using System;
using System.Security.Cryptography;
using System.Text;

namespace BaseFeatureDemo.Encrypt
{
    public enum DiscuzAuthcodeMode { Encode, Decode };

    /// <summary>
    /// C#�汾 discuz authcode�������������ϰ汾��������expiry��Ч���⡣
    /// </summary>
    public class Authcode
    {
        private static Encoding encoding = Encoding.GetEncoding("gbk");

        public Authcode()
        {
        }

        /// <summary>
        /// ���ַ�����ָ��λ�ý�ȡָ�����ȵ����ַ���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <param name="length">���ַ����ĳ���</param>
        /// <returns>���ַ���</returns>
        private static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }

                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// ���ַ�����ָ��λ�ÿ�ʼ��ȡ���ַ�����β���˷���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <returns>���ַ���</returns>
        private static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="str">ԭʼ�ַ���</param>
        /// <returns>MD5���</returns>
        public static string MD5(string str)
        {
            byte[] b = encoding.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }

        /// <summary>
        /// ���� RC4 ��������
        /// </summary>
        /// <param name="pass">�����ִ�</param>
        /// <param name="kLen">��Կ���ȣ�һ��Ϊ 256</param>
        /// <returns></returns>
        private static Byte[] GetKey(Byte[] pass, Int32 kLen)
        {
            Byte[] mBox = new Byte[kLen];

            for (Int64 i = 0; i < kLen; i++)
            {
                mBox[i] = (Byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++)
            {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }

        /// <summary>
        /// ��������ַ�
        /// </summary>
        /// <param name="lens">����ַ�����</param>
        /// <returns>����ַ�</returns>
        private static string RandomString(int lens)
        {
            char[] CharArray = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int clens = CharArray.Length;
            string sCode = "";
            Random random = new Random();
            for (int i = 0; i < lens; i++)
            {
                sCode += CharArray[random.Next(clens)];
            }
            return sCode;
        }

        /// <summary>
        /// ʹ�� authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <param name="expiry">�����ִ���Чʱ�䣬��λ����</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeEncode(string source, string key, int expiry)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Encode, expiry);

        }

        /// <summary>
        /// ʹ�� Discuz authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeEncode(string source, string key)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Encode, 0);

        }

        /// <summary>
        /// ʹ�� Discuz authcode �������ַ�������
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <returns>���ܽ��</returns>
        public static string DiscuzAuthcodeDecode(string source, string key)
        {
            return DiscuzAuthcode(source, key, DiscuzAuthcodeMode.Decode, 0);

        }

        /// <summary>
        /// ʹ�� ���ε� rc4 ���뷽�����ַ������м��ܻ��߽���
        /// </summary>
        /// <param name="source">ԭʼ�ַ���</param>
        /// <param name="key">��Կ</param>
        /// <param name="operation">���� ���ܻ��ǽ���</param>
        /// <param name="expiry">������Ч��, ����ʱ����Ч�� �� λ �룬0 Ϊ������Ч</param>
        /// <returns>���ܻ��߽��ܺ���ַ���</returns>
        private static string DiscuzAuthcode(string source, string key, DiscuzAuthcodeMode operation, int expiry)
        {
            if (source == null || key == null)
            {
                return "";
            }

            int ckey_length = 4;
            string keya, keyb, keyc, cryptkey, result;

            key = MD5(key);
            keya = MD5(CutString(key, 0, 16));
            keyb = MD5(CutString(key, 16, 16));
            keyc = ckey_length > 0 ? (operation == DiscuzAuthcodeMode.Decode ? CutString(source, 0, ckey_length) : RandomString(ckey_length)) : "";

            cryptkey = keya + MD5(keya + keyc);

            if (operation == DiscuzAuthcodeMode.Decode)
            {
                byte[] temp;
                try
                {
                    temp = System.Convert.FromBase64String(CutString(source, ckey_length));
                }
                catch
                {
                    try
                    {
                        temp = System.Convert.FromBase64String(CutString(source + "=", ckey_length));
                    }
                    catch
                    {
                        try
                        {
                            temp = System.Convert.FromBase64String(CutString(source + "==", ckey_length));
                        }
                        catch
                        {
                            return "";
                        }
                    }
                }

                result = encoding.GetString(RC4(temp, cryptkey));

                long timestamp = long.Parse(CutString(result, 0, 10));

                if ((timestamp == 0 || timestamp - UnixTimestamp() > 0) && CutString(result, 10, 16) == CutString(MD5(CutString(result, 26) + keyb), 0, 16))
                {
                    return CutString(result, 26);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                source = (expiry == 0 ? "0000000000" : (expiry + UnixTimestamp()).ToString()) + CutString(MD5(source + keyb), 0, 16) + source;
                byte[] temp = RC4(encoding.GetBytes(source), cryptkey);
                return keyc + System.Convert.ToBase64String(temp);
            }
        }

        /// <summary>
        /// RC4 ԭʼ�㷨
        /// </summary>
        /// <param name="input">ԭʼ�ִ�����</param>
        /// <param name="pass">��Կ</param>
        /// <returns>�������ִ�����</returns>
        private static Byte[] RC4(Byte[] input, String pass)
        {
            if (input == null || pass == null) return null;

            byte[] output = new Byte[input.Length];
            byte[] mBox = GetKey(encoding.GetBytes(pass), 256);

            // ����
            Int64 i = 0;
            Int64 j = 0;
            for (Int64 offset = 0; offset < input.Length; offset++)
            {
                i = (i + 1) % mBox.Length;
                j = (j + mBox[i]) % mBox.Length;
                Byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
                Byte a = input[offset];
                //Byte b = mBox[(mBox[i] + mBox[j] % mBox.Length) % mBox.Length];
                // mBox[j] һ���� mBox.Length С������Ҫ��ȡģ
                Byte b = mBox[(mBox[i] + mBox[j]) % mBox.Length];
                output[offset] = (Byte)((Int32)a ^ (Int32)b);
            }

            return output;
        }


        private static string AscArr2Str(byte[] b)
        {
            return System.Text.UnicodeEncoding.Unicode.GetString(
             System.Text.ASCIIEncoding.Convert(System.Text.Encoding.ASCII,
             System.Text.Encoding.Unicode, b)
             );
        }

        public static long UnixTimestamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            return long.Parse(timeStamp.Substring(0, timeStamp.Length - 7));
        }

        public static string urlencode(string str)
        {
            //php��urlencode��ͬ��HttpUtility.UrlEncode
            //return HttpUtility.UrlEncode(str);

            string tmp = string.Empty;
            string strSpecial = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string crt = str.Substring(i, 1);
                if (strSpecial.Contains(crt))
                    tmp += crt;
                else
                {
                    byte[] bts = encoding.GetBytes(crt);
                    foreach (byte bt in bts)
                    {
                        tmp += "%" + bt.ToString("X");
                    }
                }
            }
            return tmp;
        }

        public static long time()
        {
            TimeSpan ts = new TimeSpan(System.DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }


        /// <summary>
        /// ���������SHA1����
        /// </summary>
        /// <param name="originString"></param>
        /// <returns></returns>
        public static string ConvertToSHA1(string originString)
        {
            //����ת����SHA1��ʽ
            SHA1 sha = new SHA1CryptoServiceProvider();
            //������ת����byte[]��
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(originString);
            //Hash����
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //�����ת����string
            string hashedString = BitConverter.ToString(dataHashed).Replace("-", "");

            return hashedString.ToLower();
        }
    }
}