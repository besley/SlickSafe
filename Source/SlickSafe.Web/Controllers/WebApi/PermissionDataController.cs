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
    /// permission include role permission and user permission
    /// </summary>
    [FormAuthCookieRequest]
    public class PermissionDataController : ApiController
    {
        #region left menu data
        /// <summary>
        /// get left menu data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ResourceNode[]> GetLeftMenuList(int id)
        {
            var result = ResponseResult<ResourceNode[]>.Default();
            try
            {
                var resourceService = new PermissionService();
                ResourceQuery query = new ResourceQuery { UserID = id };
                var resourceNodes = resourceService.GetLeftMenuList(query.UserID);

                result = ResponseResult<ResourceNode[]>.Success(resourceNodes);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ResourceNode[]>.Error(
                    string.Format("获取左侧导航资源数据失败！{0}", ex.Message)
                );
            }
            return result;
        }
        #endregion

        #region role resource list
        /// <summary>
        /// get role resource list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<RoleResourcePermissionView>> GetRoleResourceList(ResourceQuery query)
        {
            var result = ResponseResult<List<RoleResourcePermissionView>>.Default();
            try
            {
                var resourceService = new PermissionService();
                var permissionList = resourceService.GetRoleResourceList(query.RoleID);

                result = ResponseResult<List<RoleResourcePermissionView>>.Success(permissionList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<RoleResourcePermissionView>>.Error(
                    string.Format("获取角色资源权限数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// save role resource data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveRoleResourceList(List<RoleResourceEntity> entityList)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new PermissionService();
                resourceService.SaveRoleResourceList(entityList);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存角色资源授权数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// clear role resource data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ClearRoleResourceList(RoleResourceEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new PermissionService();
                resourceService.ClearRoleResourceList(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("清除角色资源授权数据失败!{0}", ex.Message));
            }
            return result;
        }
        #endregion

        #region user resource list
        /// <summary>
        /// retrieve user resource data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<List<UserResourcePermissionView>> RetrieveUserResourceList(ResourceQuery query)
        {
            var result = ResponseResult<List<UserResourcePermissionView>>.Default();
            try
            {
                var resourceService = new PermissionService();
                var permissionList = resourceService.RetrieveUserResourceList(query.UserID);

                result = ResponseResult<List<UserResourcePermissionView>>.Success(permissionList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<UserResourcePermissionView>>.Error(
                    string.Format("获取用户资源权限数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// save user resource data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveUserResourceList(List<UserResourceEntity> entityList)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new PermissionService();
                resourceService.SaveUserResourceList(entityList);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存用戶资源授权数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// clear user resource
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult ClearUserResourceList(UserResourceEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new PermissionService();
                resourceService.ClearUserResourceList(entity.UserID);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("清除用户自有资源授权数据失败!{0}", ex.Message));
            }
            return result;
        }
        #endregion
    }
}
