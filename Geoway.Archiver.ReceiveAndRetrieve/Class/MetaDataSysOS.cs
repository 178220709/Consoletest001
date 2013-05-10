using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.Utility.Class;
using Geoway.OSpatial.SDOGeometry.Interface;
using Geoway.OSpatial.SDOGeometry.TranGeo;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.Core.Public.security;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 写元数据表系统维护字段
    /// </summary>
    public class MetaDataSysOS : MetaDataSysInfo,IMetaDataSys,IMetaDataQuery
    {

        public MetaDataSysOS(IDBHelper dbHelper): base(dbHelper){}

        public MetaDataSysOS(IDBHelper dbHelper, int dataID): base(dbHelper, dataID)
        {
            IMetaDataSysEdit coreMeta;
            
            if(dataID<0)
            {
                _deleteTime = DateTime.MinValue;
                _flag = -1;
                _importDate = DateTime.Now;
                _importUser = LoginControl.userName;
                _name = string.Empty;
            }
            else if (bExist(out coreMeta))//判断数据:dataID在表tableName中是否存在,若存在则对属性初始化
            {
                _deleteTime = coreMeta.DeleteTime;
                _flag= coreMeta.Flag;
                _importDate = coreMeta.ImportDate;
                _importUser = coreMeta.ImportUser;
                _name = coreMeta.Name;
            }
            else
            {
                _deleteTime = DateTime.MinValue;
                _flag = -1;
                _importDate = DateTime.Now;
                _importUser = LoginControl.userName;
                _name = string.Empty;
            }
        }
        
        #region IMetaData 成员
        /// <summary>
        /// 根据DataID删除,并级联删除TBARC_DATAIDMETA中对应记录
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            try
            {
                //1、删除元数据表中的数据
                string sql = string.Format("DELETE FROM {0} WHERE {1}={2}", TableName, FLD_NAME_F_DATAID, DataId);
                if(DBHelper.GlobalDBHelper.DoSQL(sql) > 0)
                {
                    //2、删除TBARC_DATAIDMETA表中数据
                    DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
                    dataIDMetaDAL.DataId = _dataId;
                    if(dataIDMetaDAL.Delete())
                    {
                        //3、删除TBARC_DATAPATH表中数据
                        if(DataPathDAL.Singleton.DeleteDataPathByHeadInfoID(_dataId.ToString()))
                        {
                            //4、删除TBARC_SNAPSHOT表中快视图文件
                            DataSnapshotDAL.Delete(_dataId);
                        }
                    }
                }
                return true;
                
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        public bool Insert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新核心元数据
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sqlStatement;
            string strFilter = FLD_NAME_F_DATAID + " = " + "'" + DataId + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();
            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, DataId, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DELETETIME, DeleteTime, EnumDBFieldType.FTDatetime));
            items.Add(new DBFieldItem(FLD_NAME_F_FLAG, Flag, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_IMPORTDATE, ImportDate, EnumDBFieldType.FTDate));
            items.Add(new DBFieldItem(FLD_NAME_F_IMPORTUSER, ImportUser, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_DATANAME, Name, EnumDBFieldType.FTString));
            sqlStatement = SQLStringUtility.GetUpdateSQL(TableName, items, strFilter,DBHelper.GlobalDBHelper);
            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }


        public MetaDataSysInfo Select()
        {
            IList<MetaDataSysInfo> pList;
            string strFilter = FLD_NAME_F_DATAID + " = " + DataId;
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            return pList.Count > 0 ? pList[0] : null;
        }

        #endregion

        /// <summary>
        /// 根据数据ID判断该条数据是否在对应数据库表中存在
        /// </summary>
        /// <returns></returns>
        private bool bExist(out IMetaDataSysEdit metaData)
        {
            metaData = this.Select();
            return metaData == null ? false : true;
        }
        
        #region translate
        /// <summary/>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        private IList<MetaDataSysInfo> Translate(DataTable dtResult)
        {
            IList<MetaDataSysInfo> list = new List<MetaDataSysInfo>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    MetaDataSysInfo info = new MetaDataSysInfo(_dbHelper);
                    info.DataId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);


                    info.DeleteTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_DELETETIME);
                    info.Flag = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_FLAG);
                    info.ImportDate = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_IMPORTDATE);
                    info.ImportUser = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_IMPORTUSER);

                    info.Name = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATANAME);
                    info.TableName = TableName;
                    list.Add(info);
                }
            }
            return list;
        }


        private DataTable DoQuery(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TableName + " where " + strFilter + " order by " + FLD_NAME_F_DATAID;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TableName, sqlStatement, true);
            return dtResult;
        }
        #endregion

        #region IMetaDataQuery 成员

        public string SelectWKT()
        {
            string shapeFieldName = SysSpatialParams.Para_GEOFIELDNAME;
            string sql =
                string.Format("SELECT t.{0}.GET_WKT() FROM {1} t WHERE {2}={3}", shapeFieldName, TableName,
                              FLD_NAME_F_DATAID,
                              DataId);
            DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(TableName, sql, true);
            string strWKT = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                strWKT = dt.Rows[0][0].ToString();
            }
            return strWKT;
        }

        public ESRI.ArcGIS.Geometry.IGeometry SelectGeometry()
        {
            try
            {
                string shapeFieldName = SysSpatialParams.Para_GEOFIELDNAME;
                string sql =
                    string.Format("SELECT t.{0}.GET_WKT() FROM {1} t WHERE {2}={3}", shapeFieldName, TableName, FLD_NAME_F_DATAID,
                                  DataId);
                DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(TableName, sql, true);
                string strWKT=string.Empty;
                if(dt!=null&&dt.Rows.Count>0)
                {
                    strWKT = dt.Rows[0][0].ToString();
                }
                
                ITranWKT2AEGeo tranWKT2AEGeo = new TranWKT2AEGeo();
                return  tranWKT2AEGeo.TransWKT2Geometry(strWKT);
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public DataTable SelectByDataID()
        {
            try
            {
                string fields = getAllField();
                string sql =
                    string.Format("SELECT {0} FROM {1}  WHERE {2}={3}", fields, TableName, FLD_NAME_F_DATAID,DataId);
                DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(TableName, sql, true);
                return dt;
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                return new DataTable();
            }
        }

        public IList<DataTable> SelectByFlag2List(IDBHelper dbHelper, EnumDataState enumDataState, params string[] columnNames)
        {
            
            IList<DataTable> lstDataTable = new List<DataTable>();
            try
            {
                IList<string> lstTableNames = DataidMetaDAL.SingleInstance.GetTableNames(dbHelper);
                DataTable dt = null;
                foreach (string tableName in lstTableNames)
                {
                    string fields = string.Join(",",columnNames);
                    string sql = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", fields, tableName, FLD_NAME_F_FLAG, (int)enumDataState);
                    dt = dbHelper.DoQueryEx(tableName, sql, true);
                    lstDataTable.Add(dt);
                }
                return lstDataTable;
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                return lstDataTable;
            }
        }


        /// <summary>
        /// 合并查询结果到一张表
        /// </summary>
        /// <param name="dbHelper"> </param>
        /// <param name="enumDataState"> </param>
        /// <param name="columnNames"> </param>
        /// <returns></returns>
        public DataTable SelectByFlag2Table(IDBHelper dbHelper, EnumDataState enumDataState,params string[] columnNames)
        {
            IList<DataTable> dts = SelectByFlag2List(dbHelper, enumDataState, columnNames);
            DataTable dtResult = null;
            //foreach (string columnName in columnNames)
            //{
            //    dtResult.Columns.Add(columnName);
            //}
            DataTable dtTemp = null;
            foreach (DataTable dt in dts)
            {
                dtTemp = dt.DefaultView.ToTable(false, columnNames);
                if (dtResult == null)
                {
                    dtResult = dtTemp.Clone();
                }
                dtResult.Merge(dtTemp.Copy());
            }
            return dtResult;
        }

        #endregion
    }
}
