using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// resource data service
    /// </summary>
    public interface IResourceDataService
    {
        List<ResourceEntity> GetResourceByUserID(int userID);
        ResourceEntity SaveResource(ResourceEntity entity);
        void DeleteResource(XmlTransferEntity xmlEntity);
        void DeleteResource(int resourceID);
        List<ResourceEntity> GetResourceAll();
        ResourceNode GetResourceNodeAll();

        ResourceNode[] GetLeftMenuList(int userID);

        List<RoleResourcePermissionView> GetRoleResourceList(int roleID);
        List<RoleResourcePermissionView> GetResourcePermissionAllowed(int userID);
        void SaveRoleResourceList(List<RoleResourceEntity> entityList);
        void ClearRoleResourceList(RoleResourceEntity entity);

        
        List<UserResourcePermissionView> RetrieveUserResourceList(int userID);
        void SaveUserResourceList(List<UserResourceEntity> entityList);
        void ClearUserResourceList(int userID);
        List<UserPermissionEntity> GetUserPermissionList(int userID);
    }
}
