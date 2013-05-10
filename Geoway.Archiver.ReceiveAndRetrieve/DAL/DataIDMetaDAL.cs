using System;
using System.Collections.Generic;
using System.Data;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    public class DataIDMetaDAL
    {

        #region 数据库结构

        public const string TABLE_NAME = "TBARC_DATAIDMETA";
        public const string FLD_NAME_F_DATAID = "F_DATAID";
        public const string FLD_NAME_F_METATABLE = "F_METATABLE"; //所属对象
        #endregion

        private int _dataID;
        private string _metaTable;

        public int DataId
        {
            get { return _dataID; }
            set { _dataID = value; }
        }

        public string MetaTable
        {
            get { return _metaTable; }
            set { _metaTable = value; }
        }

        public IList<DataIDMetaDAL> Select(IDBHelper db)
        {
            IList<DataIDMetaDAL> pList = new List<DataIDMetaDAL>();
            string filter=FLD_NAME_F_DATAID+" = "+_dataID;
            DataTable dtResult = DoQuery(db, filter);
            pList = Translate(dtResult);
            return pList;
        }

        public bool Insert(IDBHelper db)
        {
            string sqlStatement = string.Empty;
            IList<DBFieldItem> items = new List<DBFieldItem>();

            _dataID = db.GetNextValidID(TABLE_NAME, FLD_NAME_F_DATAID);
            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_METATABLE, _metaTable, EnumDBFieldType.FTString));
         

            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, db);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            bool bSuccess = db.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }

        public bool Delete(IDBHelper db)
        {
            string sqlStatement;
            sqlStatement = string.Format("DELETE FROM {0} WHERE {1}={2}", TABLE_NAME, FLD_NAME_F_DATAID, _dataID);
            bool bSuccess = db.DoSQL(sqlStatement) >= 0;

            return bSuccess;
        }

        public bool Update(IDBHelper db)
        {
            string sqlStatement;
            string strFilter = FLD_NAME_F_METATABLE + " = " + "'" + this._metaTable + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataID, EnumDBFieldType.FTString));
            sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter, db);

            bool bSuccess = db.DoSQL(sqlStatement) > 0;

            return bSuccess;
        }

        public DataTable DoQuery(IDBHelper db)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " order by " + FLD_NAME_F_DATAID;
            DataTable dtResult = db.DoQueryEx(TABLE_NAME, sqlStatement, true);
            
            return dtResult;
        }
        public DataTable DoQuery(IDBHelper db, string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " where " + strFilter + " order by " + FLD_NAME_F_DATAID;
            DataTable dtResult = db.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        #region 扩展查询
        public static DataIDMetaDAL SelectByDataID(IDBHelper db, int dataID)
        {
            DataIDMetaDAL dal = new DataIDMetaDAL();
            dal.DataId = dataID;
            IList<DataIDMetaDAL> dals = dal.Select(db);
            return dals.Count > 0 ? dals[0] : null;
        }
        
        #endregion

        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        /// </summary>
        public IList<DataIDMetaDAL> Translate(DataTable dtResult)
        {
            IList<DataIDMetaDAL> list = new List<DataIDMetaDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    DataIDMetaDAL info = new DataIDMetaDAL();
                    info.DataId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);
                    info.MetaTable = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_METATABLE);
                    list.Add(info);
                }
            }
            return list;
        }
        #endregion


        #region 内部方法

        private string getSelectField()
        {
            string fields = FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_METATABLE;
            return fields;
        }

        #endregion
    }
}
