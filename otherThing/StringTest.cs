using System;
using System.Threading;

namespace Consoletest001.otherThing
{
     public class StringTest
    {

        public static  void mainsdf()
        {
            string str = "qw\\er/ty";
            long ser = long.MinValue; //-2147483648
            string s1, s2, s3, s4, s5, s6;
            s1 = str.TrimEnd('t');
            s2 = str.Replace("e", "ert");
            s3 = str.TrimEnd('y');
            char[] ee = "123".ToCharArray();
            byte[] byteArray = System.Text.UTF8Encoding.Default.GetBytes("我");
            byte[] bb = System.Text.Encoding.Default.GetBytes("我是");
            s2 = System.Text.Encoding.Default.GetString(byteArray);
            s2 = System.Text.Encoding.Default.GetString(bb);

        }
           
        public StringTest()
        {
            
        }

    
    }
}