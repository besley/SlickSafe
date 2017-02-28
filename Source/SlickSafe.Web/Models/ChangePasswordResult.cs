using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SlickOne.WebUtility;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.Web.Models
{
    /// <summary>
    /// change password result
    /// </summary>
    public class ChangePasswordResult : ResponseResult<UserPassword>
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Ticket { get; set; }
    }
}