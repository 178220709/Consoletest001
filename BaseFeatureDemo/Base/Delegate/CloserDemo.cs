using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFeatureDemo.Base.Delegate
{
    public class CloserDemo
    {
        public Func<int> T1()
        {
            var n = 999;
            return () =>
            {
                Console.WriteLine(n);
                return n;
            };
        }

        public static void Main1()
        {
            var a = new CloserDemo();
            var b = a.T1();
            Console.WriteLine(b());
        }
    }
}


