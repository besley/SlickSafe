using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImp.Entity
{
    /// <summary>
    /// user password
    /// </summary>
    public class UserPassword
    {
        public int UserID
        {
            get;
            set;
        }

        public string LoginName
        {
            get;
            set;
        }

        public string OldPassword
        {
            get;
            set;
        }

        public string NewPassword
        {
            get;
            set;
        }
    }
}
