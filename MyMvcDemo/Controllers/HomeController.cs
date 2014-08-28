using System.Collections.Generic;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyProject.WeixinModel.Model;

namespace MyMvcDemo.Controllers
{
    public class TestModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(CheckModel model)
        {
            model = model ?? new CheckModel();
            model.echostr = "testhahaha";
            return View(model);
        }  
        
        public ActionResult Index1()
        {
            return View();
        }


        public ActionResult JStest(int state, [ModelBinder(typeof(JsonBinder<int>))] IEnumerable<int> goodsIds)
        {
            return PartialView();
        }
    }
}
