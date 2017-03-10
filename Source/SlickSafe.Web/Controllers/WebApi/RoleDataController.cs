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
using SlickSafe.AuthImp.Entity;
using SlickSafe.AuthImp.Service;
using SlickSafe.Web.Filter;


namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// role data controller
    /// </summary>
    [FormAuthCookieRequest]
    public class RoleDataController : ApiController
    {
        /// <summary>
        /// get all role data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<RoleEntity>> GetRoleAll()
        {
            var result = ResponseResult<List<RoleEntity>>.Default();
            try
            {
                var roleService = new RoleDataService();
                var roleList = roleService.GetRoleAll().ToList();

                result = ResponseResult<List<RoleEntity>>.Success(roleList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<RoleEntity>>.Error(
                    string.Format("获取角色数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// save role data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveRole(RoleEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.SaveRole(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存角色数据失败!{0}", ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult DeleteRole(RoleEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.DeleteRole(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("删除角色数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// get user all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<UserAccountEntity>> GetUserAll()
        {
            var result = ResponseResult<List<UserAccountEntity>>.Default();
            try
            {
                var roleService = new RoleDataService();
                var userList = roleService.GetUserAll().ToList();

                result = ResponseResult<List<UserAccountEntity>>.Success(userList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<UserAccountEntity>>.Error(
                    string.Format("获取用户数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// save user data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveUser(UserEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.SaveUser(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存用户数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// delete user data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult DeleteUser(UserEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.DeleteUser(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("删除用户数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// get users in roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<RoleUserView>> GetRoleUserAll()
        {
            var result = ResponseResult<List<RoleUserView>>.Default();
            try
            {
                var roleService = new RoleDataService();
                var userList = roleService.GetRoleUserAll().ToList();

                result = ResponseResult<List<RoleUserView>>.Success(userList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<RoleUserView>>.Error(
                    string.Format("获取角色用户数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// add user into role
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult AddRoleUser(RoleUserEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.AddRoleUser(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("添加用户到角色失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// delete user from role
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult DeleteRoleUser(RoleUserEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var roleService = new RoleDataService();
                roleService.DeleteRoleUser(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("删除角色下用户失败!{0}", ex.Message));
            }
            return result;
        }

    }
}
