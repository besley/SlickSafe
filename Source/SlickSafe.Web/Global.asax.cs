/*
* SlickSafe 企业级权限快速开发框架遵循LGPL协议，也可联系作者获取商业授权
* 和技术支持服务；除此之外的使用，则视为不正当使用，请您务必避免由此带来的
* 商业版权纠纷。
*
The SlickSafe Product.
Copyright (C) 2017  .NET Authorization Framework Software

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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
