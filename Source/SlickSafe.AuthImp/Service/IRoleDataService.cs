using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// role service
    /// </summary>
    public interface IRoleDataService
    {
        IList<RoleEntity> GetRoleAll();
        void SaveRole(RoleEntity entity);
        void DeleteRole(RoleEntity entity);

        IList<UserAccountEntity> GetUserAll();
        void SaveUser(UserEntity entity);
        void DeleteUser(UserEntity entity);

        IList<RoleUserView> GetRoleUserAll();
        IList<RoleUserView> QueryUserByRole(RoleEntity query);
        void AddRoleUser(RoleUserEntity entity);
        void DeleteRoleUser(RoleUserEntity entity);

    }
}
