using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Consoletest001.Sqlite.Entity;
using RMS_GXEA.SQLServerDAL;

namespace Consoletest001.Sqlite.DAL
{
    //t_user
    public class UserDal
    {

        public static bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from t_user");
            strSql.Append(" where ");
            strSql.Append(" id = @id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;

            return SqlHelper.ExecuteNonQuery(CommandType.Text,strSql.ToString(), parameters)>0;
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static int Add(UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into t_user(");
            strSql.Append("userid,password,username,email,role");
            strSql.Append(") values (");
            strSql.Append("@userid,@password,@username,@email,@role");
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
			            new SqlParameter("@userid", SqlDbType.NChar,20) ,            
                        new SqlParameter("@password", SqlDbType.NChar,20) ,            
                        new SqlParameter("@username", SqlDbType.NChar,10) ,            
                        new SqlParameter("@email", SqlDbType.NChar,10) ,            
                        new SqlParameter("@role", SqlDbType.Int,4)             
              
            };

            parameters[0].Value = model.userid;
            parameters[1].Value = model.password;
            parameters[2].Value = model.username;
            parameters[3].Value = model.email;
            parameters[4].Value = model.role;

            object obj = SqlHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            {
                return Convert.ToInt32(obj);
            }
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public static bool Update(UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update t_user set ");

            strSql.Append(" userid = @userid , ");
            strSql.Append(" password = @password , ");
            strSql.Append(" username = @username , ");
            strSql.Append(" email = @email , ");
            strSql.Append(" role = @role  ");
            strSql.Append(" where id=@id ");

            SqlParameter[] parameters = {
			            new SqlParameter("@id", SqlDbType.Int,4) ,            
                        new SqlParameter("@userid", SqlDbType.NChar,20) ,            
                        new SqlParameter("@password", SqlDbType.NChar,20) ,            
                        new SqlParameter("@username", SqlDbType.NChar,10) ,            
                        new SqlParameter("@email", SqlDbType.NChar,10) ,            
                        new SqlParameter("@role", SqlDbType.Int,4)             
              
            };

            parameters[0].Value = model.id;
            parameters[1].Value = model.userid;
            parameters[2].Value = model.password;
            parameters[3].Value = model.username;
            parameters[4].Value = model.email;
            parameters[5].Value = model.role;
            int rows = SqlHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public static bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_user ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;


            int rows = SqlHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public static bool DeleteList(string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_user ");
            strSql.Append(" where ID in (" + idlist + ")  ");
            int rows = SqlHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public static UserInfo GetModel(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id, userid, password, username, email, role  ");
            strSql.Append("  from t_user ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;


            UserInfo model = new UserInfo();
            DataSet ds = SqlHelper.ExecuteDataset(CommandType.Text, strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                model.userid = ds.Tables[0].Rows[0]["userid"].ToString();
                model.password = ds.Tables[0].Rows[0]["password"].ToString();
                model.username = ds.Tables[0].Rows[0]["username"].ToString();
                model.email = ds.Tables[0].Rows[0]["email"].ToString();
                if (ds.Tables[0].Rows[0]["role"].ToString() != "")
                {
                    model.role = int.Parse(ds.Tables[0].Rows[0]["role"].ToString());
                }

                return model;
            }
            return null;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public static DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM t_user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.ExecuteDataset(CommandType.Text, strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public static DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM t_user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.ExecuteDataset(CommandType.Text, strSql.ToString());
        }


    }
}

