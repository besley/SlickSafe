using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using SlickOne.Data;
using SlickOne.WebUtility;
using SlickOne.WebUtility.Security;
using SlickSafe.AuthImp.Entity;


namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// user account service
    /// </summary>
    public class AccountService : ServiceBase, IAccountService
    {
        /// <summary>
        /// user register
        /// </summary>
        /// <param name="account"></param>
        public void Register(UserAccountEntity account)
        {
            //verify input validation
            var result = ResponseResult.Default();
            var userEntity = QuickRepository.GetDefaultByName<UserAccountEntity>("LoginName", account.LoginName);
            if (userEntity != null)
            {
                throw new ApplicationException("用户名已经被占用，请重新存在！");
            }
            else if (string.IsNullOrEmpty(account.Password) || account.Password.Length < 6)
            {
                throw new ApplicationException("密码不能为空，或者长度不能小于6位！");
            }

            //create 
            try
            {
                QuickRepository.Insert<UserAccountEntity>(account);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// get user login name
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        public UserAccountEntity GetByLoginName(string loginName)
        {
            var user = QuickRepository.GetDefaultByName<UserAccountEntity>("LoginName", loginName);
            return user;
        }

        /// <summary>
        /// get user by ID
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserAccountEntity GetById(int id)
        {
            var user = QuickRepository.GetById<UserAccountEntity>(id);
            return user;
        }

        /// <summary>
        /// get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserAccountEntity GetByEmail(string email)
        {
            var user = QuickRepository.GetDefaultByName<UserAccountEntity>("EMail", email);
            return user;
        }

        /// <summary>
        /// user account paged query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<UserAccountEntity> GetPaged(UserAccountQuery query, out int count)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var sortList = new List<DapperExtensions.ISort>();
                sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });

                //administator not listed(AccountType=-1)
                if (string.IsNullOrEmpty(query.UserName))
                {
                    var predicate = Predicates.Field<UserAccountEntity>(a => a.AccountType, Operator.Eq, -1, true);
                    count = QuickRepository.Count<UserAccountEntity>(session.Connection, predicate);
                    var list = QuickRepository.GetPaged<UserAccountEntity>(session.Connection,
                        query.PageIndex, query.PageSize, predicate, sortList, false).ToList();
                    return list;
                }
                else
                {
                    //query by user name
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<UserAccountEntity>( f => f.AccountType, Operator.Eq, -1, true));
                    pg.Predicates.Add(Predicates.Field<UserAccountEntity>( f => f.UserName, 
                        Operator.Like, "%" + query.UserName + "%", false));

                    count = QuickRepository.Count<UserAccountEntity>(session.Connection, pg);
                    var list = QuickRepository.GetPaged<UserAccountEntity>(session.Connection,
                        query.PageIndex, query.PageSize, pg, sortList, true).ToList();

                    return list;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// update user account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool Update(UserAccountEntity account)
        {
            try
            {
                var user = QuickRepository.GetById<UserAccountEntity>(account.ID);
                user.Status = account.Status;
                user.AccountType = account.AccountType;
                var isOk = QuickRepository.Update<UserAccountEntity>(user);

                return isOk;
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// change user password
        /// </summary>
        /// <param name="oldPassword">old password</param>
        /// <param name="newPassword">new password</param>
        public void ChangePassword(string loginName, string oldPassword, string newPassword)
        {
            UserAccountEntity userEntity = null;
            try
            {
                userEntity = QuickRepository.GetDefaultByName<UserAccountEntity>("LoginName", loginName);
                var isChecked = CheckPassword(userEntity, oldPassword);     //it's better to limit wrong password 3 or 6 times to prevent someone crack the account
                if (!isChecked)
                {
                    throw new ApplicationException("用户名和密码不匹配，请重试.");
                }
            }
            catch (System.ApplicationException ex)
            {
                throw new ApplicationException("修改密码发生错误！");
            }

            try
            {
                var saltText = string.Empty;
                EnumHashProvider hashProvider;
                var encryptedPwd = HashingAlgorithmUtility.GetEncryptedHashText(newPassword, out saltText, out hashProvider);

                userEntity.Password = encryptedPwd;
                userEntity.PasswordFormat = (short)hashProvider;
                userEntity.PasswordSalt = saltText;

                QuickRepository.Update<UserAccountEntity>(userEntity);
            }
            catch (System.ApplicationException ex)
            {
                throw;
            }
        }

        /// <summary>
        /// instance a new user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private UserAccountEntity InstanceUser(string userName, string password, string email)
        {
            var saltText = string.Empty;
            EnumHashProvider hashProvider;
            var encryptedPwd = HashingAlgorithmUtility.GetEncryptedHashText(password, out saltText, out hashProvider);
            var pwdSHA256 = HashingAlgorithmUtility.ComputeHash(EnumHashProvider.SHA256Managed, password);

            var userEntity = new UserAccountEntity();
            userEntity.UserName = userName;
            userEntity.Password = encryptedPwd;
            userEntity.EMail = email;
            userEntity.PasswordFormat = (short)hashProvider;
            userEntity.PasswordSalt = saltText;

            return userEntity;
        }

        /// <summary>
        /// check user password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public bool CheckPassword(UserAccountEntity user, string password)
        {
            var isChecked = HashingAlgorithmUtility.CompareHash(user.PasswordFormat, 
                password, user.PasswordSalt, user.Password);

            return isChecked;
        }

        /// <summary>
        /// lock user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public void Lock(int userID)
        {
            try
            {
                var user = QuickRepository.GetById<UserAccountEntity>(userID);
                user.Status = (byte)AccountStatusEnum.Locked;
                QuickRepository.Update<UserAccountEntity>(user);
            }
            catch (System.Exception)
            {
                throw new ApplicationException("锁定用户账号操作失败！");
            }
        }

        /// <summary>
        /// unlock user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public void UnLock(int userID)
        {
            var user = QuickRepository.GetById<UserAccountEntity>(userID);
            if (user.Status != (byte)AccountStatusEnum.Locked)
            {
                throw new ApplicationException("用户账号状态不在锁定状态，不能进行解锁操作！");
            }

            try
            {
                user.Status = (byte)AccountStatusEnum.Actived;
                QuickRepository.Update<UserAccountEntity>(user);
            }
            catch (System.Exception)
            {
                throw new ApplicationException("解锁用户账号操作失败！");
            }
        }

        /// <summary>
        /// discard user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public void Discard(int userID)
        {
            IDbSession session = SessionFactory.CreateSession();
            session.BeginTrans();
            try
            {
                var user = QuickRepository.GetById<UserAccountEntity>(userID);
                user.Status = (byte)AccountStatusEnum.Discarded;
                QuickRepository.Update<UserAccountEntity>(session.Connection, user, session.Transaction);
                session.Commit();
            }
            catch (System.Exception)
            {
                session.Rollback();
                throw new ApplicationException("废弃用户账号操作失败！");
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string ResetPassword(int userID)
        {
            var session = SessionFactory.CreateSession();
            session.BeginTrans();
            try
            {
                var saltText = string.Empty;
                EnumHashProvider hashProvider;
                var r = new Random();
                var newPassword = r.Next(100000, 999999).ToString();
                var encryptedPwd = HashingAlgorithmUtility.GetEncryptedHashText(newPassword, out saltText, out hashProvider);

                var userEntity = QuickRepository.GetById<UserAccountEntity>(userID);
                userEntity.Password = encryptedPwd;
                userEntity.PasswordFormat = (short)hashProvider;
                userEntity.PasswordSalt = saltText;

                QuickRepository.Update<UserAccountEntity>(session.Connection, userEntity, session.Transaction);
                session.Commit();
                return newPassword;
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                throw new ApplicationException("用户密码修改发生错误！");
            }
            finally
            {
                session.Dispose();
            }
        }
    }
}
