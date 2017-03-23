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
    /// resource data controller
    /// </summary>
    [FormAuthCookieRequest]
    public class ResourceDataController : ApiController
    {
        #region resource basic data operation
        /// <summary>
        /// get resource all data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<List<ResourceEntity>> GetResourceAll()
        {
            var result = ResponseResult<List<ResourceEntity>>.Default();
            try
            {
                var resourceService = new ResourceService();
                var resourceList = resourceService.GetResourceAll().ToList();

                result = ResponseResult<List<ResourceEntity>>.Success(resourceList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ResourceEntity>>.Error(
                    string.Format("获取资源数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// get resource node data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<ResourceNode> GetResourceNodeAll()
        {
            var result = ResponseResult<ResourceNode>.Default();
            try
            {
                var resourceService = new ResourceService();
                var resourceList = resourceService.GetResourceNodeAll();

                result = ResponseResult<ResourceNode>.Success(resourceList);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ResourceNode>.Error(
                    string.Format("获取资源节点数据失败！{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// save resource data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult SaveResource(ResourceEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new ResourceService();
                resourceService.SaveResource(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存资源数据失败!{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// delete resource
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult DeleteResource(ResourceEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new ResourceService();
                resourceService.DeleteResource(entity.ID);
                             
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("删除资源数据失败!{0}", ex.Message));
            }
            return result;
        }
        #endregion
    }
}
