using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suijing.Utils;

namespace BaseFeatureDemo.Base.InheritDemo
{
    public class Animal
    {
        public string Name { get; set; }
    }

    public class People : Animal
    {
        public string Position { get; set; }
    }

    [TestClass]
    public class InheritDemoTest
    {
        [TestMethod]
        public  void main1()
        {
            var people = new People()
            {
                Name = "json",
                Position = "diaosi"
            };
            var an1 = (Animal)people ;
            var an2 = people as Animal ;
            var an2Str = an2.ToJson();
            var an3 = an2Str.FromJson<Animal>();

            var list1 = new List<People> {people};
            var list2 = list1.Cast<Animal>().ToList();

            var list2Str = list2.ToJson();


        }

    }
}
