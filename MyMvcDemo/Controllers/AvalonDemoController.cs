using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
      [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 1)]
    public class AvalonDemoController : Controller
    {
        [HttpGet]
        [Module(Name = "BaseBind", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Hello()
        {
            return View();
        }


          public ActionResult Index(string pageName)
          {
                  return View(pageName);
          }
    }
}
