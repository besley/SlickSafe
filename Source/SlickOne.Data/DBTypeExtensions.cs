using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickOne.Data
{
    /// <summary>
    /// 数据库类型标识
    /// </summary>
    public enum DBTypeEnum
    {
        SQLSERVER = 1,

        ORACLE = 2,

        MYSQL = 3,

        KINGBASE = 4
    }

    /// <summary>
    /// 数据库类型的标识类
    /// </summary>
    public static class DBTypeExtenstions
    {
        private static DBTypeEnum dbType = DBTypeEnum.SQLSERVER;
        public static DBTypeEnum DBType 
        { 
            get 
            { 
                return dbType; 
            } 
        }

        /// <summary>
        /// 设置数据库类型变量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        public static void SetDBType(DBTypeEnum type)
        {
            dbType = type;
        }
    }
}
