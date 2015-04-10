using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseFeatureDemo.MyGame.Number
{
    public class Point
    {
        public double X { set; get; }

        public double Y { set; get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class Line
    {
        public Point Start { set; get; }

        public Point End { set; get; }

        public double GetLenght { set; get; }

        public Line(Point startPoint, Point endPoint)
        {
            Start = startPoint;
            End = endPoint;
        }
    }

    public static class Util
    {
        public static double GetLenght(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow((start.X - end.X), 2) + Math.Pow((start.Y - end.Y), 2));
        }

        /// <summary>
        /// 点到直线的最短距离
        /// </summary>
        /// <param name="p"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static double GetLenght(Point p, Line line)
        {
            var A = (line.End.Y - line.Start.Y);
            var B = (line.Start.X - line.End.X);
            var C = (line.End.X*line.Start.Y) - (line.Start.X*line.End.Y);
            return (Math.Abs(A*p.X + B*p.Y + C)/Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2)));
        }

        public static Point GetPointFromStr(string str)
        {
            string[] temp = str.Split(' ');
            return new Point(double.Parse(temp[0]), double.Parse(temp[1]));
        }

        public static double td(this string str)
        {
            return double.Parse(str);
        }
    }
    //圆与三角形 http://www.51nod.com/onlineJudge/questionCode.html#!problemId=1298
    /*第1行：一个数T，表示输入的测试数量(1 <= T <= 10000)，之后每4行用来描述一组测试数据。
    4-1：三个数，前两个数为圆心的坐标xc, yc，第3个数为圆的半径R。(3000 <= xc, yc <= 3000, 1 <= R <= 3000）
    4-2：2个数，三角形第1个点的坐标。
    4-3：2个数，三角形第2个点的坐标。
    4-4：2个数，三角形第3个点的坐标。(3000 <= xi, yi <= 3000）
     Yes  No
     */

    public class Nod1298
    {
        public static Point GetOffsetPoint(Point start, Point end)
        {
            return new Point(1,1);
        }



        private static double GetMinLenght(Point p, Line line)
        {
            var m1 = Math.Min(Util.GetLenght(p, line.Start), Util.GetLenght(p, line.Start));
            return Math.Min(m1, Util.GetLenght(p, line));
        }
        private static double GetMaxLenght(Point p, Line line)
        {
            return Math.Max(Util.GetLenght(p, line.Start), Util.GetLenght(p, line.Start));
        }
       public static void Main1(string[] args)
        {
            int count = int.Parse(Console.ReadLine());
           
            for (int i = 0; i < count; i++)
            {
                var lineStr = Console.ReadLine();
                string[] l1s = lineStr.Split(' ');
                Point center = new Point(l1s[0].td(), l1s[1].td());
                var radius = l1s[2].td();

                var ps =
                    Enumerable.Range(0, 3)
                        .Select(a => Util.GetPointFromStr(Console.ReadLine())).ToArray();
                var l1 = new Line(ps[0], ps[1]);
                var l2 = new Line(ps[0], ps[2]);
                var l3 = new Line(ps[1], ps[2]);
                 
               
     
  
                if (lenghts.All(l => l < radius) || lenghts.All(l => l < radius))
                {
                    Console.WriteLine("No");
                }
                else
                {
                    Console.WriteLine("Yes");
                }
            }
        }
    }
}