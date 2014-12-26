using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Linq
{
    [TestClass]
    public class LinqDemo
    {
        [TestMethod]
        public void AggregateTest()
        {
           var selectOptiona = new string[]{"a","b","c"};
           string str = selectOptiona.Aggregate("", (current, @select) => current +","+ @select);
          str=  str.Trim(',');

        }

    }


    internal class Student
    {
        public string Name { get; set; }
        public List<int> Score { get; set; }
    }

}
