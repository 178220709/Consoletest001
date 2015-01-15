using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.HotelRecord.Entity;
using MyProject.HotelRecord.MongoDBDal;
using MyProject.HotelRecord.SqlDal;

namespace MyProject.HotelRecord.Manager    
{

   
    public class QueryHotel
    {
      
        public void Test1()
        {
            const int total = 20050144;

            var context = new MyContext();
            var query = context.GetAll();
            int current = 0;
            for (; current < total; current = current + 1000)
            {
                var msList = query.Skip(current).Take(current+1000).ToList();
                var mgList = msList.Select(Mapper.TransCdsgus).ToList();
                RecordService.Instance.AddList(mgList);
            }
           
        }
    }
}
