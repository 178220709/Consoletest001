using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BaseFeatureDemo.MyGame.Number
{

    public static class Util1305
    {
        public static byte Floor2(uint a, uint b)
        {
            var sum = a + b;
            if (((a << 1) > sum && b > 1) || ((b << 1) > sum && a > 1))
            {
                return 0;
            }
            else
            {
                return Floor1(a, b);
            }
        }

        public static byte Floor1(uint a, uint b)
        {

            return (byte) ((a + b)/(a*b));
        }
    }


    public class Nod1305
    {
        private static long getSum0(List<uint> A)
        {
            var sum1 = A.Count(a => a == 1);
            var sum2 = A.Count(a => a == 2);
            return sum1 * (A.Count - 1) + sum2 / 2;
        }
        private static uint getSum(List<uint> A)
        {

            uint sum = 0;
            for (int i = 0; i < A.Count; i++)
            {
                for (int j = i + 1; j < A.Count; j++)
                {
                    if (A[i] > 2 && A[j] > 2)
                    {
                        continue;
                    }

                    sum += Util1305.Floor1(A[i], A[j]);
                }
            }
            return sum;
        }

        public static void Main2(string[] args)
        {
            var A1 = new List<uint>(){1,2,3,1};     // 2 1 4
            var A2 = new List<uint>(){1,2,1,1};     // 3 1 4
            var A3 = new List<uint>(){1,2,2,1};     // 2 2 4
            var A4 = new List<uint>(){1,2,3,1,1,1}; // 4 1 6
            var A5 = new List<uint>(){1,2,2,3,3,3,3,3,3}; // 4 1 6

            var r1 = getSum(A1);
            var r2 = getSum(A2);
            var r3 = getSum(A3);
            var r4 = getSum(A4);
            var r5 = getSum(A5);
        }

        public static void Main1(string[] args)
        {
            uint count = uint.Parse(Console.ReadLine());
            var A = new List<uint>();
            for (uint i = 0; i < count; i++)
            {
                A.Add(uint.Parse(Console.ReadLine()));
            }
           
            Console.WriteLine(getSum(A));
        }
    }
}