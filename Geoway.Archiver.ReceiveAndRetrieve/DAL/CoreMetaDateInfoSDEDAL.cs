using System;
using System.Collections.Generic;
using System.Text;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    using System.Data;
    using Class;
    using ESRI.ArcGIS.Geodatabase;
    using Geoway.ADF.MIS.DB.Public;
    using Interface;
    using Interface.Util;
    using Utility;

    public class CoreMetaDateInfoSDEDAL : CoreDataInfo, ICoreMetaDateInfoDAL
    {

        //public static CoreMetaDateInfoSpatialDAL Singleton = new CoreMetaDateInfoSpatialDAL();


        public CoreMetaDateInfoSDEDAL(string tableName, long dataID) : base(tableName, dataID)
        {
        }

        public bool Add()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Delete()
        {
            try
            {
                string sql = string.Format("DELETE FROM {0} WHERE {1}={2}", TableName, FLD_NAME_F_OID, OID);
                return DBHelper.GlobalDBHelper.DoSQL(sql) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 根据DataID删除,并级联删除TBARC_DATAIDMETA中对应记录
        /// </summary>
        /// <returns></returns>
        public bool DeleteByDataID()
        {
            try
            {
                //IFeatureWorkspace featureWorkspace = InitPara.BizEsriWS as IFeatureWorkspace;
                //ITable  table= featureWorkspace.OpenTable(_tableName);
                //IQueryFilter queryFilter = new QueryFilterClass();
                //queryFilter.WhereClause = string.Format("F_DATAID = ",_dataID);
                //ICursor cursor= table.Search(queryFilter, false);
                //IRow row = cursor.NextRow();
                //while(null!=row)
                //{
                //    row.Delete();
                //    row = cursor.NextRow();
                //}
                string sql=string.Format("delete from {0} where {1}={2}",TableName,FLD_NAME_F_DATAID,DataId);
                InitPara.BizEsriWS.ExecuteSQL(sql);
                DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
                dataIDMetaDAL.DataId = (int)DataId;
                return dataIDMetaDAL.Delete();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetNextID()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IList<CoreDataInfo> Select()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public CoreDataInfo Select(int id)
        {
            IList<CoreDataInfo> pList = new List<CoreDataInfo>();

            string strFilter = FLD_NAME_F_OID + " = " + id;
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            return pList.Count > 0 ? pList[0] : null;
        }

        public CoreDataInfo SelectByDataID()
        {
            IList<CoreDataInfo> pList = new List<CoreDataInfo>();
            CoreDataInfo core = null;
            IFeatureWorkspace featureWorkspace = InitPara.BizEsriWS as IFeatureWorkspace;
            ITable table = featureWorkspace.OpenTable(TableName);
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = string.Format("F_DATAID = {0}", DataId);
            ICursor cursor = table.Search(queryFilter, false);
            IRow row = cursor.NextRow();
            while (null != row)
            {
                core = new CoreDataInfo();
                long id;
                long.TryParse(row.get_Value(row.Fields.FindField(FLD_NAME_F_DATAID)).ToString(),out id);
                core.DataId = id;
                //core.DataName = row.get_Value(row.Fields.FindField(FLD_NAME_F_DATANAME)).ToString();
                pList.Add(core);
                row = cursor.NextRow();
            }
            return pList.Count > 0 ? pList[0] : null;
        }

   




        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<CoreDataInfo> Translate(DataTable dtResult)
        {
            IList<CoreDataInfo> list = new List<CoreDataInfo>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    CoreDataInfo info = new CoreDataInfo();
                    info.DataSize = GetSafeDataUtility.ValidateDataRow_F(pRow, FLD_NAME_F_DATASIZE);
                    info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                    info.DeleteTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_DELETETIME);
                    info.Flag = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_FLAG);
                    info.ImportDate = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_IMPORTDATE);
                    info.ImportUser = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_IMPORTUSER);
                    //info.ObjectTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_OBJECTTYPENAME);
                    info.OID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_OID);
                    info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                    info.ShapeFieldName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SHAPE);
                    info.DataId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);
                    info.TableName = this.TableName;
                    list.Add(info);
                }
            }
            return list;
        }


        public DataTable DoQuery(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TableName + " where " + strFilter + " order by " + FLD_NAME_F_OID;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TableName, sqlStatement, true);
            return dtResult;
        }
        #endregion

        private string getSelectField()
        {
            string fields = FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_DATAUNIT + "," +
                            FLD_NAME_F_DELETETIME + "," +
                FLD_NAME_F_FLAG + "," +
                FLD_NAME_F_IMPORTDATE + "," +
                FLD_NAME_F_IMPORTUSER + "," +

                //FLD_NAME_F_OBJECTTYPENAME + "," +
                FLD_NAME_F_OID + "," +
                //FLD_NAME_F_SHAPE + "," +
                FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_SERVERID;
            return fields;
        }
    }
}
