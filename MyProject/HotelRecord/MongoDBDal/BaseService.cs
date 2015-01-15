using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MyProject.HotelRecord.Entity;

namespace MyProject.HotelRecord.MongoDBDal
{
    public class BaseService<T> where T : BaseEntity
    {

        #region MongoDB集合定义
        readonly string COLLECTION_NAME;
        public BaseService(string collectionName)
        {
            COLLECTION_NAME = collectionName;
        }

        protected MongoCollection<T> Collections
        {
            get
            {
                return Database.GetCollection<T>(COLLECTION_NAME, WriteConcern.Acknowledged);
            }
        }
        #endregion

        #region 在集合中添加实体对象
        public virtual WriteConcernResult AddEdit(T entity)
        {
            if (entity.Id < 1)
            {
                entity.Id = NewId;
            }
         return Collections.Save(entity);
        }
        #endregion

        #region 根据Id删除实体对象
        public virtual WriteConcernResult Delete(int id)
        {
            return Collections.Remove(Query.EQ("_id", id), RemoveFlags.Single);
        }
        #endregion 
        
        #region 
        public virtual IEnumerable<WriteConcernResult> AddList(IEnumerable<T> list)
        {
            var enumerable = list as IList<T> ?? list.ToList();
           
            foreach (var entity in enumerable)
            {
                if (entity.Id < 1)
                {
                    entity.Id = NewId;
                }
            }
            return  Collections.InsertBatch(enumerable);
        }
        #endregion

        #region 返回IQueryable对象
        public virtual IQueryable<T> Entities
        {
            get
            {
                return Collections.AsQueryable<T>();
            }
        }
        #endregion

        #region MongoDB访问字段设置
        MongoDatabase _database;
        readonly string DATABASE_NAME = "Hotel";
        readonly string CONNECTION_STRING = "mongodb://127.0.0.1";
        #endregion

        #region 数据库对象私有属性
        protected MongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var client = new MongoClient(CONNECTION_STRING);
                    var server = client.GetServer();
                    _database = server.GetDatabase(DATABASE_NAME);
                }
                return _database;
            }
        }
        #endregion

        #region 返回自增Id
        protected int NewId
        {
            get
            {
                var doc = Database.GetCollection("MySeqs").FindAndModify(new FindAndModifyArgs()
                {
                    Query = Query.EQ("_id", COLLECTION_NAME)
                    ,
                    SortBy = SortBy.Descending("seq"),
                    Update = Update.Inc("seq", 1)
                    ,
                    Upsert = true
                    ,
                    VersionReturned = FindAndModifyDocumentVersion.Modified
                });
                return doc.ModifiedDocument[1].AsInt32;
            }
        }
        #endregion

    }
}
