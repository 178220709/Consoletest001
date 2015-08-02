using System;
using System.Linq;
using LiteDbLog.Facade;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppTest
{
    [TestClass]
    public class LiteDbLogTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = DBLogInstances.GetAllInstanceName();

            Assert.IsTrue(list.Count>0);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var dao = DBLogInstances.System;
            dao.Info("test");
            var list = dao.GetAll().ToList();

            Assert.IsTrue(list.Count > 0);
        }
    }
}
