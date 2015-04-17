using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.FromNode;
using MyProject.MyHtmlAgility.Project.SpiderBase;
using Newtonsoft.Json;
using Suijing.Utils.sysTools;

namespace MyProject.MyHtmlAgility.Project.SpiderCommon
{
    /// <summary>
    /// 同步远程的数据
    /// </summary>
    public static class SyncSpiderHelper
    {
        /// <summary>
        /// 根据para从远程获取数据 并绑定 可以用cnName 和AddedTime 控制
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="typeId"></param>
        public static void StartSync(IDictionary<string, string> paras, int typeId = 1)
        {
            paras = paras ?? new Dictionary<string, string>();
            paras["pageSize"] = "100";
           // var url = SpiderConstant.RemoteUrl;
            const string url = "http://localhost:18080/api/spider/GetSpiderList";
            int sum = 0;
            var pageTotal = 1;
            int pageIndex = 1;
            do
            {
                paras.SetDefault("cnName", SpiderConstant.CnNameDictionary[typeId]);
                var instance = SpiderServiceFactory.GetByTypeId(typeId);
                var postStr = HttpRestHelper.GetPost(url, paras);
                var dto = JsonConvert.DeserializeObject<SpiderRestDto>(postStr);
                pageTotal = dto.count/dto.pageSize +1;
                var list = MapFromDTO(dto).Where(a => !instance.ExistUrl(a.Url)).ToList();
                instance.AddList(list);
                pageIndex++;
                paras["pageIndex"] = pageIndex.ToString();
                sum += list.Count();
            } while (pageIndex <= pageTotal);

            LogHepler.WriteWebReader(string.Format("在{0}从{1}导入{2}条{3}数据",
                TestHelper.TestHelper.GetCurrentTime(), url, sum, paras["cnName"]));
            int result = sum;
        }

        private static List<BaseSpiderEntity> MapFromDTO(SpiderRestDto dto)
        {
            return dto.rows.Select(a => new BaseSpiderEntity()
            {
                Url = a.url,
                Content = a.content,
                Flag = a.flag,
                AddedTime = a.addedTime,
                Title = a.title,
                Weight = (int)a.weight,
            }).ToList();
        }

        public static void SyncTaskFun(string url ,IDictionary<string, string> paras)
        {
            
        }

        public static void SetDefault(this IDictionary<string, string> paras,string key,string value )
        {
            if (paras.ContainsKey(key))
            {
                return;
            }
            else
            {
                paras.Add(key,value);
            }
        }

        
      
    }
}