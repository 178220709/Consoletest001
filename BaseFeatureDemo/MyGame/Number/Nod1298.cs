using System;
using System.Collections.Generic;
using System.IO;
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
        public static Point GetOffsetPoint(Point p1, Point p2)
        {
            if (Math.Abs((p1.X - p2.X)) < 0.0001)
            {
                return new Point(p1.X, p1.Y+(p2.Y-p1.Y)/100);
            }
            else
            {
                var K = (p1.Y - p2.Y) / (p1.X - p2.X);
                var off = (p2.X - p1.X)/1000;
                return new Point(p1.X + off, p1.Y + off * K);
            }
        }



        private static double GetMinLenght(Point p, Line line)
        {
            var len1 = Util.GetLenght(p, line.Start);
            var len2 = Util.GetLenght(p, line.End);
            var min1 = Math.Min(len1, len2);
            Point offPoint = len1<len2 ? GetOffsetPoint(line.Start, line.End) : GetOffsetPoint(line.End,line.Start);
            var offLen = Util.GetLenght(p, offPoint);
            if (offLen < min1)//点在线段宽拓补范围内
            {
                return Util.GetLenght(p, line);
            }
            else
            {
                return min1;
            }
        }
        private static double GetMaxLenght(Point p, Line line)
        {
            return Math.Max(Util.GetLenght(p, line.Start), Util.GetLenght(p, line.End));
        }
       public static void Main1(string[] args)
        {
            int count = int.Parse(Console.ReadLine());
            var results = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var lineStr = Console.ReadLine();
                string[] l1s = lineStr.Split(' ');
                Point center = new Point(l1s[0].td(), l1s[1].td());
                var radius = l1s[2].td();

                var ps =
                    Enumerable.Range(0, 3)
                        .Select(a => Util.GetPointFromStr(Console.ReadLine())).ToArray();
                var lines = new List<Line>()
                {
                    new Line(ps[0], ps[1]),
                    new Line(ps[0], ps[2]),
                    new Line(ps[1], ps[2])
                };
                //圆心到线段最小距离全部大于半径 或者 最大距离 全部小于半径 则不相交
               
                var r1= lines.Select(a => GetMinLenght(center, a)).All(a => a > radius);
                var r2= lines.Select(a => GetMaxLenght(center, a)).All(a => a < radius);
               
                if (r1 || r2)
                {
                    results.Add("No");
                }
                else
                {
                    results.Add("Yes");
                }
            }
            results.ForEach(Console.WriteLine);

          var ws =   File.CreateText("test.txt");
          results.ForEach(ws.WriteLine);
          ws.Flush();
          ws.Close();
        }
    }
}