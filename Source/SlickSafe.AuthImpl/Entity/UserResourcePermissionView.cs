using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user resource permission
    /// </summary>
    public class UserResourcePermissionView
    {
        public Int32 ID { get; set; }
        public Int32 UserID { get; set; }
        public Int32 ResourceTypeID { get; set; }
        public Int32 ParentID { get; set; }
        public String ResourceCode { get; set; }
        public String ResourceName { get; set; }
        public Int16 PermissionType { get; set; }
        public short IsAllowInherited { get; set; }
        public short IsAllowSelf { get; set; }
        public short IsDenyInherited { get; set; }
        public short IsDenySelf { get; set; }
    }
}
