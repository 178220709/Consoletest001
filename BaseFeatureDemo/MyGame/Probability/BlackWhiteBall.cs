using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace BaseFeatureDemo.MyGame
{
    /// <summary>
    /// 张三设了一个赌局，一个布兜里有黑白两个球，赌局规则如下：
    ///第一次随机盲取一个，如果是黑球，你可以得到2元钱，游戏结束；如果是白球，你可以进行第二次游戏。
    ///第二次随机盲取一个，如果是黑球，你可以得到4元钱，游戏结束；如果是白球，你可以进行第三次游戏。
    ///第三次随机盲取一个，如果是黑球，你可以得到8元钱，游戏结束；如果是白球，你可以进行第四次游戏。
    ///第N次随机盲取一个，如果是黑球，你可以得到（2的N次方）元钱，游戏结束；如果是白球，你可以进行第N+1次游戏。
    ///每次取的球都要放回布兜里。
    ///参与的人必须交门票，现在问聪明的张三把门票最低定在多少才合适？聪明的赌徒最多愿意出多少价钱的门票？
    /// </summary>
    /// <returns>返回两个bool结果：1. 如果不改变选择，是否猜中；2. 如果改变选择，是否猜中。</returns>
    public static class BlackWhiteBall
    {
        public static void Main1()
        {
            int Tiems = 100000; //游戏次数
            int tempFlag = 0;
            Func<bool> getRandWhether = () =>
            {
                int re = Guid.NewGuid().GetHashCode();
                while (re == tempFlag)
                {
                     re = Guid.NewGuid().GetHashCode();
                }
                tempFlag = re;
                
                return re%2 == 0;
            };

            Func<long> playOneGame = () =>
            {
                int income = 2;
                while (getRandWhether())
                {
                    income = income*2;
                }
                return income;
            };

            Func<int,double> doOneTest = (price) =>
            {
                var result = new List<long>();
                for (int i = 0; i < Tiems; i++)
                {
                    result.Add(playOneGame() - price);
                }
                return result.Average(a => a);
            };

            for (int i = 6; i < 9; i++)
            {
                Console.WriteLine(doOneTest(i));
            }
           

        }



    }
}