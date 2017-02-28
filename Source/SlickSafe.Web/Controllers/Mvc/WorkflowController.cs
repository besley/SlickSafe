using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SlickOne.WebUtility;

namespace SlickSafe.Web.Controllers.Mvc
{
    /// <summary>
    /// workflow controller
    /// </summary>
    public class WorkflowController : MvcAuthControllerBase
    {
        // GET: Workflow
        public ActionResult Process()
        {
            return View();
        }

        public ActionResult ProcessInstance()
        {
            return View();
        }

        public ActionResult ActivityInstance()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }
    }
}