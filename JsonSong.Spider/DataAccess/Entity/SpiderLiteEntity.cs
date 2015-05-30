using System;
using JsonSong.BaseDao.LiteDb;

namespace JsonSong.Spider.DataAccess.Entity
{
    public class SpiderLiteEntity : BaseLiteEntity
    {
        public SpiderLiteEntity()
        {

        }

        public int TypeId { get; set; }
        public string Flag { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StyleStr { get; set; }
        public int Weight { get; set; }
        public bool Valid { get; set; }
    }
}