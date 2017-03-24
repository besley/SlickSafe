using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// role entity
    /// </summary>
    [Table("SysRole")]
    public class RoleEntity
    {
        public int ID
        {
            get;
            set;
        }

        public string RoleName
        {
            get;
            set;
        }

        public string RoleCode
        {
            get;
            set;
        }
    }
}
