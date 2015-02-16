using System;
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