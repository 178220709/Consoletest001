using System;
using System.Collections.Generic;
using System.Text;

namespace Consoletest001.otherThing
{
    public class TestJiCheng

    {
        public static void  Mainsdfs()
        {
            childB b = new childB();
            b.aa();
        }
    }

    internal class A
    {
        public A()
        {
            PrintFields();
        }

        public virtual void PrintFields()
        {
            List<string> tt = new List<string>();
        }
    }


       class basea
      {
          public    void Ewwe()
          {
          }
      }

    class  childB:basea
    {
         public void aa()
         {
             base.Ewwe();
         }
    }
    internal class B : A
    {
        private int x = 1;
        private int y;

        public B()
        {
            y = -1;
        }

        public override void PrintFields()
        {
            Console.WriteLine("x={0},y={1}", x, y);
        }
        public void test(int i)
        {
            lock (this)
            {
                if (i > 10)
                {
                    i--;
                    test(i);
                }
            }
        }
    }
}