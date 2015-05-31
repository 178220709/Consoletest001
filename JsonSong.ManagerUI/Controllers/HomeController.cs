using System.Collections.Generic;
using System.Web.Mvc;
using JsonSong.ManagerUI.Extend;
using JsonSong.ManagerUI.Models;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace JsonSong.ManagerUI.Controllers
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
        public ActionResult Waiting()
        {
            return View();
        }
        public ActionResult Test()
        {
            var p1 = System.IO.Directory.GetCurrentDirectory();
            var p2 = HttpContext.Server.MapPath("~");
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
