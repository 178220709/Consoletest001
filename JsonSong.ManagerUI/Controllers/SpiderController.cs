using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using JsonSong.ManagerUI.Extend;
using JsonSong.Spider.DataAccess.DAO;
using JsonSong.ManagerUI.Models;
using JsonSong.Spider.DataAccess.Entity;
using JsonSong.Spider.Project.Haha;
using JsonSong.Spider.SpiderBase;
using Newtonsoft.Json;
using Suijing.Utils.Constants;

namespace JsonSong.ManagerUI.Controllers
{
    [Module(CSS = MyConstants.Bootstrap.Icon.Globe, Name = "爬虫", Sort = 80)]
    public class SpiderController : Controller
    {
        [HttpGet]
        [Module(Name = "哈哈最新", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public async Task<ActionResult> Haha()
        {
            var list = await HahaWebReader.GetRecommand();
            return View(list);
        }
        [HttpGet]

        [Module(Name = "List", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HahaList()
        {
            return View();
        }

        #region ajax交互区

        [HttpPost]
        public JsonResult GetList(SpiderPagerModel model, int? typeId)
        {
            var query = SpiderLiteDao.Instance.GetQueryByTypeId(typeId).ToList();
            model.Total = query.Count();
            model.Rows = query.Skip(model.Skip).Take(model.PageSize).ToList();
            return Json(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Update(string modelStr)
        {
            if (modelStr.ToLower().Contains("script"))
            {
                return Json(new ResponseJsonModel()
                {
                    success = false,
                    msg = "不允许有script标记"
                });
            }

            var model = JsonConvert.DeserializeObject<SpiderLiteEntity>(modelStr);

            return Json(new ResponseJsonModel()
            {
                success = SpiderLiteDao.Instance.UpdateContent(model)
            });
        }

        [HttpPost]
        public JsonResult Delete(string url, int? typeId)
        {
            return Json(new ResponseJsonModel()
            {
                success = SpiderLiteDao.Instance.DeleteByUrl(url)
            });
        }

        #endregion




    }
}
