/*
* SlickSafe 企业级权限快速开发框架遵循LGPL协议，也可联系作者获取商业授权
* 和技术支持服务；除此之外的使用，则视为不正当使用，请您务必避免由此带来的
* 商业版权纠纷。
*
The SlickSafe Product.
Copyright (C) 2017  .NET Authorization Framework Software

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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
    public class LogService : ServiceBase, ILogService
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
