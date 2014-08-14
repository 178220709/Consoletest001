using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseFeatureDemo.Base.Delegate
{
    public class CloserDemo2
    {
        public Func<int> T1()
        {
            var n = 999;
            Func<int> result = () =>
            {
                return n;
            };
            n = 10;
            return result;
        }

        public dynamic T2()
        {
            var n = 999;
            dynamic result = new { A = n };
            n = 10;
            return result;
        }
        public static void Main1()
        {
            var a = new CloserDemo2();
            var b = a.T1();
            var c = a.T2();
            Console.WriteLine(b());
            Console.WriteLine(c.A);
        }
    }
}


