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
using SlickOne.Biz.Entity;
using SlickOne.Biz.Service;

namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// workflow service
    /// </summary>
    public class WfDataController : ApiController
    {
        /// <summary>
        /// get process
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ProcessEntity>> GetProcessListSimple()
        {
            var result = ResponseResult<List<ProcessEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetProcessListSimple().ToList();

                result = ResponseResult<List<ProcessEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProcessEntity>>.Error(
                    string.Format("获取流程基本信息失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get process instace
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ProcessInstanceEntity>> GetProcessInstanceList()
        {
            var result = ResponseResult<List<ProcessInstanceEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetProcessInstanceList().ToList();

                result = ResponseResult<List<ProcessInstanceEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProcessInstanceEntity>>.Error(
                    string.Format("获取流程实例数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get activity instance
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ActivityInstanceEntity>> GetActivityInstanceList()
        {
            var result = ResponseResult<List<ActivityInstanceEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetActivityInstanceList().ToList();

                result = ResponseResult<List<ActivityInstanceEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ActivityInstanceEntity>>.Error(
                    string.Format("获取流程活动实例数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get task data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<TaskEntity>> GetTaskList()
        {
            var result = ResponseResult<List<TaskEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetTaskList().ToList();

                result = ResponseResult<List<TaskEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<TaskEntity>>.Error(
                    string.Format("获取任务实例数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get log data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<FormEntity>> GetFormListSimple()
        {
            var result = ResponseResult<List<FormEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetFormListSimple().ToList();

                result = ResponseResult<List<FormEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<FormEntity>>.Error(
                    string.Format("读取{0}数据失败, 错误：{1}", "Form", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get log data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<LogEntity>> GetLogList()
        {
            var result = ResponseResult<List<LogEntity>>.Default();
            try
            {
                var wfService = new WfDataService();
                var entity = wfService.GetLogList().ToList();

                result = ResponseResult<List<LogEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<LogEntity>>.Error(
                    string.Format("读取{0}数据失败, 错误：{1}", "Log", ex.Message)
                );
            }
            return result;
        }
    }
}
