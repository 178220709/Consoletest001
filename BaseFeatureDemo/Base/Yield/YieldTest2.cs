using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaseFeatureDemo.Base.Yield
{

    public class Persons : System.Collections.IEnumerable
    {
        #region IEnumerable ≥…‘±

        public System.Collections.IEnumerator GetEnumerator()
        {
            yield return "1";
            System.Threading.Thread.Sleep(1000);
            yield return "2";
            Thread.Sleep(1000);
            yield return "3";
            Thread.Sleep(1000);
            yield return "4";
            Thread.Sleep(1000);
            yield return "5";
            Thread.Sleep(1000);
            yield return "6";
        }

        #endregion
    }

    [TestClass]
    public class YieldTest2
    {
        [TestMethod]
        public void MainTest()
        {
            
            Persons arrPersons = new Persons();
            foreach (string s in arrPersons)
            {
                 Trace.WriteLine(s);
            }
        }
    }
}






