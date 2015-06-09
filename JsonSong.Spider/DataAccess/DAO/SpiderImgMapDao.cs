using System;
using System.Collections.Generic;
using System.Linq;
using JsonSong.BaseDao.LiteDb;
using JsonSong.Spider.Core;
using JsonSong.Spider.DataAccess.Entity;
using LiteDB;
using MongoDB.Bson;
using Omu.ValueInjecter;

namespace JsonSong.Spider.DataAccess.DAO
{
    public  class SpiderImgMapDao : BaseLiteDao<SpiderImgMapEntity>
    {
        private SpiderImgMapDao(string path, string cnName)
            : base(path, cnName)
        {
            
        }

        private static SpiderImgMapDao _instance;

        public static SpiderImgMapDao Instance
        {
            get
            {
                return _instance ?? (_instance = new SpiderImgMapDao("spiderImgMap", "spiderImgMap"));
            }
        }



        public void AddNoRepeat(SpiderImgMapEntity en )
        {
            var entity = GetByUrl(en.Url);
            if (entity == null)
            {
               
                Insert(en);
            }
            else
            {
                return;
            }
        }


      

        //将Flag 转换为string类型
        //    db.HahaJoke.find().forEach(function(x){
        //x.Flag=x.Flag+"";
        //db.HahaJoke.save(x)})

        public  int DeleteByUrl(string url)
        {
            
            return this.Con.Delete(a=>a.Url==url);
        }

      
        public  bool ExistUrl(string url)
        {
            var first = GetByUrl(url);
            return first != null;
        }

        public SpiderImgMapEntity GetByUrl(string url)
        {
            return  FindOne(a => a.Url == url); ;
        }

        public LiteCollection<SpiderImgMapEntity> GetCon()
        {
            return  this.Con;
        }


        public DateTime GetLastUpdateTime(int typeId=1)
        {
            var last = Con.Find(Query.And(Query.All("AddedTime", Query.Descending), Query.EQ("TypeId", typeId)), 0, 1).FirstOrDefault();
            return last==null? DateTime.MinValue:last.AddedTime;
        }
    }
}