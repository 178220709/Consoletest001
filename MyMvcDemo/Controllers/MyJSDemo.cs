using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.WeixinModel.Model;

namespace MyMvcDemo.Controllers
{

    public class MyJSDemo : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
