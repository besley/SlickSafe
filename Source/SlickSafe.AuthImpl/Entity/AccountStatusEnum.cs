using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user accout status
    /// </summary>
    public enum AccountStatusEnum
    {
        /// <summary>
        /// actived
        /// </summary>
        Actived = 0,

        /// <summary>
        /// locked
        /// </summary>
        Locked = 1,

        /// <summary>
        /// not used
        /// </summary>
        Discarded = 2
    }
}
