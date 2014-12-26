namespace BaseFeatureDemo.otherThing
{
    using System;
    public class SamplesDelegate
    {
        delegate String MyDelegate(int myInt);
        string str1 = "str1";

        private string str2 = "str2";

           
        public void test11()
        {
           
 
             MyDelegate my1 = delegate(int param) { return param + str1 + str2; };

            MyDelegate my2 = delegate(int  param) { return param + str1 + str2; };

            Console.WriteLine(my1(2));

            Console.WriteLine(my2(3));
        }

        // Declares a delegate for a method that takes in an int and returns a String.
        public delegate String myMethodDelegate(int myInt);

        // Defines some methods to which the delegate can point.
        public class mySampleClass
        {

            // Defines an instance method.
            public String myStringMethod(int myInt)
            {
                if (myInt > 0)
                    return ("positive");
                if (myInt < 0)
                    return ("negative");
                return ("zero");
            }

            // Defines a static method.
            public static String mySignMethod(int myInt)
            {
                if (myInt > 0)
                    return ("+");
                if (myInt < 0)
                    return ("-");
                return ("");
            }
        }

        public static void Mainsdfsa()
        {
            


            // Creates one delegate for each method. For the instance method, an
            // instance (mySC) must be supplied. For the static method, use the
            // class name.
            mySampleClass mySC = new mySampleClass();
            myMethodDelegate myD1 = new myMethodDelegate( mySC.myStringMethod);
            myMethodDelegate myD2 = new myMethodDelegate(mySampleClass.mySignMethod);
            myMethodDelegate myD3 = new myMethodDelegate(delegate(int e) { return ""; });

            // Invokes the delegates.
            Console.WriteLine("{0} is {1}; use the sign \"{2}\".", 5, myD1(5), myD2(5));
            Console.WriteLine("{0} is {1}; use the sign \"{2}\".", -3, myD1(-3), myD2(-3));
            Console.WriteLine("{0} is {1}; use the sign \"{2}\".", 0, myD1(0), myD2(0));
        }

    }
}