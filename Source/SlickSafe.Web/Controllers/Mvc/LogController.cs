using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// log controller
    /// </summary>
    public class LogController : MvcAuthControllerBase
    {
        // GET: Log
        public ActionResult Exception()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Operation()
        {
            return View();
        }
    }
}