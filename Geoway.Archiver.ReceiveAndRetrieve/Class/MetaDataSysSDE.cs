using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.Utility.Class;
using System.Data;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 写元数据表系统维护字段
    /// </summary>
    public class MetaDataSysSDE : MetaDataSysInfo, IMetaDataSys, IMetaDataQuery
    {
        private static readonly IFeatureWorkspace _featureWorkspace = InitPara.BizEsriWS as IFeatureWorkspace;
        private IFeatureClass _featureClass = null;
        private IQueryFilter _queryFilter = null;



        public MetaDataSysSDE(IDBHelper dbHelper, int dataID)
            : base(dbHelper, dataID)
        {
            try
            {
                _featureClass = _featureWorkspace.OpenFeatureClass(_tableName);
                _queryFilter = new QueryFilterClass();
                _queryFilter.WhereClause = string.Format("F_DATAID = {0}", DataId);
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// 根据DataID删除,并级联删除TBARC_DATAIDMETA中对应记录
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            try
            {
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

        public bool Insert()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            IFeatureCursor updateCursor = null;
            try
            {
                updateCursor = _featureClass.Update(_queryFilter, false);
                IFeature feature = updateCursor.NextFeature();
                while (null != feature)
                {
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_DATAID), DataId);
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_DELETETIME), DeleteTime);
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_FLAG), Flag);
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_IMPORTDATE), ImportDate);
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_IMPORTUSER), ImportUser);
                    feature.set_Value(feature.Fields.FindField(FLD_NAME_F_DATANAME), Name);

                    updateCursor.UpdateFeature(feature);
                    feature = updateCursor.NextFeature();
                }
                
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            finally
            {
                if (updateCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(updateCursor);
                }
            }

        }

        public MetaDataSysInfo Select()
        {
            IFeatureCursor selectCursor = null;
            try
            {
                IList<MetaDataSysInfo> pList = new List<MetaDataSysInfo>();
                MetaDataSysInfo core = null;
                _queryFilter = new QueryFilterClass();
                _queryFilter.WhereClause = string.Format("F_DATAID = {0}", DataId);
                selectCursor = _featureClass.Search(_queryFilter, false);
                IFeature feature = selectCursor.NextFeature();
                while (null != feature)
                {
                    core = new MetaDataSysInfo(_dbHelper);
                    core.DataId = ConvertEx.ToInt32(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_DATAID)));

                    core.DeleteTime = ConvertEx.ToDateTime(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_DELETETIME)));
                    core.Flag = ConvertEx.ToInt32(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_FLAG)));
                    core.ImportDate = ConvertEx.ToDateTime(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_IMPORTDATE)));
                    core.ImportUser = ConvertEx.ToString(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_IMPORTUSER)));
              
                    core.Name = ConvertEx.ToString(feature.get_Value(feature.Fields.FindField(FLD_NAME_F_DATANAME)));
                    pList.Add(core);
                    feature = selectCursor.NextFeature();
                }
                return pList.Count > 0 ? pList[0] : null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (selectCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(selectCursor);
                }
            }
        }

        #region
        
        #endregion


        #region IMetaDataQuery 成员

        public string SelectWKT()
        {
            throw new NotImplementedException();
        }

        public ESRI.ArcGIS.Geometry.IGeometry SelectGeometry()
        {
            IFeatureCursor searchCursor = null;
            try
            {
                _queryFilter = new QueryFilterClass();
                _queryFilter.WhereClause = string.Format("F_DATAID = {0}", DataId);
                searchCursor = _featureClass.Search(_queryFilter, false);
                IFeature feature = searchCursor.NextFeature();
                if (null != feature)
                {
                    return feature.Extent;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if(searchCursor!=null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(searchCursor);
                }
            }
        }

        public DataTable SelectByDataID()
        {
            throw new NotImplementedException();
        }

        public IList<DataTable> SelectByFlag2List(IDBHelper dbHelper, EnumDataState enumDataState, params string[] columnNames)
        {
            throw new NotImplementedException();
        }

        public DataTable SelectByFlag2Table(IDBHelper dbHelper, EnumDataState enumDataState, params string[] columnNames)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
