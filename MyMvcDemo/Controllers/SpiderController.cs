using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.WeixinModel.Model;
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
            var list = HahaJokeService.Instance.Entities.Take(() => 16).ToList(); 
            return View(list);
        }
       [Module(Name = "test", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Test()
        {
          
            return View();
        }   
    }
}
