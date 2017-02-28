using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// user log service
    /// </summary>
    public interface ILogDataService
    {
        UserLogEntity Get(int id);
        List<UserLogEntity> GetPaged(UserLogQuery query, out int count);
        List<UserLogEntity> GetPaged100();
        void Login(UserLogEntity log);
        void Logout(UserLogEntity log);
    }
}
