using Consoletest001.MyGame;
using Consoletest001.image;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class A
    {
        public void F()
        {
            Console.WriteLine("A��F");
        }
        //����һ���鷽��
        public virtual void G()
        {
            Console.WriteLine("A��G");
        }
    }

    class B : A
    {
        new public void F()
        {
            Console.WriteLine("B��F");
        }
        //ʵ�ּ̳����е��鷽��
        public override void G()
        {
            Console.WriteLine("B��G");
        }
    }





internal class Test
{
    private static void Main()
    {
        B b = new B();
        A a = b;
        A c = new B();
        a.F();
        b.F();
        a.G();
        b.G();
        c.F();
        c.G();
        Console.ReadLine();
    }
}