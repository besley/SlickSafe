using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlickSafe.AuthImp.Entity
{
    /// <summary>
    /// role resource permission
    /// </summary>
    public class RoleResourcePermissionView 
    {
        public Int32 ID { get; set; }
        public Int32 RoleID { get; set; }
        public Int32 ResourceTypeID { get; set; }
        public Int32 ParentID { get; set; }
        public String ResourceCode { get; set; }
        public String ResourceName { get; set; }
        public Int16 PermissionType { get; set; }
    }
}
