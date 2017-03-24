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
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using SlickOne.Data;
using SlickSafe.AuthImpl.Entity;

namespace SlickSafe.AuthImpl.Service
{
    /// <summary>
    /// resource data service implementation
    /// </summary>
    public class PermissionService : ServiceBase, IPermissionService
    {
        #region Left SideBar data
        /// <summary>
        /// get left menu data
        /// </summary>
        /// <returns></returns>
        public ResourceNode[] GetLeftMenuList(int userID)
        {
            List<ResourceEntity> list = null;
            try
            {
                var param = new DynamicParameters();

                param.Add("@userID", userID);
                list = QuickRepository.ExecProcQuery<ResourceEntity>("pr_sys_ResourceLeftMenuGetByUser", param)
                    .ToList<ResourceEntity>();

                var resourceNodes =  GetChildren(0, list);
                return resourceNodes;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("查询左侧导航树数据失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// get resource ndoe children
        /// </summary>
        /// <param name="partentID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private ResourceNode[] GetChildren(int partentID, List<ResourceEntity> list)
        {
            //获取子节点列表
            var children = (from a in list
                            where a.ParentID == partentID
                            select a).ToList<ResourceEntity>();
            var count = children.Count();

            ResourceNode[] bvArray = new ResourceNode[count];
            ResourceNode bv = null;
            ResourceEntity entity = null;

            for (var i = 0; i < count; i++)
            {
                entity = children[i];
                bv = new ResourceNode();
                bv.ID = entity.ID;
                bv.ResourceName = entity.ResourceName;
                bv.ResourceTypeID = entity.ResourceTypeID;
                bv.ParentID = entity.ParentID;
                bv.UrlAction = entity.UrlAction;
                bv.DataAction = entity.DataAction;
                bv.StyleClass = entity.StyleClass;
                //get children iteriated
                bv.children = GetChildren(entity.ID, list);
                bvArray[i] = bv;
            }
            return bvArray;
        }
        #endregion

        /// <summary>
        ///  get user resource permission allowed
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<RoleResourcePermissionView> GetResourcePermissionAllowed(int userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);

                var list = QuickRepository.ExecProcQuery<RoleResourcePermissionView>("pr_sys_ResourceListAllowedGetByUserOrRole",
                     param).ToList<RoleResourcePermissionView>();

                return list;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("获取用户资源权限列表(用于用户管理，显示有效权限)失败!", ex);
                throw;
            }
        }

        #region role resource opeation
        /// <summary>
        /// get role resource data by roleid
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<RoleResourcePermissionView> GetRoleResourceList(int roleID)
        {
            List<RoleResourcePermissionView> list = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleID", roleID);

                list = QuickRepository.ExecProcQuery<RoleResourcePermissionView>("pr_sys_RoleResourceListGetByRole",
                    param).ToList<RoleResourcePermissionView>();

                return list;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("根据角色获取资源许可失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// save role resoruce data
        /// </summary>
        /// <param name="entity"></param>
        public void SaveRoleResourceList(List<RoleResourceEntity> entityList)
        {
            int roleID = entityList[0].RoleID;
            StringBuilder sbXml = new StringBuilder();
            try
            {
                sbXml.Append("<data>");
                entityList.ForEach(info => {
                    sbXml.Append("<item>");
                    sbXml.Append("<RoleID>" + info.RoleID.ToString() + "</RoleID>");
                    sbXml.Append("<ResourceID>" + info.ResourceID.ToString() + "</ResourceID>");
                    sbXml.Append("<PermissionType>" + info.PermissionType + "</PermissionType>");
                    sbXml.Append("</item>");
                });
                sbXml.Append("</data>");
                var param = new DynamicParameters();
                param.Add("@roleID", roleID);
                param.Add("@permissionXML", sbXml.ToString());

                QuickRepository.ExecuteProc("dbo.pr_sys_RoleResourceListSaveBatch", param);
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("获取用户资源权限列表(用于用户管理，显示有效权限)失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// clear role resource 
        /// </summary>
        /// <param name="entity"></param>
        public void ClearRoleResourceList(RoleResourceEntity entity)
        {
            int roleID = entity.RoleID;
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleID", roleID);
                QuickRepository.ExecuteProc("dbo.pr_sys_RoleResourceListClear", param);
            }
            catch(System.Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region user resource operation
        /// <summary>
        /// retrieve user resource data for treeview display
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<UserResourcePermissionView> RetrieveUserResourceList(int userID) {
            List<UserResourcePermissionView> list = null;
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);

                list = QuickRepository.ExecProcQuery<UserResourcePermissionView>("pr_sys_UserResourceListRetrieveByUser",
                    param).ToList<UserResourcePermissionView>();

                return list;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("根据角色获取资源许可失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// save user resource data
        /// </summary>
        /// <param name="entity"></param>
        public void SaveUserResourceList(List<UserResourceEntity> entityList)
        {
            int userID = entityList[0].UserID;
            StringBuilder sbXml = new StringBuilder();
            try
            {
                sbXml.Append("<data>");
                entityList.ForEach(info => {
                    sbXml.Append("<item>");
                    sbXml.Append("<UserID>" + info.UserID.ToString() + "</UserID>");
                    sbXml.Append("<ResourceID>" + info.ResourceID.ToString() + "</ResourceID>");
                    sbXml.Append("<PermissionType>" + info.PermissionType + "</PermissionType>");
                    sbXml.Append("<IsInherited>" + info.IsInherited + "</IsInherited>");
                    sbXml.Append("</item>");
                });
                sbXml.Append("</data>");
                var param = new DynamicParameters();
                param.Add("@userID", userID);
                param.Add("@permissionXML", sbXml.ToString());

                QuickRepository.ExecuteProc("dbo.pr_sys_UserResourceListSaveBatch", param);
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("获取用户资源权限列表(用于用户管理，显示有效权限)失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// clear user resource data
        /// </summary>
        /// <param name="entity"></param>
        public void ClearUserResourceList(int userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);
                QuickRepository.ExecuteProc("dbo.pr_sys_UserResourceListClear", param);
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// get user permission list
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<UserPermissionEntity> GetUserPermissionList(int userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);
                return QuickRepository.ExecProcQuery<UserPermissionEntity>("dbo.pr_sys_UserResourceListGet", param)
                    .ToList<UserPermissionEntity>();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
