using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JsonSong.SpiderApp.Data;

namespace JsonSong.SpiderApp.Base
{
    public abstract class BaseManager<T> where T:BaseEntity
    {
        public static IQueryable<T> Query
        {
            get { return MyDbContext.GetQuery<T>(); }  
        }

        public static IQueryable<T> GetQuery(IEnumerable<Expression<Func<T, bool>>> wheres = null)
        {
            var query = Query;
            if (wheres != null)
            {
                foreach (var where in wheres)
                {
                    query = query.Where(@where);
                }
            }
            return query;
        }
    }
}