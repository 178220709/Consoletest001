using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BaseFeatureDemo.Base
{
    public class List
    {
        //using System.Collections;
        public static IEnumerable Power(int number, int exponent)
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
            foreach (int i in Power(2, 8))
            {
                Console.Write("{0} ", i);
            }
        }


        class MyList : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
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