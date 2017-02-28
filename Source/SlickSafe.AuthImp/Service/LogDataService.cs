using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using SlickOne.Data;
using SlickSafe.AuthImp.Entity;

namespace SlickSafe.AuthImp.Service
{
    /// <summary>
    /// user log service
    /// </summary>
    public class LogDataService : ServiceBase, ILogDataService
    {
        /// <summary>
        /// get single record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserLogEntity Get(int id)
        {
            var entity = QuickRepository.GetById<UserLogEntity>(id);
            return entity;
        }

        /// <summary>
        /// get paged data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<UserLogEntity> GetPaged(UserLogQuery query, out int count)
        {
            IDbSession session = SessionFactory.CreateSession();
            try
            {
                var sortList = new List<DapperExtensions.ISort>();
                sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });

                IPredicate predicate = null;
                count = QuickRepository.Count<UserLogEntity>(session.Connection, predicate);
                var list = QuickRepository.GetPaged<UserLogEntity>(session.Connection,
                    query.PageIndex, query.PageSize, predicate, sortList, false).ToList();

                return list;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                session.Dispose();
            }
        }

        /// <summary>
        /// get user log record 100
        /// </summary>
        /// <returns></returns>
        public List<UserLogEntity> GetPaged100()
        {
            try
            {
                var sql = "SELECT TOP 100 * FROM SysUserLog ORDER BY ID DESC";
                var list = QuickRepository.Query<UserLogEntity>(sql).ToList<UserLogEntity>();
                return list;
            }
            catch(System.Exception ex)
            {
                throw;
            }
                 
        }

        /// <summary>
        /// insert login record
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public void Login(UserLogEntity log)
        {
            try
            {
                var entity = new UserLogEntity();
                entity.UserID = log.UserID;
                entity.LoginName = log.LoginName;
                entity.LoginTime = System.DateTime.Now;
                entity.LogoutTime = null;
                entity.SessionGUID = log.SessionGUID;
                entity.IPAddress = log.IPAddress;
                QuickRepository.Insert<UserLogEntity>(entity);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// insert logout record
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public void Logout(UserLogEntity log)
        {
            try
            {
                var sql = "SELECT * FROM SysUserLog WHERE SessionGUID = @sessionGUID";
                var entity = QuickRepository.Query<UserLogEntity>(sql,
                    new { 
                        sessionGUID = log.SessionGUID
                    }).ToList<UserLogEntity>()[0];

                if (entity != null)
                {
                    entity.LogoutTime = System.DateTime.Now;
                    QuickRepository.Update<UserLogEntity>(entity);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
