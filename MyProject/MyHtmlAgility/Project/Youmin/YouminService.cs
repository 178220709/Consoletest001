using System.Linq;
using MongoDB.Driver;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Core;

namespace MyProject.MyHtmlAgility.Project.Youmin
{
    public class YouminService : BaseService<BaseSpiderEntity>
    {
        private const string _CollectionName = "Youmin";
        private YouminService(string collectionName)
            : base(collectionName)
        {
        }
        private static YouminService _Instance;

        public static YouminService Instance
        {
            get
            {
                if (_Instance==null)
                {
                    _Instance = new YouminService(_CollectionName);
                }
                return _Instance;
            }
        }


        /// <summary>
        /// 更新content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateHaha(BaseSpiderEntity model)
        {
            var manager = YouminService.Instance;
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
}