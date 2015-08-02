using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JsonSong.Front.Extend;
using JsonSong.Spider.FromNode;
using LiteDbLog.Facade;
using LiteDbLog.LiteDBLog;
using Newtonsoft.Json;
using Suijing.Utils.Constants;

namespace JsonSong.Front.Controllers
{
      [Module(CSS = MyConstants.Bootstrap.Icon.Globe,Sort = 80,Name = "系统调试")]
    public class SystemController : Controller
    {
        [HttpGet]
        [Module(Name = "Log", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult LogIndex(string path,string name)
        {
            return View();
        }

        [HttpGet]
        [Module(Name = "DBLog", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult DBLogIndex(string path, string name)
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDbLogList(string name,DBLogLevelEnum level, string content)
        {
          var list =  DBLogInstances.GetnstanceByName(name).Find(a => a.Level == level).ToList();
            var lis2 = DBLogInstances.Spider.GetAll();
            if (!string.IsNullOrWhiteSpace(content))
          {
              list = list.Where(a => a.Content.Contains(content)).ToList();
          }
            return Json(new ResponseJsonModel()
            {
                success = true,
                rows = list,
            });
        }


        [HttpGet]
        [Module(Name = "SysInfo", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult SysInfoIndex(string path, string name)
        {
            return View();
        } 
          
        [HttpGet]
        [Module(Name = "HttpDebugger", CSS = MyConstants.Bootstrap.Icon.Globe)]
        public ActionResult HttpDebugger(string path, string name)
        {
            return View();
        }



        [HttpPost]
        public ContentResult GetHttpResult(string url , string paras)
        {
            var dic = JsonConvert.DeserializeObject<IDictionary<string, string>>(paras);
            var result = HttpRestHelper.GetPost(url, dic);

          return Content(result);
        }   



        [HttpPost]
        public JsonResult GetLog(string path, string name)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name))
            {
                return Json(new ResponseJsonModel
                {
                    success = false,
                    msg = "非法参数"
                });
            }

            return Json(new ResponseJsonModel
            {
                success = true,
                result = ViewLogHelp.GetrFileContent(path, name)
            });
        }   


        [HttpPost]
        public JsonResult GetLogByFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName) )
            {
                return Json(new ResponseJsonModel
                {
                    success = false,
                    msg = "非法参数"
                });
            }

            return Json(new ResponseJsonModel
            {
                success = true,
                result = ViewLogHelp.GetrFileContent(fullName)
            });
        }   

    }
}
