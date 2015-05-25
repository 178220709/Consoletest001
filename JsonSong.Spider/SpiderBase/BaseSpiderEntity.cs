using System;
using JsonSong.BaseDao.MongoDB;
using MongoDB.Bson;

namespace JsonSong.Spider.SpiderBase
{
    public class BaseSpiderEntity : BaseEntity
    {
        public BaseSpiderEntity()
        {
            AddedTime = DateTime.Now;
        }
        public int TypeId { get; set; }
        public string Flag { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string StyleStr { get; set; }
        public DateTime AddedTime { get; set; }
        public int Weight { get; set; }
        public bool Valid { get; set; }

    }


}