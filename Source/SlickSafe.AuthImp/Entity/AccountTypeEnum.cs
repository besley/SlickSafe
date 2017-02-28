using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImp.Entity
{
    /// <summary>
    /// user account type
    /// </summary>
    public enum AccountTypeEnum
    {
        /// <summary>
        /// administrator (not listed in user list)
        /// </summary>
        Admin = -1,

        /// <summary>
        /// normal user
        /// </summary>
        User = 0,

        /// <summary>
        /// editor
        /// </summary>
        Editor = 1
    }
}
