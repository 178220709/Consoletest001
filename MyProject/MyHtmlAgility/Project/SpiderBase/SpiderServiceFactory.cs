using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.MyHtmlAgility.Project.SpiderCommon;
using MyProject.MyHtmlAgility.Project.Youmin;

namespace MyProject.MyHtmlAgility.Project.SpiderBase
{
    public  class SpiderServiceFactory
    {
        private static readonly Dictionary<int, SpiderService> DicServices = SpiderConstant.DicServices;
        private static readonly Dictionary<int, string> CnNameDictionary = SpiderConstant.CnNameDictionary;
        public static SpiderService GetByTypeId(int typeId)
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