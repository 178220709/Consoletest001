using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
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
          

    }
}
