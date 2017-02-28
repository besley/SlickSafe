using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SlickOne.WebUtility
{
    /// <summary>
    /// Session 管理器
    /// </summary>
    public class SessionManager
    {
        private const string WEB_LOGON_USER = "SLICKONE_WEB_LOGON_USER";
        private const string WEB_LOGON_USER_ID = "SLICKONE_WEB_LOGON_USER_ID";
        private const string WEB_LOGON_SESSION_GUID = "SLICKONE_WEB_LOGON_SESSION_GUID";
        private const string WEB_LOGIN_IMAGE_TEXT = "SLICKONE_WEB_LOGIN_IMAGE_TEXT";
        private const string WEB_LOGON_USER_TICKET = "SLICKONE_WEB_LOGON_USER_TICKET";
        private const string WEB_LOGON_USER_ACCOUNT_TYPE = "SLICKONE_WEB_LOGON_USER_ACCOUNT_TYPE";
        private const string WEB_LOGON_USER_PERMISSION_LIST = "SLICKONE_WEB_LOGON_USER_PERMISSION_LIST";
        private HttpSessionStateBase CurrentSession = null;


        /// <summary>
        /// 取出Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return CurrentSession[key];
        }

        /// <summary>
        /// 获取登录用户对象
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public object GetLogonUser()
        {
            return Get(WEB_LOGON_USER);
        }

        /// <summary>
        /// 获取登录用户ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public int GetLogonUserID()
        {
            return (int)Get(WEB_LOGON_USER_ID);
        }

        /// <summary>
        /// 获取登录用户Session的GUID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public string GetLogonUserSessionGUID()
        {
            return Get(WEB_LOGON_SESSION_GUID).ToString();
        }

        /// <summary>
        /// 获取登录用户票据
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public string GetLogonUserTicket()
        {
            var obj = Get(WEB_LOGON_USER_TICKET);
            var ticket = obj != null ? obj.ToString() : string.Empty;
            return ticket;
        }

        /// <summary>
        /// 获取登录用户账户类型
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public string GetLogonUserAccountType()
        {
            var obj = Get(WEB_LOGON_USER_ACCOUNT_TYPE);
            var accountType = obj != null ? obj.ToString() : string.Empty;
            return accountType;
        }

        /// <summary>
        /// 获取用户登录的权限列表
        /// </summary>
        /// <returns></returns>
        public object GetLogonUserPermissionList()
        {
            return Get(WEB_LOGON_USER_PERMISSION_LIST);
        }

        /// <summary>
        /// 获取登录前的图片字符串
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public string GetLogonImageText()
        {
            return Get(WEB_LOGIN_IMAGE_TEXT).ToString();
        }

        /// <summary>
        /// 设置会话
        /// </summary>
        /// <param name="session"></param>
        public void SetSession(HttpSessionStateBase session)
        {
            CurrentSession = session;
        }

        /// <summary>
        /// 写入session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void Save(string key, object value)
        {
            CurrentSession[key] = value;
        }

        /// <summary>
        /// 保存登录用户对象
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void SaveLogonUser(object value)
        {
            Save(WEB_LOGON_USER, value);
        }

        /// <summary>
        /// 保存登录用户ID
        /// </summary>
        /// <param name="session"></param>
        /// <param name="userId"></param>
        private void SaveLogonUserID(int userId)
        {
            Save(WEB_LOGON_USER_ID, userId);
        }

        /// <summary>
        /// 保存登录用户票据
        /// </summary>
        /// <param name="session"></param>
        /// <param name="ticket"></param>
        private void SaveLogonUserTicket(string ticket)
        {
            Save(WEB_LOGON_USER_TICKET, ticket);
        }

        /// <summary>
        /// 保存用户账户类型
        /// </summary>
        /// <param name="session"></param>
        /// <param name="accountType"></param>
        private void SaveLogonUserAccountType(string accountType)
        {
            Save(WEB_LOGON_USER_ACCOUNT_TYPE, accountType);
        }

        /// <summary>
        /// 保存登录用户Session的GUID
        /// </summary>
        /// <param name="session"></param>
        private string SaveLogonSessionGUID()
        {
            string sessionGUID = Guid.NewGuid().ToString();
            Save(WEB_LOGON_SESSION_GUID, sessionGUID);
            return sessionGUID;
        }

        /// <summary>
        /// 保存用户资源数据
        /// </summary>
        /// <param name="permissionList"></param>
        public void SaveLogonUserPermission(object permissionList)
        {
            Save(WEB_LOGON_USER_PERMISSION_LIST, permissionList);
        }

        /// <summary>
        /// 保存登录前图片验证字符串
        /// </summary>
        /// <param name="session"></param>
        /// <param name="text"></param>
        private void SaveLogonImageText(string text)
        {
            Save(WEB_LOGIN_IMAGE_TEXT, text);
        }

        /// <summary>
        /// 保存用户身份凭证信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="loginName"></param>
        /// <param name="ticket"></param>
        public string SaveLogonUserInfo(int userID, string loginName, string ticket)
        {
            SaveLogonUserID(userID);
            string sessionGUID = SaveLogonSessionGUID();
            SaveLogonUserTicket(ticket);

            var webUser = new WebLogonUser { UserID = userID, LoginName = loginName, Ticket = ticket };
            SaveLogonUser(webUser);

            return sessionGUID;
        }
    }
}
