using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user log entity
    /// </summary>
    [Table("SysUserLog")]
    public class UserLogEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string SessionGUID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string IPAddress { get; set; }
    }
}
