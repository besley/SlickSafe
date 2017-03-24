using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user entity
    /// </summary>
    [Table("SysUser")]
    public class UserEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
    }
}
