using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace BaseFeatureDemo.jiami
{
    class RSAManager
    {
        private string _pubkey = "";
        private string _prikey = "";

        public  RSAManager()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // 公钥  
                _pubkey = rsa.ToXmlString(false);

                // 私钥  
                _prikey = rsa.ToXmlString(true);
            }
        }

        private string  encrypt( string  sourceStr){

   
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_prikey);
                // 加密对象  
                RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsa);
                f.SetHashAlgorithm("SHA1");
                byte[] source = System.Text.ASCIIEncoding.ASCII.GetBytes(sourceStr);
                SHA1Managed sha = new SHA1Managed();
                byte[] result = sha.ComputeHash(source);

                byte[] b = f.CreateSignature(result);

                return Convert.ToBase64String(b);
            } 

        }

        public static void mainsdfsfd()
        {
            RSAManager rm = new RSAManager();
            string str = rm.encrypt("2013-04-15");
        }
    }
}
