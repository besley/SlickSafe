using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user resource entity
    /// </summary>
    [Table("SysUserResource")]
    public class UserResourceEntity
    {
        public Int32 ID { get; set; }
        public Int32 UserID { get; set; }
        public Int32 ResourceID { get; set; }
        public Int16 PermissionType { get; set; }
        public Int16 IsInherited { get; set; }
    }
}
