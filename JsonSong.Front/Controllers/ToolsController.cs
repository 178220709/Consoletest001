using System.Web.Mvc;
using JsonSong.Front.Extend;
using Suijing.Utils.Constants;

namespace JsonSong.Front.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 31)]
    public class ToolsController : JsonNetController
    {
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index()
        {
            return View();
        }
       
        [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Name = "js2coffee")]
        public ActionResult Js2Coffee()
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
