using System;
using System.Collections.Generic;
using System.Threading;

namespace BaseFeatureDemo.Base.ThreadDemo.ThreadBase
{
    public class ListDemo
    {
        public static void Main1()
        {
            //list里存放10个数字
            List<int> list = new List<int>(10);
            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }

            //10个数字,分成10组,其实每组就一个元素,每组的元素是不相同的
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            for (int i = 0; i < 10; i++)
            {
                int k = i % 10;
                if (dict.ContainsKey(k))
                {
                    dict[k].Add(i);
                }
                else
                {
                    dict[k] = new List<int>();
                    dict[k].Add(i);
                }
            }



            using (Dictionary<int, List<int>>.Enumerator enumerator = dict.GetEnumerator())
            {
                KeyValuePair<int, List<int>> keyValue;
                while (enumerator.MoveNext())
                {
                    keyValue = enumerator.Current;
   
                    System.Threading.Thread thread = new System.Threading.Thread(Display);
                    thread.Start(keyValue.Value);
                }
            }

            using (Dictionary<int, List<int>>.Enumerator enumerator = dict.GetEnumerator())
            {
                KeyValuePair<int, List<int>> keyValue;
                while (enumerator.MoveNext())
                {
                    keyValue = enumerator.Current;
                    System.Threading.Thread thread = new System.Threading.Thread(delegate()
                    {
                        foreach (var item in keyValue.Value)
                        {
                            Console.WriteLine(item.ToString());
                        }
                    }
                  );
                    thread.Start();
                }
            }

        }

        public static void Display(object o)
        {
            List<int> list = o as List<int>;
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }

    }
}