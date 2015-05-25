using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using Suijing.Utils.ConfigTools;


namespace JsonSong.BaseDao.LiteDb
{
    public abstract class BaseDao<TEntity> where TEntity : BaseEntity, new()
    {



        protected string Path { get; set; }
        protected string CnName { get; set; }
        protected LiteDatabase DB { get { return new LiteDatabase(Path); } }
        protected LiteCollection<TEntity> Con { get { return new LiteDatabase(Path).GetCollection<TEntity>(CnName); } }

        protected BaseDao(string path="",string cnName="")
        {
            Path = string.IsNullOrWhiteSpace(path) ? PathHelper.GetDBPath() : path;
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
        public int Delete( long id)
        {
            Expression<Func<TEntity, bool>> predicate = entity => entity.Id==id;
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

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate,int skip=0,int limit = int.MaxValue)
        {
            return Con.Find(predicate,skip,limit);
        }


        #endregion
    }
}
