using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
      [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 60)]
    public class MyQunitDemoController : Controller
    {
        [HttpGet]
        [Module(Name = "Index", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Hello()
        {
            return View();
        }   
          

    }
}
