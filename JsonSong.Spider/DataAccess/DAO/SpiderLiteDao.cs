using System;
using System.Collections.Generic;
using System.Linq;
using JsonSong.BaseDao.LiteDb;
using JsonSong.Spider.Core;
using JsonSong.Spider.DataAccess.Entity;
using LiteDB;
using Omu.ValueInjecter;

namespace JsonSong.Spider.DataAccess.DAO
{
    public  class SpiderLiteDao : BaseLiteDao<SpiderLiteEntity>
    {
        private SpiderLiteDao(string path,string cnName):base(path, cnName)
        {
            
        }

        private static SpiderLiteDao _instance;

        public static SpiderLiteDao Instance
        {
            get
            {
                return _instance ?? (_instance = new SpiderLiteDao("spider", "spider"));
            }
        } 

        public  IEnumerable<SpiderLiteEntity> GetQueryByTypeId(int? typeId)
        {
            var type = typeId ?? 1;
            return  GetQueryByTypeId(type);
        }

        public IEnumerable<SpiderLiteEntity> GetQueryByTypeId(int typeId)
        {
            return Find(a => a.TypeId == typeId);
        }


        public void AddNoRepeat( ReadResult re, int typeId=1)
        {
            var entity =   GetByUrl(re.Url);
            if (entity == null)
            {
                var en = new SpiderLiteEntity { TypeId = typeId };
                en.InjectFrom(re);
                Insert(en);
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 更新content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContent(SpiderLiteEntity model)
        {
            var entity =  GetByUrl(model.Url);
            if (entity == null)
            {
                return false;
            }
            entity.Content = model.Content;
            return Update(entity);
        }

        //将Flag 转换为string类型
        //    db.HahaJoke.find().forEach(function(x){
        //x.Flag=x.Flag+"";
        //db.HahaJoke.save(x)})

        public bool DeleteByUrl(string url)
        {
            var entity =  GetByUrl(url);
            if (entity == null)
            {
                return false;
            }
            entity.Valid = false;
            return Update(entity);
        }

      
        public  bool ExistUrl(string url)
        {
            var first = GetByUrl(url);
            return first != null;
        }

        public SpiderLiteEntity GetByUrl(string url)
        {
            return  FindOne(a => a.Url == url); ;
        }

        public LiteCollection<SpiderLiteEntity> GetCon()
        {
            return  this.Con;
        }


        public DateTime GetLastUpdateTime()
        {
            var last = Con.Find(Query.All("AddedTime", Query.Descending), 0, 1).FirstOrDefault();
            return last==null? DateTime.MinValue:last.AddedTime;
        }
    }
}