using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;
using Suijing.Utils;
using Suijing.Utils.WebTools;

namespace BaseFeatureTest.EFDemo    
{

    [TestClass()]
    public class MongoDBTest
    {
        public static IList<Record> GetRecords(int n)
        {
            IList<Record> list = new List<Record>();
            for (int i = 0; i < n; i++)
            {
                var re = new Record();
                CreateDataHepler.SetProWithProName(re,i);
                list.Add(re);
            }
            return list;
        }
        [TestMethod()]
        public void AddAndQuery1()
        {
            var se = RecordService.Instance;
            Record entity = new Record()
            {
                Name = "test_ayrusyru",
                Code = "888888888888888"
            };
            se.AddEdit(entity);
           var en = se.Entities.First(a => a.Name == entity.Name);
           Assert.IsTrue(en.Code == entity.Code);
        }

        [TestMethod()]
        public void AddAndQuery2()
        {
            int max = int.MaxValue;
            var se = RecordService.Instance;

            var priList = GetRecords(9).ToList();
            se.AddList(priList);
            var dre = se.DeleteAll();
            var alre = se.AddList(priList);
            var list = se.Entities.ToList();
            Assert.IsTrue(list.Count == 9);
        }
    }
}
