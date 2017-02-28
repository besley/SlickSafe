using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// user account service
    /// </summary>
    public interface IAccountService
    {
        void Register(UserAccountEntity account);
        void ChangePassword(string userName, string oldPassword, string newPassword);
        UserAccountEntity GetById(int id);
        UserAccountEntity GetByLoginName(string loginName);
        UserAccountEntity GetByEmail(string email);
        List<UserAccountEntity> GetPaged(UserAccountQuery query, out int count);
        bool Update(UserAccountEntity account);
        void Lock(int userID);
        void UnLock(int userID);
        void Discard(int userID);
        string ResetPassword(int userID);
    }
}
