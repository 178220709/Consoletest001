using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyProject.MongoDBDal;

namespace MyProject.MyHtmlAgility.Project.SpiderBase
{
    public  class SpiderService : BaseService<BaseSpiderEntity>
    {
        public SpiderService(string collectionName)
            : base(collectionName)
        {
           
        }

        /// <summary>
        /// ����content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContent(BaseSpiderEntity model)
        {

            var entity = this.Entities.FirstOrDefault(a => a.Flag == model.Flag);
            if (entity == null)
            {
                return false;
            }
            entity.Content = model.Content;
            this.AddEdit(entity);
            return true;
        }

        //��Flag ת��Ϊstring����
        //    db.HahaJoke.find().forEach(function(x){
        //x.Flag=x.Flag+"";
        //db.HahaJoke.save(x)})

        public bool DeleteByFlag(string flag)
        {
            var re = this.Collections.Remove(Query.EQ("Flag", flag), RemoveFlags.Single);
            return re.Ok;
        }
    }
}