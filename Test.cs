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
            Console.WriteLine("A的F");
        }
        //定义一个虚方法
        public virtual void G()
        {
            Console.WriteLine("A的G");
        }
    }

    class B : A
    {
        new public void F()
        {
            Console.WriteLine("B的F");
        }
        //实现继承类中的虚方法
        public override void G()
        {
            Console.WriteLine("B的G");
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