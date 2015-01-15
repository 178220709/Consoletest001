using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.EFDemo    
{

    [TestClass()]
    public class EFTest
    {

        [TestMethod()]
        public void Test1()
        {
            var total = 20050144;

            var context = new MyContext();
            var query = context.GetAll();
            var list = query.Take(100);
            var set = context.RecordsSet;
            var list2 = set.Take(100);
        }


        [TestMethod()]
        public void Test2()
        {
            var se = RecordService.Instance;
            Record entity = new Record()
            {
                Name = "test",
                Code = "888888888888888"
            };
            se.AddEdit(entity);
           var list = se.Entities.ToList();
        }
    }
}
