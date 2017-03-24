using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// resource query
    /// </summary>
    public class ResourceQuery : QueryBase
    {
        public int RoleID { get; set; }
        public int UserID { get; set; }
    }
}
