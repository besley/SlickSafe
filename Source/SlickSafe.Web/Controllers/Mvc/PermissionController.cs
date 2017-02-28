using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// permisison controller
    /// </summary>
    public class PermissionController : MvcAuthControllerBase
    {
        // GET: Permission
        public ActionResult Role()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }
    }
}