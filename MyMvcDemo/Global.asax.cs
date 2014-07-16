using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.Text;

namespace MyMvcDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            JsConfig.IncludeNullValues = true;
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            JsConfig<DateTime>.SerializeFn = dt => dt.ToString("s");
            JsConfig<DateTime?>.SerializeFn = dt => dt != null ? dt.Value.ToString("s") : "null";
            JsConfig.ExcludeTypeInfo = true;

            AreaRegistration.RegisterAllAreas();

           // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ImageResizer.Configuration.Config.Current.Pipeline.AuthorizeImage += Pipeline_AuthorizeImage;
        }

        //保证ImageResizer只有在登录后才能被访问
        private void Pipeline_AuthorizeImage(IHttpModule sender, HttpContext context, ImageResizer.Configuration.IUrlAuthorizationEventArgs e)
        {
            e.AllowAccess = context.Request.IsAuthenticated;
        }
    }
}