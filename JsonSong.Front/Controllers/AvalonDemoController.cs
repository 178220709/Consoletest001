using System.Collections.Generic;
using System.Web.Mvc;
using JsonSong.Front.Extend;
using Suijing.Utils.Constants;

namespace JsonSong.Front.Controllers
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

        [HttpGet]
        [Module(Name = "Demo", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Demo()
        {
            return View();
        }

          public ActionResult Index(string pageName)
          {
                  return View(pageName);
          }
    }
}
