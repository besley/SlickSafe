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
    /// Mvc 控制器基类
    /// </summary>
    public class MvcControllerBase : Controller
    {
        /// <summary>
        /// Session 管理器
        /// </summary>
        protected SessionManager SessionManager
        {
            get; private set;
        }

        public MvcControllerBase() : base()
        {
            if (SessionManager == null)
            {
                SessionManager = new SessionManager();
            }
        }
    }
}
