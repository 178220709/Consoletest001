using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.DB.Public.Interface;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    public class MetadataRegisterDALOS// : IMetaDataEdit, IMetaDataOper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        public MetadataRegisterDALOS(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
   
        #region 私有变量
        private readonly IDBHelper _dbHelper;
        private int _dataId = -1;
        private string _tableName = string.Empty;
        private List<DBFieldItem> _fieldItems;
        private string _wkt;
        #endregion


        #region Implementation of IMetaDataEdit

        /// <summary>
        /// 数据ID
        /// </summary>
        public int DataId
        {
            get { return _dataId; }
            set { _dataId = value; }
        }

        /// <summary>
        /// 元数据表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set { _tableName = value; }
        }

        public List<DBFieldItem> FieldItems
        {
            set { _fieldItems = value; }
        }

        public string WKT
        {
            get { return _wkt; }
            set { _wkt = value; }
        }

        #endregion

        #region 数据操作
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            string insertSQLString = SQLStringUtility.GetInsertSQL(TableName, _fieldItems, _dbHelper);
            return _dbHelper.DoSQL(insertSQLString);
        }
     

        public bool Update(DBFieldItem item)
        {
            string strFilter = FixedFieldName.FLD_NAME_F_DATAID + " = " + "'" + _dataId + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();
            items.Add(item);
            string sqlStatement = SQLStringUtility.GetUpdateSQL(TableName, items, strFilter, _dbHelper);
            bool bSuccess = _dbHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }


        public bool Update(IList<DBFieldItem> items)
        {
            string strFilter = FixedFieldName.FLD_NAME_F_DATAID + " = " + "'" + _dataId + "'";
   
            string sqlStatement = SQLStringUtility.GetUpdateSQL(TableName, items, strFilter, _dbHelper);
            bool bSuccess = _dbHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }
        

        public object GetValue(string fieldName)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", fieldName, _tableName,
                                       FixedFieldName.FLD_NAME_F_DATAID, _dataId);
            DataTable dt = _dbHelper.DoQueryEx(_tableName, sql, true);
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            return dt.Rows[0][fieldName];
        }

        /// <summary>
        /// 根据DataID删除,并级联删除TBARC_DATAIDMETA中对应记录
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            string sql = string.Format("DELETE FROM {0} WHERE {1} = {2}", _tableName, FixedFieldName.FLD_NAME_F_DATAID, _dataId);
            return _dbHelper.DoSQL(sql) >= 0;

        }


        public void Select()
        {
            _wkt = SelectWKT();
        }
        #endregion
   

        private string SelectWKT()
        {
            string shapeFieldName = SysSpatialParams.Para_GEOFIELDNAME;
            string sql =
                string.Format("SELECT t.{0}.GET_WKT() FROM {1} t WHERE {2}={3}", shapeFieldName, TableName,
                              SysParams.FLD_NAME_F_DATAID,
                              DataId);
            DataTable dt = _dbHelper.DoQueryEx(TableName, sql, true);
            string strWKT = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                strWKT = dt.Rows[0][0].ToString();
            }
            return strWKT;
        }
    }
}
