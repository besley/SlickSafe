using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// role user controller
    /// </summary>
    public class RoleUserController : MvcAuthControllerBase
    {
        // GET: RoleUser
        public ActionResult List()
        {
            return View();
        }
    }
}