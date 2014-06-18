using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base
{

    public class YieldDemo
    {
        //using System.Collections;
        public static IEnumerable<int> Power(int number, int exponent)
        {
            int counter = 0;
            int result = 1;
            while (counter++ < exponent)
            {
                result = result * number;
                yield return result;
            }
        }   
        public static IEnumerator<int> Power2(int number, int exponent)
        {
            int counter = 0;
            int result = 1;
            while (counter++ < exponent)
            {
                result = result * number;
                yield return result;
            }
        }

        IList<string> FindBobs(IEnumerable<string> names)
        {
            var bobs = new List<string>();

            foreach (var currName in names)
            {
                if (currName == "Bob")
                    bobs.Add(currName);
            }

            return bobs;
        }

        IEnumerable<string> FindBobs2(IEnumerable<string> names)
        {
            foreach (var currName in names)
            {
                if (currName == "Bob")
                    yield return currName;
            }
        }


        public static void Main1()
        {
            // Display powers of 2 up to the exponent 8:
            foreach (var i in Power(2, 8))
            {
                Console.Write("{0} ", i);
            }

          
            IList<int> myList = new List<int>(){1,2,3,4};
            var ienu1 = myList.GetEnumerator();
            ienu1.MoveNext();
     
            Console.ReadLine();
        }

        public static void Main2()
        {
            foreach (var i in new MyCCC())
            {
                Console.WriteLine(" foreach {0} ", i);
            }
            Console.ReadLine();
        }


        class MyList : IEnumerable
        {
            public IEnumerable List = new List<string>();
            public IEnumerator List2 = new myEnumerator();
            public IEnumerator GetEnumerator()
            {
                foreach (var VARIABLE in List)
                {
                    yield return VARIABLE;
                }
            }
        }

        class myEnumerator : IEnumerator
        {
            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public object Current { get; private set; }
        }
    }
    /*
    Output:
    2 4 8 16 32 64 128 256 
    */



}