using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Core;
using Omu.ValueInjecter;

namespace MyProject.MyHtmlAgility.SpiderBase
{
    public  class SpiderService : BaseService<BaseSpiderEntity>
    {
        public SpiderService(string collectionName)
            : base(collectionName)
        {
           
        }

        private static SpiderService _spiderService;

        public static SpiderService Instance
        {
            get
            {
                if (_spiderService==null)
                {
                    _spiderService = new SpiderService("spider");
                }
                return _spiderService;
            }
        }


        public  IQueryable<BaseSpiderEntity> GetQueryByTypeId(int typeId)
        {
            return this.Entities.Where(a => a.TypeId == typeId);
        }

        public  IQueryable<BaseSpiderEntity> GetQueryByTypeId(int? typeId)
        {
            var type = typeId ?? 1;
            return GetQueryByTypeId(type);
        }


        public void AddNoRepeat( ReadResult re, int typeId=1)
        {
            if (this.Entities.Any(a => a.Url == re.Url))
            {
               return ;
            }
            var en = new BaseSpiderEntity {TypeId = typeId};
            en.InjectFrom(re);
            this.AddEdit(en);
        }


        /// <summary>
        /// 更新content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContent(BaseSpiderEntity model)
        {

            var entity = this.Entities.FirstOrDefault(a => a.Url == model.Url);
            if (entity == null)
            {
                return false;
            }
            entity.Content = model.Content;
            this.AddEdit(entity);
            return true;
        }

        //将Flag 转换为string类型
        //    db.HahaJoke.find().forEach(function(x){
        //x.Flag=x.Flag+"";
        //db.HahaJoke.save(x)})

        public bool DeleteByUrl(string url)
        {
            var re = this.Collections.Remove(Query.EQ("Url", url), RemoveFlags.Single);
            return re.Ok;
        }

      
        public bool ExistUrl(string s)
        {
           return Entities.Count(a => a.Url == s) > 0;
        }
    }
}