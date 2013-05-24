<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Consoletest001.MyGame
{
    public  class  Point
    {
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
         private static double GetLenght(Point p1 ,Point p2)
         {
           return  Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
         }
         private static bool IsEqual(double l1, double l2,double l3)
         {
             if (Math.Abs(l1-l2)<0.001 && Math.Abs(l2-l3)<0.001 && Math.Abs(l3-l1)<0.001 )
             {
                 return true;
             }
             return false;
         }
         public static bool IsTrl(Point p1, Point p2,Point p3)
         {
             if (IsEqual(GetLenght(p1,p2),GetLenght(p2,p3),GetLenght(p1,p3)) )
             {
                 return true;
             }
             return false;
         }
         public double X { get; set; }
         public double Y { get; set; }
    }
   public  class TenPointGame
    {
        public  Point [] tenP = new  Point[11];
        public TenPointGame()
        {
            InitTenPints();
        }
        public bool IsTrl(int p1, int p2,int p3)
        {
            return Point.IsTrl(this.tenP[p1], this.tenP[p2], this.tenP[p3]);
        }
        private void InitTenPints()
        {
            double q3 = Math.Sqrt(3);
           
                tenP[1] = new Point(3,3*q3);
                tenP[2] = new Point(2, 2 * q3); 
                tenP[3] = new Point(4, 2 * q3); 
                tenP[4] = new Point(1,  q3); 
                tenP[5] = new Point(3,  q3); 
                tenP[6] = new Point(5,  q3); 
                tenP[7] = new Point(0, 0); 
                tenP[8] = new Point(2, 0); 
                tenP[9] = new Point(4, 0); 
                tenP[10] = new Point(6, 0); 
        }

       private IList<int[]> results = new List<int[]>();

       /// <summary>
       /// 组合算法，a为目标集合，n为集合数目，m和M为取的个数，b为new int[m]
       /// </summary>
        public void combine(int[] a, int n, int m, int[] b, int M )
        {

            for (int i = n; i >= m; i--) // 注意这里的循环范围
            {
                b[m - 1] = i - 1;
                if (m > 1)
                    combine(a, i - 1, m - 1, b, M);
                else // m == 1, 输出一个组合
                {
                    int[] temp = new int[M];

                    for (int j = M - 1; j >= 0; j--)
                    {
                        
                        temp[j] = a[b[j]];
                    }
                    results.Add(temp);
                  
                }
            }
        }

       /// <summary>
       /// 验证结果集的一种，里面的所有3个点，都不能组成正三角形，则返回true
       /// </summary>
       /// <param name="relust3"></param>
       /// <returns></returns>
        public bool allFaile(int[] relust6)
       {
           int[] b2 = new int[3];
           this.results.Clear();
           this.combine(relust6, 6, 3, b2, 3);
           IList<int[]> results3 = new List<int[]>();
           this.comcopy(this.results, results3);
           int countSucess = 0;
           foreach (var p3 in results3)
           {
               if (this.IsTrl(p3[0], p3[1], p3[2]))
               {
                   countSucess++;
                   break;
               }
           }
           if (countSucess == 0)
           {
               return true;
           }
           else
           {
               return false;
           }
       }

       public void comcopy(IList<int[]> oldList ,IList<int[]> newList)
       {
           foreach (var intse in oldList)
           {
               newList.Add(intse);
           }
       }

       public string printResult(int[] List)
       {
           string str = "";
           foreach (int i in List)
           {
               str += i + " ";
           }
           return str;
       }

       public static void Maintest1()
        {
            TenPointGame game = new TenPointGame();
           IList<string> resultstr = new List<string>();

           int[] a = {1,2,3,4,5,6,7,8,9,10};

            int[] b = new int[6] ;
            game.results.Clear();
           game.combine(a,10,6,b,6);
           IList<int[]> results6 = new List<int[]>( );
           game.comcopy(game.results, results6);
           int count = 0;
           for (int i = 0; i < results6.Count;i++ )
           {
               if (game.allFaile(results6[i]) )
               {
                   resultstr.Add(game.printResult(results6[i]));
               }
           }

           Console.WriteLine(resultstr[0]);
        }
    }
}
=======
﻿
>>>>>>> 5fbbcce73ae87060fff41903a3c26e7645f7241e
