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
            var url = string.Format("{0}/LogData/Login", WebApiApplication.WebAPIHostUrl);
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

            var url = string.Format("{0}/LogData/Logout", WebApiApplication.WebAPIHostUrl);
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