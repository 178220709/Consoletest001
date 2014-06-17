using System;

namespace BaseFeatureDemo.Delegate
{
 

    public  class Programtest
    {
        public static void Mainsdf()
        {
            Func<int, int> fac = null;
            fac = x => x <= 1 ? 1 : x * fac(x - 1);
            Console.WriteLine(fac(5)); // 120;

            Func<int, int> facAlias = fac;
            fac = x => x;
            Console.WriteLine(facAlias(5)); // 20

        }
    }
}
