using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlickSafe.AuthImp.Entity
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
