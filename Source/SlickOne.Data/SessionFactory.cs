/*
* SlickOne 企业级Web快速开发框架遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2016  .NET Web Framwork Library

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
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
//using Oracle.ManagedDataAccess.Client;

namespace SlickOne.Data
{
    /// <summary>
    /// Session 创建类
    /// </summary>
    public static class SessionFactory
    {
        /// <summary>
        /// 创建类的构造方法
        /// </summary>
        static SessionFactory()
        {
            InitializeDBType(DBTypeEnum.SQLSERVER);     //多数据库枚举类型，ORACLE, MYSQL等
        }

        /// <summary>
        /// 设置数据库类型相关的变量
        /// </summary>
        /// <param name="type">数据库类型</param>
        private static void InitializeDBType(DBTypeEnum type)
        {
            DBTypeExtenstions.SetDBType(type);

            //update sql dialect in dapper extension
            if (type == DBTypeEnum.ORACLE)
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleSqlDialect();
            else if (type == DBTypeEnum.MYSQL)
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
            else if (type == DBTypeEnum.KINGBASE)
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.KingbaseSqlDialect();
        }

        /// <summary>
        /// 根据DBType类型，创建数据库连接
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateConnectionByDBType()
        {
            IDbConnection conn = null;
            dynamic appSettings = new AppSettingsWrapper();
            string appSettingDBConnection = appSettings.WebAppDBConnectionString.ToString();
            var connStringSetting = ConfigurationManager.ConnectionStrings[appSettingDBConnection];
            if (DBTypeExtenstions.DBType == DBTypeEnum.SQLSERVER)
            {
                conn = new SqlConnection(connStringSetting.ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.ORACLE)
            {
                //conn = new OracleConnection(connStringSetting.ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.MYSQL)
            {
                //conn = new MySqlConnection(connStringSetting.ConnectionString);
            }
            return conn;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = CreateConnectionByDBType();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 创建数据库连接会话
        /// </summary>
        /// <returns></returns>
        public static IDbSession CreateSession()
        {
            IDbConnection conn = CreateConnection();
            IDbSession session = new DbSession(conn);

            return session;
        }

        /// <summary>
        /// 创建数据库事务会话
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static IDbSession CreateSession(IDbConnection conn, IDbTransaction trans)
        {
            IDbSession session = new DbSession(conn, trans);
            return session;
        }
    }
}
