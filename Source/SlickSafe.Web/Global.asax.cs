using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Web.Optimization;
using System.Web.Routing;

namespace SlickSafe.Web
{
    /// <summary>
    /// Web Application 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        internal static bool WebApiSecurityEnabled = true;

        private static string _webApiHostUrl = string.Empty;
        internal static string WebAPIHostUrl
        {
            get
            {
                if (_webApiHostUrl == string.Empty)
                    _webApiHostUrl = WebConfigurationManager.AppSettings["WebApiHostUrl"].ToString();

                return _webApiHostUrl;
            }
        }

        /// <summary>
        /// Web 应用程序启动
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //improve page response time
            ViewEngines.Engines.Clear();
            IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(razorEngine);

            //use Json format 
            SetJsonFormat();
        }

        /// <summary>
        /// 设置JSon格式
        /// </summary>
        private void SetJsonFormat()
        {
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
