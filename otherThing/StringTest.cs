using System;
using System.Collections;
using System.Security.Policy;
using System.Threading;
using System.Linq;

namespace Consoletest001.otherThing
{
    public class StringTest
    {

        public static void mainsdf()
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

        #region  hashtable是值类型还是引用类型

        /// <summary>
        /// hashtable是值类型还是引用类型
        /// </summary>
        public static void hashtableTest()
        {
            System.Collections.Hashtable table = new Hashtable();
            table.Add("1","1231");
            hashtableTest2(table);
            table.Add("4", "1234");
        }

        private static void hashtableTest2( Hashtable tb)
        {
            tb.Add("2","1232");
            tb.Add("3", "1233");
        }
        //结果 ： 是引用类型
        #endregion

        //两个关键点 str1 == obj1 true 会自动将obj装箱 比较值
        // object obj2 = new object();  obj2 = "123"; 因为前面str1已经实例化了123 
        //后面手写的123同样是那段内存 直接不用开辟新的内存空间就把str1的值拿过来了 所以obj1 == obj2
        //反例可见 string s1 = "12"; s1 +="3"; 因为是拼接的，所以使用的新的堆内存空间
           

        /// <summary>
        /// 
        /// </summary>
        public static  void StringTest1()
        {
            string str1 = "123";
            object obj1 = str1;
            object obj2 = new object();
            object obj3 = new object();
            object obj4 = new object();
            obj2 = "123";
            obj4 = new string("1234".Where(ch => ch < '4').ToArray());
            Console.WriteLine(str1 == obj2.ToString());
            Console.WriteLine(str1 == obj1);
            Console.WriteLine(str1 == obj3);
            Console.WriteLine(str1 == obj4);
            Console.WriteLine(str1.Equals(obj4));
            Console.WriteLine(str1 .Equals(obj2.ToString()));
            Console.WriteLine(obj1 == obj2);

            string str3 = "123";
            StringTest2(str3);

            string s1 = "12";
            s1 +="3";
            object obj5 = new object();
            obj5 = "123";
            Console.WriteLine(s1 == obj5);

        }

        private static  void StringTest2(String s)
        {
            s = "1234";
        }

    }
}