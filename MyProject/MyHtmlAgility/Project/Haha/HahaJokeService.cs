using System.Linq;
using MongoDB.Driver;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Core;

namespace MyProject.MyHtmlAgility.Project.Haha
{
    public class HahaJokeService : BaseService<BaseSpiderEntity>
    {
        private const string _CollectionName = "HahaJoke";
        private HahaJokeService(string collectionName)
            : base(collectionName)
        {
        }
        private static HahaJokeService _Instance;

        public static HahaJokeService Instance
        {
            get
            {
                if (_Instance==null)
                {
                    _Instance = new HahaJokeService(_CollectionName);
                }
                return _Instance;
            }
        }

        public CommandResult DeleteAll()
        {
          //  return Collections.Drop();
          return Collections.RemoveAll();
        }

        /// <summary>
        /// 更新content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateHaha(BaseSpiderEntity model)
        {
            var manager = HahaJokeService.Instance;
            var entity = manager.Entities.FirstOrDefault(a => a.Flag == model.Flag);
            if (entity == null)
            {
                return false;
            }
            entity.Content = model.Content;
            manager.AddEdit(entity);
            return true;
        }

        //将Flag 转换为string类型
    //    db.HahaJoke.find().forEach(function(x){
    //x.Flag=x.Flag+"";
    //db.HahaJoke.save(x)})

    }
}