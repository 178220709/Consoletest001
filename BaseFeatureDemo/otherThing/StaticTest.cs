using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseFeatureDemo.otherThing
{
    internal class StaticTest
    {
        public int _age = 16;

        public readonly int _age2 = 16;

        public static int _age3 = 16;

        public static void Test1(StaticTest ttt, int age)
        {
            ttt._age = age;
        }

        public static void Test2(int age)
        {
        }
    }


    public static class SClass
    {
        public static void Testttt()
        {
            StaticTest pen1 = new StaticTest() {_age = 16};
            pen1._age = 18;
            StaticTest pen2 = new StaticTest() {_age = 16};
            pen2._age = 18;

            StaticTest._age3 = 18;

            Console.WriteLine(" this  is a  common fun");
        }
    }
}