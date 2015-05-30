using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Utility;


namespace JsonSong.BaseDao.LiteDb
{
    public abstract class BaseLiteDao<TEntity> where TEntity : BaseLiteEntity, new()
    {
        protected string Path { get; set; }
        protected string CnName { get; set; }
        protected LiteDatabase DB { get { return new LiteDatabase(Path); } }
        protected LiteCollection<TEntity> Con { get { return DB.GetCollection<TEntity>(CnName); } }

        protected BaseLiteDao(string dbName = "", string cnName = "")
        {
            Path =  PathHelper.GetDBPath(dbName);
            CnName = string.IsNullOrWhiteSpace(cnName) ? typeof(TEntity).Name.ToLower() : cnName;
        }

        #region Async Methods
        public long Insert(TEntity entity)
        {
            return Con.Insert(entity);
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
