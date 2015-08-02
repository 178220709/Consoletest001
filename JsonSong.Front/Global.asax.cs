using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LiteDbLog.Facade;
using LiteDbLog.LiteDBLog;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.sysTools;

namespace JsonSong.Front
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
          //  LogHelper.SetConfig();
            DBLogInstances.System.Log("the mvcDemo is start up",level:DBLogLevelEnum.Debugger);
            var path = JSConfigHelper.GetJsConfigPath();
            DBLogInstances.System.Log("JSConfigHelper.GetJsConfigPath " + path, level: DBLogLevelEnum.Debugger);

        }
    }
}
