using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// resource controller
    /// </summary>
    public class ResourceController : MvcAuthControllerBase
    {
        // GET: Resource
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            return View();
        }
    }
}