using System;
using System.IO;
using System.Linq;
using JsonSong.Spider.DataAccess.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Suijing.Utils.Utility;

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
            var list = SpiderLiteDao.Instance.GetCon().FindAll().Where((a => a.Content.Contains("和老婆在一小面馆吃面"))).ToList();
            list.ToList().ForEach(a =>
            {
                var re = SpiderLiteDao.Instance.Delete(d=>d.Url==a.Url);
                var in1 =  re;
              
            });

        }

        //temp
        [TestMethod()]
        public void Test3()
        {
            var updateTime1 = SpiderLiteDao.Instance.GetLastUpdateTime(1).ToMinTime();

        }
    }
}
