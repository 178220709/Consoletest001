using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonSong.Spider.DataAccess.Entity;
using JsonSong.Spider.FromNode;
using JsonSong.Spider.SpiderBase;
using Newtonsoft.Json;
using Suijing.Utils.sysTools;

namespace JsonSong.Spider.SpiderCommon
{
    /// <summary>
    /// 同步远程的数据
    /// </summary>
    public static class SpiderDownloadHelper
    {
        /// <summary>
        /// 将content中的图片下载到本地
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="typeId"></param>
        public static async Task StartSync(IDictionary<string, string> paras, int typeId = 1)
        {
            
        }

        private static List<SpiderMongoEntity> MapFromDTO(SpiderRestDto dto, int typeId)
        {
            return dto.rows.Select(a => new SpiderMongoEntity()
            {
                TypeId = typeId,
                Url = a.url,
                Content = a.content,
                Flag = a.flag,
                AddedTime = a.addedTime,
                Title = a.title,
                Weight = (int) a.weight,
            }).ToList();
        }

        public static void SyncTaskFun(string url, IDictionary<string, string> paras)
        {
        }

        public static void SetDefault(this IDictionary<string, string> paras, string key, string value)
        {
            if (paras.ContainsKey(key))
            {
                return;
            }
            else
            {
                paras.Add(key, value);
            }
        }


        public static SpiderService GetSpiderCn(string cnName)
        {
            return new SpiderService(cnName);
        }
    }
}