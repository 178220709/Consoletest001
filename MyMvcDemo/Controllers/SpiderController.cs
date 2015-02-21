using System.Collections.Generic;
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
        [Module(Name = "哈哈", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Haha()
        {
            var list = HahaWebReader.GetRecommand();
            return View(list);
        }

       [Module(Name = "test", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Test()
        {
          
            return View();
        }   
    }
}
