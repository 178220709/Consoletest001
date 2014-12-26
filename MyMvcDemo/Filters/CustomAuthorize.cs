using System;
using System.Web;
using System.Web.Mvc;

namespace MyMvcDemo.Filters
{

    internal class CustomAuthorize : AuthorizeAttribute
    {
       
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="RequiresSuperAdmin">是否需要超管验证</param>
        public CustomAuthorize( bool RequiresSuperAdmin)
        {
            this.RequiresSuperAdmin = RequiresSuperAdmin;
        }
     
        /// <summary>
        /// 是否需要超管验证
        /// </summary>
        public bool RequiresSuperAdmin { get; set; }

        private bool isTimeOut { get; set; }
       

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
           
            if (!isTimeOut && this.RequiresSuperAdmin)
            {
                throw new Exception("只有超级管理员才能访问该模块");
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                throw new HttpException(678, "请求未通过验证");
            }
            else
            {               
                filterContext.Result = new System.Web.Mvc.RedirectResult("/");
            }                 
        }
    }
}
