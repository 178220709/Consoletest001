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
           
            //����SHA1����
            SHA1 sha = new SHA1CryptoServiceProvider();

            //��mystrת����byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(_prikey);

            //Hash����
            byte[] dataHashed = sha.ComputeHash(dataToHash);
        }
    }
}
