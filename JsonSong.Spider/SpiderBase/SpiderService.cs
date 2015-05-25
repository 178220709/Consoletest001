using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonSong.BaseDao.MongoDB;
using JsonSong.Spider.Core;
using MongoDB.Driver;
using Omu.ValueInjecter;

namespace JsonSong.Spider.SpiderBase
{
    public  class SpiderService : BaseDao<BaseSpiderEntity>
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
         public  IQueryable<BaseSpiderEntity> GetQuery()
         {
             return this.NewCollection.AsQueryable().AsQueryable();
         }
         public IQueryable<BaseSpiderEntity> Entities { get { return GetQuery(); } }
        
        

        public async Task<IEnumerable<BaseSpiderEntity>> GetQueryByTypeId(int typeId)
        {
            return await FindAsync(a=>a.TypeId==typeId);
        }

        public async Task<IEnumerable<BaseSpiderEntity>> GetQueryByTypeId(int? typeId)
        {
            var type = typeId ?? 1;
            return await GetQueryByTypeId(type);
        }


        public async Task AddNoRepeat( ReadResult re, int typeId=1)
        {
            var entity = await GetByUrlAsync(re.Url);
            if (entity == null)
            {
                return ;
            }
            var en = new BaseSpiderEntity {TypeId = typeId};
            en.InjectFrom(re);
           await this.InsertOneAsync(en);
        }


        /// <summary>
        /// 更新content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContent(BaseSpiderEntity model)
        {

            var entity = await GetByUrlAsync(model.Url);
            if (entity == null)
            {
                return false;
            }
            entity.Content = model.Content;
            await ReplaceOneAsync(entity);
            return true;
        }

        //将Flag 转换为string类型
        //    db.HahaJoke.find().forEach(function(x){
        //x.Flag=x.Flag+"";
        //db.HahaJoke.save(x)})

        public async Task<bool> DeleteByUrl(string url)
        {
            var entity = await GetByUrlAsync(url);
            if (entity == null)
            {
                return false;
            }
            entity.Valid = false;
            await ReplaceOneAsync(entity);
            return true;
        }

      
        public  bool ExistUrl(string url)
        {
            var first = this.NewCollection.AsQueryable().FirstOrDefault(a => a.Url == url);
            return first != null;
        }

        public async Task<BaseSpiderEntity> GetByUrlAsync(string url)
        {
            return await FindOneAsync(a => a.Url == url); ;
        }
    }
}