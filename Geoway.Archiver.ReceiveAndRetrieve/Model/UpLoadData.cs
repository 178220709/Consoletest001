using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Geoway.ADF.GIS.GeoDB;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;

using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.Utility.Definition;
using EnumDataExecuteState = Geoway.ADF.MIS.CatalogDataModel.Public.EnumDataExecuteState;
using Path = System.IO.Path;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Catalog.Model;
using Geoway.Archiver.Catalog.Definition;
using Geoway.Archiver.Modeling.Model;
using Geoway.ADF.MIS.DataModel;
using ESRI.ArcGIS.DataSourcesRaster;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public delegate void LogEventHandler(object sender, LogEventArgs e);
    /// <summary>
    /// 2012.11.6 修改doachive2 保存坐标文件如：jgw。修改保存策略，先判断服务端有没有，没有则在本地temp文件夹写入
    /// 然后进入快视图入库操作，也是先判断服务端文件夹内有没有，没有则在temp文件夹去找。
    /// </summary>
    public class UpLoadData
    {
        #region 事件相关

        public delegate void SetProgressEventHandler(
            object sender, int position, int max, string message, enumLogType enumLogType);

        public event SetProgressEventHandler SetProgress;
        public event LogEventHandler WriteLog;

        public void OnWriteLog(LogEventArgs e)
        {
            LogEventHandler handler = WriteLog;
            if (handler != null) handler(this, e);
        }

        public void OnSetProgress(int position, int max, string message, enumLogType enumLogType)
        {
            SetProgressEventHandler handler = SetProgress;
            if (handler != null) handler(this, position, max, message, enumLogType);
        }

        #endregion

        #region 私有变量

        private StorageServer _server;
        private IDBHelper _dbHelper;
        private UpLoadSetting _setting;
        private Dictionary<string, object> _dicMetaData;

        private List<DBFieldItem> _fieldItems;
        private DataFilePathInfoEx _dataFilePath;
        private string _dataName;
        private int _dataID = -1;
        private string _currentServerFile; //当前正在上传文件的目标服务器相对路径
        private string _currentPackagePath;
        private string _currentPath;
        private string _xmlPath;
        private XmlInfo _xmlInfo;
        private long _filesize = -1;
        private EnumPathStorageType _enumPathStorageType;
        private bool _writePathResult = true;
        private LogRecord _logRecord; //日志信息
        private string _wkt;
        private DataTable _subTable;

        #endregion

        #region 构造函数

        public UpLoadData(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
            _logRecord = new LogRecord();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 入库日志记录
        /// </summary>
        public LogRecord LogRecord
        {
            get { return _logRecord; }
            set { _logRecord = value; }
        }

        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataName
        {
            set { _dataName = value; }
        }

        public UpLoadSetting UpLoadSetting
        {
            set
            {
                _setting = value;
                _enumPathStorageType = ((IMSDZSetting) _setting).enumPathStorageType;
            }
        }

        /// <summary>
        /// 单条元数据信息
        /// </summary>
        public Dictionary<string, object> DicMetaData
        {
            set { _dicMetaData = value; }
        }

        /// <summary>
        /// 数据路径信息
        /// </summary>
        public DataFilePathInfoEx DataFilePath
        {
            get { return _dataFilePath; }
            set { _dataFilePath = value; }
        }

        private EnumUIType _enumUIType;

        public EnumUIType EnumUIType
        {
            set { _enumUIType = value; }
        }

        /// <summary>
        /// （主表）入库字段信息
        /// </summary>
        public List<DBFieldItem> FieldItems
        {
            get { return _fieldItems; }
            set { _fieldItems = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string WKT
        {
            get { return _wkt; }
            set { _wkt = value; }
        }

        /// <summary>
        /// 从表
        /// </summary>
        public DataTable SubTable
        {
            get { return _subTable; }
            set { _subTable = value; }
        }

        #endregion

        #region 公共方法，主库的主函数，唯一的对外借口

        /// <summary>
        /// 执行归档操作
        /// </summary>
        /// <returns></returns>
        public bool DoAchive()
        {
            bool isDeleteEntity = !_setting.BScanMode;
            IMetaDataOper metaDataOper = null;
            try
            {
                _dataID = -1;
                _logRecord.ListLog.Add(new LogInfo("开始入库...", enumLogType.eInformation));
                //1、写元数据表扩展+固有+系统信息
                if (!WriteMetaData(out _dataID) || _dataID == -1)
                {
                    //DataOper.DeleteMetaData(_dbHelper, _dataID);
                    OnWriteLog(new LogEventArgs("入库元数据：元数据写入失败。", enumLogType.eFailureAudit));
                    _logRecord.ListLog.Add(new LogInfo("入库元数据：元数据写入失败", enumLogType.eFailureAudit));
                    return false;
                }
                //OnWriteLog(new LogEventArgs("入库元数据：元数据写入成功。", enumLogType.eSuccessAudit));
                _logRecord.ListLog.Add(new LogInfo("入库元数据：元数据写入成功", enumLogType.eSuccessAudit));

                metaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace, _dataID);
                //2、入库空间数据
                if (_setting.CatalogNode.NodeExInfo.IsStoreToSpatialDS)
                {
                    ImportSpatialData importSpatialData = new ImportSpatialData();
                    importSpatialData.EnumWorkspaceType = EnumWorkspaceType.enumShp;
                    importSpatialData.FileName = _dataFilePath.MainInstance.FullFileName;
                    importSpatialData.TargetDatasetName = "";
                    importSpatialData.TargetFcName =
                        Path.GetFileNameWithoutExtension(_dataFilePath.MainInstance.FullFileName);
                    importSpatialData.TargetWorkspace =
                        GwWorkspace.SelectByKey(_dbHelper, _setting.CatalogNode.NodeExInfo.DataSourceKey).EsriWorkspace
                        as IFeatureWorkspace;
                    if (!importSpatialData.DoImport())
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库空间数据：空间数据入库失败", enumLogType.eFailureAudit));
                        _logRecord.ListLog.Add(new LogInfo("入库空间数据：空间数据入库失败", enumLogType.eFailureAudit));
                        return false;
                    }
                    //OnWriteLog(new LogEventArgs("入库空间数据：空间数据入库成功。", enumLogType.eSuccessAudit));
                    _logRecord.ListLog.Add(new LogInfo("入库空间数据：空间数据入库成功", enumLogType.eSuccessAudit));
                }

                //3、写快视图、拇指图
                string snapshotPath = "";
                if (_dataFilePath != null && _dataFilePath.BOutSnopshot && _dataFilePath.OutSnapShotFileInstance != null)
                {
                    snapshotPath = _dataFilePath.OutSnapShotFileInstance.FullFileName;
                }
                else if (_dataFilePath != null && !_dataFilePath.BOutSnopshot &&
                         _dataFilePath.SnapShotFileInstance != null)
                {
                    snapshotPath = _dataFilePath.SnapShotFileInstance.FullFileName;
                }

                if (!string.IsNullOrEmpty(snapshotPath) && File.Exists(snapshotPath))
                {
                    IRasterDataset raster = RasterDataOperater.OpenRasterDataset(snapshotPath);
                    IGeoDataset dataset = raster as IGeoDataset;
                    if (dataset == null)
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库快视图：快视图获取失败", enumLogType.eFailureAudit));
                        _logRecord.ListLog.Add(new LogInfo("入库快视图：快视图获取失败", enumLogType.eFailureAudit));
                        return false;
                    }
                    string spatitalReferenceName = string.Empty;
                    if (dataset.SpatialReference != null && dataset.SpatialReference.Name != "Unknown")
                    {
                        spatitalReferenceName = dataset.SpatialReference.Name;
                    }
                    else
                    {
                        ISpatialReference pSR = SysSpatialParams.Para_SR;
                        if (pSR != null)
                        {
                            spatitalReferenceName = pSR.Name;
                        }
                    }
                    ComReleaser.ReleaseCOMObject(raster);

                    //获取拇指图
                    string thumbFilePath = "";
                    if (!_dataFilePath.BOutThumb && _dataFilePath.ThumbFileInstance != null)
                    {
                        thumbFilePath = _dataFilePath.ThumbFileInstance.FullFileName;
                    }
                    else if (_dataFilePath.BOutThumb && _dataFilePath.OutThumbFileInstance != null)
                    {
                        thumbFilePath = _dataFilePath.OutThumbFileInstance.FullFileName;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(snapshotPath))
                        {
                            //从快视图中抽取拇指图
                            string targetThumbPath = string.Format("{0}\\temp\\{1}_thumb.jpg", Application.StartupPath,
                                                                   Path.GetFileNameWithoutExtension(
                                                                       snapshotPath));

                            if (!CreateThumbHelper.CreateThumb(snapshotPath, targetThumbPath))
                            {
                                CreateThumbHelper.CreateGISThumb(snapshotPath, targetThumbPath);
                            }
                            if (File.Exists(targetThumbPath))
                            {
                                thumbFilePath = targetThumbPath;
                            }
                            else
                            {
                                DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                                OnWriteLog(new LogEventArgs("入库快视图：拇指图获取失败", enumLogType.eFailureAudit));
                                _logRecord.ListLog.Add(new LogInfo("入库快视图：拇指图获取失败", enumLogType.eFailureAudit));
                                return false;
                            }
                        }
                    }
                    //写入快视图和拇指图
                    int snapshotID = DataSnapshotOper.ImportSnapshot(snapshotPath, thumbFilePath, spatitalReferenceName,
                                                                     _dataID);
                    if (snapshotID < 0)
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库快视图：快视图入库失败", enumLogType.eFailureAudit));
                        _logRecord.ListLog.Add(new LogInfo("入库快视图：快视图入库失败", enumLogType.eFailureAudit));
                        return false;
                    }
                    //OnWriteLog(new LogEventArgs("入库快视图：快视图入库成功。", enumLogType.eSuccessAudit));
                    _logRecord.ListLog.Add(new LogInfo("入库快视图：快视图入库成功", enumLogType.eSuccessAudit));
                }
                //设置服务器存储路径（动态路径需要从元数据获取，因此需要元数据入库成功后才能设置,lhy,20121115）
                _setting.StoragePath = ServerPathManager.GetServerStoragePath(_dbHelper, _setting.CatalogNode, _dataID);
                //4、上传实体数据
                if (_setting.BAffix)
                {
                    _setting.StoragePath += "/" + _dataID;
                    metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_LOCATION, _setting.StoragePath,
                                                        EnumDBFieldType.FTString));

                }

                if (_setting.BUpLoadDataToServer) //迁移式/上传附件
                {
                    if (!initServer())
                    {
                        metaDataOper.Delete(false);
                        //DataOper.DeleteMetaData(_dbHelper, _dataID);
                        return false;
                    }

                    _server.BeginPut += _server_BeginPut;
                    _server.Progress += _server_Progress;
                    _server.EndPut += _server_EndPut;
                    
                    if (!DoUpLoadDataToServer(_setting.StoragePath))
                    {
                        metaDataOper.Delete(false);
                        //_server.ReleaseConnect();
                        return false;
                    }
                    //_server.ReleaseConnect();
                }
                else if (_setting.BScanMode) //扫描式/不上传附件
                {
                    // qfc-modify
                    bool isJzglScan = false;
                    //if (!SysParams.IsLocal)
                    //{
                        isJzglScan = _setting.CatalogNode.NodeExInfo.MetaDatumTypeFlag ==
                                          EnumMetaDatumType.enumSTELEDatumMED
                                          & _setting.BScanMode; //新增挂USB盘后更改（介质关联+扫描式）

                        if (!isJzglScan && !initServer())
                        {
                            //_server.ReleaseConnect();
                            DataOper.DeleteMetaData(_dbHelper, _dataID, false);
                            return false;
                        }
                        //_server.ReleaseConnect();
                    //}

                    if (!WriteDataPathInfo())
                    {
                        OnWriteLog(new LogEventArgs("上传数据：记录上传数据路径信息失败", enumLogType.eFailureAudit));
                        _logRecord.ListLog.Add(new LogInfo("上传数据：记录上传数据路径信息失败", enumLogType.eFailureAudit));
                        metaDataOper.Delete(false);
                        return false;
                    }
                }
                //OnWriteLog(new LogEventArgs("上传数据：数据实体文件入库成功。", enumLogType.eSuccessAudit));
                _logRecord.ListLog.Add(new LogInfo("上传数据：数据实体文件入库成功", enumLogType.eSuccessAudit));

                //5、更新元数据标识信息(0：入库成功)
                metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_FLAG, 0,
                                                    EnumDBFieldType.FTNumber));
                if (_setting.BAffix)
                {
                    _setting.StoragePath = _setting.StoragePath.Substring(0,
                                                                          _setting.StoragePath.IndexOf(
                                                                              _dataID.ToString()) - 1);
                }
                //_logRecord.ListLog.Add(new LogInfo(string.Format("[{0}] 入库成功。", DateTime.Now), enumLogType.eSuccessAudit));

                return true;
            }
            catch (Exception ex)
            {
                if (metaDataOper != null)
                {
                    metaDataOper.Delete(isDeleteEntity);
                }
                OnWriteLog(new LogEventArgs(ex.ToString(), enumLogType.eFailureAudit));
                LogHelper.Error.Append(ex);
                return false;
            }
            finally
            {
                ReleaseServer();
            }
        }
        /// <summary>
        /// 执行归档操作(目前用于资三自动归档）
        /// </summary>
        /// <returns></returns>
        public int DoAchive2()
        {
            bool isDeleteEntity = !_setting.BScanMode;
            IMetaDataOper metaDataOper = null;
            try
            {
                _dataID = -1;
                OnWriteLog(new LogEventArgs("开始入库...", enumLogType.eInformation));
                //1、写元数据表扩展+固有+系统信息
                if (!WriteMetaData(out _dataID) || _dataID == -1)
                {
                    //DataOper.DeleteMetaData(_dbHelper, _dataID);
                    OnWriteLog(new LogEventArgs("入库元数据：元数据写入失败。", enumLogType.eFailureAudit));

                    return -1;
                }
                OnWriteLog(new LogEventArgs("入库元数据：元数据写入成功。", enumLogType.eSuccessAudit));
                
                metaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace, _dataID);
                //2、入库空间数据
                if (_setting.CatalogNode.NodeExInfo.IsStoreToSpatialDS)
                {
                    ImportSpatialData importSpatialData = new ImportSpatialData();
                    importSpatialData.EnumWorkspaceType = EnumWorkspaceType.enumShp;
                    importSpatialData.FileName = _dataFilePath.MainInstance.FullFileName;
                    importSpatialData.TargetDatasetName = "";
                    importSpatialData.TargetFcName =
                        Path.GetFileNameWithoutExtension(_dataFilePath.MainInstance.FullFileName);
                    importSpatialData.TargetWorkspace =
                        GwWorkspace.SelectByKey(_dbHelper, _setting.CatalogNode.NodeExInfo.DataSourceKey).EsriWorkspace
                        as IFeatureWorkspace;
                    if (!importSpatialData.DoImport())
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库空间数据：空间数据入库失败", enumLogType.eFailureAudit));
                        return -1;
                    }
                    OnWriteLog(new LogEventArgs("入库空间数据：空间数据入库成功。", enumLogType.eSuccessAudit));
                }

                //3、写快视图、拇指图
                string snapshotPath = "";
                if (_dataFilePath != null && _dataFilePath.BOutSnopshot && _dataFilePath.OutSnapShotFileInstance != null)
                {
                    snapshotPath = _dataFilePath.OutSnapShotFileInstance.FullFileName;
                }
                else if (_dataFilePath != null && !_dataFilePath.BOutSnopshot &&
                         _dataFilePath.SnapShotFileInstance != null)
                {
                    snapshotPath = _dataFilePath.SnapShotFileInstance.FullFileName;
                }

                if (!string.IsNullOrEmpty(snapshotPath) && File.Exists(snapshotPath))
                {
                    IRasterDataset raster = RasterDataOperater.OpenRasterDataset(snapshotPath);
                    IGeoDataset dataset = raster as IGeoDataset;
                    if (dataset == null)
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库快视图：快视图获取失败", enumLogType.eFailureAudit));
                        //_logRecord.ListLog.Add(new LogInfo("入库快视图：快视图获取失败", enumLogType.eFailureAudit));
                        return -1;
                    }
                    string spatitalReferenceName = string.Empty;
                    if (dataset.SpatialReference != null && dataset.SpatialReference.Name != "Unknown")
                    {
                        spatitalReferenceName = dataset.SpatialReference.Name;
                    }
                    else
                    {
                        ISpatialReference pSR = SysSpatialParams.Para_SR;
                        if (pSR != null)
                        {
                            spatitalReferenceName = pSR.Name;
                        }
                    }
                    

                    //获取拇指图
                    string thumbFilePath = "";
                    if (!_dataFilePath.BOutThumb && _dataFilePath.ThumbFileInstance != null)
                    {
                        thumbFilePath = _dataFilePath.ThumbFileInstance.FullFileName;
                    }
                    else if (_dataFilePath.BOutThumb && _dataFilePath.OutThumbFileInstance != null)
                    {
                        thumbFilePath = _dataFilePath.OutThumbFileInstance.FullFileName;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(snapshotPath))
                        {
                            //从快视图中抽取拇指图
                            string targetThumbPath = string.Format("{0}\\temp\\{1}_thumb.jpg", Application.StartupPath,
                                                                   Path.GetFileNameWithoutExtension(
                                                                       snapshotPath));

                            if (!CreateThumbHelper.CreateThumb(snapshotPath, targetThumbPath))
                            {
                                CreateThumbHelper.CreateGISThumb(snapshotPath, targetThumbPath);
                            }
                            if (File.Exists(targetThumbPath))
                            {
                                thumbFilePath = targetThumbPath;
                            }
                            else
                            {
                                DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                                OnWriteLog(new LogEventArgs("入库快视图：拇指图获取失败", enumLogType.eFailureAudit));
                                //_logRecord.ListLog.Add(new LogInfo("入库快视图：拇指图获取失败", enumLogType.eFailureAudit));
                                return -1;
                            }
                        }
                    }
                    //判断快视图参考文件，没有则创建
                    string kstExtension = Path.GetExtension(snapshotPath).TrimStart('.').ToUpper(); //快视图后缀名
                    string wfFrt = DataSnapshotOper.GetWordFileExtention(kstExtension);
                    string wordFileServerName = Path.ChangeExtension(snapshotPath, wfFrt); //获取wordfile路径
                    string wordFileName = Path.GetFileName(wordFileServerName);
                    string wordFileLocalName = string.Format("{0}\\temp\\{1}", Application.StartupPath,
                                                             wordFileName);
                    if (File.Exists(wordFileServerName) == false && !string.IsNullOrEmpty(_wkt))
                    {
                //先获取jpg的行列数
                IRasterBandCollection pBandCollection = raster as IRasterBandCollection;
                IRasterBand pRasterBand = pBandCollection.Item(0);
                IRawPixels pRawPixels = pRasterBand as IRawPixels;
                IRasterProps pRasterPros = pRawPixels as IRasterProps;
                int sColumnCount = pRasterPros.Width; //栅格数据集行列数
                int sRowCount = pRasterPros.Height;   //栅格数据集行列数

                //释放变量 ！！！如果不释放 那么添加图层的范围有误，为配准前的范围
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pBandCollection);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRasterBand);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRawPixels);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRasterPros);
                
                //生成jgw文件
                IPolygon polygon = DataSnapshotOper.CreatGeoByWKT(_wkt);
                DataSnapshotOper.ExportWordFile(sRowCount, sColumnCount, polygon, wordFileLocalName);

            }
            ComReleaser.ReleaseCOMObject(raster);
                    //写入快视图和拇指图
                    int snapshotID = DataSnapshotOper.ImportSnapshot(snapshotPath, thumbFilePath, spatitalReferenceName,
                                                                     _dataID);
                    if (snapshotID < 0)
                    {
                        DataOper.DeleteMetaData(_dbHelper, _dataID, isDeleteEntity);
                        OnWriteLog(new LogEventArgs("入库快视图：快视图入库失败", enumLogType.eFailureAudit));
                        //_logRecord.ListLog.Add(new LogInfo("入库快视图：快视图入库失败", enumLogType.eFailureAudit));
                        return -1;
                    }
                    OnWriteLog(new LogEventArgs("入库快视图：快视图入库成功。", enumLogType.eSuccessAudit));
                    //_logRecord.ListLog.Add(new LogInfo("入库快视图：快视图入库成功", enumLogType.eSuccessAudit));
                }
                #region 实体不上传
                /*
                //4、上传实体数据
                if (_setting.BAffix)
                {
                    _setting.StoragePath += "/" + _dataID;
                    metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_LOCATION, _setting.StoragePath,
                                                        EnumDBFieldType.FTString));

                }

                if (_setting.BUpLoadDataToServer) //迁移式/上传附件
                {
                    if (!initServer())
                    {
                        metaDataOper.Delete(false);
                        //DataOper.DeleteMetaData(_dbHelper, _dataID);
                        return false;
                    }

                    _server.BeginPut += _server_BeginPut;
                    _server.Progress += _server_Progress;
                    _server.EndPut += _server_EndPut;
                    //完善存储路径，加上动态存储路径,因为需要元数据内容，所以放到这里 20121026 lhy
                    if (_setting.CatalogNode.NodeExInfo.IsHasAutoPath && _setting.CatalogNode.NodeExInfo.DynamicPathRules != null)
                    {
                        foreach (DynamicPathRuleInfo info in _setting.CatalogNode.NodeExInfo.DynamicPathRules)
                        {
                            if (info.ElementType == EnumElementType.enumString)
                            {
                                //固定字符串形式
                                _setting.StoragePath = _setting.StoragePath + "/" + info.FixedValue;
                            }
                            else
                            {
                                //从元数据字段获取形式
                                if (_setting.CatalogNode.NodeExInfo.DatumTypeObj.Fields != null && _setting.CatalogNode.NodeExInfo.DatumTypeObj.Fields.Count > 0)
                                {
                                    foreach (DatumTypeField field in _setting.CatalogNode.NodeExInfo.DatumTypeObj.Fields)
                                    {
                                        if (field.MetaFieldObj.ID == info.MetaFieldId)
                                        {
                                            //获取字段值
                                            object obj = metaDataOper.GetValue(field.MetaFieldObj.Name);
                                            if (obj != null)
                                            {
                                                if (field.MetaFieldObj.Type == EnumFieldType.DateTime)
                                                {
                                                    if (string.IsNullOrEmpty(info.MetaFieldRule) == false)
                                                    {
                                                        DateTime dtTemp;
                                                        if (DateTime.TryParse(obj.ToString(), out dtTemp))
                                                        {
                                                            string[] strs = info.MetaFieldRule.Split("|".ToCharArray());
                                                            foreach (string strItem in strs)
                                                            {
                                                                _setting.StoragePath = _setting.StoragePath + "/" + dtTemp.ToString(strItem);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _setting.StoragePath = _setting.StoragePath + "/" + obj.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!DoUpLoadDataToServer(_setting.StoragePath))
                    {
                        metaDataOper.Delete(false);
                        //_server.ReleaseConnect();
                        return false;
                    }
                    //_server.ReleaseConnect();
                }
                else if (_setting.BScanMode) //扫描式/不上传附件
                 * */
                #endregion
                {
                    bool isJzglScan = _setting.CatalogNode.NodeExInfo.MetaDatumTypeFlag ==
                                      EnumMetaDatumType.enumSTELEDatumMED
                                      & _setting.BScanMode; //新增挂USB盘后更改（介质关联+扫描式）

                    if (!isJzglScan && !initServer())
                    {
                        //_server.ReleaseConnect();
                        DataOper.DeleteMetaData(_dbHelper, _dataID, false);
                        return -1;
                    }
                    //_server.ReleaseConnect();

                    if (!WriteDataPathInfo())
                    {
                        OnWriteLog(new LogEventArgs("上传数据：记录上传数据路径信息失败", enumLogType.eFailureAudit));
                        //_logRecord.ListLog.Add(new LogInfo("上传数据：记录上传数据路径信息失败", enumLogType.eFailureAudit));
                        metaDataOper.Delete(false);
                        return -1;
                    }
                }
                OnWriteLog(new LogEventArgs("上传数据：数据实体文件入库成功。", enumLogType.eSuccessAudit));
                //_logRecord.ListLog.Add(new LogInfo("上传数据：数据实体文件入库成功", enumLogType.eSuccessAudit));

                //5、更新元数据标识信息(0：入库成功)
                metaDataOper.Update(new DBFieldItem(FixedFieldName.FLD_NAME_F_FLAG, 0,
                                                    EnumDBFieldType.FTNumber));
                //if (_setting.BAffix)
                //{
                //    _setting.StoragePath = _setting.StoragePath.Substring(0,
                //                                                          _setting.StoragePath.IndexOf(
                //                                                              _dataID.ToString()) - 1);
                //}
                OnWriteLog(new LogEventArgs(string.Format("[{0}] 入库成功。", DateTime.Now), enumLogType.eSuccessAudit));
                //_logRecord.ListLog.Add(new LogInfo(string.Format("[{0}] 入库成功。", DateTime.Now), enumLogType.eSuccessAudit));

                return _dataID;
            }
            catch (Exception ex)
            {
                if (metaDataOper != null)
                {
                    metaDataOper.Delete(isDeleteEntity);
                }
                OnWriteLog(new LogEventArgs(ex.ToString(), enumLogType.eFailureAudit));
                LogHelper.Error.Append(ex);
                return -1;
            }
            finally
            {
                ReleaseServer();
            }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化上传服务器
        /// </summary>
        /// <returns></returns>
        private bool initServer()
        {
            if (_server == null)
            {
                StorageServerParam serverParam = CatalogModelEngine.GetStorageNodeByID(_dbHelper,
                                                                                       _setting.ServerID);
                if (serverParam == null)
                {
                    OnWriteLog(new LogEventArgs("上传数据：无法获取服务器信息", enumLogType.eFailureAudit));
                    return false;
                }

                _server =
                    CatalogModelEngine.CreateCatalogDataSource(serverParam);
                if (_server == null)
                {
                    OnWriteLog(new LogEventArgs("上传数据：获取服务器失败", enumLogType.eFailureAudit));
                    return false;
                }
                //_server.ReleaseConnect();
                if (!_server.Connected())
                {
                    OnWriteLog(new LogEventArgs("上传数据：服务器连接失败", enumLogType.eFailureAudit));
                    return false;
                }
      
            }
            return true;
        }

        /// <summary>
        /// 释放上传服务器连接
        /// </summary>
        /// <returns></returns>
        private bool ReleaseServer()
        {
            if (_server == null)
            {
                return false;
            }
            _server.ReleaseConnect();
            return true;
        }

        /// <summary>
        /// 写元数据信息
        /// </summary>
        /// <param name="dataID"></param>
        /// <returns></returns>
        private bool WriteMetaData(out int dataID)
        {
            bool isDeleteEntity = !_setting.BScanMode;
            //dataID = -1;
            IMetaDataOper metaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace,
                                                                _setting.CatalogNode.MetaTableName);
            //try
            //{
            //1.1 写元数据表（主表）
            IMetaDataEdit metaDataEdit = metaDataOper as IMetaDataEdit;
            metaDataEdit.FieldItems = _fieldItems;
            metaDataEdit.WKT = _wkt;
            if (metaDataOper.Insert() <= 0)
            {
                dataID = metaDataEdit.DataId;
                return false;
            }
            dataID = metaDataEdit.DataId;

            //1.2 写从表信息
            if (_subTable != null)
            {
                foreach (DataRow dataRow in _subTable.Rows)
                {
                    IMetaDataOper subMetaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace,
                                                                           _setting.CatalogNode.SubMetaTableName);
                    IMetaDataEdit subMetadataEdit = subMetaDataOper as IMetaDataEdit;
                    List<DBFieldItem> items = MetaDataConverter.ToList(dataRow,
                                                                       _setting.CatalogNode.NodeExInfo.
                                                                           DatumTypeObj) as List<DBFieldItem>;
                    subMetadataEdit.FieldItems = items;
                    //空间范围
                    if (_subTable.Columns.Contains(SysParams.Alias_F_WKT))
                    {
                        subMetadataEdit.WKT = dataRow[SysParams.Alias_F_WKT].ToString();
                    }
                    DBFieldItem item = items.Find(ExsitRelDataID); //查找关联字段
                    if (item != null)
                    {
                        item.Value = dataID; //为关联字段赋值
                    }
                    else //不存在关联字段
                    {
                        metaDataOper.Delete(isDeleteEntity);
                        dataID = -1;
                        return false;
                    }
                    if (subMetaDataOper.Insert() <= 0) //从表插入失败
                    {
                        metaDataOper.Delete(isDeleteEntity);
                        dataID = -1;
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ExsitRelDataID(DBFieldItem obj)
        {
            return obj.Name == FixedFieldName.FLD_NAME_F_REDATAID;
        }

        private bool _bLastFile;

        /// <summary>
        /// 上传数据实体文件
        /// </summary>
        /// <param name="storagePath"></param>
        /// <returns></returns>
        private bool DoUpLoadDataToServer(string storagePath)
        {
            try
            {
                _filesize = 0;

                if (_enumPathStorageType == EnumPathStorageType.EnumXML)
                {
                    string directoryPath = string.Format(@"{0}\temp", Application.StartupPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    _xmlPath = string.Format(@"{0}\{1}_{2}_{3}.xml", directoryPath, "path", _setting.CatalogNode.ID,
                                             _dataID);
                    _xmlInfo = new XmlInfo(_xmlPath, true);
                }

                //入库数据存在数据类型并且其数据包名称与完整数据包所在的文件夹同名时，则按其数据包组织数据（既包含承载该数据的父文件夹），有待完善（暂时spot影像存在此状况）
                if (_dataFilePath.DataObject != null)
                {

                    bool isDataPackage = false;
                    DataPackage dataPackage = _dataFilePath.DataObject as DataPackage;
                    GwDataObject dataKeyObject = dataPackage.GetDataKeyObject();
                    if (dataKeyObject.ObjectType == EnumObjectType.DataPackage)
                    {
                        isDataPackage = true;
                    }

                    //获取目标路径
                    if ((!string.IsNullOrEmpty(_dataFilePath.DataName) && _dataFilePath.DataName == _dataFilePath.FolderName)
                        || isDataPackage)
                    {
                        storagePath += "/" + _dataFilePath.FolderName;
                    }
                }

                //上传数据文件
                List<FileInstance> listFileInstance = _dataFilePath.AllUpLoadFilesInstance;
                foreach (FileInstance filePath in listFileInstance)
                {
                    //是否为最后一条数据（在数据路径为xml存储模式下，非最后一条数据时在上传完该数据后只将该数据路径记录记录到临时xml文件中，若为最后一条数据则在上传完成后，将xml文件入库并记录相关信息到datapath数据库表）
                    if (_enumPathStorageType == EnumPathStorageType.EnumXML)
                    {
                        _bLastFile = filePath == listFileInstance[listFileInstance.Count - 1];
                    }

                    //获取文件上传后的路径
                    string serverFile = GetServerFile(storagePath, filePath);


                    //服务器上的相对路径，用于记录入库
                    _currentServerFile = serverFile;
                    //数据在数据包中的路径，用于记录入库
                    _currentPackagePath = filePath.PackagePath;

                    string fileFullPath;


                    if (FileHelper.IsFileInUse(filePath.FullFileName))
                    {
                        string tempPath = string.Format("{0}\\temp\\{1}", Application.StartupPath,
                                                        Path.GetFileName(filePath.FullFileName));
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                        File.Copy(filePath.FullFileName, tempPath);
                        fileFullPath = tempPath;
                    }
                    else
                    {
                        fileFullPath = filePath.FullFileName;
                    }

                    _currentPath = fileFullPath;
                    //执行上传数据，上传完成后执行记录入库事件
                    _writePathResult = false;
                    if (!_server.PutSingleFile(fileFullPath, serverFile))
                    {
                        return false;
                    }
                    if (!_writePathResult) //写路径表失败
                    {
                        if (_enumPathStorageType == EnumPathStorageType.EnumXML && _bLastFile)
                        {
                            return false;
                        }
                        if (_enumPathStorageType == EnumPathStorageType.EnumDB)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        private string GetServerFile(string storagePath, FileInstance filePath)
        {
            string serverFile="";
            DataPackage dataPackage = _dataFilePath.DataObject as DataPackage;
             if(dataPackage==null)//无数据类型，暂时只存在于控制点入库这一种情况下
             {
                 serverFile = DataInstanceHelperEx.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                     storagePath,
                                                                     filePath.PackagePath,
                                                                     filePath.FullFileName,
                                                                     _dataFilePath.RootFolderPath);
                 return serverFile;
             }
            GwDataObject dataKeyObject = dataPackage.GetDataKeyObject();
           
            
            EnumObjectType enumObjectType = dataKeyObject.ObjectType;

            switch (enumObjectType)
            {
                case EnumObjectType.DataPackage:
                    serverFile = _server.ServerParameter.FtpPath + "/" + storagePath.TrimEnd('/') + "/" +
                                 filePath.FullFileName.Replace(_dataFilePath.RootFolderPath, "").TrimStart('\\');
                    break;
                case EnumObjectType.DataFolder:
                    break;
                case EnumObjectType.DataFile:
                    serverFile = DataInstanceHelperEx.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                      storagePath,
                                                                      filePath.PackagePath,
                                                                      filePath.FullFileName,
                                                                      _dataFilePath.RootFolderPath);
                    break;
                default:
                    break;
            }
            return serverFile;
        }

        private void _server_Progress(object o, ServerProgressEventArgs e)
        {
            OnSetProgress((int) e.Position, (int) e.Length, string.Format("正在上传数据：“{0}”", e.FileName),
                          enumLogType.eUnkown);
        }

        private void _server_BeginPut(object o, ServerFileEventArgs e)
        {
            OnSetProgress(-1, 100, string.Format("开始上传数据：“{0}”", e.FileName), enumLogType.eUnkown);
        }

        /// <summary>
        /// 上传完成
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void _server_EndPut(object o, ServerFileEventArgs e)
        {
            //上传不成功则返回
            if (e.Status != EnumDataExecuteState.Successed)
            {
                if (e.Exp != null)
                {
                    LogHelper.Error.Append(e.Exp);
                    OnSetProgress(-1, -1, string.Format("上传数据文件：“{0}”失败：{1}", _currentPath, e.Exp),
                                  enumLogType.eFailureAudit);
                }
                return;
            }
            OnSetProgress(-1, -1, string.Format("上传数据文件：“{0}”成功", _currentPath), enumLogType.eSuccessAudit);
            //上传成功则记录到数据路径表
            try
            {
                DataPathInfo info = new DataPathInfo();
                info.RegisterLayerName = _setting.CatalogNode.MetaTableName;
                info.ObjectID = _dataID.ToString();
                info.SourceType = EnumDataFileSourceType.DataUnit;
                info.ServerID = _server.ServerParameter.ID;

                switch (_enumPathStorageType)
                {
                    case EnumPathStorageType.EnumDB:
                        info.PackagePath = _currentPackagePath;
                        info.FileLocation = _currentServerFile;
                        info.DataSize = e.FileSize/1024;
                        info.EnumStorageType = EnumPathStorageType.EnumDB;
                        _writePathResult = info.Insert();
                        break;
                    case EnumPathStorageType.EnumXML:
                        string path = "/" + _setting.StoragePath + "/" +
                                      StringHelper.TrimStart(e.FileName, _dataFilePath.RootFolderPath + "\\");
                        _xmlInfo.InsertNode(@"//root", "File", path);
                        _filesize += e.FileSize/1024;

                        if (_bLastFile)
                        {
                            info.DataSize = _filesize;
                            info.XmlPath = _xmlPath;
                            info.EnumStorageType = EnumPathStorageType.EnumXML;
                            _writePathResult = info.Insert();
                        }
                        break;
                }


            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        private bool WriteDataPathInfo()
        {
            string storagePath = _setting.StoragePath;
            if (_dataFilePath.DataObject != null)
            {
                //获取目标路径
                if (!string.IsNullOrEmpty(_dataFilePath.DataName) && _dataFilePath.DataName == _dataFilePath.FolderName)
                {
                    storagePath += "/" + _dataFilePath.FolderName;
                }
            }

            foreach (FileInstance filePath in _dataFilePath.AllUpLoadFilesInstance)
            {
                string serverFile = "";
                string serverID = "";

                //if (!SysParams.IsLocal)// qfc-add
                //{
                    if (_setting.CatalogNode.NodeExInfo.MetaDatumTypeFlag == EnumMetaDatumType.enumSTELEDatumMED && _setting.BScanMode)
                    {
                        serverFile = storagePath + "\\" + Path.GetFileName(filePath.FullFileName);
                    }
                    else
                    {
                        if (filePath.FullFileName.StartsWith("\\\\"))
                        {
                            string ip;
                            string domin;
                            NetControl.ParseDomainName(filePath.FullFileName, out ip, out domin);

                            string addressIP = _server.ServerParameter.Address.ToUpper().Replace(domin.ToUpper(), ip);
                            serverFile = filePath.FullFileName.ToUpper().Replace(addressIP, "").Replace(
                                _server.ServerParameter.Address.ToUpper(), "");
                        }
                        else
                        {
                            serverFile = DataInstanceHelperEx.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                              storagePath,
                                                                              filePath.PackagePath,
                                                                              filePath.FullFileName,
                                                                              _dataFilePath.RootFolderPath);
                        }
                    }
                //}
                //else
                //{
                //    serverFile = storagePath + "\\" + Path.GetFileName(filePath.FullFileName);
                //}
                DataPathInfo info = new DataPathInfo();
                info.RegisterLayerName = _setting.CatalogNode.MetaTableName;
                info.ObjectID = _dataID.ToString();
                info.SourceType = EnumDataFileSourceType.DataUnit;
                //info.ServerID = _server.ServerParameter.ID;
                info.ServerID = _setting.ServerID;
                info.PackagePath = filePath.PackagePath;
                info.FileLocation = serverFile;
                info.DataSize = FileNameUtil.GetFileSize2(filePath.FullFileName)/1024;
                info.EnumStorageType = EnumPathStorageType.EnumDB;
                if (!info.Insert())
                {
                    return false;
                }
            }
            return true;
        }

       

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="dataPathInfo"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        private bool DeleteDataByXml(StorageServer server, DataPathInfo dataPathInfo, ref string errInfo)
        {
            string fileName = Application.StartupPath + "\\temp\\datapath.xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            _dbHelper.ReadBlob2File(fileName,
                                    string.Format("{0} = {1}", DataPathDAL.FLD_NAME_F_OBJECTID,
                                                  dataPathInfo.ObjectID), DataPathDAL.TABLE_NAME,
                                    DataPathDAL.FLD_NAME_F_XML);
            if (!File.Exists(fileName))
            {
                return false;
            }

            XmlInfo xmlInfo = new XmlInfo(fileName, false);
            List<string> pathes = xmlInfo.ReadNodes(@"//root/File");
            foreach (string path in pathes)
            {
                if (!server.DeleteFile(path))
                {
                    errInfo = string.Format("文件【{0}】删除失败。", path);
                    return false;
                }
            }
            return true;
        }
    }


    public class LogEventArgs : EventArgs
    {
        public string Message;
        public enumLogType EnumLogType;

        public LogEventArgs(string message, enumLogType enumLogType)
        {
            Message = message;
            EnumLogType = enumLogType;
        }
    }
}
    

