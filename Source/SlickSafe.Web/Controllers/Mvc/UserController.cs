using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// user controller
    /// </summary>
    public class UserController : MvcAuthControllerBase
    {
        // GET: User
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Dialog()
        {
            return View();
        }
    }
}