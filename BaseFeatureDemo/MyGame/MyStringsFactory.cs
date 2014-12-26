using System;
using System.Collections.Generic;
using System.Text;

namespace BaseFeatureDemo.MyGame
{
    
  
  public   class MyStringsFactory
    {
        public  static string DoWhat1()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= 12; i++)
            {
                sb.AppendFormat("\"{0}月新签\",",i);
                sb.AppendFormat("\"{0}月续签\",",i);
            }
            return sb.ToString();
        }
       
    }
}
