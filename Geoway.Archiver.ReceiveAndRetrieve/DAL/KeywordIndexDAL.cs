using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 查询关键字索引数据库操作类
    /// </summary>
    public class KeywordIndexDAL : DALBase<KeywordIndexDAL>
    {
        private const string CONST_TABLE_NAME = "TBIMG_KEYWORDINDEX";
        private const string CONST_FLD_NAME_F_VALUE = "F_VALUE";
        private const string CONST_FLD_NAME_F_TYPE = "F_TYPE";
        private const string CONST_FLD_NAME_F_TIMES = "F_TIMES";

        #region 单例模式
        public static KeywordIndexDAL Singleton = new KeywordIndexDAL();
        #endregion

        #region 属性

        private EnumKeywordIndexType _indexType;
        /// <summary>
        /// 索引类别
        /// </summary>
        public EnumKeywordIndexType IndexType
        {
            get { return _indexType; }
            set { _indexType = value; }
        }

        private string _indexValue = string.Empty;
        /// <summary>
        /// 索引值
        /// </summary>
        public string IndexValue
        {
            get { return _indexValue; }
            set { _indexValue = value; }
        }

        private int _times = 1;
        /// <summary>
        /// 出现次数
        /// </summary>
        public int Times
        {
            get { return _times; }
            set { _times = value; }
        }

        #endregion

        #region 重载函数
        public override bool Insert()
        {
            string sqlStatement = string.Empty;
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(CONST_FLD_NAME_F_TYPE, (int)_indexType, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(CONST_FLD_NAME_F_VALUE, _indexValue, EnumDBFieldType.FTString));

            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(CONST_TABLE_NAME, items, DBHelper.GlobalDBHelper);
                return DoSQL(sqlStatement);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return false;
        }

        public override bool Delete()
        {
            string filter = string.Format("{0}={1} AND {2}='{3}'",
                                          CONST_FLD_NAME_F_TYPE, (int)_indexType,
                                          CONST_FLD_NAME_F_VALUE, _indexValue);
            return DBHelper.GlobalDBHelper.DeleteRow(CONST_TABLE_NAME, filter)>0;
        }

        public override bool Update()
        {
            throw new Exception("Update is refused!");
        }

        // <summary>
        /// 选择
        /// </summary>
        /// <returns>所有数据列表</returns>
        public override IList<KeywordIndexDAL> Select()
        {
            IList<KeywordIndexDAL> pList = new List<KeywordIndexDAL>();
            DataTable dtResult = SelectEx();
            pList = Translate(dtResult);
            return pList;
        }

        public override string ToString()
        {
            return _indexValue;
        }

        #endregion

        #region 扩展属性查询

        public KeywordIndexDAL Select(string keyword, EnumKeywordIndexType type)
        {
            string strFilter = "LOWER(" + CONST_FLD_NAME_F_VALUE + ") = '" + keyword.ToLower() + "' AND " + CONST_FLD_NAME_F_TYPE + " = " + (int)type;
            DataTable dtResult = DoQurey(strFilter, "", true);
            if (dtResult.Rows.Count > 0)
            {
                return Translate(dtResult.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 出现次数加一
        /// </summary>
        /// <returns></returns>
        public bool IncreaseTimes()
        {
            string sql = string.Format("update {0} set {1}={1}+1 where {2}={3} AND {4}='{5}'",
                                        CONST_TABLE_NAME,
                                        CONST_FLD_NAME_F_TIMES,
                                        CONST_FLD_NAME_F_TYPE, (int)_indexType,
                                        CONST_FLD_NAME_F_VALUE, _indexValue);
            return DoSQL(sql);
        }

        /// <summary>
        /// 出现次数减一
        /// </summary>
        /// <returns></returns>
        public bool DecreaseTimes()
        {
            string sql = string.Format("update {0} set {1}={1}-1 where {2}={3} AND {4}='{5}'",
                                        CONST_TABLE_NAME,
                                        CONST_FLD_NAME_F_TIMES,
                                        CONST_FLD_NAME_F_TYPE, _indexType,
                                        CONST_FLD_NAME_F_VALUE, _indexValue);
            return DoSQL(sql);
        }

        public IList<KeywordIndexDAL> Select(string keyword)
        {
            string strFilter = "LOWER(" + CONST_FLD_NAME_F_VALUE + ") = '" + keyword.ToLower() + "'";
            DataTable dtResult = DoQurey(strFilter, "", true);
            return Translate(dtResult);
        }

        public IList<KeywordIndexDAL> Select(EnumKeywordIndexType type)
        {
            string filter = CONST_FLD_NAME_F_TYPE + '=' + (int)type;
            DataTable dtResult = DoQurey(filter, CONST_FLD_NAME_F_VALUE, true);
            return Translate(dtResult);
        }

        private DataTable DoQurey(string filter, string orderField, bool esc)
        {
            string sql = "SELECT " + getSelectField() + " FROM " + CONST_TABLE_NAME + " WHERE " + filter;
            if (!string.IsNullOrEmpty(orderField))
            {
                sql += " ORDER BY " + orderField + (esc ? "" : "DESC");
            }
            return DBHelper.GlobalDBHelper.DoQueryEx(CONST_TABLE_NAME, sql, true);
        }

        public DataTable SelectEx()
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + CONST_TABLE_NAME + " order by " + CONST_FLD_NAME_F_TIMES + " desc ";
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(CONST_TABLE_NAME, sqlStatement, true);
            return dtResult;
        }
        #endregion

        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<KeywordIndexDAL> Translate(DataTable dtResult)
        {
            IList<KeywordIndexDAL> list = new List<KeywordIndexDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow pRow = null;
                string sLayers = string.Empty;
                KeywordIndexDAL info = null;
                foreach (DataRow row in dtResult.Rows)
                {
                    info = Translate(row);
                    list.Add(info);
                }
            }
            return list;
        }

        private KeywordIndexDAL Translate(DataRow row)
        {
            KeywordIndexDAL info = new KeywordIndexDAL();
            info.IndexType = (EnumKeywordIndexType)GetSafeDataUtility.ValidateDataRow_N(row, CONST_FLD_NAME_F_TYPE);
            info.IndexValue = GetSafeDataUtility.ValidateDataRow_S(row, CONST_FLD_NAME_F_VALUE);
            info.Times = GetSafeDataUtility.ValidateDataRow_N(row, CONST_FLD_NAME_F_TIMES);
            return info;
        }

        #endregion

        #region 内部方法

        private string getSelectField()
        {
            return CONST_FLD_NAME_F_VALUE + "," +
                   CONST_FLD_NAME_F_TYPE + "," +
                   CONST_FLD_NAME_F_TIMES;
        }

        #endregion
    }
}
