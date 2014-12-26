using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseFeatureDemo.Base.Reg
{
    internal class RegexDemmo
    {
        public static void Main1()
        {
          var what =  String.Join(
                ", ",
                Regex.Matches("00101111110111101110", "1+")
                    .Cast<Match>()
                    .Select(m =>
                        m.Length == 1
                            ? m.Index + 1 + ""
                            : String.Format("{0} - {1}", m.Index + 1, m.Index + m.Length))
                    .ToArray());

            var w1 = Regex.Matches("00101111110111101110", "1+");

//嗯嗯，更好地利用了类库中正则表达式的功能，因此整体上更“函数式”一些，也更流畅了一些，
//不过基本思路是和之前的Ruby实现是一样的。不过好像……还是不够意思？没事，咱可以扩展嘛，简单扩展以后就可以变成这样了：

        }
    }
}

