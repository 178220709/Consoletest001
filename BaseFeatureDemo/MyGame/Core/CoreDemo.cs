﻿using System;
using System.Collections.Generic;
using BaseFeatureDemo.MyGame.Core;

namespace BaseFeatureDemo.MyGame.Core
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

    public sealed class Util
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
            var C = (line.End.X * line.Start.Y) - (line.Start.X * line.End.Y);
            return (Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2)));
        }


        public static Point GetPointFromStr(string str)
        {
            string[] temp = str.Split(' ');
            return new Point(double.Parse(temp[0]), double.Parse(temp[1]));
        }
    }


   public class LineCross
    {
       public static void Main1(string[] args)
        {
            int count = int.Parse(Console.ReadLine());

            List<string> result = new List<string>();
            for (int i = 0; i < count; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
               
            }

            foreach (var re in result)
            {
                Console.WriteLine(re);
            }
        }
    }
}