using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImpl.Entity
{
    /// <summary>
    /// user account query
    /// </summary>
    public class UserAccountQuery : QueryBase
    {
        public string UserName { get; set; }
    }
}
