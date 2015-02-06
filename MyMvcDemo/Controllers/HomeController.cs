using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{

    [Module(Name = "主页",CSS = MyConstants.Bootstrap.Icon.Globe , Sort = 0)]
    public class HomeController : Controller
    {
        [HttpGet]
      
        public ActionResult Index(IndexModel model)
        {
            model = model ?? new IndexModel();

            model.Admin = new AdministratorDTO()
            {
                Name = "碎景"
            };
            model.Modules = ControllerHelper.GetIndexModules();
            return View(model);
        }
         [Module(Name = "首页", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult IndexContent()
        {
            return View();
        }
         [Module(Name = "目录", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult ContextContent()
        {
            return View();
        }

        [Module(Name = "LTEDemo1", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index1()
        {
            return View();
        }


        public ActionResult LteDemo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return View("LteDemos/index");
            }
            var viewPath = string.Format("LteDemos/{0}", path.Replace(".html",""));
            return View(viewPath);
        }

       

    }
}
