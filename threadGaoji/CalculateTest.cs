using System;

namespace Consoletest001.threadGaoji
{
    class CalculateTest
    {
        public static void MainSDFABSFDBA()
        {
            Calculate calc = new Calculate();
            Console.WriteLine("Result = {0}.",
                              calc.Result(234).ToString());
            Console.WriteLine("Result = {0}.",
                              calc.Result(55).ToString());
        }
    }
}