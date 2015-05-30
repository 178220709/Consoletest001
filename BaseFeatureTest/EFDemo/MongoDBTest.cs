using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.Manager;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;
using Suijing.Utils;
using Suijing.Utils.sysTools;
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
           
        }

        [TestMethod()]
        public void AddAndQuery2()
        {
           
        }   
        
        [TestMethod()]
        public void ManagerTest()
        {
            QueryHotel query = new QueryHotel();
            query.TransDataToMG();
        }
    }
}
