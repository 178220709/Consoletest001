using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaseFeatureDemo.Base.Linq
{
  public   class ExpressDemo
    {

      public static void Main1()
      {
          Expression<Func<int, int, int>> expression = (a, b) => a * b + 2;

          Func<int, int, int> expression2 = (a, b) => a * b + 2;
      }
      public void Base1_1()
      {
          Expression<Func<int, int, int>> expression = (a, b) => a * b + 2;

          Func<int, int, int> expression2 = (a, b) => a * b + 2;


      }


    }
}
