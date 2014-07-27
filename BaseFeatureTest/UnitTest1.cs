using System;
using System.Collections.Generic;
using System.ComponentModel;
using BaseFeatureDemo.MyGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omu.ValueInjecter;

namespace BaseFeatureTest
{
    [TestClass]
    public class MyTest1
    {
        public class TargeClass
        {
            public int? Id { get; set; }
        }

        [TestMethod]
        public  void Test1()
        {
            
        }


        public static void Main(string[] args)
        {
            var target = new TargeClass();
            var pros = target.GetProps();
            foreach (PropertyDescriptor pro in pros)
            {
                var value = "16";
                try
                {
                    pro.SetValue(target, value);
                }
                catch (Exception)
                {
                    try
                    {
                        var result = pro.Converter.ConvertFromString(value);
                        pro.SetValue(target, result);
                    }
                    catch (Exception)
                    {
                        
                        continue;
                    }
                 

                    //if (pro.GetType()==typeof(int?))
                    //{
                    //    int res = 0;
                    //    int.TryParse(value, out res);
                    //    pro.SetValue(target, res);
                    //}
                }

            }
        }
    }

   
}
