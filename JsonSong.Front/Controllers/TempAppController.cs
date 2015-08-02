using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JsonSong.Front.Extend;
using MyProject.TempApp.DataAccess.DAO;
using Suijing.Utils.Constants;

namespace JsonSong.Front.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe, Sort = 9)]
    public class TempAppController : Controller
    {
        [HttpGet]
        [Module(Name = "TempApp", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Pioneer()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InjectTest()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {
            var list = ParticipantLiteDao.Instance.GetAll().OrderBy(a => a.AddedTime);
            return Json(list.Select(a => a.Name).ToList());
        }

        [HttpPost]
        public JsonResult Add(string name)
        {
            ParticipantLiteDao.Instance.AddNoRepeat(name);
            return Json("");
        }
    }
}
