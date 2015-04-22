using System;
using MyProject.MongoDBDal;

namespace MyProject.MyHtmlAgility.SpiderBase
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

    }
}