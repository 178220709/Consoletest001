using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseFeatureDemo.MyGame
{
    public class SudokuDemo
    {
        public static void Main1()
        {
            int[] source =
            {
                5, 4, 0, 2, 9, 0, 0, 1, 0,
                0, 2, 0, 0, 0, 6, 3, 0, 0,
                3, 0, 0, 1, 0, 0, 0, 5, 4,
                0, 6, 0, 0, 0, 8, 9, 0, 0,
                2, 5, 0, 6, 7, 0, 0, 3, 1,
                0, 0, 1, 0, 2, 0, 0, 6, 0,
                8, 3, 0, 0, 0, 4, 0, 0, 6,
                0, 0, 5, 9, 0, 0, 0, 8, 0,
                0, 7, 0, 0, 3, 1, 0, 4, 2
            }; // http://www.sudoku.name/index-cn.php #10332 数独来自这个网站

            int[] result = source.ToArray(); // result数组保存解算中间数据和结果
            Func<bool> IsFinished = () => result.Where(x => x == 0).Count() == 0; // 判断是否解算完成
            Func<int> NextNumber = () => result.Select((x, i) => new {x, i}).First(x => x.x == 0).i;
            // 取下一个空格（这个算法不是唯一的，你也可以从后往前填写，或者别的方法）
            Func<IEnumerable<int>> TryValues = () =>
            {
                int pos = NextNumber(); // 获取空格
                int col = pos%9; // 行号
                int row = pos/9; // 列号
                int group = (row/3)*3 + col/3; // 宫号
                var colnums =
                    Enumerable.Range(1, 9)
                        .Except(Enumerable.Range(0, 81).Where(x => x%9 == col).Select(x => result[x]).Where(x => x != 0));
                // 让1-9和本列已有数据对比，求差集，差集是对于列，允许填入的数字，下面类似
                var rownums =
                    Enumerable.Range(1, 9)
                        .Except(Enumerable.Range(0, 81).Where(x => x/9 == row).Select(x => result[x]).Where(x => x != 0));
                var groupnumbers =
                    Enumerable.Range(1, 9)
                        .Except(
                            Enumerable.Range(0, 81)
                                .Where(x => ((x/9)/3)*3 + (x%9)/3 == group)
                                .Select(x => result[x])
                                .Where(x => x != 0));
                return colnums.Intersect(rownums).Intersect(groupnumbers); //数据是行、列、宫的交集
            }; // 找出填写这个空格的所有可能尝试的数据

            Action DisplayResult =
                () =>
                    Console.WriteLine(
                        string.Join("\r\n",
                            result.Select((x, i) => new {x, i})
                                .GroupBy(x => x.i/9)
                                .Select(x => string.Join(" ", x.Select(y => y.x)))) + "\r\n"); // 显示结果
            Action Solve = () => { }; // 递归Lambda必须先定义一个空的。
            Solve = () =>
            {
                if (IsFinished())
                {
                    DisplayResult(); //如果全部填满，就输出结果（严格地，应该考虑无解的情况，这里忽略）
                }
                else
                {
                    int pos = NextNumber(); // 获取空格位置
                    foreach (int item in TryValues()) // 依次尝试所有可能的数字
                    {
                        result[pos] = item; // 将盘面设置为尝试数字
                        Solve(); //下一层解算
                    }
                    result[pos] = 0; // 尝试完还不行，恢复盘面，回溯上一层
                }
            }; // 算法主体
            Solve(); // 开始解算
        }
    }
}