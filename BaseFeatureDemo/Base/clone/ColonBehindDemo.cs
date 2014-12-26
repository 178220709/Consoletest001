using System;

namespace BaseFeatureDemo.Base
{
    public class BaseClass
    {
        public BaseClass()
        {
            Console.WriteLine(" BaseClass() is called");
        }
        public BaseClass(string str)
        {
            Console.WriteLine(" BaseClass({0}) is called", str);
        }
        public void FunA()
        {
            Console.WriteLine(" base funa is called");
        }
    }

    public class SuperClass:BaseClass
    {
        public SuperClass():base("this test")
        {
            Console.WriteLine(" SuperClass() is called");
        }
        public SuperClass(string str)
        {
            Console.WriteLine(" SuperClass({0}) is called", str);
        }
        public void FunA()
        {
            Console.WriteLine(" base funa is called");
        }
    }


   public  class  ColonBehindDemo
   {
       public static void main1()
       {
            SuperClass a = new SuperClass();
            SuperClass b = new SuperClass("test");
       }

   }
}
