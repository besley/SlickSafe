using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// profile controller
    /// </summary>
    public class ProfileController : MvcAuthControllerBase
    {
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
    }
}