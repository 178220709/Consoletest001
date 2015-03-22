using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.MyHtmlAgility.Project.Youmin;

namespace MyProject.MyHtmlAgility.Project.SpiderBase
{
    public  class SpiderServiceFactory  
    {
        private  static readonly Dictionary<int , SpiderService>  DicServices = new Dictionary<int, SpiderService>()
        {
            {1, HahaJokeService.Instance},
            {2, YouminService.Instance}
        };
        public static SpiderService GetByTypeId(int typeId )
        {
            if (DicServices.ContainsKey(typeId))
            {
                return DicServices[typeId];
            }
            else
            {
                return DicServices[1];
            }
        }

        public static SpiderService GetByTypeId(int? typeId)
        {
            int type = typeId ?? 1;
            return GetByTypeId(type);
        }
    }
}