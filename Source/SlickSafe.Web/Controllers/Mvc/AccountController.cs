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
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Sockets;
using System.Web.Security;
using System.Threading.Tasks;
using SlickOne.WebUtility;
using SlickSafe.AuthImpl.Entity;
using SlickSafe.Web.Models;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// user accout controller
    /// </summary>
    public class AccountController : MvcControllerBase
    {
        /// <summary>
        /// register page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// login page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            return View();
        }

        /// <summary>
        /// login post
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginUser loginUser, string returnUrl)
        {
            var accountModel = new AccountModel();
            var userAccount = accountModel.GetUserAccount(loginUser);
            var isOK = accountModel.IdentifyLogonUser(loginUser, userAccount);

            if (isOK)
            {
                loginUser.ErrorMessage = string.Empty;

                //write user identity information into session
                loginUser.Ticket = accountModel.CreateTicket(userAccount.ID, loginUser.LoginName, loginUser.Password);
                this.SessionManager.SetSession(HttpContext.Session);
                string sessionGUID = this.SessionManager.SaveLogonUserInfo(userAccount.ID, 
                    userAccount.LoginName, loginUser.Ticket);

                //write log info into sysuserlog table
                var ipAddress = GetIPAddress();
                Task secondTask = Task.Factory.StartNew(() =>
                {
                    //get user permission list
                    if (userAccount.AccountType != -1)
                    {
                        var permissionList = accountModel.GetUserPermissionList(userAccount.ID, loginUser.Ticket);
                        this.SessionManager.SaveLogonUserPermission(permissionList);
                    }

                    //write user login info
                    var logInfoModel = new LogInfoModel();
                    logInfoModel.WriteLoginInfo(userAccount.ID, userAccount.LoginName,
                        sessionGUID,
                        ipAddress);
                });

                return RedirectToReturnUrl(returnUrl);
            }
            else
            {
                loginUser.ErrorMessage = "用户名和密码不匹配，请重新尝试！"; 
                return View(loginUser);
            }
        }

        /// <summary>
        /// redirect page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            { 
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        /// <summary>
        /// logout 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            var session = HttpContext.Session;
            var ipAddress = GetIPAddress();
            //record logout 
            Task logTask = Task.Factory.StartNew(() =>
            {
                this.SessionManager.SetSession(session);
                var webUser = this.SessionManager.GetLogonUser() as WebLogonUser;

                var logInfoModel = new LogInfoModel();
                logInfoModel.WriteLogoutInfo(webUser.UserID, webUser.LoginName, this.SessionManager.GetLogonUserSessionGUID(), ipAddress);
            });

            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// send email
        /// </summary>
        /// <param name="recieveEMail"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SendEMail(dynamic email)
        {
            ResponseResult result = ResponseResult.Default();
            try
            { 
                var logInfoModel = new LogInfoModel();
                logInfoModel.SendEMail(email);
                result = ResponseResult.Success();
            }
            catch
            {
                result = ResponseResult.Error("发送邮件发生错误！");
            }

            return result;
        }

        /// <summary>
        /// change password
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// change password post
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Password(UserPassword password)
        {
            var accountModel = new AccountModel();
            var ticket = this.SessionManager.GetLogonUserTicket();
            var isOK = accountModel.ChangePassword(password, ticket);
            if (isOK)
            {
                var newTicket = accountModel.CreateTicket(password.UserID, password.LoginName, password.NewPassword);
                this.SessionManager.SaveLogonUserInfo(password.UserID, password.LoginName, newTicket);
            }

            return Json(password, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get ip address
        /// </summary>
        /// <returns></returns>
        private string GetIPAddress()
        {
            string ip = null;
            if (System.Web.HttpContext.Current != null)
            { // ASP.NET
                ip = string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                    ? System.Web.HttpContext.Current.Request.UserHostAddress
                    : System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (string.IsNullOrEmpty(ip) || ip.Trim() == "::1")
            { // still can't decide or is LAN
                var lan = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(r => r.AddressFamily == AddressFamily.InterNetwork);
                ip = lan == null ? string.Empty : lan.ToString();
            }
            return ip;
        }

        [HttpGet]
        public ActionResult Find()
        {
            return View();
        }
    }
}