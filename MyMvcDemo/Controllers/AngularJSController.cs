using System.Web.Mvc;
using MyMvcDemo.Extend;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
    public class AngularJSController : JsonNetController
    {
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index()
        {
            return View();
        }
       
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult ModelBind()
        {
            return View();
        }
    }
}
