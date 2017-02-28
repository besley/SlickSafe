using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// test controller
    /// </summary>
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SideBar()
        {
            return View();
        }

        public ActionResult NavBar()
        {
            return View();
        }
    }
}