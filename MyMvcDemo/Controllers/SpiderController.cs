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
using MyProject.MyHtmlAgility.Project.SpiderBase;
using MyProject.MyHtmlAgility.Project.Youmin;
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
        [Module(Name = "哈哈top16", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaTop(int? typeId)
        {
            var list = SpiderServiceFactory.GetByTypeId(typeId).Entities.Take(16).ToList();
            return View(list);
        }
        [Module(Name = "List", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaList()
        {
            return View();
        }

        #region ajax交互区

        [HttpPost]
        public JsonResult GetList(SpiderPagerModel model, int? typeId)
        {
            var ins = SpiderServiceFactory.GetByTypeId(typeId);
            model.Total = ins.Entities.Count();
            model.Rows = ins.Entities.Skip(model.Skip).Take(model.PageSize).ToList();
            return Json(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Update(BaseSpiderEntity model, int? typeId)
        {
            return Json(new ResponseJsonModel()
            {
                success = SpiderServiceFactory.GetByTypeId(typeId).UpdateContent(model)
            });
        }

        [HttpPost]
        public JsonResult Delete(string flag, int? typeId)
        {
            return Json(new ResponseJsonModel()
            {
                success = SpiderServiceFactory.GetByTypeId(typeId).DeleteByFlag(flag)
            });
        }

        #endregion




    }
}
