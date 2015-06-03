using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using JsonSong.ManagerUI.Extend;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.ManagerUI.Models;
using JsonSong.Spider.DataAccess.Entity;
using JsonSong.Spider.Project.Haha;
using JsonSong.Spider.Project.Youmin;
using JsonSong.Spider.SpiderBase;
using Newtonsoft.Json;
using Suijing.Utils.Constants;
using Suijing.Utils.Utility;

namespace JsonSong.ManagerUI.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe, Name = "爬虫", Sort = 80)]
    public class SpiderController : Controller
    {
        [HttpGet]
        [Module(Name = "功能清单", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        [Module(Name = "数据展示", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaList()
        {
            return View();
        }

        #region ajax交互区

        [HttpPost]
        public JsonResult GetList(SpiderPagerModel model, int? typeId)
        {
            var query = SpiderLiteDao.Instance.GetQueryByTypeId(typeId).ToList();
            model.Total = query.Count();
            model.Rows = query.Skip(model.Skip).Take(model.PageSize).ToList();
            return Json(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Update(string modelStr)
        {
            if (modelStr.ToLower().Contains("script"))
            {
                return Json(new ResponseJsonModel()
                {
                    success = false,
                    msg = "不允许有script标记"
                });
            }

            var model = JsonConvert.DeserializeObject<SpiderLiteEntity>(modelStr);

            return Json(new ResponseJsonModel()
            {
                success = SpiderLiteDao.Instance.UpdateContent(model)
            });
        }

        [HttpPost]
        public JsonResult Delete(string url, int? typeId)
        {
            return Json(new ResponseJsonModel()
            {
                success = SpiderLiteDao.Instance.DeleteByUrl(url)
            });
        }


        //index

        [HttpPost]
        public async Task<JsonResult> SpiderRecommand(int? typeId)
        {
            int type = typeId ?? 0;
            try
            {
                if (type==0)
                {
                    var list1 = await HahaWebReader.GetRecommand();
                    var list2 = await YouminWebReader.GetRecommand();
                }
                else
                {
                    if (type==1)
                    {
                        var list1 = await HahaWebReader.GetRecommand();
                    }
                    else if (type == 2)
                    {
                        var list2 = await YouminWebReader.GetRecommand();
                    }
                }
            }
            catch (Exception ex)
            {
                var ex2 = ex;
            }
           
            return Json(new ResponseJsonModel()
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult GetSpiderInfo(int? typeId)
        {
            var count1 = SpiderLiteDao.Instance.GetCon().Count(a => a.Valid && a.TypeId == 1);
            var count2 = SpiderLiteDao.Instance.GetCon().Count(a => a.Valid && a.TypeId == 2);
            var updateTime1 = SpiderLiteDao.Instance.GetLastUpdateTime(1).ToMinTime();
            var updateTime2 = SpiderLiteDao.Instance.GetLastUpdateTime(2).ToMinTime();

            return Json(new ResponseJsonModel()
            {
                result = new List<string>
                {
                   string.Format("type{0}  count={1},updateTime = {2} ",1,count1,updateTime1),
                   string.Format("type{0}  count={1},updateTime = {2} ",2,count2,updateTime2),

                }
            });
        }


        #endregion




    }
}
