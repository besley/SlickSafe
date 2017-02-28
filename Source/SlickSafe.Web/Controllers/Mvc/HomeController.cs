using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;
using SlickSafe.Web.Models;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// home page
    /// </summary>
    public class HomeController : MvcAuthControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}