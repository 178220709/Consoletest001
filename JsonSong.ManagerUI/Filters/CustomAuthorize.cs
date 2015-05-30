using System;
using System.Web;
using System.Web.Mvc;

namespace JsonSong.ManagerUI.Filters
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
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            return true;
            
            //取得当前登录用户
            object user = null;

            if (!base.AuthorizeCore(httpContext) || user == null)
            {
                isTimeOut = true;
            }

            //bool result = base.AuthorizeCore(httpContext) && //确保用户已登录
            //                user != null && //用户不为null
            //                !user.IsDeleted &&
            //                user.IsActive;

            ////需要超管认证？
            //if (this.RequiresSuperAdmin)
            //{
            //    result = result && user.IsSuper; //确认用户是超管
            //}

           
        }


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
