using System;
using System.Collections.Generic;
using System.Text;

namespace BaseFeatureDemo.MyGame
{
    internal class QueenInfo
    {
       
        public QueenInfo (int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }

        public int Y { get; set; }

        public static bool IsOkResult(int[] oneResult )
        {
            QueenInfo[] infos = new QueenInfo[8];
            for (int i = 0; i < 8; i++)
            {
                 infos[i] = new QueenInfo(i+1,oneResult[i]);
            }
            int sumx = 0, sumy = 0;
            foreach(var info in infos)
            {
                sumx += info.X;
                sumy += info.Y;
            }
            if (sumx == 36 && sumy == 36)
            {
            }
            else
            {
                return false;
            }
            Combine com = new Combine();
           IList<int[]> linelist = com.combineFromInput(8, 2);
            
            foreach (var intse in linelist)
            {
                if ((infos[intse[0]].X - infos[intse[0]].Y) ==
                    (infos[intse[1]].X - infos[intse[1]].Y))
                {
                    return false;
                }
            }
            return true;

        }
    }

    internal class QueenGame
    {
        private IList<string> _result = new List<string>();

        public   void  MainTest()
        {
            Combine com = new Combine();
            IList<int[]> comlist = com.combineFromInput(64, 8);
            foreach (var aCom in comlist)
            {
                if (QueenInfo.IsOkResult(aCom))
                {
                    _result.Add(com.PrintResult(aCom));
                }
            }

        }

         public  static void  MainTest2323()
         {
             QueenGame game = new QueenGame();
             game.MainTest();
             IList<string> result = game._result;
         }
    }
}
