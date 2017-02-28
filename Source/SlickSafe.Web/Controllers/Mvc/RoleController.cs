using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// role controller
    /// </summary>
    public class RoleController : MvcAuthControllerBase
    {
        public ActionResult List()
        {
            return View();
        }

        // GET: Role
        public ActionResult Edit()
        {
            return View();
        }
    }
}