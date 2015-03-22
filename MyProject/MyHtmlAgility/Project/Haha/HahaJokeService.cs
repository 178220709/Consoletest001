using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.SpiderBase;

namespace MyProject.MyHtmlAgility.Project.Haha
{
    internal class HahaJokeService 
    {
        protected const string _CollectionName = "HahaJoke";
       

        private static SpiderService _Instance;

        public static SpiderService Instance
        {
             get
            {
                if (_Instance==null)
                {
                    _Instance = new SpiderService(_CollectionName);
                }
                return _Instance;
            }
        }


    }
}