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
using SlickSafe.AuthImp.Entity;
using SlickSafe.AuthImp.Service;


namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// user log controller
    /// </summary>
    public class LogDataController : ApiController
    {
        private ILogDataService _logDataService;
        protected ILogDataService LogDataService
        {
            get
            {
                if (_logDataService == null)
                {
                    _logDataService = new LogDataService();
                }
                return _logDataService;
            }
        }

        /// <summary>
        /// get user log
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<UserLogEntity> Get(int id)
        {
            var result = ResponseResult<UserLogEntity>.Default();
            try
            {
                var entity = LogDataService.Get(id);
                result = ResponseResult<UserLogEntity>.Success(entity);
            }
            catch (System.Exception)
            {
                result = ResponseResult<UserLogEntity>.Error("获取用户登录日志数据失败！");
            }
            return result;
        }

        /// <summary>
        /// get paged data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<UserLogEntity>> GetUserLogPaged(UserLogQuery query)
        {
            var result = ResponseResult<List<UserLogEntity>>.Default();
            try
            {
                var count = 0;
                var list = LogDataService.GetPaged(query, out count);

                result = ResponseResult<List<UserLogEntity>>.Success(list);
                result.TotalRowsCount = count;
                result.TotalPages = (count + query.PageSize - 1) / query.PageSize;
            }
            catch (System.Exception)
            {
                result = ResponseResult<List<UserLogEntity>>.Error("获取用户登录日志数据失败！");
            }
            return result;
        }

        /// <summary>
        /// get top 100 user log record
        /// </summary>
        /// <returns></returns>
        public ResponseResult<List<UserLogEntity>> GetUserLogPaged100()
        {
            var result = ResponseResult<List<UserLogEntity>>.Default();
            try
            {
                var list = LogDataService.GetPaged100();
                result = ResponseResult<List<UserLogEntity>>.Success(list);
            }
            catch (System.Exception)
            {
                result = ResponseResult<List<UserLogEntity>>.Error("获取用户登录日志数据失败！");
            }
            return result;
        }

        /// <summary>
        /// login data
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult Login(UserLogEntity log)
        {
            var result = ResponseResult.Default();
            try
            {
                LogDataService.Login(log);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("记录登录日志数据失败, {0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// logout data
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult Logout(UserLogEntity log)
        {
            var result = ResponseResult.Default();
            try
            {
                LogDataService.Logout(log);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("用户注销记录日志失败, {0}", ex.Message));
            }
            return result;
        }

    }
}
