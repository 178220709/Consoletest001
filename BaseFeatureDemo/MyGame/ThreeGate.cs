using System;
using System.Diagnostics;
using System.Linq;

namespace BaseFeatureDemo.MyGame
{
    public static class ThreeGate
    {
        const int Size = 3;     //wall数
        const int Award = 1;    //奖品数

        /// <summary>
        /// 进行一次测试。
        /// </summary>
        /// <returns>返回两个bool结果：1. 如果不改变选择，是否猜中；2. 如果改变选择，是否猜中。</returns>
        private static Tuple<bool, bool> testc()
        {
            var gates = (from n in Enumerable.Range(0, Size)
                         orderby Rnd.Next()
                         select n < Award).ToList();
            var guess = Rnd.Next(0, Size);       //首先猜测选择这个
            int open = (from n in Enumerable.Range(0, Size)
                        where n != guess && !gates[n]
                        select n).First();   //选择这个门打开
            int newGuess = (from n in Enumerable.Range(0, Size)
                            where n != guess && n != open
                            orderby Rnd.Next()
                            select n).First();
            return new Tuple<bool, bool>(gates[guess], gates[newGuess]);
        }

        static Random Rnd = new Random();

         public  static void t1()
        {
            var cn = 10000;     //进行1万次反复测试
            var game = Enumerable.Range(0, cn).Select(x => testc()).ToList();
            var result1 = game.Count(x => x.Item1);      //不改变决定成功次数
            var result2 = game.Count(x => x.Item2);    //改变决定成功次数
            Debug.Assert(result2 > result1 + 3000);
        }

    }
}