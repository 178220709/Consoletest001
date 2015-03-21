using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.WeixinModel.Model;

namespace BaseFeatureDemo.Linq
{
    [TestClass]
    public class LinqDemo
    {
        [TestMethod]
        public void AggregateTest()
        {
            for (int i = 0; i < 10 && Check(); i++)
            {
                string str1 = "";
            }




           var selectOptiona = new string[]{"a","b","c"};
           string str = selectOptiona.Aggregate("", (current, @select) => current +","+ @select);
          str=  str.Trim(',');

        }

        private bool Check()
        {
            string str = "";
            return true;
        }
    }


    internal class Student
    {
        public string Name { get; set; }
        public List<int> Score { get; set; }
    }

}
