using System.Linq;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using JsonSong.Spider.Project.Haha;
using MyProject.MyHtmlAgility.SpiderBase;
using Newtonsoft.Json;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe, Name = "爬虫", Sort = 80)]
    public class SpiderController : Controller
    {
        [HttpGet]
        [Module(Name = "哈哈最新", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Haha()
        {
            var list = HahaWebReader.GetRecommand();
            return View(list);
        }
        [HttpGet]

        [Module(Name = "List", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaList()
        {
            return View();
        }

        #region ajax交互区

        [HttpPost]
        public JsonResult GetList(SpiderPagerModel model, int? typeId)
        {
            var query = SpiderService.Instance.GetQueryByTypeId(typeId);
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

            var model = JsonConvert.DeserializeObject<BaseSpiderEntity>(modelStr);

            return Json(new ResponseJsonModel()
            {
                success = SpiderService.Instance.UpdateContent(model)
            });
        }

        [HttpPost]
        public JsonResult Delete(string url, int? typeId)
        {
            return Json(new ResponseJsonModel()
            {
                success = SpiderService.Instance.DeleteByUrl(url)
            });
        }

        #endregion




    }
}
