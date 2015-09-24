using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Utility;


namespace JsonSong.BaseDao.LiteDb
{
    public  class BaseLiteDao<TEntity> where TEntity : BaseLiteEntity, new()
    {
        public string Path { get; set; }
        public string CnName { get; set; }
        public LiteDatabase DB { get { return new LiteDatabase(Path); } }

        public LiteCollection<TEntity> Con
        {
            get{return DB.GetCollection<TEntity>(CnName); }
        }

        public BaseLiteDao(string dbName  , string cnName = "")
        {
            Path =  PathHelper.GetDBPath(dbName);
            CnName = string.IsNullOrWhiteSpace(cnName) ? typeof(TEntity).Name.ToLower() : cnName;
        }

        #region Async Methods
        public int Insert(TEntity entity)
        {
            var idBson = Con.Insert(entity);
            return idBson.AsInt32;
        }
        public int Insert(IEnumerable<TEntity> entities)
        {
            return Con.Insert(entities);
        }
        public int Delete(long id)
        {
            Expression<Func<TEntity, bool>> predicate = entity => entity.Id == id;
            return Con.Delete(predicate);
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return Con.Delete(predicate);
        }

        public bool Update(TEntity entity)
        {
            return Con.Update(entity);
        }

        public IList<TEntity> GetAll()
        {
            return Con.FindAll().ToList();
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return Con.FindOne(predicate);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, int skip = 0, int limit = int.MaxValue)
        {
            return Con.Find(predicate, skip, limit);
        }
        public IEnumerable<TEntity> test()
        {
            var orderKeys = new Dictionary<string, int>()
            {
                {"AddedTime", Query.Descending},
                {"OtherFiled", Query.Ascending}
            };

            Func<TEntity, bool> predicate0 = t => t.AddedTime < DateTime.Now;



            Expression<Func<TEntity, bool>> predicate = t => t.AddedTime < DateTime.Now;
            var exp = PredicateBuilder.And(predicate, predicate);

            return null;
        }

        #endregion
    }
}
