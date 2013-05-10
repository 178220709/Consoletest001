using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Consoletest001.Sqlite.Common
{
    public enum RoleEnum
    {
        管理员 = 0,
        Admin = 0,
        User = 1
    }
    public sealed class MyConfig
    {
        public static string DbPath = "e:\\mydb.db";
        public static MyConfig Instane = new MyConfig();
        private  SQLiteConnection _conn;
        public SQLiteConnection Conn
        {
            get
            {
                if (_conn==null)
                {
                    _conn = GetThisConn();
                    return _conn;
                }
                else
                {
                    return _conn;
                }
            }
        }

        public SQLiteConnection GetThisConn()
        {
           
            var connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = DbPath;
            return new SQLiteConnection(connstr.ToString());

        }

    }
}
