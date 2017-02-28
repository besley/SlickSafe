using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickSafe.AuthImp.Entity
{
    /// <summary>
    /// login user entity
    /// </summary>
    public class LoginUser
    {
        public string LoginName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string EMail
        {
            get;
            set;
        }

        public string ValidatedText
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string Ticket
        {
            get;
            set;
        }
    }
}
