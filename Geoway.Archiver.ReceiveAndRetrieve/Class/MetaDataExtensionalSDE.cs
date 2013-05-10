using System;
using System.Collections.Generic;
using Geoway.ADF.MIS.Utility.Core;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Geoway.ADF.MIS.DataModel;
using Geoway.ADF.MIS.DataModel.Public;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Modeling.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 根据元数据文件写扩展属性
    /// </summary>
    class MetaDataExtensionalSDE :IMetaDataExtensionalEdit, IMetaData
    {
       
        #region IRegisterMetaTable 成员
        private string _metaFilePath;
        public string MetaFilePath
        {
            set { _metaFilePath = value; }
        }

        public int CatalogId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public EnumMetaDatumType EnumMetaDatumType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Dictionary<string, object> MetaDataSource
        {
            get { return _metaDataSource; }
            set { _metaDataSource = value; }
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private int _metaTableId;
        public int DatumTypeId
        {
            get { return _metaTableId; }
            set { _metaTableId = value; }
        }

        private int _dataId;
        public int DataId
        {
            get { return _dataId; }

            set { _dataId=value;}
        }

        public bool Insert()
        {
            try
            {
                //1、获取元数据文件中的信息
                MetaDataReader metaData = new MetaDataReader();
                metaData.MetaFileFullName = _metaFilePath;
                Dictionary<string, string> FieldAndValuePairs = metaData.FieldAndValuePairs;

                //2、获取元数据表中的字段信息
                List<MetaField> metaFields = MetaTemplateManager.GetMetaTemplateFields(_dbHelper, _metaTableId);

                //3、获取字段与值的集合
                IList<DBFieldItem> items = new List<DBFieldItem>();

                foreach (MetaField metaField in metaFields)
                {
                    if (FieldAndValuePairs.ContainsKey(metaField.Name))
                    {
                        items.Add(new DBFieldItem(metaField.Name, FieldAndValuePairs[metaField.Name],
                                                  MetaFieldOper.MetaTypeToDBType(metaField.Type)));
                    }

                }

                //4、获取数据空间范围
                IGeometry extent = DataExtentHelper.GetRasterExtentFromMetaFile(items);
                IFeatureWorkspace featureWorkspace = _bizEsriWS as IFeatureWorkspace;
                IFeatureClass featueClass = featureWorkspace.OpenFeatureClass(TableName);
                IFeatureCursor featureCursor = featueClass.Insert(true);
                IFeatureBuffer featureBuffer = featueClass.CreateFeatureBuffer();
                ISpatialReference pSR = (featueClass as IGeoDataset).SpatialReference;
                if (extent.SpatialReference == null || extent.SpatialReference.Name == "Unknown")
                {
                    extent.SpatialReference = pSR;
                }
                else if (extent.SpatialReference.Name != pSR.Name)
                {
                    extent.Project(pSR);
                }
                //double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                //pSR.GetDomain(out x1, out y1, out x2, out y2);
                featureBuffer.Shape = extent;

                //5、为要素赋值
                _dataId = _dbHelper.GetNextValidID(_tableName, featueClass.OIDFieldName);
                featureBuffer.set_Value(featureBuffer.Fields.FindField(featueClass.OIDFieldName), _dataId);

                foreach (DBFieldItem item in items)
                {
                    featureBuffer.set_Value(featureBuffer.Fields.FindField(item.Name), item.Value);
                }

                featureCursor.InsertFeature(featureBuffer);
                featureCursor.Flush();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IInitializePro 成员

        private IDBHelper _dbHelper;
        public IDBHelper DBHelper
        {
            set { _dbHelper=value; }
        }

        private IWorkspace _bizEsriWS;
        private Dictionary<string, object> _metaDataSource;

        public IWorkspace BizEsriWS
        {
            set { _bizEsriWS = value; }
        }

        #endregion
    }
}
