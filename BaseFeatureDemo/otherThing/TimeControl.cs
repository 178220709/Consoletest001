using System;
using System.Threading;

namespace BaseFeatureDemo.otherThing
{
 public    class TimeControl
    {

        public static  void mainsdf()
        {
          DateTime dt =   DateTime.Parse(" 23:59:59");
            TimeSpan ts = dt - DateTime.Now;
         
            bool bbb=  (dt > DateTime.Now);
            Console.WriteLine(   ts.TotalSeconds);
        }

        public TimeControl()
        {
            
        }

    
    }
}