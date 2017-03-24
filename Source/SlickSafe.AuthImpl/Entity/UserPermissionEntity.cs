using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user permisison entity
    /// </summary>
    public class UserPermissionEntity
    {
        public int UserID { get; set; }
        public int ResourceID { get; set; }
        public string ResourceCode { get; set; }
        public string ResourceName { get; set; }
        public int ResourceTypeID { get; set; }
    }
}
