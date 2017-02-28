using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using SlickOne.WebUtility.Security;

namespace SlickOne.WebUtility
{
    /// <summary>
    /// 权限验证控制器基类
    /// </summary>
    [Authorize]
    public abstract class MvcAuthControllerBase : MvcControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attr = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);
            if (isAnonymous == false)
            {
                var session = filterContext.HttpContext.Session;
                this.SessionManager.SetSession(session);

                var user = this.SessionManager.GetLogonUser() as WebLogonUser;

                if (user == null)
                {
                    //非登录用户，或者Session过期
                    //filterContext.HttpContext.Response.Redirect("~/Home/UnAuthorized", true);
                    filterContext.HttpContext.Response.Redirect("~/Account/Login", true);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
