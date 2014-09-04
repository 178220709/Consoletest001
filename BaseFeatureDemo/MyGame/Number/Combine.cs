using System;
using System.Collections.Generic;
using System.Text;

namespace BaseFeatureDemo.MyGame
{
    
  
    class Combine
    {

        private IList<int[]> results = new List<int[]>();
             /// <summary>
       /// 组合算法，a为目标集合，n为集合数目，m和M为取的个数，b为new int[m]
       /// </summary>
        public  void combine(int[] a, int n, int m, int[] b, int M )
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

        public  IList<int[]> combineFromInput(int nNum,int aNum)
        {
            int[] source = new int[nNum];
            int[] b = new int[aNum];
            combine(source,nNum,aNum,b,aNum);
            this.results.Clear();
            IList<int[]> combineAuto = new List<int[]>( );
            ListCopy(this.results,combineAuto);
            return combineAuto;
        }

        private  void ListCopy(IList<int[]> oldList, IList<int[]> newList)
        {
            foreach (var intse in oldList)
            {
                newList.Add(intse);
            }
        }

        public string PrintResult(int[] list)
        {
            string str = "";
            foreach (int i in list)
            {
                str += i + " ";
            }
            return str;
        }


        
    }
}
