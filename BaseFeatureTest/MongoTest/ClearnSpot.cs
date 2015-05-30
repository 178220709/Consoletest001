using System;
using JsonSong.BaseDao.MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace BaseFeatureTest.MongoTest
{
    [BsonIgnoreExtraElements]
    public class ClearnSpot :BaseMongoEntity
    {
        /// <summary>
        /// 0待审核 1已审核 2审核驳回
        /// </summary>
        public int Status { get; set; }
        public DateTime InClearnTime { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public string Specification { get; set; }
        public string Factory { get; set; }

        public string Operator { get; set; }

        public DateTime OperationTime { get; set; }

        public string StandardName { get; set; }

        public string CategoryCode { get; set; }

        /// <summary>
        /// 规则的id的ToString
        /// </summary>
        public string RuleId { get; set; }

       //用在管理系统里面的clearnSpot无需添加此属性
      //  public Spot Spot { get; set; }
    }
}