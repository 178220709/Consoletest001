using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;


namespace Suijing.Utils.Extention
{
    public static class LinqEx
    {
        public static IEnumerable<T> WhereAsync<T>(this IEnumerable<T> list,Func<T,Task<bool>> filter )
        {
            var newList = new List<T>();
            list.ToList().ForEach(async a =>
            {
                var re = await filter(a);
                if (re)
                {
                    newList.Add(a);
                }
            });

            return newList;
        }

    }
}
