using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.HotelRecord.Entity;

namespace MyProject.HotelRecord.SqlDal
{
   public class MyContext : DbContext
    {
        public MyContext()
            : base("data source=127.0.0.1;initial catalog=shifenzheng;persist security info=True;user id=sa;password=aaaaaa;multipleactiveresultsets=True;")
        {
        }
        public ObjectContext ObjectContext
        {
            get
            {
                var ctx = ((IObjectContextAdapter)this).ObjectContext;
                return ctx;
            }
        }

        public DbSet<cdsgus> RecordsSet { get; set; }

        public IQueryable<cdsgus> GetAll()
        {
           return ObjectContext.CreateObjectSet<cdsgus>().AsQueryable();
        }
              
    }
}
