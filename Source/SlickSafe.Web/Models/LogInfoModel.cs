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
using SlickOne.WebUtility;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.Web.Models
{
    /// <summary>
    /// log infor model
    /// </summary>
    public class LogInfoModel
    {
        /// <summary>
        /// login record
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="loginName"></param>
        /// <param name="sessionGUID"></param>
        /// <param name="context"></param>
        internal void WriteLoginInfo(int userID, string loginName, string sessionGUID, string ipaddress)
        {
            var logInfo = new UserLogEntity
            {
                UserID = userID,
                LoginName = loginName,
                SessionGUID = sessionGUID,
                IPAddress = ipaddress
            };
            var url = string.Format("{0}/Log/Login", WebApiApplication.WebAPIHostUrl);
            var clientHelper = HttpClientHelper.CreateHelper(url);

            clientHelper.Post<UserLogEntity, ResponseResult<UserLogEntity>>(logInfo);
        }

        /// <summary>
        /// logout record
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="sessionGUID"></param>
        /// <param name="ipAddress"></param>
        internal void WriteLogoutInfo(int userID, string loginName, string sessionGUID, string ipAddress)
        {
            var log = new UserLogEntity
            {
                UserID = userID,
                LoginName = loginName,
                SessionGUID = sessionGUID,
                IPAddress = ipAddress
            };

            var url = string.Format("{0}/Log/Logout", WebApiApplication.WebAPIHostUrl);
            var clientHelper = HttpClientHelper.CreateHelper(url);

            clientHelper.Post<UserLogEntity, ResponseResult<UserLogEntity>>(log);
        }

        /// <summary>
        /// send email
        /// </summary>
        /// <param name="recieveEMail"></param>
        internal void SendEMail(string recieveEMail)
        {
            string title = "您的账号和密码注册信息";
            string content = "";
            EMailUtility.SendEMail(title, content, recieveEMail);
        }
    }
}