using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


/*有一个未完成的等式：1 2 3 4 5 6 7 8 9=N
当给出整数N的具体值后，请你在2，3，4，5，6，7，8，9这8个数字的每一个前面，或插入运算符号“+”，或插入一个运算符号“-”，或不插入任何运算符号，使等式成立，并统计出能使等式成立的算式总数，若无解，则输出0。
例如：取N为108时，共能写出15个不同的等式，以下就是其中的二个算式：
1+23+4+56+7+8+9=108
123-45+6+7+8+9=108
输入一个数N
输出一个数，表示能使等式成立的算式总数。*/


namespace BaseFeatureDemo.MyGame
{
    public static class CheckStrEquation
    {
        private const string TargetStr = "123456789";

        private static string[] CountType = { "+", "-", "" };
                                      
                                 

        public static void Main1()
        {
            
        }

        public static IList<string> _tarList ;
        public static IList<string> TarList
        {
            get
            {
                if  ((_tarList != null && _tarList.Count!=0))
                {
                    return _tarList;
                }
                _tarList = new List<string>();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        _tarList.Add(TargetStr.Insert(j, CountType[i]));
                    }
                }
                return _tarList;
            }
        }


        public static long GetResultFromStr( string countStr)
        {
            List<string> mList = new List<string>(){"+","-"};
            var numbers = countStr.Split('+', '-');


            return 0;
        }

    }
}