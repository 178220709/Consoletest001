using System;
using System.Collections.Generic;

namespace BaseFeatureDemo.Delegate
{
    internal class MyClass
    {
        public int Value;
        public string Information;
    }

    internal class Program
    {
        /// <summary>
        /// ����һ��MyClass���͵Ķ��󼯺�
        /// </summary>
        /// <returns></returns>
        private static List<MyClass> GetMyClassList()
        {
            List<MyClass> lst = new List<MyClass>();
            Random ran = new Random();
            MyClass obj = null;
            for (int i = 0; i < 10; i++)
            {
                obj = new MyClass {Value = ran.Next(1, 100), Information = "object" + i.ToString()};
                lst.Add(obj);
            }
            return lst;
        }

        /// <summary>
        /// ��ӡһ��MyClass���󼯺ϵ����г�Ա
        /// </summary>
        /// <param name="lst"></param>
        private static void PrintList(List<MyClass> lst)
        {
            if (lst == null)
                return;
            foreach (MyClass obj in lst)
                Console.WriteLine("Infomation={0},Value={1}", obj.Information, obj.Value);
        }
        public delegate bool MyPredicate<in T>(T obj);

        private static bool GreaterThan50(MyClass elem)
        {
            if (elem.Value > 50)
                return true;
            return false;
        }

        public static void Main1(string[] args)
        {
            Predicate<MyClass> pred = GreaterThan50;
            MyPredicate<MyClass> pred1 = GreaterThan50;
            Func<MyClass,bool> pred2 = GreaterThan50;
            MyPredicate<MyClass> pred3 = GreaterThan50;
            
            List<MyClass> lst = GetMyClassList();
            Console.WriteLine("���ɵ�MyClass���󼯺�Ϊ��");
            PrintList(lst);
            MyClass foundElement = lst.Find(pred);
            MyClass foundElement2 = lst.Find(a=>a.Value>50);
           // MyClass foundElement3 = lst.Find(pred2); 
            if (foundElement != null)
                Console.WriteLine("�ҵ��˷��������Ķ���Infomation={0},Value={1}", foundElement.Information, foundElement.Value);
            else
                Console.WriteLine("δ�ҵ����������Ķ���");
            Console.ReadKey();
        }
    }
}
