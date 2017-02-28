using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Security.Cryptography;
using SlickOne.Data;
using SlickOne.WebUtility;
using SlickSafe.AuthImp.Entity;
using SlickSafe.AuthImp.Service;
using SlickSafe.Web.Filter;

namespace SlickSafe.Web.Controllers.WebApi
{
    /// <summary>
    /// user account controller
    /// </summary>
    public class AccountController : ApiController
    {
        #region account service
        /// <summary>
        /// account service instance
        /// </summary>
        private IAccountService _accountService;
        protected IAccountService AccountService
        {
            get
            {
                if (_accountService == null)
                {
                    _accountService = new AccountService();
                }
                return _accountService;
            }
        }
        #endregion

        /// <summary>
        /// register
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult Register(UserAccountEntity account)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Register(account);
                result = ResponseResult.Success("创建用户成功！");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error("创建用户失败，错误代码：4001！");
            }
            return result;
        }

        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult<UserAccountEntity> GetById(int id)
        {
            var result = ResponseResult<UserAccountEntity>.Default();
            try
            {
                var user = AccountService.GetById(id);
                result = ResponseResult<UserAccountEntity>.Success(user);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<UserAccountEntity>.Error("获取用户信息失败！");
            }
            return result;
        }

        /// <summary>
        /// query by user name
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<UserAccountEntity> QueryUserByName(LoginUser user)
        {
            var result = ResponseResult<UserAccountEntity>.Default();
            try
            {
                var entity = AccountService.GetByLoginName(user.LoginName);
                result = ResponseResult<UserAccountEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<UserAccountEntity>.Error("获取用户信息失败！");
            }
            return result;
        }

        /// <summary>
        /// change user password
        /// </summary>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult ChangePassword(UserPassword userPwd)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.ChangePassword(userPwd.LoginName, userPwd.OldPassword, userPwd.NewPassword);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error("用户密码修改发生错误！");
            }
            return result;
        }

        /// <summary>
        /// reset user password
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult ResetPassword(UserAccountEntity account)
        {
            var result = ResponseResult.Default();
            try
            {
                var newPassword = AccountService.ResetPassword(account.ID);
                result = ResponseResult.Success(string.Format("用户密码重置成功，新密码是:{0}", newPassword));
                result.ExtraData = newPassword;
            }
            catch (System.ApplicationException ex)
            {
                result = ResponseResult.Error("用户密码重置发生错误！");
            }
            return result;
        }

        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Update(UserAccountEntity account)
        {
            var result = ResponseResult.Default();

            try
            {
                var isOk = AccountService.Update(account);
                if (isOk == true)
                {
                    result = ResponseResult.Success();
                }
                else
                {
                    result = ResponseResult.Error("用户账户资料修改发生错误！");
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(string.Format("用户账户资料修改发生错误，描述：{0}", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// lock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Lock(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Lock(id);
                result = ResponseResult.Success("锁定用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("锁定用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult UnLock(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.UnLock(id);
                result = ResponseResult.Success("解锁用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("解锁用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// discard user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult Discard(dynamic id)
        {
            var result = ResponseResult.Default();
            try
            {
                AccountService.Discard(int.Parse(id.ID.ToString()));
                result = ResponseResult.Success("废弃用户账号操作成功！");
            }
            catch (System.Exception)
            {
                result = ResponseResult.Error("废弃用户账号操作失败！");
            }
            return result;
        }

        /// <summary>
        /// user account paged query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ResponseResult<List<UserAccountEntity>> SelectPaged(UserAccountQuery query)
        {
            var result = ResponseResult<List<UserAccountEntity>>.Default();
            try
            {
                var count = 0;
                var list = AccountService.GetPaged(query, out count);

                result = ResponseResult<List<UserAccountEntity>>.Success(list);
                result.TotalRowsCount = count;
                result.TotalPages = (count + query.PageSize - 1) / query.PageSize;
            }
            catch (System.Exception)
            {
                result = ResponseResult<List<UserAccountEntity>>.Error("获取用户账号分页数据失败！");
            }
            return result;
        }

    }
}
