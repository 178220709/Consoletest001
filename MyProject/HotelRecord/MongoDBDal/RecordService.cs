using System;
using System.Data;
using System.Text;
using System.Xml;
using BaseFeatureDemo.XML;
using JsonSong.BaseDao.MongoDB;
using MongoDB.Driver;
using MyProject.HotelRecord.Entity;


namespace MyProject.HotelRecord.MongoDBDal
{
    public class RecordService : BaseMongoDao<Record>
    {
        private const string _CollectionName = "HotelRecord";
        private RecordService(string collectionName)
            : base(collectionName)
        {
        }
        private static RecordService _Instance;

        public static RecordService Instance
        {
            get
            {
                if (_Instance==null)
                {
                    _Instance = new RecordService(_CollectionName);
                }
                return _Instance;
            }
        }

     


    }
}