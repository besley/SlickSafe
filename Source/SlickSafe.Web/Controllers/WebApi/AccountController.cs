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
using System.Web.Http;
using System.Web.Security;
using System.Security.Cryptography;
using SlickOne.Data;
using SlickOne.WebUtility;
using SlickSafe.AuthImpl.Entity;
using SlickSafe.AuthImpl.Service;
using SlickSafe.Web.Filter;

namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// user account controller
    /// </summary>
    public class AccountController : ApiController
    {
        #region account service
        /// <summary>
        /// account service instance
        /// </summary>
        private IAccountService _accountService;
        protected IAccountService AccountService
        {
            get
            {
                if (_accountService == null)
                {
                    _accountService = new AccountService();
                }
                return _accountService;
            }
        }
        #endregion

        /// <summary>
        /// register
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult Register(UserAccountEntity account)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Register(account);
                result = ResponseResult.Success("创建用户成功！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error("创建用户失败，错误代码：4001！");
            }
            return result;
        }

        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<UserAccountEntity> GetById(int id)
        {
            var result = ResponseResult<UserAccountEntity>.Default();
            try
            {
                var user = AccountService.GetById(id);
                result = ResponseResult<UserAccountEntity>.Success(user);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<UserAccountEntity>.Error("获取用户信息失败！");
            }
            return result;
        }

        /// <summary>
        /// query by user name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<UserAccountEntity> QueryUserByName(LoginUser user)
        {
            var result = ResponseResult<UserAccountEntity>.Default();
            try
            {
                var entity = AccountService.GetByLoginName(user.LoginName);
                result = ResponseResult<UserAccountEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<UserAccountEntity>.Error("获取用户信息失败！");
            }
            return result;
        }

        /// <summary>
        /// change user password
        /// </summary>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult ChangePassword(UserPassword userPwd)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.ChangePassword(userPwd.LoginName, userPwd.OldPassword, userPwd.NewPassword);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error("用户密码修改发生错误！");
            }
            return result;
        }

        /// <summary>
        /// reset user password
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult ResetPassword(UserAccountEntity account)
        {
            var result = ResponseResult.Default();
            try
            {
                var newPassword = AccountService.ResetPassword(account.ID);
                result = ResponseResult.Success(string.Format("用户密码重置成功，新密码是:{0}", newPassword));
                result.ExtraData = newPassword;
            }
            catch (System.ApplicationException ex)
            {
                result = ResponseResult.Error("用户密码重置发生错误！");
            }
            return result;
        }

        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Update(UserAccountEntity account)
        {
            var result = ResponseResult.Default();

            try
            {
                var isOk = AccountService.Update(account);
                if (isOk == true)
                {
                    result = ResponseResult.Success();
                }
                else
                {
                    result = ResponseResult.Error("用户账户资料修改发生错误！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("用户账户资料修改发生错误，描述：{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// lock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Lock(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Lock(id);
                result = ResponseResult.Success("锁定用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("锁定用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult UnLock(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.UnLock(id);
                result = ResponseResult.Success("解锁用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("解锁用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// discard user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Discard(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Discard(int.Parse(id.ID.ToString()));
                result = ResponseResult.Success("废弃用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("废弃用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// user account paged query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult<List<UserAccountEntity>> SelectPaged(UserAccountQuery query)
        {
            var result = ResponseResult<List<UserAccountEntity>>.Default();
            try
            {
                var count = 0;
                var list = AccountService.GetPaged(query, out count);

                result = ResponseResult<List<UserAccountEntity>>.Success(list);
                result.TotalRowsCount = count;
                result.TotalPages = (count + query.PageSize - 1) / query.PageSize;
            }
            catch (System.Exception)
            {
                result = ResponseResult<List<UserAccountEntity>>.Error("获取用户账号分页数据失败！");
            }
            return result;
        }

    }
}
