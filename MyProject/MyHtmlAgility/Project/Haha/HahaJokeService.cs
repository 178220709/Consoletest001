using System;
using System.Linq;
using MongoDB.Driver;
using MyProject.MongoDBDal;

namespace MyProject.MyHtmlAgility.Project.Haha
{
    public class HahaJokeService : BaseService<JokeEntity>
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
        public bool UpdateHaha(JokeEntity model)
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
    }

    public class JokeEntity : BaseEntity
    {
        public int Flag { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string StyleStr { get; set; }
        public DateTime Date { get; set; }
        public int Weight { get; set; }

    }

}