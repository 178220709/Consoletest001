using System.Threading;

namespace JsonSong.BaseDao.Common
{
    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }


    public static class PagerEx
    {
        public static int GetSkip(this Pager pager)
        {
            return (pager.PageIndex - 1)*pager.PageSize;
        }
    }
}
