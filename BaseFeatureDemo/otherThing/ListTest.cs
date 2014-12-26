using System;
using System.Collections;
using System.Threading;

namespace BaseFeatureDemo.otherThing
{
    public class ListTest
    {
        public static void mainsdf()
        {
            int[] ss = {1, 2, 3, 4, 5, 6, 7, 8};

            for (int i = 8; i < Math.Min(9, ss.Length); i++)
            {
                bool b = ss[8] == null;
            }
        }

        public static void Test2()
        {
            ArrayList arr = new ArrayList();
            for (int i = 0; i < 13; i++)
            {
                arr.Add(i);
            }


            for (int i = 0; i < arr.Count;)
            {
                if ((int) arr[i]%3 == 0)
                {
                    arr.Remove(arr[i]);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}