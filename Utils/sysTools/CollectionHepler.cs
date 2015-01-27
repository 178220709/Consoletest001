using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Omu.ValueInjecter;

namespace Suijing.Utils.sysTools
{
    public static  class CollectionEx
    {
        public static bool HasValue<T>(this IEnumerable<T> list)
        {
            return (list != null && list.Any());
        }

    }
}
