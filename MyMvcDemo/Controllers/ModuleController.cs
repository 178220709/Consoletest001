using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using MyProject.WeixinModel.Model;
using Suijing.Utils.Constants;

namespace MyMvcDemo.Controllers
{
    public class ModuleController : Controller
    {
     

        public JsonResult GetAllModules()
        {
           var model = new IndexModel();
           var modules = ControllerHelper.GetIndexModules();

           return Json(modules.Select(a => new
           {
               Name = a.Name,
               VName = a.VName,
               Url = a.Url
           }));
        }

    }
}
