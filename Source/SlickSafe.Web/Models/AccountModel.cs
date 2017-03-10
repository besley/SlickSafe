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
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SlickOne.WebUtility;
using SlickOne.WebUtility.Security;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.Web.Models
{
    /// <summary>
    /// user account model
    /// </summary>
    public class AccountModel
    {
        #region user account verify
        /// <summary>
        /// get user account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal UserAccountEntity GetUserAccount(LoginUser user)
        {
            var url = string.Format("{0}/Account/QueryUserByName", WebApiApplication.WebAPIHostUrl);
            var clientHelper = HttpClientHelper.CreateHelper(url);
            var response = clientHelper.Post<LoginUser, ResponseResult<UserAccountEntity>>(user);
            UserAccountEntity account = response.Entity;

            return account;
        }

        /// <summary>
        /// identify logon user
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        internal bool IdentifyLogonUser(LoginUser loginUser, UserAccountEntity account)
        {
            var isPwdMachted = (account != null
                && Validate(account, loginUser.Password));

            return isPwdMachted;
        }

        /// <summary>
        /// verify user password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        private bool Validate(UserAccountEntity user, string password)
        {
            var isChecked = HashingAlgorithmUtility.CompareHash(user.PasswordFormat, password, user.PasswordSalt, user.Password);
            return isChecked;
        }

        /// <summary>
        /// create user ticket information
        /// </summary>
        /// <param name="loginName"></param>
        internal string CreateTicket(int userID, string loginName, string password)
        {
            //serialized user basic information
            var userData = new WebLogonUserData { UserID = userID, LoginName = loginName, Password = password };
            var userDataContent = JsonConvert.SerializeObject(userData);

            //create form ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, loginName, DateTime.Now, DateTime.Now.AddMinutes(240),
                true, userDataContent, FormsAuthentication.FormsCookiePath);

            string ticString = FormsAuthentication.Encrypt(ticket);

            //write cookies in response
            //SetAuthCookie mark identity status true
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("SlickOneWebCookie", ticString));
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("SlickOneWebUserIDCookie", userID.ToString()));
            FormsAuthentication.SetAuthCookie(loginName, true, "/");
            
            //set user ticket into httpcontext user identity property
            IIdentity identity = new FormsIdentity(ticket);
            IPrincipal principal = new GenericPrincipal(identity, null);
            HttpContext.Current.User = principal;

            return ticString;
        }
        #endregion

        #region user permission list
        /// <summary>
        /// get user permission list
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        internal List<UserPermissionEntity> GetUserPermissionList(int userID, string ticket)
        {
            var url = string.Format("{0}/PermissionData/GetUserPermissionList/{1}", 
                WebApiApplication.WebAPIHostUrl, userID);
            var clientHelper = HttpClientHelper.CreateHelper(url, ticket);

            var permissionList = clientHelper.Get<List<UserPermissionEntity>>();
            return permissionList;
        }
        #endregion

        #region user password modification
        /// <summary>
        /// change user password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        internal Boolean ChangePassword(UserPassword password, string ticket)
        {
            var isOk = false;
            var url = string.Format("{0}/Account/ChangePassword", WebApiApplication.WebAPIHostUrl);
            var clientHelper = HttpClientHelper.CreateHelper(url, ticket);

            var response = clientHelper.Post<UserPassword, ResponseResult>(password);
            if (response.Status == 1)
            {
                isOk = true;
            }
            return isOk;
        }
        #endregion
    }
}