using System.Collections.Generic;
using System.Web.Mvc;
using JsonSong.ManagerUI.Extend;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace JsonSong.ManagerUI.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 20)]
    public class MyJsDemoController : Controller
    {
        [HttpGet]
        [Module(Name = "Index", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index()
        {
            return View();
        }


        [Module(Name = "DateTime", CSS = MyConstants.Bootstrap.Icon.User)]
        public ActionResult DateTime()
        {
            return View();
        }

        [Module(Name = "Closure", CSS = MyConstants.Bootstrap.Icon.User)]
        public ActionResult Closure1()
        {
            return View();
        }

        [Module(Name = "Base", CSS = MyConstants.Bootstrap.Icon.User)]
        public ActionResult Base()
        {
            return View();
        }   
          

    }
}
