using System.Data;
using System.Data.SQLite;
using Consoletest001.Sqlite.Common;
using Consoletest001.Sqlite.Entity;

namespace Consoletest001.Sqlite.DAL
{

    public class PhoneDal
    {
        //创建表
        public static bool CreadUserTable()
        {
            string sql = "CREATE TABLE phone (name VARCHAR(20), phoneNumber TEXT, homeNumber TEXT)";
            return SQLiteHelper.ExecuteNonQuery(MyConfig.Instane.Conn, sql, null) > 0;
        }

        public static int insertInfo(PhoneInfo  info)
        {
            string str =
                "insert into phone(name,phoneNumber,homeNumber) values" +
                " (@name,@phoneNumber,@homeNumber)";
            return SQLiteHelper.ExecuteNonQuery(MyConfig.Instane.Conn, str, info.Name,info.PhoneNumber,info.HomeNumber);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int DeleteInfo(string name)
        {
            string str = "delete from phone  where name = @name";

            return SQLiteHelper.ExecuteNonQuery(MyConfig.Instane.Conn, str, name);
        }

        public static int DeleteInfo(PhoneInfo info)
        {
            return DeleteInfo(info.Name);
        }

        public static int UpdatePhone(PhoneInfo info)
        {
            string str = "update  phone set phoneNumber=@phoneNumber,homeNumber=@homeNumber where name=@name";
            return SQLiteHelper.ExecuteNonQuery(MyConfig.Instane.Conn, str, info.PhoneNumber,info.HomeNumber,info.Name);
        }

        public static DataTable GetPhoneTable(string name)
        {
            string str = "select * from   phone where name=@name";
            DataSet ds =  SQLiteHelper.ExecuteDataSet(MyConfig.Instane.Conn,str,name);

            return ds.Tables[0];
        }

        public static DataTable GetAllPhoneTable()
        {
            string str = "select * from   phone";
            DataSet ds = SQLiteHelper.ExecuteDataSet(MyConfig.Instane.Conn, str, null);

            return ds.Tables[0];
        }

        public static PhoneInfo GetPhoneInfo(string name)
        {
            DataTable dt = GetPhoneTable(name);
            PhoneInfo info = new PhoneInfo();

            info.Name = name;
            info.PhoneNumber = dt.Rows[0]["phoneNumber"].ToString();
            info.HomeNumber = dt.Rows[0]["homeNumber"].ToString();
            return info;
        }

    }
}
