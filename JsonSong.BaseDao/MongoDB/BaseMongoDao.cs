using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.WireProtocol.Messages;

namespace JsonSong.BaseDao.MongoDB
{
    public class BaseMongoDao<TEntity> : IDisposable where TEntity : BaseMongoEntity
    {
        #region Properties

        protected IMongoCollection<TEntity> NewCollection { get; private set; }

        protected IMongoDatabase DataBase { get; private set; }

        #endregion

        #region Constructors

        public BaseMongoDao(string cnName="") :
            this(cnName,DataBaseManager.ConnectionKey)
        {
           
        }

        public BaseMongoDao(string cnName = "", string key = "")
        {
            cnName = cnName ?? typeof (TEntity).Name.ToLower();
            var db = DataBaseManager.GetDatabaseByKey(key);
            DataBase = db;
            NewCollection = db.GetCollection<TEntity>(cnName);
        }

        /// <summary>
        /// The default connectionKey is "MongoDB", you can change it here.
        /// </summary>
        /// <param name="key"></param>
        protected static void InitConnectionKey(string key)
        {
            DataBaseManager.ConnectionKey = key;
        }
        #endregion

        #region Async Methods
        public async Task InsertOneAsync(TEntity entity)
        {
           await NewCollection.InsertOneAsync(entity);
        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            await NewCollection.InsertManyAsync(entities);
        }

        public async Task ReplaceOneAsync(TEntity entity)
        {
            await NewCollection.ReplaceOneAsync(a => a.Id == entity.Id, entity);
        }

        public async Task DeleteOneAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await NewCollection.FindOneAndDeleteAsync(filter);
        } 
        public async Task DeleteOneAsync(string  idStr)
        {
            ObjectId id;
            if (ObjectId.TryParse(idStr, out id))
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                await NewCollection.FindOneAndDeleteAsync(filter);
            }
            else
            {
                throw new Exception("idStr is incorrect");
            }
        }

        public async Task<TEntity> FindOneAsync(string id)
        {
            var objId = ObjectId.Parse(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objId);

            return await NewCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await NewCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await NewCollection.Find(filter).ToListAsync();
        }

        public  IAggregateFluent<TEntity> Aggregate(AggregateOptions options)
        {
            return  NewCollection.Aggregate(options);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await NewCollection.CountAsync(filter);
        } 
          
        public  IFindFluent<TEntity, TEntity> GetQuery(Expression<Func<TEntity, bool>> filter)
        {
             

            return  NewCollection.Find(filter);
        
        } 
        
      

        #endregion



        #region Dispose

        public void Dispose()
        {

        }

        #endregion
    }
}
