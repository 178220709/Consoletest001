using System.Collections.Generic;
using System.Web.Mvc;
using JsonSong.ManagerUI.Extend;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace JsonSong.ManagerUI.Controllers
{
      [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 10)]
    public class SeaJSDemoController : Controller
    {
        [HttpGet]
        [Module(Name = "SeaJSDemo", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Hello()
        {
            return View();
        }

        [Module(Name = "SeaJSDemo", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Underscore()
        {
            return View();
        }  
    }
}
