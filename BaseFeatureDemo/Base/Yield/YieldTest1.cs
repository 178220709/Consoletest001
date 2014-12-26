using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.Delegate
{

    public static class Extensions
    {

        public static IEnumerable<S> MySelect<T, S>(this IEnumerable<T> source, Func<T, S> selector)

        {
            foreach (T element in source)
            {
                yield return selector(element);
                // yield return selector(element);
            }
        }

    }


    [TestClass]
    public class YieldTest1
    {
        [TestMethod]
        public void MainT()
        {
            var contacts = new[]
            {
                new {Name = "Chris Smith", PhoneNumbers = new[] {"206-555-0101", "425-882-8080"}},

                new {Name = "Bob Harris", PhoneNumbers = new[] {"650-555-0199"}}
            };
            var name = contacts.MySelect(j => j.Name).ToList();

            name.ForEach(a => Trace.WriteLine(a));

        }

    }
}






