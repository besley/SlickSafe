using System;
using System.Collections.Generic;
using System.Linq;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// role resource entity
    /// </summary>
    [Table("SysRoleResource")]
	public partial class RoleResourceEntity
	{
		public Int32 ID { get; set; }	
		public Int32 RoleID { get; set; }	
		public Int32 ResourceID { get; set; }	
		public Int16 PermissionType { get; set; }	
	}
}