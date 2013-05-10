using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using Geoway.ADF.MIS.Core.Public.security;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class DataDeleteHelper
    {
        #region 事件定义
        /// <summary>
        /// 显示删除进度信息时触发
        /// </summary>
        public event DeleteProgressEventHandler ShowDeleteProgress;
        /// <summary>
        /// 全部删除完成后触发
        /// </summary>
        public event AfterDeleteDatasEventHandler AfterDeleteDatas;
        /// <summary>
        /// 开始删除一条数据时触发
        /// </summary>
        public event BeginDeleteDataEventHandler BeginDeleteData;
        /// <summary>
        /// 删除一条数据后触发
        /// </summary>
        public event EndDeleteDataEventHandler EndDeleteData;
        #endregion

        private DeleteDataProgressEventArgs _eArgs = new DeleteDataProgressEventArgs();
        private IList<RegisterKey> _dataIDs = null;
        private IList<RegisterKey> _succeedDataIDs = null;
        private StorageServer _storageServer = null;
        private IWorkspace _curWorkspace = null;
        private int _pos = 0;
        
        public DataDeleteHelper(IWorkspace workspace)
        {
            _curWorkspace = workspace;
        }

        public DataDeleteHelper()
        {

        }
        
        public DataDeleteHelper(IList<RegisterKey> dataIDs, IWorkspace workspace)
        {
            _dataIDs = dataIDs;
            _curWorkspace = workspace;
            _eArgs.TotalCount = dataIDs.Count;
            _succeedDataIDs = new List<RegisterKey>();
        }

        public void Start()
        {
            //SpatialExtentInfoDAL data = null;
            bool succeed = true;
            DataRegisterInfo data = null;
            DataRegisterInfo oper = null;   //TODO:初始化
            long datasize = 0;//记录删除数据的数据量
            _eArgs.Maximum = 100 * _dataIDs.Count;

            for (int i = 0; i < _dataIDs.Count; i++)
            {
                succeed = true;
                _pos = 100 * i;

                oper = new DataRegisterInfo(_dataIDs[i].LayerName);
                data = oper.Select(_dataIDs[i].DataID) as DataRegisterInfo;
                data.LayerName = _dataIDs[i].LayerName;

                _eArgs.CurrentData = i + 1;
                _eArgs.CurrentDataName = data.DataName;

                InvokeBeginDeleteData(data.DataName);

                //删除数据文件
                _eArgs.CurrentItem = EnumDeleteItem.DataFile;
                _eArgs.CurrentAction = EnumDeleteAction.Beginning;
                InvokeDeleteProgressEvent();

                if (DeleteDataFile(data) && DeletePublicDataFile(data))
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Succeed;
                    InvokeDeleteProgressEvent();
                }
                else
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Failed;
                    InvokeDeleteProgressEvent();
                    succeed = false;
                }

                //删除元数据
                _eArgs.CurrentItem = EnumDeleteItem.Metadata;
                _eArgs.CurrentAction = EnumDeleteAction.Beginning;
                InvokeDeleteProgressEvent();
                if (DeleteMetadata(data))
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Succeed;
                    InvokeDeleteProgressEvent();
                }
                else
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Failed;
                    InvokeDeleteProgressEvent();
                    succeed = false;
                }

                //删除快视图
                _eArgs.CurrentItem = EnumDeleteItem.SnapShot;
                _eArgs.CurrentAction = EnumDeleteAction.Beginning;
                InvokeDeleteProgressEvent();
                if (DeleteSnapShot(data))
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Succeed;
                    InvokeDeleteProgressEvent();
                }
                else
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Failed;
                    InvokeDeleteProgressEvent();
                    succeed = false;
                }

                //删除空间索引要素
                _eArgs.CurrentAction = EnumDeleteAction.Beginning;
                if (DeleteIndexFeature(data))
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Succeed;
                }
                else
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Failed;
                    succeed = false;
                }

                //删除注册信息
                _eArgs.CurrentItem = EnumDeleteItem.RegisterInfo;
                _eArgs.CurrentAction = EnumDeleteAction.Beginning;
                InvokeDeleteProgressEvent();
                if (DeleteRegisterInfo(data))
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Succeed;
                }
                else
                {
                    _eArgs.CurrentAction = EnumDeleteAction.Failed;
                    succeed = false;
                }
                if (succeed)
                {
                    _succeedDataIDs.Add(_dataIDs[i]);
                }
                InvokeEndDeleteData(data.DataName, succeed);
                datasize += data.DataSize;
            }
            double size = (double)datasize / 1024 / 1024;
            LoginControl.WriteToLog(205, "数据删除：删除影像 " + _succeedDataIDs.Count.ToString() + " 幅，总量" + size.ToString("f3") + " M");
            _pos = _eArgs.Maximum;
            InvokeDeleteProgressEvent();

            if (this.AfterDeleteDatas != null)
            {
                this.AfterDeleteDatas(_succeedDataIDs);
            }
        }

        private void InvokeDeleteProgressEvent()
        {
            _eArgs.Postion = _pos;
            if (this.ShowDeleteProgress != null)
            {
                this.ShowDeleteProgress(_eArgs);
            }
        }

        private void InvokeBeginDeleteData(string dataName)
        {
            if (this.BeginDeleteData != null)
            {
                this.BeginDeleteData(dataName);
            }
        }

        private void InvokeEndDeleteData(string dataName, bool succeed)
        {
            if (this.EndDeleteData != null)
            {
                this.EndDeleteData(dataName, succeed);
            }
        }

        void _storageServer_Progress(object o, ServerProgressEventArgs e)
        {
            _eArgs.FileName = e.FileName;
            InvokeDeleteProgressEvent();
        }

        /// <summary>
        /// 删除注册信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool DeleteRegisterInfo(DataRegisterInfo data)
        {
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            try
            {
                //DataPathInfoDAL.Singleton.DeleteDataPathByHeadInfoID(data.ID);
                data.Delete();
                //if (data.Year > 0)
                //{
                //    KeywordIndexHelper.DeleteKeywordIndex(data.Year.ToString(), EnumKeywordIndexType.Year);
                //}
                //if (!string.IsNullOrEmpty(data.Scale))
                //{
                //    KeywordIndexHelper.DeleteKeywordIndex(data.Scale, EnumKeywordIndexType.Scale);
                //}
                //if (!string.IsNullOrEmpty(data.Resolution))
                //{
                //    KeywordIndexHelper.DeleteKeywordIndex(data.Resolution, EnumKeywordIndexType.Resolution);
                //}
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        /// <summary>
        /// 删除元数据信息
        /// </summary>
        /// <param name="lstMetaData"> </param>
        /// <returns></returns>
        public bool DeleteMetaData(IList<IMetaDataEdit> lstMetaData)
        {
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            try
            {
                IMetaDataOper metaData = null;
                foreach (IMetaDataEdit edit in lstMetaData)
                {
                    metaData = edit as IMetaDataOper;
                    metaData.Delete();
                }
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }
        /// <summary>
        /// 删除元数据表
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static bool DeleteMetaData(IDBHelper dbHelper, int dataID)
        {
            try
            {
                IMetaDataOper metaDataOper = new MetadataRegisterInfoOS(dbHelper,dataID);
                metaDataOper.Delete();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        public bool DeleteIndexFeature(DataRegisterInfo data)
        {
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            try
            {
                //SpatialExtentInfoDAL dal = new SpatialExtentInfoDAL();
                //dal.SetWorkspace(_curWorkspace);
                //return dal.Delete(data.DataID);
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        public bool DeletePublicDataFile(DataRegisterInfo data)
        {
            if (data.DataPackageID > 0)
            {
                System.Data.DataTable dt = DataRegisterInfo.GetDataPackageContent(data.LayerName, data.DataPackageID);
                if (dt == null || dt.Rows.Count == 1)
                {
                    IList<DataPathDAL> lstPath = DataPathDAL.Singleton.SeletByObjectID(data.LayerName, data.DataPackageID, EnumDataFileSourceType.DataPackage);
                    if (lstPath.Count <= 0)
                    {
                        return true;
                    }
                    if (_storageServer == null || _storageServer.ServerParameter.ID != data.ServerID)
                    {
                        //_storageServer = StorageServerFactory.CreateStorageServer(data.ServerID);
                        _storageServer = CatalogModelEngine.CreateCatalogDataSource(CatalogModelEngine.GetStorageNodeByID(DBHelper.GlobalDBHelper as IDBHelper, data.ServerID));
                        _storageServer.Progress += new ServerProgressEventHandler(_storageServer_Progress);
                    }
                    _eArgs.CurrentAction = EnumDeleteAction.Processing;
                    foreach (DataPathDAL path in lstPath)
                    {
                        try
                        {
                            if (DataPathDAL.GetDataPathCountByServerPath(path.FileLocation, _storageServer.ServerParameter.ID) > 1)
                            {
                                path.Delete();
                                continue;
                            }
                            else
                            {
                                if (_storageServer.DeleteFile(path.FileLocation))
                                {
                                    path.Delete();
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        catch (Exception exp)
                        {
                            LogHelper.Error.Append(exp);
                        }
                        finally
                        {
                        }
                    }
                    //DataPackageInfoDAL.Singleton.Delete(data.DataPackageID);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 删除数据文件
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public bool DeleteDataFile(DataRegisterInfo data)
        {
            IList<DataPathDAL> lstPath = DataPathDAL.Singleton.SeletByObjectID(data.LayerName, data.ID, EnumDataFileSourceType.DataUnit);
            if (lstPath.Count <= 0)
            {
                return true;
            }
            int step = 95 / lstPath.Count;
            if (_storageServer == null || _storageServer.ServerParameter.ID != data.ServerID)
            {
                _storageServer = CatalogModelEngine.CreateCatalogDataSource(CatalogModelEngine.GetStorageNodeByID(DBHelper.GlobalDBHelper as IDBHelper, data.ServerID)); 
                _storageServer.Progress += new ServerProgressEventHandler(_storageServer_Progress);
            }
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            foreach (DataPathDAL path in lstPath)
            {
                try
                {
                    if (DataPathDAL.GetDataPathCountByServerPath(path.FileLocation, _storageServer.ServerParameter.ID) > 1)
                    {
                        path.Delete();
                        continue;
                    }
                    else
                    {
                        if (_storageServer.DeleteFile(path.FileLocation))
                        {
                            path.Delete();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogHelper.Error.Append(exp);
                }
                finally
                {
                    _pos += step;
                }
            }
            return true;
        }

        /// <summary>
        /// 删除元数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool DeleteMetadata(DataRegisterInfo data)
        {
            if (data.MetaID <= 0 || string.IsNullOrEmpty(data.MetaTableName))
            {
                return true;
            }
            string sql = string.Format("delete {0} where {1}={2}",
                                       data.MetaTableName,
                                       "F_OID",     //元数据表唯一标识字段
                                       data.MetaID);
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            InvokeDeleteProgressEvent();
            try
            {
                DBHelper.GlobalDBHelper.DoSQL(sql);
                //DeleteSpatialMeteData(data);// 删除空间元数据信息
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        /// <summary>
        /// 删除元数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="registerlayerName"> </param>
        /// <param name="dataID"> </param>
        /// <returns></returns>
        public bool DeleteMetadata(string registerlayerName,string dataID)
        {
            string sql = string.Format("delete {0} where {1}={2}",
                                       registerlayerName,
                                       "F_DATAID",     //元数据表唯一标识字段
                                       dataID);
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            InvokeDeleteProgressEvent();
            try
            {
                DBHelper.GlobalDBHelper.DoSQL(sql);
                //同时删除TBARC_DATAIDMETA中对应记录
                DataIDMetaDAL dataIDMetaDAL = new DataIDMetaDAL();
                dataIDMetaDAL.DataId = Convert.ToInt32(dataID);
                dataIDMetaDAL.Delete();
                //DeleteSpatialMeteData(data);// 删除空间元数据信息
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }
        
        
        /// <summary>
        /// 删除空间元数据信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool DeleteSpatialMeteData(DataRegisterInfo data)
        {
            if (data.MetaID <= 0 || string.IsNullOrEmpty(data.LayerName))
            {
                return true;
            }
            IFeatureWorkspace pFeatureWS = (IFeatureWorkspace)DataRegisterInfo.TargetWorkspace;
            string tableName = data.LayerName + "_DT";
            ITable pTable = null;
            try
            {
                pTable = pFeatureWS.OpenTable(data.LayerName);
            }
            catch
            {
                pTable = null;
                return false;
            }

            if (pTable != null)
            {
                string whereClause = " F_metaID = " + data.MetaID;//" F_OID = " + data.MetaID;

                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = whereClause;
                ICursor cursor = pTable.Search(pFilter, false);
                IRow row = cursor.NextRow();
                while (row != null)
                {
                    row.Delete();
                    row = cursor.NextRow();
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cursor);
            }
            return true;
        }
        /// <summary>
        /// 删除快视图
        /// </summary>
        /// <param name="data">待删除数据</param>
        /// <returns></returns>
        public bool DeleteSnapShot(DataRegisterInfo data)
        {
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            InvokeDeleteProgressEvent();
            int rasterID = -1;
            if (int.TryParse(data.SnapLayer, out rasterID))
            {
                if (rasterID <= 0)
                {
                    return true;
                }
                return DataSnapshotDAL.Delete(rasterID);
            }
            return true;
        }

        /// <summary>
        /// 删除快视图
        /// </summary>
        /// <param name="dataID">待删除快视图对应数据ID</param>
        /// <returns></returns>
        public bool DeleteSnapShot(int dataID)
        {
            _eArgs.CurrentAction = EnumDeleteAction.Processing;
            InvokeDeleteProgressEvent();
            //int rasterID = -1;
            //if (int.TryParse(data.SnapLayer, out rasterID))
            ////{
            //    if (rasterID <= 0)
            //    {
            //        return true;
            //    }
            //    else
            //    {
                    return DataSnapshotDAL.Delete(dataID);
                //}
            //}
            //else
            //{
            //    return true;
            //}
        }

    }



}
