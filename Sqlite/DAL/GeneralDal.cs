using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using Consoletest001.Sqlite.Common;
using Consoletest001.Sqlite.Entity;

namespace Consoletest001.Sqlite.DAL
{

    public class GeneralDal
    {
        //获取所有的表结构
        public static DataTable GetAlltbInfo()
        {
            string sql = "select * from sqlite_master where type = 'table'";
            return SQLiteHelper.ExecuteDataSet(MyConfig.Instane.Conn, sql, null).Tables[0] ;
        }

        public static DataTable GetTableFromName(string tableName)
        {
            string sql = string.Format(" select * from ({0})", tableName);
            return SQLiteHelper.ExecuteDataSet(MyConfig.Instane.Conn, sql, null).Tables[0];
        }

        /// <summary>
        /// 输入的是GetAlltbInfo()返回的值，包含了所有数据库的信息。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
       public static bool SaveTableToSqlserver( DataTable dt)
       {
          

           return true;
       }

       public static bool CreadTableStructure(DataTable dt)
        {
            
        }

    }
}
