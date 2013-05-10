using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.DataModel;
using Geoway.ADF.MIS.DataModel.Public;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.Modeling.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using Catalog.Interface;

    /// <summary>
    /// 根据元数据文件写扩展属性
    /// </summary>
    class MetaDataExtensionalOS : MetaDataBaseInfo, IMetaDataExtensionalEdit, IMetaData
    {
        public const string FLD_NAME_F_OID = "F_OID";

        #region 构造函数
        public MetaDataExtensionalOS(IDBHelper dbHelper):base(dbHelper){}
        public MetaDataExtensionalOS(IDBHelper dbHelper,int dataId):base(dbHelper,dataId){ }
        #endregion
        
        #region 私有变量
        private int _oid;
        private ICatalogNode _SelCatalogNode;
        private EnumMetaDatumType _enumMetaDatumType;
        private Dictionary<string, object> _metaDataSource;
        #endregion
        
        /// <summary>
        /// 备注：原资料类型为介质资料时设置
        /// </summary>
        public Dictionary<string, object> MetaDataSource
        {
            get { return _metaDataSource; }
            set { _metaDataSource = value; }
        } 
        /// <summary>
        /// 原资料类型
        /// </summary>
        public EnumMetaDatumType EnumMetaDatumType
        {
            get { return _enumMetaDatumType; }
            set { _enumMetaDatumType = value; }
        }
        
        #region IMetaDataExtensionalEdit 成员
        private int _catalogId;
        /// <summary>
        /// 入库节点ID
        /// </summary>
        public int CatalogId
        {
            get { return _catalogId; }
            set
            {
                _catalogId = value;
                try
                {
                    _SelCatalogNode = CatalogFactory.GetCatalogNode(_dbHelper, _catalogId);
                    _tableName = _SelCatalogNode.MetaTableName;
                    //_datumTypeId = _SelCatalogNode.DatumTypeObj.;
                }
                catch (Exception ex)
                {
                    LogHelper.Error.Append(ex);
                }
            }
        }

        #endregion
        #region IMetaData成员
        public bool Insert()
        {
            DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
            dataIDMetaDAL.MetaTable = _tableName;
            try
            {
                //1、获取元数据文件中的信息
               
                //2、获取元数据表中的字段信息(全部字段)
                List<DatumTypeField> metaFields = CatalogFactory.GetCatalogNode(_dbHelper, _catalogId).NodeExInfo.DatumTypeObj.Fields;

                //3、获取字段与值的集合
                IList<DBFieldItem> items = new List<DBFieldItem>();
                foreach (DatumTypeField datumTypeField in metaFields)
                {
                    if (_metaDataSource.ContainsKey(datumTypeField.MetaFieldObj.AliasName) &&
                        datumTypeField.MetaFieldObj.Name.ToUpper() != "F_DATAID")
                    {
                        items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name,
                                                  _metaDataSource[datumTypeField.MetaFieldObj.AliasName],
                                                  MetaFieldOper.MetaTypeToDBType(datumTypeField.MetaFieldObj.Type)));
                    }
                }
                //信息写入数据ID维护表
                dataIDMetaDAL.Insert();
                _dataId = dataIDMetaDAL.DataId;
                _oid = DBHelper.GlobalDBHelper.GetNextValidID(_tableName, FLD_NAME_F_OID);
                items.Add(new DBFieldItem(FLD_NAME_F_OID, _oid, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber));
                //4、获取插入语句
                string insertSQLString = SQLStringUtility.GetInsertSQL(_tableName, items, DBHelper.GlobalDBHelper);
                int relRows = DBHelper.GlobalDBHelper.DoSQL(insertSQLString);
                
                if(relRows<0)
                {
                    return false;
                }

                //5、更新空间字段信息

                if (_metaDataSource.ContainsKey(SysParams.Alias_F_WKT) && DBQuery.ExsitCol(_dbHelper, _tableName))
                {
                    string wkt = _metaDataSource[SysParams.Alias_F_WKT].ToString();

                    if (wkt.Length > 0)
                    {
                        string spatialFieldName = SysSpatialParams.Para_GEOFIELDNAME;
                        int srid = SysSpatialParams.Para_SRID;
                        return OSpatial.ODatabase.TableModel.OTable.UpdateShapeFieldValue(_tableName, spatialFieldName,srid, wkt,string.Format("{0}={1}","F_DATAID",_dataId),DBHelper.GlobalDBHelper);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                dataIDMetaDAL.Delete();
                _dataId = -1;
                return false;
            }
        }


        public bool Update()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }
        #endregion

        
    }
}
