/*
* SlickSafe 企业级权限快速开发框架遵循LGPL协议，也可联系作者获取商业授权
* 和技术支持服务；除此之外的使用，则视为不正当使用，请您务必避免由此带来的
* 商业版权纠纷。
*
The SlickSafe Product.
Copyright (C) 2017  .NET Authorization Framework Software

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/


using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using SlickOne.WebUtility;
using SlickOne.WebUtility.Security;
using SlickSafe.AuthImpl.Entity;
using SlickSafe.AuthImpl.Service;

namespace SlickSafe.Web.Filter
{
    /// <summary>
    /// action fileter used for authentication
    /// </summary>
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// user id
        /// </summary>
        internal int UserID { get; private set; }

        /// <summary>
        /// check authorizaton information when action executing
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //get user authorization data
            var authorization = actionContext.Request.Headers.Authorization;
            
            if ((authorization != null) && (authorization.Parameter != null))
            {
                //decrypted user ticket information
                var encryptTicket = authorization.Parameter;
                if (ValidateUserTicket(encryptTicket))
                    base.OnActionExecuting(actionContext);
                else
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);                   
            }
            else
            {
                //verify webapi security setting
                bool isRquired = (WebConfigurationManager.AppSettings["WebApiSecurityEnabled"].ToString() == "true");
                if (isRquired)
                {
                    //check anonymous attribute
                    var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                    bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

                    if (isAnonymous)
                        base.OnActionExecuting(actionContext);
                    else
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    base.OnActionExecuting(actionContext);
                }
            }
        }

        /// <summary>
        /// verify user authorization ticket information
        /// </summary>
        /// <param name="encryptTicket"></param>
        /// <returns></returns>
        private bool ValidateUserTicket(string encryptTicket)
        {
            var loginTicket = FormsAuthentication.Decrypt(encryptTicket);
            var userDataContent = loginTicket.UserData;
            var webLogonUserData = JsonConvert.DeserializeObject<WebLogonUserData>(userDataContent);

            UserID = webLogonUserData.UserID;

            string loginName = webLogonUserData.LoginName;
            string password = webLogonUserData.Password;

            //check user password
            var service = new AccountService();
            var user = service.GetByLoginName(loginName);
            var isChecked = HashingAlgorithmUtility.CompareHash(user.PasswordFormat, password, user.PasswordSalt, user.Password);

            return isChecked;
        }
    }
}
