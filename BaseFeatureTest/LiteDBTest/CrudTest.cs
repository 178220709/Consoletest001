using System;
using System.IO;
using System.Linq;
using JsonSong.Spider.DataAccess.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BaseFeatureTest.LiteDBTest    
{

    [TestClass()]
    public class CrudTest
    {
        [TestMethod()]
        public void Test1()
        {
            var list = SpiderLiteDao.Instance.GetCon().FindAll();
            var hasAdd = false;
            list.GroupBy(a=>a.Url).Where(g=>g.Count()>1)
                .SelectMany(g=>g).ToList()
                .ForEach(a =>
                {
                    if (!hasAdd)
                    {
                        a.Valid = true;
                        hasAdd = true;
                        SpiderLiteDao.Instance.Update(a);
                    }
                    else
                    {
                        SpiderLiteDao.Instance.Delete(b=>b.Id==a.Id );
                    }

                });
                   
            var updateTime = SpiderLiteDao.Instance.GetLastUpdateTime();
        }


        [TestMethod()]
        public void Test2()
        {

        }
    }
}
