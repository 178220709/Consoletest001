using System.Web.Mvc;
using MyMvcDemo.Extend;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 31)]
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
        
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult ChkDemo()
        {
            return View();
        }

        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HttpDemo()
        {
            return View();
        }  
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult FilterDemo()
        {
            return View();
        }
    }
}
