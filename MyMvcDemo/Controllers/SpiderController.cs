using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.Haha;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
      [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Name = "爬虫",Sort = 80)]
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
        public ActionResult HahaTop()
        {
            var list = HahaJokeService.Instance.Entities.Take( 16).ToList(); 
            return View(list);
        }
       [Module(Name = "哈哈List", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaList()
        {
            return View();
        }


        #region ajax交互区

       [HttpPost]
       public JsonResult GetHahaList(HahaPagerModel model )
       {
           model.Total = HahaJokeService.Instance.Entities.Count();
           model.Rows = HahaJokeService.Instance.Entities.Skip(model.Skip).Take(model.PageSize).ToList();
           return Json(model);
       }

       [ValidateInput(false)]
       [HttpPost]
       public JsonResult UpdateHaha(BaseSpiderEntity model )
       {
           return Json(new ResponseJsonModel()
           {
               success = HahaJokeService.Instance.UpdateHaha(model)
           });
       }

       #endregion




    }
}
