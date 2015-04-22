using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Fasterflect;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.MyHtmlAgility.Project.Youmin;
using MyProject.MyHtmlAgility.SpiderBase;
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
            var query = SpiderService.Instance. GetQueryByTypeId(typeId);
            model.Total = query.Count();
            model.Rows = query.Skip(model.Skip).Take(model.PageSize).ToList();
            return Json(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Update(BaseSpiderEntity model, int? typeId)
        {
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
