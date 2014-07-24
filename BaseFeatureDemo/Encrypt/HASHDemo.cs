using System;
using System.Security.Cryptography;
using System.Text;

namespace BaseFeatureDemo.Encrypt
{
  public    class HASHDemo
    {
        private static string _pubkey = "";
        private static string _prikey = "suijing";

      
        public static void mainsdfsfd()
        {
           
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将mystr转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(_prikey);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
        }
    }
}
