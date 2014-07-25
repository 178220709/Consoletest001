using System;
using System.Collections.Generic;
using BaseFeatureDemo.MyGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureTest
{
    [TestClass]
    public class SuitSort
    {
        public List<string> SuitList { get; set; }

        public SuitSort(List<string> list)
        {
            SuitList = list;
        }

        public void SortSuitList()
        {
            SuitList.Sort(SuitComparer);
        }

        private static int SuitComparer(string porkerA, string porkerB)
        {
            var a = GetPorkerNumber(porkerA);
            var b = GetPorkerNumber(porkerB);
            return a - b;
        }

        private static int GetPorkerNumber(string porker)
        {
            if (string.IsNullOrEmpty(porker))
            {
                return 0;
            }
            var suitValue = 0;
            char c = porker[0];
            switch (c)
            {
                case '♠':
                    suitValue = 100;
                    break;
                case '♥':
                    suitValue = 200;
                    break;
                case '♦':
                    suitValue = 300;
                    break;
                case '♣':
                    suitValue = 400;
                    break;
                default:
                    break;
            }

            var numStr = porker.Substring(1);
            var number = 0;
            switch (numStr)
            {
                case "A":
                    number = 14;
                    break;
                case "K":
                    number = 13;
                    break;
                case "Q":
                    number = 12;
                    break;
                case "J":
                    number = 11;
                    break;
                default:
                    number = Convert.ToInt32(numStr);
                    break;
            }

            number = (15 - number) + suitValue;
            return number;
        }
    }

    public class RankSort
    {
        public List<string> RankList { get; set; }

        public RankSort(List<string> list)
        {
            RankList = list;
        }

        public void SortRankList()
        {
            RankList.Sort(SuitComparer);
        }

        private static int SuitComparer(string porkerA, string porkerB)
        {
            var a = GetPorkerNumber(porkerA);
            var b = GetPorkerNumber(porkerB);

            return a - b;
        }

        private static int GetPorkerNumber(string porker)
        {
            if (string.IsNullOrEmpty(porker))
            {
                return 0;
            }
            var numStr = porker.Substring(1);
            var number = 0;
            switch (numStr)
            {
                case "A":
                    number = 14;
                    break;
                case "K":
                    number = 13;
                    break;
                case "Q":
                    number = 12;
                    break;
                case "J":
                    number = 11;
                    break;
                default:
                    number = Convert.ToInt32(numStr);
                    break;
            }

            number = (15 - number) * 100;

            char c = porker[0];
            switch (c)
            {
                case '♠':
                    number += 1;
                    break;
                case '♥':
                    number += 2;
                    break;
                case '♦':
                    number += 3;
                    break;
                case '♣':
                    number += 4;
                    break;
                default:
                    break;
            }

            return number;
        }
    }
}
