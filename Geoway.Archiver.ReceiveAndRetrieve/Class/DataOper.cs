using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.Utility.Class;
using ESRI.ArcGIS.Carto;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using ESRI.ArcGIS.Geometry;
using System;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.ADF.MIS.Utility.DevExpressEx;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using System.Data;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.Modeling.Model;
using Geoway.Archiver.Catalog.Utiltiy;
using Geoway.Archiver.Catalog.Interface;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.Query.Interface;
using Geoway.Archiver.Query.Factory;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.Catalog.Definition;
using Geoway.Archiver.Query.Model;
using Geoway.Archiver.Utility.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 查询结果的相关操作
    /// </summary>
    public class DataOper
    {
        /// <summary>
        /// 查询节点下的所有数据
        /// </summary>
        /// <param name="catalogNode"></param>
        /// <returns></returns>
        public static List<RegisterKey> GetDataByNode(IDBHelper dbHelper, ICatalogNode catalogNode)
        {
            List<RegisterKey> lstDataID = new List<RegisterKey>();

            if (catalogNode.NodeType == EnumCatalogNodeType.DataNode)
            {
                DataTable dt = MetadataTableHelper.QueryCatalogMetadataTable(dbHelper, catalogNode);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int dataID = row[FixedFieldName.FLD_NAME_F_DATAID] == DBNull.Value ? -1 : Convert.ToInt32(row[FixedFieldName.FLD_NAME_F_DATAID]);
                        if (dataID > 0)
                        {
                            string tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(dbHelper, dataID);
                            lstDataID.Add(new RegisterKey(dataID, tableName));
                        }
                    }
                }
            }
            else if (catalogNode.NodeType == EnumCatalogNodeType.LogicNode)
            {
                foreach (ICatalogNode item in catalogNode.GetAllChildDataNode())
                {
                    DataTable dt = MetadataTableHelper.QueryCatalogMetadataTable(dbHelper, item);
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            int dataID = row[FixedFieldName.FLD_NAME_F_DATAID] == DBNull.Value ? -1 : Convert.ToInt32(row[FixedFieldName.FLD_NAME_F_DATAID]);
                            if (dataID > 0)
                            {
                                string tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(dbHelper, dataID);
                                lstDataID.Add(new RegisterKey(dataID, tableName));
                            }
                        }
                    }
                }
            }
            return lstDataID;
        }
        public static IDictionary<string, IList<int>> PreProcess(IList<RegisterKey> dataIDs)
        {
            IDictionary<string, IList<int>> endIDs = new Dictionary<string, IList<int>>();

            bool isExist = false;
            foreach (RegisterKey rk in dataIDs)
            {
                isExist = false;
                foreach (KeyValuePair<string, IList<int>> vp in endIDs)
                {
                    if (rk.MetaTableName == vp.Key)
                    {
                        vp.Value.Add(rk.DataID);
                        isExist = true;
                    }
                }
                if (!isExist)
                {
                    IList<int> ids = new List<int>();
                    ids.Add(rk.DataID);
                    endIDs.Add(rk.MetaTableName, ids);
                }
            }
            return endIDs;

        }
        /// <summary>
        /// 删除数据到回收站
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="lstKeys"></param>
        public static void Delete(IDBHelper dbHelper, IList<RegisterKey> lstKeys)
        {
            try
            {
                foreach (RegisterKey key in lstKeys)
                {
                    IMetaDataOper metaDataOper = MetadataFactory.Create(DBHelper.GlobalDBHelper, SysParams.BizWorkspace, key.DataID);
                    metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_FLAG, 1, EnumDBFieldType.FTNumber));
                    metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_DELETETIME, DateTime.Now,
                                                        EnumDBFieldType.FTServerNowDatetime));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// 删除已入库数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <param name="isDeleteEntity"> </param>
        /// <returns></returns>
        public static bool DeleteMetaData(IDBHelper dbHelper,int dataID,bool isDeleteEntity)
        {
            if (dataID == -1) return true; 
            try
            {
                IMetaDataOper metaDataOper = MetadataFactory.Create(dbHelper, SysParams.BizWorkspace, dataID);

                return metaDataOper.Delete(isDeleteEntity);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static string GetTableNameByDataID(IDBHelper dbHelper,int dataID)
        {
            string tableName = string.Empty;
            DataIDMetaDAL dal = DataIDMetaDAL.SelectByDataID(dbHelper, dataID);
            if(dal!=null)
            {
                tableName = dal.MetaTable;
            }
            return tableName;

        }
        
        public static DataTable  GetTableByDataID(IDBHelper dbHelper,int dataID)
        {
            string tableName = GetTableNameByDataID(dbHelper, dataID);
            string fields = GetFields(dbHelper, dataID, EnumFldType.enumAll, SysParams.FLD_NAME_F_DATAID,
                                      SysParams.FLD_NAME_F_FLG);
            string sql = string.Format("select {0} from {1} where {2} = {3}", fields, tableName,
                                       SysParams.FLD_NAME_F_DATAID, dataID);
            DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);
            DatumType datumType = GetDatumType(dbHelper, dataID);
            MetadataTableHelper.ConvertDataTableColumnCaption(dbHelper, dataTable, datumType);
            return dataTable;
        }
        public static DataTable GetTableByDataID(IDBHelper dbHelper, string tableName,int dataID)
        {
            string fields = GetFields(dbHelper, dataID, EnumFldType.enumAll, SysParams.FLD_NAME_F_DATAID,
                                      SysParams.FLD_NAME_F_FLG);
            string sql = string.Format("select {0} from {1} where {2} = {3}", fields, tableName,
                                       SysParams.FLD_NAME_F_DATAID, dataID);
            DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);
           
            return dataTable;
        }
        public static DataTable GetTableByDataID(IDBHelper dbHelper, string tableName,string fields, int dataID)
        {
            string sql = string.Format("select {0} from {1} where {2} = {3}", fields, tableName,
                                       SysParams.FLD_NAME_F_DATAID, dataID);
            DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);

            return dataTable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <param name="enumFldType"></param>
        /// <param name="fields">必需包含的字段 </param>
        /// <returns></returns>
        public static string GetFields(IDBHelper dbHelper,int dataID,EnumFldType enumFldType,params string[] fields)
        {
            DatumType datumType = GetDatumType(dbHelper, dataID);

            List<string> lstFields = new List<string>();
            IList<DatumTypeField> lstDatumTypeFields = datumType.GetDatumFields(enumFldType);

            foreach (DatumTypeField datumTypeField in lstDatumTypeFields)
            {
                if(datumTypeField.ModelType==EnumModelType.MasterSlaveTable)
                {
                    continue;
                }
                
                if (!lstFields.Contains(datumTypeField.MetaFieldObj.Name))
                {
                    lstFields.Add(datumTypeField.MetaFieldObj.Name);
                }
            }

            foreach (string field in fields)
            {
                if(!lstFields.Contains(field))
                {
                    lstFields.Add(field);
                }
            }

            return string.Join(",", lstFields.ToArray());

        }
        /// <summary>
        /// 获取标识字段
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="datumType"></param>
        /// <returns></returns>
        public static string GetBiaoshiFields(IDBHelper dbHelper, DatumType datumType)
        {
            List<string> lstFields = new List<string>();
            IList<DatumTypeField> lstDatumTypeFields = datumType.GetDatumFields(EnumDatumFldAtt.enumIsIdentify, EnumModelType.SingleTable);

            foreach (DatumTypeField datumTypeField in lstDatumTypeFields)
            {
                if (!lstFields.Contains(datumTypeField.MetaFieldObj.Name))
                {
                    lstFields.Add(datumTypeField.MetaFieldObj.Name);
                }
            }
            return string.Join(",", lstFields.ToArray());

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static DatumType GetDatumType(IDBHelper dbHelper, int dataID)
        {
            string tableName = GetTableNameByDataID(dbHelper, dataID);
            string catalogID = tableName.Replace(SysParams.ResourceMetaTablePrefix, "");
            ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(dbHelper, int.Parse(catalogID));
            DatumType datumType = catalogNode.NodeExInfo.DatumTypeObj;
            return datumType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static EnumMetaDatumType GetMetaDatumTypeFlag(IDBHelper dbHelper, int dataID)
        {
            string tableName = GetTableNameByDataID(dbHelper, dataID);
            string catalogID = tableName.Replace(SysParams.ResourceMetaTablePrefix, "");
            ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(dbHelper, int.Parse(catalogID));
            EnumMetaDatumType MetaDatumTypeFlag = catalogNode.NodeExInfo.MetaDatumTypeFlag;
            return MetaDatumTypeFlag;
        }

        public static ICatalogNode GetCatalogNodeByDataID(IDBHelper dbHelper, int dataID)
        {
            string tableName = GetTableNameByDataID(dbHelper, dataID);
            ICatalogNode catalogNode = GetCatalogNodeByTableName(dbHelper, tableName);
            return catalogNode;
        }
        
        public static ICatalogNode GetCatalogNodeByTableName(IDBHelper dbHelper, string tableName)
        {
            string catalogID = tableName.Replace(SysParams.ResourceMetaTablePrefix, "");
            ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(dbHelper, int.Parse(catalogID));
            return catalogNode;
        }
        /// <summary>
        /// 判断待入库数据在数据库中是否已经存在
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="catalogNode"></param>
        /// <param name="dicMetaData"></param>
        /// <param name="enumModelType"> </param>
        /// <returns></returns>
        public static bool ExsitData(IDBHelper dbHelper, ICatalogNode catalogNode,Dictionary<string,object> dicMetaData,EnumModelType enumModelType)
        {
            try
            {
                DatumType datumType = catalogNode.NodeExInfo.DatumTypeObj;
                List<DatumTypeField> lstFields = datumType.GetDatumFields(EnumDatumFldAtt.enumIsIdentify, enumModelType);
                if (lstFields == null || lstFields.Count == 0)
                {
                    return false;
                }

                StringBuilder whereClause = new StringBuilder();
                foreach (DatumTypeField datumTypeField in lstFields)
                {
                    string value = dicMetaData.ContainsKey(datumTypeField.MetaFieldObj.AliasName)
                                       ? dicMetaData[datumTypeField.MetaFieldObj.AliasName].ToString()
                                       : "";
                    if (value.Length == 0)
                    {
                        whereClause.Append(string.Format(" and {0} IS NULL ", datumTypeField.MetaFieldObj.Name));
                    }
                    else
                    {
                        whereClause.Append(string.Format(" and {0} = '{1}' ", datumTypeField.MetaFieldObj.Name, value));
                    }

                }

                string where = StringHelper.TrimStart(whereClause.ToString(), " and");

                string sql = string.Format("select {0} from {1} where {2}", SysParams.FLD_NAME_F_DATAID,
                                           catalogNode.MetaTableName, where);
                DataTable dataTable = dbHelper.DoQueryEx(catalogNode.MetaTableName, sql, true);
                return dataTable.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }

        }
        /// <summary>
        /// 单条录入时判断添加数据是否已经存在于已添加的数据（dataTable）中
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="catalogNode"></param>
        /// <param name="dataTable"></param>
        /// <param name="dicMetaData"></param>
        /// <returns></returns>
        public static bool ExsitData(IDBHelper dbHelper, ICatalogNode catalogNode,DataTable dataTable, Dictionary<string, object> dicMetaData,EnumModelType enumModelType)
        {
            
            if(dataTable==null||dataTable.Rows.Count==0)
            {
                return false;
            }
            
            DatumType datumType = catalogNode.NodeExInfo.DatumTypeObj;
            List<DatumTypeField> lstFields = datumType.GetDatumFields(EnumDatumFldAtt.enumIsIdentify, enumModelType);
            if (lstFields == null || lstFields.Count==0)
            {
                return false;
            }

            StringBuilder strDic = new StringBuilder();
            foreach (DatumTypeField datumTypeField in lstFields)
            {
                string value = dicMetaData.ContainsKey(datumTypeField.MetaFieldObj.AliasName)
                                   ? dicMetaData[datumTypeField.MetaFieldObj.AliasName].ToString()
                                   : "";
                strDic.Append("_" + value);
            }

            StringBuilder strDT = new StringBuilder();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (DatumTypeField datumTypeField in lstFields)
                {
                    string value = dataTable.Columns.Contains(datumTypeField.MetaFieldObj.AliasName)
                                   ? dataRow[datumTypeField.MetaFieldObj.AliasName].ToString()
                                   : "";
                    strDT.Append("_" + value);
                }
                if(strDic.ToString()==strDT.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static bool CanDownLoad(IDBHelper dbHelper,int nodeId)
        {
            ICatalogNode catalogNode = GetCatalogNodeByDataID(dbHelper, nodeId);
            if(catalogNode.NodeExInfo.IsHasAFile||!string.IsNullOrEmpty(catalogNode.NodeExInfo.StorageServers))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置资料状态
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="regKey"></param>
        /// <param name="flag">删除为1，废弃为2</param>
        /// <returns></returns>
        public static bool UpdateDatumFlag(IDBHelper dbHelper, RegisterKey regKey, int flag)
        {
            string strSQL = string.Format("update {0} set {1}={2} where {3}={4}", regKey.MetaTableName, FixedFieldName.FLD_NAME_F_FLAG, flag, FixedFieldName.FLD_NAME_F_DATAID, regKey.DataID);
            return dbHelper.DoSQL(strSQL) > 0;
        }
        /// <summary>
        /// 设置磁盘结构
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="regKey"></param>
        /// <param name="flag">已设置为1，未设置为0</param>
        /// <returns></returns>
        public static bool UpdateDatumStruct(IDBHelper dbHelper, RegisterKey regKey, int num)
        {
            string strSQL = string.Format("update {0} set {1}={2} where {3}={4}", regKey.MetaTableName, FixedFieldName.FLD_NAME_F_HDSTRUCT, num, FixedFieldName.FLD_NAME_F_DATAID, regKey.DataID);
            return dbHelper.DoSQL(strSQL) > 0;
        }
        /// <summary>
        /// 电子介质资料关联
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="regKey">电子介质资料</param>
        /// <param name="glNodeId">关联介质资料id</param>
        /// <returns></returns>
        public static bool UpdateGLDataID(IDBHelper dbHelper, RegisterKey regKey, int glNodeId)
        {
            string strSQL = string.Format("update {0} set {1}={2} where {3}={4}", regKey.MetaTableName,
                FixedFieldName.FLD_NAME_F_REDATAID, glNodeId, FixedFieldName.FLD_NAME_F_DATAID, regKey.DataID);
            return dbHelper.DoSQL(strSQL) > 0;
        }
        /// <summary>
        /// 获取电子介质资料关联的介质资料
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="dataID">电子介质资料ID</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetGLJZByDataID(IDBHelper dbHelper, string tableName, int dataID)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            string sql = string.Format("select {0} from {1} where {2} = {3}", FixedFieldName.FLD_NAME_F_REDATAID, tableName,
                                       SysParams.FLD_NAME_F_DATAID, dataID);
            DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);
            if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows[0][0]!=DBNull.Value)
            {
                int reDataID;
                int.TryParse(dataTable.Rows[0][0].ToString(), out reDataID);
                string tbName = GetTableNameByDataID(dbHelper, reDataID);
                string fields = GetFields(dbHelper, reDataID, EnumFldType.enumAll, SysParams.FLD_NAME_F_DATAID,
                                          SysParams.FLD_NAME_F_FLG);
                sql = string.Format("select {0} from {1} where {2} = {3}", fields, tbName,
                                           SysParams.FLD_NAME_F_DATAID, reDataID);
                DataTable newTable = dbHelper.DoQueryEx("MetaTable", sql, true);
                DatumType datumType = GetDatumType(dbHelper, reDataID);
                MetadataTableHelper.ConvertDataTableColumnCaption(dbHelper, newTable, datumType);
                dicResult.Add(datumType.Name, newTable);
            }

            return dicResult;
        }

        /// <summary>
        /// 获取电子介质资料关联的介质资料
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="dataID">电子介质资料ID</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetGLJZByDataID(IDBHelper dbHelper, IList<RegisterKey> keyList, ref Dictionary<string, int> dicTag)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            foreach (RegisterKey key in keyList)
            {
                string sql = string.Format("select {0} from {1} where {2} = {3}", FixedFieldName.FLD_NAME_F_REDATAID, key.MetaTableName,
                                           SysParams.FLD_NAME_F_DATAID, key.DataID);
                DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);
                if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
                {
                    int reDataID;
                    int.TryParse(dataTable.Rows[0][0].ToString(), out reDataID);
                    string tbName = GetTableNameByDataID(dbHelper, reDataID);
                    string fields = GetFields(dbHelper, reDataID, EnumFldType.enumAll, SysParams.FLD_NAME_F_DATAID,
                                              SysParams.FLD_NAME_F_FLG);
                    sql = string.Format("select {0} from {1} where {2} = {3}", fields, tbName,
                                               SysParams.FLD_NAME_F_DATAID, reDataID);
                    DataTable newTable = dbHelper.DoQueryEx("MetaTable", sql, true);
                    DatumType datumType = GetDatumType(dbHelper, reDataID);
                    MetadataTableHelper.ConvertDataTableColumnCaption(dbHelper, newTable, datumType);
                    if (!dicResult.ContainsKey(datumType.Name))
                    {
                        dicResult.Add(datumType.Name, newTable);
                        dicTag.Add(datumType.Name, datumType.MetaDatumTypeObj.ID);
                    }
                    else
                    {
                        DataTable tempDt = (DataTable)dicResult[datumType.Name];
                        tempDt.Merge(newTable);
                    }
                }
            }

            return dicResult;
        }
        /// <summary>
        /// 获取电子介质资料关联的介质资料
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        /// <param name="dataID">电子介质资料ID</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetGLJZByDataID(IDBHelper dbHelper, IList<RegisterKey> keyList)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            foreach (RegisterKey key in keyList)
            {
                string sql = string.Format("select {0} from {1} where {2} = {3}", FixedFieldName.FLD_NAME_F_REDATAID, key.MetaTableName,
                                           SysParams.FLD_NAME_F_DATAID, key.DataID);
                DataTable dataTable = dbHelper.DoQueryEx("MetaTable", sql, true);
                if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
                {
                    int reDataID;
                    int.TryParse(dataTable.Rows[0][0].ToString(), out reDataID);
                    string tbName = GetTableNameByDataID(dbHelper, reDataID);
                    string fields = GetFields(dbHelper, reDataID, EnumFldType.enumAll, SysParams.FLD_NAME_F_DATAID,
                                              SysParams.FLD_NAME_F_FLG);
                    sql = string.Format("select {0} from {1} where {2} = {3}", fields, tbName,
                                               SysParams.FLD_NAME_F_DATAID, reDataID);
                    DataTable newTable = dbHelper.DoQueryEx("MetaTable", sql, true);
                    DatumType datumType = GetDatumType(dbHelper, reDataID);
                    MetadataTableHelper.ConvertDataTableColumnCaption(dbHelper, newTable, datumType);
                    if (!dicResult.ContainsKey(datumType.Name))
                    {
                        dicResult.Add(datumType.Name, newTable);
                    }
                    else
                    {
                        DataTable tempDt = (DataTable)dicResult[datumType.Name];
                        tempDt.Merge(newTable);
                    }
                }
            }

            return dicResult;
        }
    }
}

