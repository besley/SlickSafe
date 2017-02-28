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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
                resourceService.SaveRoleResourceList(entityList);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("保存角色资源授权数据失败!{0}", ex.Message));
            }
            return result;
        }

        [HttpPost]
        public ResponseResult ClearRoleResourceList(RoleResourceEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var resourceService = new ResourceDataService();
                resourceService.ClearRoleResourceList(entity);

                result = ResponseResult.Success();
            }
            catch(System.Exception ex)
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
                var resourceService = new ResourceDataService();
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
