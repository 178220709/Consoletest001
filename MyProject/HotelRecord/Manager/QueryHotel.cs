using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;
using Omu.ValueInjecter;

namespace MyProject.HotelRecord.Manager    
{

    [TestClass()]
    public class QueryHotel
    {
        [TestMethod()]
        public void TransDataToMG()
        {
            const int total = 20050144;
           
            var context = new MyContext();
           // var total2 = context.RecordsSet.Count();
            var query = context.GetAll().OrderBy(a => a.id);
            int cIndex = 0;
          //  int cId = 1;
            for (; cIndex < total; cIndex = cIndex + 10000)
            {
                var msList = query.Where(a => a.id > cIndex + 1).Take(cIndex + 10000).Select(model => new Record()
                {
                    OldId = model.id,
                    Address = model.Address,
                    Name = model.Name,
                    //BirthDay = ParseToInt(model.Birthday),
                    OldBirthDay = model.Birthday,
                    Code = model.CtfId,
                    CtfTp = model.CtfTp,
                    Email = model.EMail,
                    Fax = model.Fax,
                    Gender = model.Gender == "M",
                    Mobile = model.Mobile,
                    Tel = model.Tel,
                   // Date = ParseDateTime(model.Version)
                    OldDate = model.Version
                }).ToList();
               msList.ForEach(a =>
                {
                    a.BirthDay = Mapper.ParseToInt(a.OldBirthDay);
                    a.Date = Mapper.ParseDateTime(a.OldDate);
                });
               // RecordService.Instance.AddList(mgList);
                Trace.WriteLine("next dealing is :" + cIndex);
               // Trace.WriteLine();
            }
           
        }
    }
}
