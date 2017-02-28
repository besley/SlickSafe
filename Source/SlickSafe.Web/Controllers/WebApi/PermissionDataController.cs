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
    /// permission data controller
    /// </summary>
    [BasicAuthentication]
    public class PermissionDataController : ApiController
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
                var resourceService = new ResourceDataService();
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