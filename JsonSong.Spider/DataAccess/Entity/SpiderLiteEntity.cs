using System;
using JsonSong.BaseDao.LiteDb;
using LiteDB;

namespace JsonSong.Spider.DataAccess.Entity
{
    public class SpiderLiteEntity : BaseLiteEntity
    {
        public SpiderLiteEntity()
        {
            Valid = true;
        }

        public int TypeId { get; set; }
        public string Flag { get; set; }
        [BsonIndex]
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StyleStr { get; set; }
        public int Weight { get; set; }
        public bool Valid { get; set; }
    }
}