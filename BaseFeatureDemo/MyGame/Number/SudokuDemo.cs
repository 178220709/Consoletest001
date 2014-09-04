/*
 * author csdn版主:caozhy
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.MyGame
{
    [TestClass]
    public class SudokuDemo
    {
        [TestMethod]
        public static void Main1()
        {
            int tryCount = 0;
            int[] source =
            {
                0,8,6,0,7,0,0,1,0,
                1,3,0,6,0,0,0,7,8,
                4,7,9,8,1,3,6,2,5,
                0,4,0,5,6,2,0,8,1,
                0,1,0,0,0,8,2,6,7,
                6,2,8,1,0,7,5,4,0,
                8,6,1,0,5,9,7,3,0,
                3,5,0,7,0,1,8,9,6,
                0,9,0,3,8,6,1,5,0
            }; // http://www.sudoku.name/index-cn.php #10332 数独来自这个网站

            int[] result = source.ToArray(); // result数组保存解算中间数据和结果
            Func<bool> isFinished = () => !result.Where(x => x == 0).Any(); // 判断是否解算完成
            Func<int> nextNumber = () => result.Select((x, i) => new { x, i }).First(x => x.x == 0).i;
            // 取下一个空格（这个算法不是唯一的，你也可以从后往前填写，或者别的方法）
            Func<IEnumerable<int>> TryValues = () =>
            {
                int pos = nextNumber(); // 获取空格
                int col = pos % 9; // 行号
                int row = pos / 9; // 列号
                int group = (row / 3) * 3 + col / 3; // 宫号
                var colnums =
                    Enumerable.Range(1, 9)
                        .Except(Enumerable.Range(0, 81).Where(x => x % 9 == col).Select(x => result[x]).Where(x => x != 0));
                // 让1-9和本列已有数据对比，求差集，差集是对于列，允许填入的数字，下面类似
                var rownums =
                    Enumerable.Range(1, 9)
                        .Except(Enumerable.Range(0, 81).Where(x => x / 9 == row).Select(x => result[x]).Where(x => x != 0));
                var groupnumbers =
                    Enumerable.Range(1, 9)
                        .Except(
                            Enumerable.Range(0, 81)
                                .Where(x => ((x / 9) / 3) * 3 + (x % 9) / 3 == group)
                                .Select(x => result[x])
                                .Where(x => x != 0));
                tryCount++;
                Console.WriteLine("'{0}' is try ,times:{1}",pos,tryCount);
                return colnums.Intersect(rownums).Intersect(groupnumbers); //数据是行、列、宫的交集
            }; // 找出填写这个空格的所有可能尝试的数据

            
            Action Solve = () => { }; // 递归Lambda必须先定义一个空的。
            Solve = () =>
            {
                if (isFinished())
                {
                    result.ShowNow(); //如果全部填满，就输出结果（严格地，应该考虑无解的情况，这里忽略）
                }
                else
                {
                    int pos = nextNumber(); // 获取空格位置
                    foreach (int item in TryValues()) // 依次尝试所有可能的数字
                    {
                        result[pos] = item; // 将盘面设置为尝试数字
                        Solve(); //下一层解算
                    }
                    //result.ShowNow();
                    result[pos] = 0; // 尝试完还不行，恢复盘面，回溯上一层
                }
            }; // 算法主体
            Solve(); // 开始解算
           result.ShowNow();
        }
    }

    public static class ThisEx
    {
        public static void ShowNow(this IEnumerable<int>  list)
        {
            string str = string.Join("\r\n",
               list.Select((x, i) => new { x, i })
                   .GroupBy(x => x.i / 9)
                   .Select(x => string.Join(" ", x.Select(y => y.x))));
            Console.WriteLine(str + "\r\n");
        }
    }

}