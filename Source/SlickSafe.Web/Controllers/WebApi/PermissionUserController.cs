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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SlickOne.WebUtility;
using SlickSafe.AuthImpl.Entity;
using SlickSafe.AuthImpl.Service;
using SlickSafe.Web.Filter;

namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// permission data controller
    /// </summary>
    [BasicAuthentication]
    public class PermissionUserController : ApiController
    {
        /// <summary>
        /// get user permission list
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<UserPermissionEntity>> GetUserPermissionList(int id)
        {
            var result = ResponseResult<List<UserPermissionEntity>>.Default();
            try
            {
                var resourceService = new PermissionService();
                var permissionList = resourceService.GetUserPermissionList(id);
                result = ResponseResult<List<UserPermissionEntity>>.Success(permissionList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<UserPermissionEntity>>.Error(ex.Message);
            }
            return result;
        }
    }
}