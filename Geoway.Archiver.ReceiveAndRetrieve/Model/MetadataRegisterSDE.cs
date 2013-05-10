using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.Catalog.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.Utility.DAL;
using Geoway.OSpatial.SDOGeometry.Interface;
using Geoway.OSpatial.SDOGeometry.TranGeo;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    internal class MetadataRegisterSDE : IMetaDataEdit,IMetaDataOper
    {
        /// <summary>
        /// 数据不存在时调用
        /// 插入操作时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="workspace"> </param>
        /// <param name="tableName"></param>
        public MetadataRegisterSDE(IDBHelper dbHelper, IWorkspace workspace, string tableName)
        {
            _dbHelper = dbHelper;
            _workspace = workspace;
            _tableName = tableName;
            //_dataId = -1;
            //_featureclass可以通过_workspace/_tableName进一步获取，通过调用私有属性（FeatureClass）而得到
        }

        /// <summary>
        /// 数据已存在
        /// 更新或删除操作时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="workspace"> </param>
        /// <param name="dataId"></param>
        public MetadataRegisterSDE(IDBHelper dbHelper, IWorkspace workspace, int dataId)
        {
            _dbHelper = dbHelper;
            _workspace = workspace;
            _dataId = dataId;
            //_tableName/_catalogId可以通过_dbHelper/_dataid进一步获取，通过调用公有属性（TableName/CatalogId）而得到
            //_featureclass可以通过_workspace/_tableName进一步获取，通过调用私有属性（FeatureClass）而得到
        }

        #region private variables
        private List<DBFieldItem> _fieldItems;
        
        private string _wkt;
        
        private readonly IDBHelper _dbHelper;
        
        private readonly IWorkspace _workspace;

        private IFeatureClass _metaFeatureClass;
        
        private int _dataId = -1;
        
        private string _tableName = string.Empty;
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
        /// 在类内部调用时使用属性而非使用私有变量
        /// </summary>
        public string TableName
        {
            get
            {
                if(string.IsNullOrEmpty(_tableName))
                {
                    _tableName = Geoway.Archiver.ReceiveAndRetrieve.Class.DataOper.GetTableNameByDataID(_dbHelper, _dataId);
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



        /// <summary>
        /// 插入扩展元数据
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
            try
            {
                //1、信息写入数据ID维护表(主表执行，若为从表则不执行)
                if (!_tableName.EndsWith(SysParams.ResourceMetaTableSuffix))//有待更改
                {
                    dataIDMetaDAL.MetaTable = TableName;
                    if (!dataIDMetaDAL.Insert(_dbHelper) || dataIDMetaDAL.DataId < 0)
                    {
                        return 0;
                    }
                    _dataId = dataIDMetaDAL.DataId;
                }

                //2、赋值
                IFeatureBuffer featureBuffer = MetaFeatureClass.CreateFeatureBuffer();
                int count = featureBuffer.Fields.FieldCount;
                for (int i = 0; i < count; i++)
                {
                     _fieldName = featureBuffer.Fields.get_Field(i).Name;

                    DBFieldItem item = _fieldItems.Find(Exsit);

                    if (item == null && _fieldName != MetaFeatureClass.ShapeFieldName && _fieldName != SysParams.FLD_NAME_F_DATAID)
                    {
                        continue;
                    }

                    if (_fieldName == MetaFeatureClass.ShapeFieldName && !string.IsNullOrEmpty(_wkt)) //空间字段
                    {
                        ITranWKT2AEGeo tranWKT2AEGeo = new TranWKT2AEGeo();
                        IGeometry geometry = tranWKT2AEGeo.TransWKT2Geometry(_wkt);
                        featureBuffer.Shape = geometry;
                    }
                    else if (_fieldName == SysParams.FLD_NAME_F_DATAID) //数据ID，每个图层必定包含此字段
                    {
                        featureBuffer.set_Value(i, _dataId);
                    }
                    else if (_fieldName != MetaFeatureClass.OIDFieldName && _fieldName != MetaFeatureClass.ShapeFieldName) //其它属性
                    {
                        try
                        {
                            featureBuffer.set_Value(i, item.Value);
                        }
                        catch (Exception ex)
                        {
                            if (item.FieldType == Geoway.ADF.MIS.DB.Public.Enum.EnumDBFieldType.FTDatetime)
                            {
                                DateTime dtime;
                                DateTime.TryParse(item.Value.ToString(), out dtime);
                                if (dtime != DateTime.MinValue)
                                {
                                  //原    featureBuffer.set_Value(i, item.Value);
                                    featureBuffer.set_Value(i, dtime);
                                }
                            }
                        }
                    }
                }
                IFeatureCursor insertCursor = MetaFeatureClass.Insert(true);
                insertCursor.InsertFeature(featureBuffer);
                Marshal.ReleaseComObject(insertCursor);
                return 1;
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
        /// 根据数据ID删除
        /// </summary>
        /// <returns></returns>
        public bool Delete(bool isDeleteEntity)
        {
            //0、删除实体数据
            if (isDeleteEntity)
            {
                string errInfo = "";
                try
                {
                    if (!DeleteHelper.DeleteData(_dbHelper, SysParams.BizWorkspace, _dataId, out errInfo))
                    {
                        throw new Exception(errInfo);
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error.Append(ex);
                    throw new Exception(errInfo);
                }
                
            }
            
            string mainTableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, _dataId);
            //1、删除主表
            string strFilter = SysParams.FLD_NAME_F_DATAID + " = " + this._dataId;
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = strFilter;
            IFeatureClass featureClass = (_workspace as IFeatureWorkspace).OpenFeatureClass(mainTableName);
            ITable table = featureClass as ITable;
            table.DeleteSearchedRows(queryFilter);
            //2、删除从表
            int nodeID = ConvertEx.ToInt32(mainTableName.Replace(SysParams.ResourceMetaTablePrefix, ""));
            ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(_dbHelper, nodeID);
            String subMetaTableName = catalogNode.SubMetaTableName;
            if (!string.IsNullOrEmpty(subMetaTableName))//判断是否存在从表
            {
                strFilter = FixedFieldName.FLD_NAME_F_REDATAID + "=" + this._dataId;
                queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = strFilter;
                featureClass =(_workspace as IFeatureWorkspace).OpenFeatureClass(subMetaTableName);
                table = featureClass as ITable;
                table.DeleteSearchedRows(queryFilter);
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
        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        public bool Update(DBFieldItem item)
        {
            string strFilter = SysParams.FLD_NAME_F_DATAID + " = " + "'" + _dataId + "'";
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = strFilter;
            IFeatureCursor updateCursor = MetaFeatureClass.Update(queryFilter, false);
            IFeature feature = updateCursor.NextFeature();
            if (feature != null) //正常情况下只可能存在一个Feature
            {
                feature.set_Value(feature.Fields.FindField(item.Name), item.Value);
                updateCursor.UpdateFeature(feature);
            }
            Marshal.ReleaseComObject(updateCursor);
            return true;
        }

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        public bool Update(IList<DBFieldItem> items)
        {
            string strFilter = SysParams.FLD_NAME_F_DATAID + " = " + "'" + _dataId + "'";
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = strFilter;
            IFeatureCursor updateCursor = MetaFeatureClass.Update(queryFilter, false);
            IFeature feature = updateCursor.NextFeature();
            if (feature != null) //正常情况下只可能存在一个Feature
            {
                foreach (DBFieldItem item in items)
                {
                    try
                    {
                        feature.set_Value(feature.Fields.FindField(item.Name), item.Value);
                        updateCursor.UpdateFeature(feature);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error.Append(ex);
                    }
                }

            }
            Marshal.ReleaseComObject(updateCursor);
            return true;
        }

        /// <summary>
        /// 根据给定字段名获取字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public object GetValue(string fieldName)
        {
            if (MetaFeatureClass == null)
            {
                return null;
            }
            string strFilter = SysParams.FLD_NAME_F_DATAID + " = " + _dataId;
            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = strFilter;
            IFeatureCursor searchCursor = MetaFeatureClass.Search(queryFilter, false);
            IFeature feature = searchCursor.NextFeature();
            if (feature != null) //正常情况下只可能存在一个Feature
            {
                return feature.get_Value(feature.Fields.FindField(fieldName));
            }
            Marshal.ReleaseComObject(searchCursor);
            return null;
        }

        #endregion

        #region private method
        private IFeatureClass MetaFeatureClass
        {
            get
            {
                if (null == _metaFeatureClass)
                {
                    IFeatureWorkspace featureWorkspace = _workspace as IFeatureWorkspace;
                    try
                    {
                        _metaFeatureClass = featureWorkspace.OpenFeatureClass(TableName);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error.Append(ex);
                    }
                }
                return _metaFeatureClass;
            }
        }

        private string _fieldName;
        private bool Exsit(DBFieldItem obj)
        {
            if (obj.Name.ToUpper() == _fieldName.ToUpper())
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
