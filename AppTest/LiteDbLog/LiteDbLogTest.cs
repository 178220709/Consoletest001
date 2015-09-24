using System;
using System.Linq;
using JsonSong.BaseDao.LiteDb;
using LiteDbLog.Facade;
using LiteDbLog.LiteDBLog;
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
            var list00 = DBLogInstances.GetnstanceByName("System").GetAll().ToList();

          var list0 =  DBLogInstances.GetnstanceByName("System").Find(a => a.Level >2).ToList();
            var levelEnum = (DBLogLevelEnum)(1);

            var dao = DBLogInstances.System;
            dao.Info("test");
            var list = dao.GetAll().ToList();

            

            Assert.IsTrue(list.Count > 0);
        } 
        
        
        /// <summary>
        /// enum test
        /// </summary>
        [TestMethod]
        public void TestMethod3()
        {
            var dao = new BaseLiteDao<MyClass>("temp", "MyClass2");
            dao.Delete(a => a.Num>=0);
            for (int i = 0; i < 100; i++)
            {
                var en = new MyClass()
                {
                    Num = i%5+1,
                    Level = (DBLogLevelEnum) (i%5 +1)
                };
                dao.Insert(en);
            }
            var list = dao.GetAll();
        }

        class MyClass :BaseLiteEntity
        {
            public int Num { get; set; }
            public DBLogLevelEnum Level { get; set; }
        }

    }
}
