using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;

namespace MyProject.HotelRecord.Manager    
{

    [TestClass()]
    public class QueryHotel
    {
        [TestMethod()]
        public void Test1()
        {
            const int total = 20050144;
           
            var context = new MyContext();
           // var total2 = context.RecordsSet.Count();
            var query = context.GetAll().OrderBy(a => a.id);
            int current = 0;
            current = 583000;
            for (; current < total; current = current + 10000)
            {
                var msList = query.Skip(current).Take(current+10000).ToList();
                var mgList = msList.Select(Mapper.TransCdsgus).ToList();
                RecordService.Instance.AddList(mgList);
                Trace.WriteLine("current dealing is :" + current);
               // Trace.WriteLine();
            }
           
        }
    }
}
