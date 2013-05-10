using System;
using System.Collections.Generic;
using System.Text;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    using System.Data;
    using System.Diagnostics;
    using ADF.MIS.DB.Public;
    using ADF.MIS.DataModel;
    using Archiver.Utility.Class;
    using ESRI.ArcGIS.Geodatabase;
    using ESRI.ArcGIS.Geometry;
    using Geoway.ADF.MIS.DataModel.Public;
    using Geoway.ADF.MIS.DB.Public.Enum;
    using Geoway.ADF.MIS.Utility.Core;
    using Geoway.Archiver.ReceiveAndRetrieve.Utility;

    /// <summary>
    /// 扩展元数据表注册
    /// </summary>
    class ExMetaDataRegisterInfo
    {
        private IWorkspace _ywSdeWorkspace;

        //private EnumSourceType _enumType;
        
        
        public ExMetaDataRegisterInfo(string metaTableName)
        {
            _metaTableName = metaTableName;
        }
        
        public ExMetaDataRegisterInfo(string metaFilePath,string metaTableName)
        {
            _metaTableName = metaTableName;
            _metaFilePath = metaFilePath;
        }

        private const string FLD_NAME_F_OID = "F_OID";
        private int _metaTableID;//元数据模版ID
        private string _metaTableName;//元数据表名
        private string _metaFilePath;//元数据文件路径
        /// <summary>
        /// 元数据表名
        /// </summary>
        public string MetaTableName
        {
            get { return _metaTableName; }
            set { _metaTableName = value; }
        }
        /// <summary>
        /// 元数据表对应的模版ID
        /// </summary>
        public int MetaTableId
        {
            get
            {
                _metaTableID = GetMetaTalbeID();
                return _metaTableID;
            }
            set
            {
                _metaTableID = value;
            }
        }

        public string MetaFilePath
        {
            get { return _metaFilePath; }
            set { _metaFilePath = value; }
        }


        /// <summary>
        /// 根据给定元数据表名获取元数据字段(不使用，改用MetaTemplateManager.GetMetaTemplateFields)
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFields()
        {
            try
            {
                string sql =
                    string.Format(
                        @"select b.f_name from tbdm_meta_templatefield a, tbdm_meta_fields b WHERE a.f_fieldid=b.f_id
AND 
a.f_templateid = ( SELECT t.f_metamodelid FROM tbcms_classification t WHERE t.f_metatablename='{0}')",
                        _metaTableName);
                DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(_metaTableName, sql, true);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return new List<string>();
                }
                else
                {
                    return  ConvertEx.ConvertToList(dt, "F_NAME");
                }
            }
            catch(Exception ex)
            {
                TraceHandler.AddErrorMsg(ex);
                return new List<string>();
            }
            
        }
        /// <summary>
        /// 根据表名判断数据库中是否存在该表
        /// </summary>
        /// <returns></returns>
        public bool IsExsit()
        {
            try
            {
                string sql = string.Format(@"select t.f_id from tbdm_tables t WHERE t.f_name='{0}'", _metaTableName);
                DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(_metaTableName, sql, true);
                return null == dt || dt.Rows.Count == 0 ? false : true;
            }
            catch(Exception ex)
            {
                TraceHandler.AddErrorMsg(ex);
                return false;
            }
        }
        /// <summary>
        /// 根据元数据表名获取元数据模板ID
        /// </summary>
        /// <returns></returns>
        private int GetMetaTalbeID()
        {
            try
            {
                int id;
                string sql =
                    string.Format(@"select t.f_metamodelid from tbcms_classification t WHERE t.f_metatablename='{0}'",
                                  _metaTableName);
                DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx("tbcms_classification", sql, true);
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch(Exception ex)
            {
                TraceHandler.AddErrorMsg(ex);
                return -1;
            }
        }


        public bool Insert(ref int rowID)
        {
            try
            {
                //1、获取元数据文件中的信息
                MetaDataReader metaData = new MetaDataReader();
                metaData.MetaFileFullName = _metaFilePath;
                Dictionary<string, string> FieldAndValuePairs = metaData.FieldAndValuePairs;

                //2、获取元数据表中的字段信息
                ExMetaDataRegisterInfo exMetaDataRegisterInfo = new ExMetaDataRegisterInfo(_metaTableName);
                List<MetaField> metaFields = null;
                if (exMetaDataRegisterInfo.IsExsit())
                {
                    metaFields = MetaTemplateManager.GetMetaTemplateFields(DBHelper.GlobalDBHelper,
                                                                           exMetaDataRegisterInfo.
                                                                               MetaTableId);
                }

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

                rowID = DBHelper.GlobalDBHelper.GetNextValidID(_metaTableName, FLD_NAME_F_OID);
                items.Add(new DBFieldItem(FLD_NAME_F_OID, rowID, EnumDBFieldType.FTNumber));
                //4、获取插入语句
                string insertSQLString = SQLStringUtility.GetInsertSQL(_metaTableName, items, DBHelper.GlobalDBHelper);

                int relRows= DBHelper.GlobalDBHelper.DoSQL(insertSQLString);
                
                if(relRows>0)
                {
                    //Geoway.OSpatial.ODatabase.TableModel.OTable.UpdateShapeFieldValue(_metaTableName, "F_SHAPE", 8307, FieldAndValuePairs[""], FLD_NAME_F_OID + " = " + rowID, DBHelper.GlobalDBHelper);
                }
                
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool InsertToSde(ref int rowID)
        {
            try
            {
                //1、获取元数据文件中的信息
                MetaDataReader metaData = new MetaDataReader();
                metaData.MetaFileFullName = _metaFilePath;
                Dictionary<string, string> FieldAndValuePairs = metaData.FieldAndValuePairs;

                //2、获取元数据表中的字段信息
                ExMetaDataRegisterInfo exMetaDataRegisterInfo = new ExMetaDataRegisterInfo(_metaTableName);
                List<MetaField> metaFields = null;
                if (exMetaDataRegisterInfo.IsExsit())
                {
                    metaFields = MetaTemplateManager.GetMetaTemplateFields(DBHelper.GlobalDBHelper,
                                                                           exMetaDataRegisterInfo.
                                                                               MetaTableId);
                }

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
                IFeatureWorkspace featureWorkspace = _ywSdeWorkspace as IFeatureWorkspace;
                IFeatureClass featueClass = featureWorkspace.OpenFeatureClass(MetaTableName);
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
                rowID = DBHelper.GlobalDBHelper.GetNextValidID(_metaTableName, featueClass.OIDFieldName);
                featureBuffer.set_Value(featureBuffer.Fields.FindField(featueClass.OIDFieldName),rowID);
                
                foreach (DBFieldItem item in items)
                {
                    featureBuffer.set_Value(featureBuffer.Fields.FindField(item.Name), item.Value);
                }

                featureCursor.InsertFeature(featureBuffer);
                featureCursor.Flush();

                
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
