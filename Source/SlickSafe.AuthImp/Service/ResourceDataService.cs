using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using SlickOne.Data;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// resource data service implementation
    /// </summary>
    public class ResourceDataService : ServiceBase, IResourceDataService
    {
        #region resource basic operation
        /// <summary>
        /// get all resource record
        /// </summary>
        /// <returns></returns>
        public List<ResourceEntity> GetResourceAll()
        {
            List<ResourceEntity> list = new List<ResourceEntity>();
            try
            {
                list = QuickRepository.GetAll<ResourceEntity>().ToList<ResourceEntity>();
                return list;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("获取所有应用资源信息失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// save resource
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResourceEntity SaveResource(ResourceEntity entity)
        {
            try
            {
                if (entity.ID > 0)
                {
                    QuickRepository.Update<ResourceEntity>(entity);
                }
                else
                {
                    var newID = QuickRepository.Insert<ResourceEntity>(entity);
                    entity.ID = newID;
                }
                return entity;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("删除资源数据，并且删除相关联的表中数据失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// delete resource
        /// </summary>
        /// <param name="xmlEntity"></param>
        public void DeleteResource(XmlTransferEntity xmlEntity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@strXml", xmlEntity.xmlBody);
                QuickRepository.ExecuteProc("pr_prd_DeleteResourceBeth", param);
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("删除资源数据，并且删除相关联的表中数据失败!", ex);
                throw;
            }
        }

        /// <summary>
        /// delete resource
        /// </summary>
        /// <param name="resourceID">资源ID</param>
        public void DeleteResource(int resourceID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@resourceID", resourceID);
                QuickRepository.ExecuteProc("pr_sys_ResourceListDeleteByID", param);
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("删除资源数据，并且删除相关联的表中数据失败!", ex);
                throw;
            }
        }
        #endregion

        #region 获取资源节点树
        /// <summary>
        /// get resource node --TreeView for ag-grid
        /// </summary>
        /// <returns></returns>
        public ResourceNode GetResourceNodeAll()
        {
            try
            {
                var rootNode = new ResourceNode { ID = 0, ParentID = -1, ResourceName = "资源列表", group = true };
                var resourceList = QuickRepository.GetAll<ResourceEntity>().ToList<ResourceEntity>();
                var rootItems = from a in resourceList
                                where a.ParentID == 0
                                select a;
                int index = 0;
                ResourceNode[] resourceTreeTop = new ResourceNode[rootItems.Count()];
                foreach (var item in rootItems)
                {
                    ResourceNode[] childrenItems = GetResourceNodeListIteratedly(item.ID, resourceList);
                    resourceTreeTop[index++] = CreateResourceNodeSingle(item, childrenItems);
                }
                rootNode.children = resourceTreeTop;
                return rootNode;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// get resource node iterated
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private ResourceNode[] GetResourceNodeListIteratedly(int parentID, List<ResourceEntity> list)
        {
            var filterList = from a in list
                             where a.ParentID == parentID
                             select a;
            int index = 0;
            ResourceNode[] resourceTreeInner = new ResourceNode[filterList.Count()];
            foreach (var item in filterList)
            {
                ResourceNode[] childrenItems = GetResourceNodeListIteratedly(item.ID, list);
                resourceTreeInner[index++] = CreateResourceNodeSingle(item, childrenItems);
            }
            return resourceTreeInner;
        }

        /// <summary>
        /// create resource node
        /// </summary>
        /// <param name="item"></param>
        /// <param name="childrenItems"></param>
        /// <returns></returns>
        private ResourceNode CreateResourceNodeSingle(ResourceEntity item, ResourceNode[] childrenItems)
        {
            ResourceNode resourceNode = null;
            resourceNode = new ResourceNode
            {
                ID = item.ID,
                ParentID = item.ParentID,
                ResourceTypeID = item.ResourceTypeID,
                ResourceName = item.ResourceName,
                UrlAction = item.UrlAction,
                DataAction = item.DataAction,
                StyleClass = item.StyleClass,
                OrderNum = item.OrderNum
            };

            if (childrenItems.Count() > 0)
            {
                resourceNode.children = childrenItems;
                resourceNode.group = true;
            } else
            {
                resourceNode.group = false;
            }
            return resourceNode;
        }
        #endregion

        /// <summary>
        /// get resource by user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ResourceEntity> GetResourceByUserID(int userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);

                var list = QuickRepository.ExecProcQuery<ResourceEntity>("pr_sys_ResourceListGetByUserID",
                     param).ToList<ResourceEntity>();

                return list;
            }
            catch (System.Exception ex)
            {
                //NLogWriter.Error("根据UserID获取资源列表(用于用户登录,保存可操作资源)失败!", ex);
                throw;
            }
        }

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
