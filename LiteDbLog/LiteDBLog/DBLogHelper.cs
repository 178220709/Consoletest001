using System.Collections.Generic;
using System.Linq;

namespace LiteDbLog.LiteDBLog
{
    public static  class DBLogHelper
    {
        public static bool HasValue<T>(this IEnumerable<T> list)
        {
            return (list != null && list.Any());
        }

    }
}
