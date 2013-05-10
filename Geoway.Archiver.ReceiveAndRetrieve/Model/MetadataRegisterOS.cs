using System;
using System.Collections.Generic;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.Catalog.Interface;
using Geoway.Archiver.Catalog;
using Geoway.ADF.MIS.Utility.DevExpressEx;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    internal class MetadataRegisterOS : IMetaDataEdit,IMetaDataOper
    {
        /// <summary>
        /// 数据不存在
        /// 插入操作时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="tableName"></param>
        public MetadataRegisterOS(IDBHelper dbHelper, string tableName)
        {
            _dbHelper = dbHelper;
            //_dataId = -1;//多余赋值
            _tableName = tableName;
        }
        /// <summary>
        /// 数据已存在
        /// 更新或删除操作时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataId"></param>
        public MetadataRegisterOS(IDBHelper dbHelper, int dataId)
        {
            _dbHelper = dbHelper;
            _dataId = dataId;
            //this.Select();
        }

        #region private variables
        private IDBHelper _dbHelper;

        private int _dataId = -1;

        private string _tableName = "";

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
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, _dataId);
                }
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

        #region Implementation of IMetaDataOper
        public int Insert()
        {
            DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
            try
            {
                int oid = _dbHelper.GetNextValidID(TableName, FixedFieldName.FLD_NAME_F_OID);
                AddItem(_fieldItems, FixedFieldName.FLD_NAME_F_OID, oid, EnumDBFieldType.FTNumber);
                //1、信息写入数据ID维护表(主表执行，若为从表则不执行)
                if (!_tableName.EndsWith(SysParams.ResourceMetaTableSuffix))//有待更改
                {
                    dataIDMetaDAL.MetaTable = TableName;
                    dataIDMetaDAL.Insert(_dbHelper);
                    _dataId = dataIDMetaDAL.DataId; 
                    //主表必有F_DATAID字段
                    AddItem(_fieldItems, FixedFieldName.FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber);
                }
                //2、插入数据
                int rows = this.ToDAL().Insert();
                if (rows <= 0)
                {
                    return rows;
                }
                //3、更新空间字段信息
                if (!string.IsNullOrEmpty(_wkt) && DBQuery.ExsitCol(_dbHelper, TableName))
                {
                    string spatialFieldName = SysSpatialParams.Para_GEOFIELDNAME;
                    int srid = SysSpatialParams.Para_SRID;
                    string where = string.Format("{0} = {1}",FixedFieldName.FLD_NAME_F_DATAID,_dataId);
                    OSpatial.ODatabase.TableModel.OTable.UpdateShapeFieldValue(TableName, 
                                                                               spatialFieldName, 
                                                                               srid,
                                                                               _wkt,
                                                                               where, 
                                                                               _dbHelper);
                }
                return rows;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                dataIDMetaDAL.Delete(_dbHelper);
                _dataId = -1;
                return 0;
            }
        }
        /// <summary>
        /// 根据数据ID删除,
        /// </summary>
        /// <returns></returns>
        public bool Delete(bool isDeleteEntity)
        {
            try
            {
                //0、删除实体数据
                if (isDeleteEntity)
                {
                    try
                    {
                        string errInfo = "";
                        if (!DeleteHelper.DeleteData(_dbHelper, null, _dataId, out errInfo))
                        {
                            throw new Exception(errInfo);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogHelper.Error.Append(ex);
                    }
                    
                }

                string mainTableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, _dataId);
                //1、删除元数据表(主表)中的数据
                MetadataRegisterDALOS dal = this.ToDAL();
                dal.TableName = mainTableName;
                dal.Delete();
                //2、删除从表数据
                int nodeID = ConvertEx.ToInt32(mainTableName.Replace(SysParams.ResourceMetaTablePrefix, ""));
                ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(_dbHelper, nodeID);
                if (!string.IsNullOrEmpty(catalogNode.SubMetaTableName))
                {
                    string delSQL = string.Format("DELETE FROM {0}_F WHERE {1}={2}", TableName,
                                                  FixedFieldName.FLD_NAME_F_REDATAID, _dataId);
                    _dbHelper.DoSQL(delSQL);
                }
                //3、删除TBARC_DATAIDMETA表中数据
                DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
                dataIDMetaDAL.DataId = _dataId;
                dataIDMetaDAL.Delete(_dbHelper);
                //4、删除TBARC_DATAPATH表中数据
                DataPathDAL.Singleton.DeleteDataPathByObjID(_dataId.ToString());
                //5、删除TBARC_SNAPSHOT表中快视图文件
                DataSnapshotDAL.Delete(_dataId);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }
        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        public bool Update(DBFieldItem item)
        {
            return this.ToDAL().Update(item);
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        public bool Update(IList<DBFieldItem> items)
        {
            return this.ToDAL().Update(items);
        }

        /// <summary>
        /// 根据给定字段名获取字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object GetValue(string fieldName)
        {
            return this.ToDAL().GetValue(fieldName);
        }

        #endregion
        
        #region private method
        
        
        private MetadataRegisterDALOS ToDAL()
        {
            MetadataRegisterDALOS dal = new MetadataRegisterDALOS(_dbHelper);
            dal.DataId = this._dataId;
            dal.TableName = this.TableName;
            dal.FieldItems = this._fieldItems;
            dal.WKT = this._wkt;
            return dal;
        }
        
        private void ToInfo(MetadataRegisterDALOS dal)
        {
            this._dataId = dal.DataId;
            this._tableName = dal.TableName;
            this._wkt = dal.WKT;
        }
        
        private void Select()
        {
            MetadataRegisterDALOS dal = this.ToDAL();
            dal.Select();
            this.ToInfo(dal);
        }
        
        private static void AddItem(List<DBFieldItem> items, string fieldName, object value, EnumDBFieldType enumDbFieldType)
        {
            _fieldName = fieldName;
            DBFieldItem item = items.Find(Exsit);
            if (item == null)
            {
                item = new DBFieldItem(fieldName, value, enumDbFieldType);
                items.Add(item);
            }
            else
            {
                item.Value = value;
                item.FieldType = enumDbFieldType;
            }
        }
        
        private static string _fieldName = "";
        private static bool Exsit(DBFieldItem obj)
        {
            if (obj.Name == _fieldName)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
