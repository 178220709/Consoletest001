using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geometry;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.Core.Public.security;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.DataModel.Public;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.ADF.MIS.DataModel;
using Geoway.Archiver.Utility.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Path = System.IO.Path;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.UpLoad;
using Geoway.Archiver.Modeling.Definition;
using System.Globalization;
using Geoway.ADF.GIS.GeoDB;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    /// <summary>
    /// 入库任务数据类
    /// </summary>
    public class UpLoadTaskData : TaskData, IUploadDataPackage,IUploadSnapShot,IWriteImageInfo,IRollback,IWriteMetaData
    {
        private int _dataID;
        /// <summary>
        /// 
        /// </summary>
        public int DataID
        {
            get { return _dataID; }
            set { _dataID = value; }
        }
        
        private StorageServer _server;
        /// <summary>
        /// 存储服务器
        /// </summary>
        public StorageServer StorageServer
        {
            get { return _server; }
            set { _server = value; }
        }

        public UpLoadTaskData()
        {
            base._state = new DataExecuteState();
            base._log = new StringBuilder();
        }

        #region 重载函数
        public override bool Add()
        {
            TaskDataDAL taskDataDAL = Translate(this);
            if (taskDataDAL.Insert())
            {
                base.TaskDataID = taskDataDAL.TaskDataID;
                return true;
            }
            return false;
        }

        public override bool Delete()
        {
            TaskDataDAL taskDataDAL = Translate(this);
            return taskDataDAL.Delete();
        }

        public override bool Update()
        {
            TaskDataDAL taskDataDAL = Translate(this);
            return taskDataDAL.Update();
        }

        public override IList<TaskData> Select()
        {
            IList<TaskData> result = new List<TaskData>();
            IList<TaskDataDAL> taskDataDALs = TaskDataDAL.Singleton.Select();

            foreach (TaskDataDAL taskDataDAl in taskDataDALs)
            {
                result.Add(Translate(taskDataDAl));
            }
            return result;
        }

        public override TaskData Select(int taskDataID)
        {
            TaskDataDAL taskDataDAL = TaskDataDAL.Singleton.Select(taskDataID);
            return Translate(taskDataDAL);
        }

        public override IList<TaskData> Select2(int taskID)
        {
            IList<TaskData> result = new List<TaskData>();
            IList<TaskDataDAL> taskDataDALs = TaskDataDAL.Singleton.Select2(taskID);
            
            foreach (TaskDataDAL taskDataDAl in taskDataDALs)
            {
                result.Add(Translate(taskDataDAl));
            }
            return result;
        }

        public override int GetNextID()
        {
            return TaskDataDAL.Singleton.GetNextID(TaskDataDAL.TABLE_NAME, TaskDataDAL.FLD_NAME_F_DATAID);
        }

        public override void StopTransferFile()
        {
            if (base.IsTransferFile)
            {
                base.IsTransferFile = false;
                _server.AbortTransfer();
            }
        }

        #endregion

        #region 公共函数
        public static TaskData Translate(TaskDataDAL taskDataDAL)
        {
            UpLoadTaskData taskData = new UpLoadTaskData();
            taskData.TaskDataID = taskDataDAL.TaskDataID;
            taskData.TaskDataName = taskDataDAL.TaskDataName;
            taskData.TaskID = taskDataDAL.TaskID;
            taskData.State = taskDataDAL.State;
            taskData.Size = taskDataDAL.DataSize;
            taskData.PacPath = taskDataDAL.PacPath;
            taskData.DataId = taskDataDAL.ObjectID;
            //taskData.PackageID = taskDataDAL.PackageID;
            taskData.ValidateInfo = taskDataDAL.ValidateInfo;
            taskData.ValidateResult = taskDataDAL.ValidateResult;
            taskData.TableName = taskDataDAL.RegisterLayerName;
            //taskData.MetaFieldcontent = taskDataDAL.MetaFieldContent;
            taskData.SnapshotFullName = taskDataDAL.SnapshotFileFullName;
            taskData.MetadataFullName = taskDataDAL.MetadataFileFullName;
            taskData.MetaTableName = taskDataDAL.MetaTableName;
            taskData.IsUploadSnapshotFile = taskDataDAL.IsUploadSnapshotFile;
            taskData.IsUploadMetadataFile = taskDataDAL.IsUploadMetadataFile;
            return taskData;
        }

        public static TaskDataDAL Translate(TaskData taskData)
        {
            UpLoadTaskData upLoadTaskData = (UpLoadTaskData)taskData;

            TaskDataDAL taskDataDAL = new TaskDataDAL();
            taskDataDAL.TaskDataID = upLoadTaskData.TaskDataID;
            taskDataDAL.TaskDataName = upLoadTaskData.TaskDataName;
            taskDataDAL.DataSize = upLoadTaskData.Size;
            taskDataDAL.TaskID = upLoadTaskData.TaskID;
            taskDataDAL.DataSize = upLoadTaskData.Size;
            taskDataDAL.InOrOut = (int)EnumTaskType.Upload;
            taskDataDAL.State = upLoadTaskData.State;
            taskDataDAL.ObjectID = upLoadTaskData.DataId;
            taskDataDAL.PacPath = upLoadTaskData.PacPath;
            //taskDataDAL.PackageID = upLoadTaskData.PackageID;
            taskDataDAL.ValidateInfo = upLoadTaskData.ValidateInfo;
            taskDataDAL.ValidateResult = upLoadTaskData.ValidateResult;
            taskDataDAL.RegisterLayerName = upLoadTaskData.TableName;
            //taskDataDAL.MetaFieldContent = upLoadTaskData.MetaFieldcontent;
            taskDataDAL.SnapshotFileFullName = upLoadTaskData.SnapshotFullName;
            taskDataDAL.MetadataFileFullName = upLoadTaskData.MetadataFullName;
            //zqq+
            taskDataDAL.MetaTableName = upLoadTaskData.MetaTableName;
            //
            taskDataDAL.IsUploadSnapshotFile = upLoadTaskData.IsUploadSnapshotFile;
            taskDataDAL.IsUploadMetadataFile = upLoadTaskData.IsUploadMetadataFile;
            
            return taskDataDAL;
        }



        #endregion

        #region  私有函数

        private void BeforeUploadPackage()
        {
            _server.BeginPut += server_BeginGet;
            _server.Progress += server_Progress;
            _server.EndPut += server_EndGet;
        }

        private void AfterUploadPackage()
        {
            _server.BeginPut -= server_BeginGet;
            _server.Progress -= server_Progress;
            _server.EndPut -= server_EndGet;
        }

        private void server_Progress(object o, ServerProgressEventArgs e)
        {
            InvokeProgress(this, e);
        }

        private void server_EndGet(object o, ServerFileEventArgs e)
        {
            InvokeEndGet(this, e);
        }

        private void server_BeginGet(object o, ServerFileEventArgs e)
        {
            InvokeBeginGet(this, e);
        }

        #endregion

        #region TaskFile操作

        private IList<TaskFileDAL> _taskFiles = null;

        private string _realPkgPath = string.Empty;
        /// <summary>
        /// 对应于当前任务数据的数据包路径
        /// </summary>
        public string RealPackagePath
        {
            get { return _realPkgPath; }
            set { _realPkgPath = value; }
        }

        private DataPackage _package = null;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataPackage Package
        {
            get { return _package; }
            set { _package = value; }
        }

        string _mainDataPath = string.Empty;
        /// <summary>
        /// 主数据文件路径
        /// </summary>
        public string MainDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_mainDataPath))
                {
                    foreach (TaskFileDAL file in _taskFiles)
                    {
                        if (file.FileCustom.CompareTo(FileAttribute.FILE_MAIN) == 0)
                        {
                            _mainDataPath = file.FileLocation;
                            break;
                        }
                    }
                }
                return _mainDataPath;
            }
            set { _mainDataPath = value; }
        }

        /// <summary>
        /// 从文件系统创建任务文件列表
        /// </summary>
        public void CreateTaskFileListFromFS(FileUploadSetting snapshotFileSetting, FileUploadSetting metadataFileSetting)
        {
            CreateSnapshotFileFullName(snapshotFileSetting);
            CreateMetadataFileFullName(metadataFileSetting);
            CreateThumbFileFullName();

            if (_taskFiles == null)
            {
                _taskFiles = new List<TaskFileDAL>();
                //DataPackage tmpPackage = _package.Clone() as DataPackage;
                //tmpPackage.ParentObject = null;
                if (!string.IsNullOrEmpty(_mainDataPath))
                {
                    DataFile dataFile = _package.GetMainDataFile();
                    TaskFileDAL taskFile = CreateTaskFile(_mainDataPath,
                                                          dataFile,
                                                          FileAttribute.FILE_MAIN,
                                                          EnumDataFileSourceType.DataUnit);
                    _taskFiles.Add(taskFile);
                    _size += taskFile.DataSize;
                }
                //元数据
                if (_isUploadMetadataFile)
                {
                    if (!string.IsNullOrEmpty(_metadataFullName))
                    {
                        IList<DataFile> dataFiles = _package.GetSpecificDataFile("Metadata");
                        DataFile metadataFileObj = null;
                        if (dataFiles.Count > 0)
                        {
                            metadataFileObj = dataFiles[0];
                        }

                        if (metadataFileObj != null && !metadataFileObj.IsMainDataFile)
                        {
                            TaskFileDAL taskFile = CreateTaskFile(_metadataFullName,
                                                                  metadataFileObj,
                                                                  FileAttribute.METADATA,
                                                                  EnumDataFileSourceType.DataUnit);
                            if (!_taskFiles.Contains(taskFile))
                            {
                                _taskFiles.Add(taskFile);
                                _size += taskFile.DataSize;
                            }
                        }
                    }
                }
                //快视图
                if (_isUploadSnapshotFile)
                {

                    if (!string.IsNullOrEmpty(_snapshotFullName))
                    {
                        IList<DataFile> dataFiles = _package.GetSpecificDataFile("Snapshot");
                        DataFile metadataFileObj = null;
                        if (dataFiles.Count > 0)
                        {
                            metadataFileObj = dataFiles[0];
                        }

                        if (metadataFileObj != null && !metadataFileObj.IsMainDataFile)
                        {
                            TaskFileDAL taskFile = CreateTaskFile(_snapshotFullName,
                                                                  metadataFileObj,
                                                                  FileAttribute.SNAPSHOT,
                                                                  EnumDataFileSourceType.DataUnit);
                            if (!_taskFiles.Contains(taskFile))
                            {
                                _taskFiles.Add(taskFile);
                                _size += taskFile.DataSize;
                            }
                        }
                    }
                }

                //剩余文件
                _size += DataInstanceHelper.GetDataFileByProperty(_taskdataID,
                                                                  _package,
                                                                  _realPkgPath,
                                                                  EnumDataFileProperty.NormalFile,
                                                                  _mainDataPath,
                                                                  _taskFiles,
                                                                  EnumDataFileSourceType.DataUnit);
                
                #endregion
            }
        }

        /// <summary>
        /// 创建TaskFile
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileObj"></param>
        /// <param name="custom"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private TaskFileDAL CreateTaskFile(string filePath, DataFile fileObj,
                                           string custom, EnumDataFileSourceType sourceType)
        {
            TaskFileDAL taskFile = new TaskFileDAL();
            //qfc
            if (fileObj.IsMainDataFile)
            {
                taskFile.FileAttribute = 1;
            }
            else
            {
                switch (fileObj.Properties[0])
                {
                    case FileAttribute.FILE_MAIN:
                        taskFile.FileAttribute = 1;
                        break;
                    case FileAttribute.SNAPSHOT:
                        taskFile.FileAttribute = 2;
                        break;
                    case FileAttribute.METADATA:
                        taskFile.FileAttribute = 3;
                        break;
                    case FileAttribute.MZTFILE:
                        taskFile.FileAttribute = 4;
                        break;
                    case FileAttribute.INDEXFILE:
                        taskFile.FileAttribute = 5;
                        break;
                    default:
                        taskFile.FileAttribute = 0;
                        break;
                }
            }
            taskFile.FileLocation = filePath;
            taskFile.FileCustom = custom;
            taskFile.DataSize = FileNameUtil.GetFileSize2(filePath);
            taskFile.PackagePath = fileObj.GetXPath();
            taskFile.TaskDataID = _taskdataID;
            taskFile.SourceType = sourceType;
            return taskFile;
        }

        /// <summary>
        /// 从数据库创建任务文件列表
        /// </summary>
        public void CreateTaskFileListFromDB()
        {
            if (_taskFiles == null)
            {
                _taskFiles = TaskFileDAL.Singleton.SelectByTaksDataID(_taskdataID);
            }
        }

        /// <summary>
        /// 获取RealPakcageName
        /// </summary>
        /// <returns></returns>
        public string GetRealPackageName()
        {
            int index;
            int length;
            string realPath = string.Empty;
            string tempPath = Path.GetDirectoryName(MainDataPath);
            GwDataObject dataObj = _package.GetMainDataFile().ParentObject;
            while (dataObj != null)
            {
                index = tempPath.LastIndexOf('\\');
                length = tempPath.Length;
                if (dataObj.ObjectType == EnumObjectType.DataPackage)
                {
                    realPath = tempPath.Substring(index + 1, length - index - 1) + "/" + realPath;
                }
                if (index < 0)
                {
                    break;
                }
                tempPath = tempPath.Substring(0, index);
                dataObj = dataObj.ParentObject;
            }
            return realPath.Trim(new char[] { '/', '\\' });
        }

        /// <summary>
        /// 保存任务数据文件
        /// </summary>
        /// <returns></returns>
        public bool SaveTaskFile()
        {
            if (_taskFiles != null && _taskFiles.Count > 0)
            {
                foreach (TaskFileDAL file in _taskFiles)
                {
                    if (!file.Insert())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 验证数据文件是否存在
        /// </summary>
        /// <returns></returns>
        public string ValidateTaskFile()
        {
            if (_taskFiles == null)
            {
                CreateTaskFileListFromDB();
            }
            StringBuilder result = new StringBuilder();
            foreach (TaskFileDAL file in _taskFiles)
            {
                if (!File.Exists(file.FileLocation))
                {
                    result.Append("文件");
                    result.Append(file.FileLocation);
                    result.Append("不存在");
                    result.Append("\r\n");
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        /// <summary>
        /// 是否已经包含指定文件（比较文件地址）
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool ContainsFile(TaskFileDAL file)
        {
            if (_taskFiles == null)
            {
                throw new Exception("_taskFiles is null");
            }
            else
            {
                foreach (TaskFileDAL var in _taskFiles)
                {
                    if (var.FileLocation.CompareTo(file.FileLocation) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        #region 验证

        private EnumValidateResult _validateResult = EnumValidateResult.None;
        /// <summary>
        /// 是否已验证合法性
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private TaskDataValidateInfo _validateInfo = new TaskDataValidateInfo();
        /// <summary>
        /// 验证结果
        /// </summary>
        public TaskDataValidateInfo ValidateInfo
        {
            get { return _validateInfo; }
            set { _validateInfo = value; }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取具体文件的文件名（用于快视图和元数据文件）
        /// </summary>
        /// <param name="fileSetting">文件上传设置</param>
        /// <param name="dataFile">DataFile对象</param>
        /// <returns></returns>
        private string GetSpecificFileFullName(FileUploadSetting fileSetting, DataFile dataFile, List<string> files)
        {
            string fileName = string.Empty;


            if (fileSetting != null && fileSetting.UseOutsideFile)
            {
                if (files == null)
                {
                    files = new List<string>(Directory.GetFiles(fileSetting.OutsidePath, "*." + fileSetting.FileFormat.TrimStart('.')));
                }
                //if (dataFile == null)
                //{
                //foreach (string file in files)
                //{
                //    //TODO:需要改进的话，在此处增加命名规则限制

                //    if (System.IO.Path.GetFileNameWithoutExtension(file).ToUpper().CompareTo(base.Name.ToUpper()) == 0)
                //    {
                //        fileName = file;
                //        files.Remove(file);
                //        break;
                //    }
                //}
                //}
                //else
                //{
                foreach (string file in files)
                {
                    if (DataInstanceHelper.CanAdd(dataFile, file, _realPkgPath, _mainDataPath))
                    {
                        fileName = file;
                        files.Remove(file);
                        break;
                    }
                }
                //}
            }
            else if (fileSetting == null || !fileSetting.UseOutsideFile)
            {
                if (dataFile != null)
                {
                    IList<TaskFileDAL> tempFiles = new List<TaskFileDAL>();
                    EnumDataFileProperty enumDataFileProperty;

                    switch (dataFile.Properties[0])
                    {
                        case "Metadata":
                            enumDataFileProperty = EnumDataFileProperty.MetadataFile;
                            break;
                        case "Snapshot":
                            enumDataFileProperty = EnumDataFileProperty.QuickImageFile;
                            break;
                        case "MZTFile":
                            enumDataFileProperty = EnumDataFileProperty.MZTFILE;
                            break;
                        default:
                            enumDataFileProperty = new EnumDataFileProperty();
                            break;
                    }



                    // qfc
                    DataInstanceHelper.GetDataFileByProperty(_taskdataID,
                                                             _package,
                                                             _realPkgPath,
                                                             enumDataFileProperty,
                                                             _mainDataPath,
                                                             tempFiles,
                                                             EnumDataFileSourceType.DataUnit);


                    if (tempFiles.Count > 0)
                    {
                        fileName = tempFiles[0].FileLocation;
                    }
                }
            }
            return fileName;
        }

        #endregion

        #region IUploadDataPackage 成员
        private DataInstance _dataInstance;
        public DataInstance DataInstance
        {
            get
            {
                if (_dataInstance == null)
                {
                    _dataInstance = DataInstance.Select(DBHelper.GlobalDBHelper, _taskdataID);
                }
                return _dataInstance;
            }
        }

        public bool UpLoadDataPackageToServer(int registerID, string serverPath)
        {
            return UpLoadDataPackageToServer(registerID, serverPath, true);
        }

        /// <summary>
        /// 上传数据到服务器
        /// </summary>
        /// <param name="registerID">数据在元数据表中对应F_DATAID</param>
        /// <param name="serverPath"></param>
        /// <param name="isUpLoad"></param>
        /// <returns></returns>
        public bool UpLoadDataPackageToServer(int registerID, string serverPath, bool isUpLoad)
        {
            if (registerID < 0)
            {
                this.State.ServerState = Definition.EnumDataExecuteState.Failed;
                Update();//更新任务数据表：TBIMG_TASKDATA
                return false;
            }
            if (_server == null)
            {
                _server = base.BelongtoTask.StorageServer;
            }
            if (_server == null || !_server.Connected())
            {
                this.State.ServerState = Definition.EnumDataExecuteState.Failed;
                Update();//更新任务数据表：TBIMG_TASKDATA
                InvokeTaskDataProcessInfo(this, "入库数据文件失败，原因：连接存储服务器失败");
                return false;
            }

            if (isUpLoad)
            {
                InvokeTaskDataProcessInfo(this, "入库数据文件...");
            }

            //string msg = "正在上传数据:" + name;
            //InvokeBeginUploadData(id, msg);
            BeforeUploadPackage();

            //if (_server == null)
            //{
            //    this.State.ServerState = Definition.EnumDataExecuteState.Failed;
            //    return false;
            //}

            bool succeed = true;
            bool tmpSucceed;
            DataPathInfoDAL dataPathDAL;
            string serverFile;

            base.IsTransferFile = true;
            foreach (TaskFileDAL file in _taskFiles)
            {
                if (!base.IsTransferFile)
                {
                    succeed = false;
                    break;
                }

                if (isUpLoad)
                {
                    serverFile = DataInstanceHelper.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                      serverPath,
                                                                      _pacPath,
                                                                      file.FileLocation);
                    try
                    {
                        //上传实体数据
                        tmpSucceed = _server.PutSingleFile(file.FileLocation, serverFile);

                        
                    }
                    catch (Exception exp)
                    {
                        LogHelper.Error.Append(exp);
                        tmpSucceed = false;
                        InvokeTaskDataProcessInfo(this, exp.Message);
                    }
                }
                else
                {

                    serverFile = DataInstanceHelper.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                      serverPath,
                                                                      _pacPath,
                                                                      file.FileLocation);
                    //serverFile = DataInstanceHelper.GetServerFilePath(_server.ServerParameter.FtpPath, file.FileLocation);
                    tmpSucceed = true;
                }
                if (tmpSucceed)
                {
                    //文件记录写入数据库
                    dataPathDAL = new DataPathInfoDAL();
                    dataPathDAL.RegisterLayerName = MetaTableName;
                    dataPathDAL.ObjectID = registerID.ToString();
                    dataPathDAL.PackagePath = file.PackagePath;
                    dataPathDAL.DataSize = file.DataSize;
                    dataPathDAL.FileLocation = serverFile;
                    dataPathDAL.SourceType = EnumDataFileSourceType.DataUnit;
                    dataPathDAL.ServerID = _server.ServerParameter.ID;
                    tmpSucceed = dataPathDAL.Insert();
                }
                succeed = succeed && tmpSucceed;
            }

            if (isUpLoad)
            {
                this.State.ServerState = succeed ? Definition.EnumDataExecuteState.Successed : Definition.EnumDataExecuteState.Failed;
                succeed = Update() && succeed;
            }
            AfterUploadPackage();
            //msg = "上传数据:" + name + "结束";
            //InvokeEndUploadData(id, msg);

            if (isUpLoad)
            {
                InvokeTaskDataProcessInfo(this, "数据文件入库" + (succeed ? "成功" : "失败"));
            }

            return succeed;
        }

        public int WritePackageInfoToRegister(int dataPackageID)
        {
            
            //InvokeTaskDataProcessInfo(this, "写入注册信息...");

            //bool isNewNotUpdate = false;
            //if (_curHeadInfo == null)
            //{
            //    _curHeadInfo = new DataRegisterInfo(base.RegisterLayerName);
            //    isNewNotUpdate = true;
            //}

            //UpLoadTask uploadTask = base.BelongtoTask as UpLoadTask;

            //_curHeadInfo.DataName = this.Name;
            //_curHeadInfo.ImportUser = LoginControl.userName;
            //_curHeadInfo.ImportDate = DBHelper.GlobalDBHelper.getServerDate();
            //_curHeadInfo.ServerID = _server.ServerParameter.ID;//uploadTask.StorageServer.ServerParameter.ID;
            //_curHeadInfo.CatalogID = uploadTask.CatalogID;

            //_curHeadInfo.DataSize = this.Size;
            //_curHeadInfo.Flag = EnumObjectState.Normal;     //状态
            //_curHeadInfo.DataPackageID = dataPackageID;     //数据包
            //_curHeadInfo.CatalogType = uploadTask.CatalogTreeID;      //目录树ID
            //_curHeadInfo.ObjectTypeName = _package.Name;    //数据类型名称
            //_curHeadInfo.RealPackageName = GetRealPackageName();
            ////关键字
            //_curHeadInfo.Keywords = this.Name + ","
            //                      + _curHeadInfo.RealPackageName + ","
            //                      + _curHeadInfo.ObjectTypeName + ","
            //    //+ _curHeadInfo.Year + ","
            //    //+ _curHeadInfo.Scale + ","
            //    //+ _curHeadInfo.Resolution + ","
            //                      + _curHeadInfo.ImportUser;

            //try
            //{

            //    if (isNewNotUpdate == false)
            //    {
            //        if (_curHeadInfo.Update())
            //        {
            //            InvokeTaskDataProcessInfo(this, "写入注册信息成功");
            //            return _curHeadInfo.ID;
            //        }
            //    }
            //    else
            //    {
            //        if (_curHeadInfo.Add())
            //        {
            //            InvokeTaskDataProcessInfo(this, "写入注册信息成功");
            //            return _curHeadInfo.ID;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //   LogHelper.Error.Append(ex);
            //}
            //InvokeTaskDataProcessInfo(this, "写入注册信息失败");
            return 1;
        }

        #endregion

        #region IUploadSnapShot 成员
        private List<string> _snapshotFiles = null; //外部快视图候选文件列表

        private int _snapshotID;
        public int SnapshotID
        {
            get { return _snapshotID; }
            set { _snapshotID = value; }
        }

        private string _snapshotFullName = string.Empty;
        public string SnapshotFullName
        {
            get { return _snapshotFullName; }
            set { _snapshotFullName = value; }
        }

        private string _thumbFullName;
        public string ThumbFullName
        {
            get { return _thumbFullName; }
            set { _thumbFullName = value; }
        }

        private bool _isUploadSnapshotFile = false;
        /// <summary>
        /// 获取或设置是否上传快视图数据文件
        /// </summary>
        public bool IsUploadSnapshotFile
        {
            get { return _isUploadSnapshotFile; }
            set { _isUploadSnapshotFile = value; }
        }

        public int UpLoadSnapShotToServer()
        {
            InvokeTaskDataProcessInfo(this, "入库快视图...");
            int snapshotID = -1;
            if (string.IsNullOrEmpty(_snapshotFullName))
            {
                InvokeTaskDataProcessInfo(this, "未找到快视图文件");
                return -1;
            }
            try
            {
                IRasterDataset raster = RasterDataOperater.OpenRasterDataset(_snapshotFullName);
                IGeoDataset dataset = raster as IGeoDataset;
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
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(raster);
                snapshotID = DataSnapshotOper.ImportSnapshot(_snapshotFullName, _thumbFullName,spatitalReferenceName,_dataID);

                #region Old Code
                //if (string.IsNullOrEmpty(targetCatalogName))
                //{
                //    InvokeTaskDataProcessInfo(this, "入库快视图失败，原因：未指定快视图存储目录");
                //    rasterName = "";
                //    return false;
                //}
                //rasterName = System.IO.Path.GetFileName(_snapShotPath);

                //RasterLoadOperator rasterLoadOperater = new RasterLoadOperator();
                //if (_rasterStorageSetting != null)
                //{
                //    rasterLoadOperater.RasterStorageDef = _rasterStorageSetting.ConvertToStorageDef();
                //}
                //rasterLoadOperater.RasterStorageDef = Geoway.ImageDB.Utility.Class.RasterDataOperater.GetDefaultRasterStorageDef();

                //if (DataAccess.TargetWorkspace != null)
                //{
                //    successed = rasterLoadOperater.AddRasterToCatalogEx(DataAccess.TargetWorkspace, targetCatalogName, rasterName, catalogID, objectID, dataID, _snapShotPath);
                //    //log.Append(rasterLoadOperater.LogMessage.ToString().TrimEnd("\r\n".ToCharArray()));
                //    InvokeTaskDataProcessInfo(this, "入库快视图：" + rasterLoadOperater.LogMessage.ToString().TrimEnd("\r\n".ToCharArray()));
                //    rasterLoadOperater.Clear();

                //for (int i = 0; i < 1; i++)
                //{
                //    byte[] bytes = Geoway.ImageDB.Utility.Class.RasterDataOperater.Raster2Bytes(_snapShotPath);

                //    int id = DBHelper.GlobalDBHelper.GetNextValidID("tbimg_snapshot", "f_id");
                //    string sql = "insert into tbimg_snapshot (f_id) values(" + id + ")";
                //    int rlt = DBHelper.GlobalDBHelper.DoSQL(sql);
                //    DBHelper.GlobalDBHelper.SaveBytes2Blob("f_id=" + id, "tbimg_snapshot", "f_image", ref bytes);
                //}
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
#if DEBUG
                InvokeTaskDataProcessInfo(this, "入库快视图：" + ex.Message);
#endif
                snapshotID = -1;
            }
            finally
            {
                if (snapshotID > 0)
                {
                    InvokeTaskDataProcessInfo(this, "入库快视图成功");
                }
                else
                {
                    InvokeTaskDataProcessInfo(this, "入库快视图失败");
                }
            }
            return snapshotID;
        }

        /// <summary>
        /// 创建快视图文件路径
        /// </summary>
        /// <param name="snapshotFileSetting">快视图文件上传设置</param>
        private void CreateSnapshotFileFullName(FileUploadSetting snapshotFileSetting)
        {
            try
            {
                List<DataFile> lstDataFile = _package.GetSpecificDataFile("Snapshot");
                if(lstDataFile.Count>=1)
                {
                    DataFile dataFile = _package.GetSpecificDataFile("Snapshot")[0];
                    _snapshotFullName = GetSpecificFileFullName(snapshotFileSetting, dataFile, _snapshotFiles);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        /// <summary>
        /// 获取拇指图路径
        /// </summary>
        public void CreateThumbFileFullName()
        {
            //外部调用时赋值
            if (_package == null)
            {
                this._package =
                    CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, (BelongtoTask as UpLoadTask).CatalogID).NodeExInfo.DataType as DataPackage;
                this._mainDataPath =TaskFileDAL.Singleton.Select(this.TaskDataID, EnumDataFileProperty.MainDataFile).FileLocation;
                this._realPkgPath = Path.GetDirectoryName(_mainDataPath);
            }

            try
            {
                List<DataFile> lstDataFile = _package.GetSpecificDataFile("MZTFile");
                if (lstDataFile.Count >= 1)
                {
                    DataFile dataFile = _package.GetSpecificDataFile("MZTFile")[0];
                    _thumbFullName = GetSpecificFileFullName(null, dataFile, null);
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region IWriteImageInfo 成员
        private List<string> errors = null;
        private List<string> warnings = null;

        public bool WriteImageRegisteInfo(int registerID)
        {
            errors = new List<string>();
            warnings = new List<string>();
            bool flg = false;
            //try
            //{

            //    //InvokeTaskDataProcessInfo(this, "影像注册信息...");
            //    if (_curHeadInfo == null)
            //    {
            //        DataRegisterInfo dataRegisterInfo = new DataRegisterInfo(base.RegisterLayerName);
            //        _curHeadInfo = (DataRegisterInfo)dataRegisterInfo.Select(registerID);
            //    }
            //    if (_curHeadInfo == null)
            //    {
            //        errors.Add("未找到指定的注册信息");
            //        flg = false;
            //    }
            //    else
            //    {
            //        // _curHeadInfo.MetaTableName = metaTableName;
            //        // _curHeadInfo.MetaID = metaID;
            //        //if (_curHeadInfo.MetaTableName == null || _curHeadInfo.MetaTableName == string.Empty)
            //        //{
            //        //    //warnings.Add("未上传元数据");
            //        //}
            //        DataTable meta = DBHelper.GlobalDBHelper.DoQueryEx("meta", "SELECT * FROM " + _curHeadInfo.MetaTableName + " WHERE F_OID =" + _curHeadInfo.MetaID, true);
            //        MetaRegisterConfigDAL config = MetaRegisterConfigDAL.Singleton.Select(_curHeadInfo.MetaTableName);

            //        if (meta == null || meta.Rows.Count == 0)
            //        {
            //            errors.Add("未找到相关的元数据");
            //            flg = false;
            //        }
            //        else
            //        {
            //            if (config == null)
            //            {
            //                errors.Add("未配置注册字段");
            //                return false;
            //            }

            //            if (meta.Columns.Contains(config.DataTime) == false)
            //            {
            //                errors.Add("元数据表不包含【日期】信息");
            //            }
            //            else
            //            {
            //                DateTime year_n;
            //                try
            //                {
            //                    year_n = GetSafeDataUtility.ValidateDataRow_T(meta.Rows[0], config.DataTime);
            //                    if (year_n.Equals(DateTime.MinValue))
            //                    {
            //                        warnings.Add("不是有效的日期值");
            //                    }
            //                    _curHeadInfo.DataTime = year_n;
            //                    _curHeadInfo.Year = year_n.Year;
            //                    //_curHeadInfo.Keywords += "," + _curHeadInfo.Year.ToString();
            //                    KeywordIndexHelper.AddKeywordIndex(_curHeadInfo.Year.ToString(), EnumKeywordIndexType.Year);
            //                }
            //                catch
            //                {
            //                    warnings.Add("不是有效的日期值");
            //                }
            //            }

            //            if (meta.Columns.Contains(config.Scale) == false)
            //            {
            //                errors.Add("元数据表不包含【比例尺】信息");
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    string scale_str = GetSafeDataUtility.ValidateDataRow_S(meta.Rows[0], config.Scale);
            //                    if (scale_str == null || scale_str == string.Empty)
            //                    {
            //                        warnings.Add("不是有效的比例尺数值");
            //                    }
            //                    _curHeadInfo.Scale = scale_str;
            //                    //_curHeadInfo.Keywords += "," + scale_str;
            //                    KeywordIndexHelper.AddKeywordIndex(scale_str, EnumKeywordIndexType.Scale);
            //                }
            //                catch
            //                {
            //                    warnings.Add("不是有效的比例尺数值");
            //                }
            //            }

            //            if (meta.Columns.Contains(config.Resulation) == false)
            //            {
            //                errors.Add("元数据表不包含【分辨率】信息");
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    string resolution_str = GetSafeDataUtility.ValidateDataRow_S(meta.Rows[0], config.Resulation);
            //                    if (resolution_str == null || resolution_str == string.Empty)
            //                    {
            //                        warnings.Add("不是有效的分辨率数值");
            //                    }
            //                    _curHeadInfo.Resolution = resolution_str;
            //                    //_curHeadInfo.Keywords += "," + resolution_str;
            //                    KeywordIndexHelper.AddKeywordIndex(resolution_str, EnumKeywordIndexType.Resolution);
            //                }
            //                catch
            //                {
            //                    warnings.Add("不是有效的分辨率数值");
            //                }
            //            }

            //        }

            //        flg = _curHeadInfo.Update();
            //    }
            //}
            //catch (Exception exp)
            //{
            //    LogHelper.Error.Append(exp);
            //}
            return flg;

        }

        public string[] GetImageInfoSaveErrors
        {
            get
            {
                if (errors == null)
                {
                    return null;
                }
                else
                {
                    return errors.ToArray();
                }
            }
        }

        public string[] GetImageInfoSaveWarnings
        {
            get
            {
                if (warnings == null)
                {
                    return null;
                }
                else
                {
                    return warnings.ToArray();
                }
            }
        }

        #endregion

        #region IRollback 成员

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            InvokeBeginRollbackTaskData();
            bool succeed = true;
            //核心元数据信息
            IMetaDataSys coreMetaDate =
                MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper,EnumMetaDataType.EnumSystem,  EnumMetaDatumType.enumDefault,  this.DataId) as IMetaDataSys;
            bool tmpSucceed = false;

            if (coreMetaDate == null)
            {
                this._state.ServerState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                this._state.MetaState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                this._state.SnapShotState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                this._state.ExtentState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                this._state.ThumbImageState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                InvokeRollbackTaskDataProgress(0, 0, "数据已经被删除");
                Update();
            }
            else
            {

                #region 删除实体文件
                if (this._state.ServerState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    tmpSucceed = DeleteDataFiles();
                    if (tmpSucceed)
                    {
                        InvokeRollbackTaskDataProgress(70, 100, "删除文件成功");
                        this._state.ServerState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                    }
                    else
                    {
                        InvokeRollbackTaskDataProgress(70, 100, "删除文件失败");
                    }
                    succeed = succeed && tmpSucceed;
                }
                #endregion

                #region 删除快视图

                if (this._state.SnapShotState !=
                    Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    //InvokeRollbackTaskDataProgress(90, 100, "开始删除快视图");
                    //if (string.IsNullOrEmpty(_curHeadInfo.SnapLayer))
                    //{
                    //    InvokeRollbackTaskDataProgress(95, 100, "快视图未入库");
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        tmpSucceed = dataDeleteHelper.DeleteSnapShot(_curHeadInfo);
                    //        this.state.SnapShotState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                    //    }
                    //    catch (Exception exp)
                    //    {
                    //        tmpSucceed = false;
                    //        LogHelper.Error.Append(exp);
                    //        InvokeRollbackTaskDataProgress(92, 100, "删除快视图出错：" + exp.Message);
                    //    }
                    //    succeed = succeed && tmpSucceed;
                    //    InvokeRollbackTaskDataProgress(95, 100, "删除快视图" + (tmpSucceed ? "成功" : "失败"));
                    //}
                }

                #endregion

                #region 删除元数据

                if (this._state.MetaState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    InvokeRollbackTaskDataProgress(80, 100, "开始删除元数据");

                    try
                    {
                        
                        tmpSucceed = coreMetaDate.Delete();
                        //tmpSucceed = dataDeleteHelper.DeleteMetadata(_curHeadInfo);
                        
                        this._state.MetaState =
                            Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                    }
                    catch (Exception exp)
                    {
                        tmpSucceed = false;
                        LogHelper.Error.Append(exp);
                        InvokeRollbackTaskDataProgress(90, 100, "删除元数据出错：" + exp.Message);
                    }
                    succeed = succeed && tmpSucceed;
                    InvokeRollbackTaskDataProgress(100, 100, "删除元数据" + (tmpSucceed ? "成功" : "失败"));
                }

                #endregion

                #region 删除注册信息、拇指图、空间范围

                //InvokeRollbackTaskDataProgress(97, 100, "开始删除注册信息、拇指图、空间范围");
                //try
                //{
                //tmpSucceed = _curHeadInfo.Delete();
                //_curHeadInfo = null;
                //this.state.ThumbImageState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                //this.state.ExtentState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                //}
                //catch (Exception exp)
                //{
                //    tmpSucceed = false;
                //    LogHelper.Error.Append(exp);
                //    InvokeRollbackTaskDataProgress(98, 100, "删除注册信息、拇指图、空间范围出错：" + exp.Message);
                //}
                //succeed = succeed && tmpSucceed;
                //InvokeRollbackTaskDataProgress(100, 100, "删除注册信息、拇指图、空间范围" + (tmpSucceed ? "成功" : "失败"));
                //Update();

                #endregion
            }
            InvokeEndRollbackTaskData(succeed);
            return succeed;
        }

        /// <summary>
        /// 删除数据文件
        /// </summary>
        /// <param name="regInfo"></param>
        /// <returns></returns>
        private bool DeleteDataFiles(DataRegisterInfo regInfo)
        {
            bool succeed = true;
            bool tmpSucceed = true;
            List<DataPathInfoDAL> allFiles = new List<DataPathInfoDAL>();
            IList<DataPathInfoDAL> lstPath = DataPathInfoDAL.Singleton.SeletByObjectID(regInfo.LayerName, regInfo.ID, EnumDataFileSourceType.DataUnit);
            allFiles.AddRange(lstPath);
            if (regInfo.DataPackageID > 0)
            {
                DataTable dt = DataRegisterInfo.GetDataPackageContent(regInfo.LayerName, regInfo.DataPackageID, true);
                if (dt == null || dt.Rows.Count == 1)
                {
                    lstPath = DataPathInfoDAL.Singleton.SeletByObjectID(regInfo.LayerName, regInfo.DataPackageID, EnumDataFileSourceType.DataPackage);
                    allFiles.AddRange(lstPath);
                }
            }
            if (allFiles.Count <= 0)
            {
                return true;
            }
            if (_server == null)
            {
                _server = base.BelongtoTask.StorageServer;
            }
            string msg = "";
            int step = 90 / lstPath.Count;
            int pos = 0;
            foreach (DataPathInfoDAL path in allFiles)
            {
                msg = "正在删除文件 " + path;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
                try
                {
                    if (DataPathInfoDAL.GetDataPathCountByServerPath(path.FileLocation, _server.ServerParameter.ID) > 1)
                    {
                        path.Delete();
                        msg = "文件" + path + "正被其他数据共享，暂时不能删除";
                    }
                    else
                    {
                        tmpSucceed = _server.DeleteFile(path.FileLocation);
                        succeed = succeed && tmpSucceed;
                        if (tmpSucceed)
                        {
                            path.Delete();
                            msg = "删除文件 " + path + "成功";
                        }
                        else
                        {
                            msg = "删除文件 " + path + "失败";
                        }
                    }
                }
                catch (Exception exp)
                {
                    msg = "删除文件 " + path + "失败，原因" + exp.Message;
                    succeed = false;
                    LogHelper.Error.Append(exp);
                }
                pos += step;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
            }
            if (succeed)
            {
                //qfc
                //DataPackageInfoDAL.Singleton.Delete(regInfo.DataPackageID);
            }
            return succeed;
        }

        /// <summary>
        /// 删除数据文件
        /// </summary>
        /// <returns></returns>
        private bool DeleteDataFiles()
        {
            bool succeed = true;
            bool tmpSucceed = true;

            IList<DataPathInfoDAL> lstPath = DataPathInfoDAL.Singleton.SeletByObjectID(
                this.DataId.ToString(), EnumDataFileSourceType.DataUnit);//获取本条数据对应的文件在服务器上的路径


            if (lstPath.Count <= 0)
            {
                return true;
            }
            if (_server == null)
            {
                _server = base.BelongtoTask.StorageServer;
            }
            string msg = "";
            int step = 60 / lstPath.Count;
            int pos = 0;
            foreach (DataPathInfoDAL path in lstPath)
            {
                msg = "正在删除文件 " + path;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
                try
                {
                    if (DataPathInfoDAL.GetDataPathCountByServerPath(path.FileLocation, _server.ServerParameter.ID) > 1)
                    {
                        path.Delete();
                        msg = "文件" + path + "正被其他数据共享，暂时不能删除";
                    }
                    else
                    {
                        tmpSucceed = _server.DeleteFile(path.FileLocation);
                        succeed = succeed && tmpSucceed;
                        if (tmpSucceed)
                        {
                            path.Delete();
                            msg = "删除文件 " + path + "成功";
                        }
                        else
                        {
                            msg = "删除文件 " + path + "失败";
                        }
                    }
                }
                catch (Exception exp)
                {
                    msg = "删除文件 " + path + "失败，原因" + exp.Message;
                    succeed = false;
                    LogHelper.Error.Append(exp);
                }
                pos += step;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
            }
            if (succeed)
            {
                //qfc
                //DataPackageInfoDAL.Singleton.Delete(regInfo.DataPackageID);
            }
            return succeed;
        }

        private void InvokeBeginRollbackTaskData()
        {
            if (this.BeginRollbackTaskData != null)
            {
                this.BeginRollbackTaskData(this);
            }
        }
        private void InvokeEndRollbackTaskData(bool succeed)
        {
            if (this.EndRollbackTaskData != null)
            {
                this.EndRollbackTaskData(this, succeed);
            }
        }
        private void InvokeRollbackTaskDataProgress(int position, int max, string msg)
        {
            if (this.RollbackTaskDataProgress != null)
            {
                this.RollbackTaskDataProgress(this, position, max, msg);
            }
        }

        public event BeginRollbackTaskDataEventHandler BeginRollbackTaskData;

        public event EndRollbackTaskDataEventHandler EndRollbackTaskData;

        public event RollbackTaskDataProgressEventHandler RollbackTaskDataProgress;

        #endregion

        #region IWriteMetaData成员
        private List<string> _metadataFiles = null; //外部元数据候选文件列表
        string _metadataFullName = string.Empty;
        /// <summary>
        /// 元数据文件名
        /// </summary>
        public string MetadataFullName
        {
            get
            {
                return _metadataFullName;
            }
            set
            {
                _metadataFullName = value;
            }
        }

        private bool _isUploadMetadataFile = false;
        /// <summary>
        /// 获取或设置是否上传元数据文件
        /// </summary>
        public bool IsUploadMetadataFile
        {
            get { return _isUploadMetadataFile; }
            set { _isUploadMetadataFile = value; }
        }

        string _metaTableName = string.Empty;
        public string MetaTableName
        {
            get
            {
                return _metaTableName;
            }
            set
            {
                _metaTableName = value;
            }
        }
        
        private IMetaData _metaDataDBOper = null;
        public IMetaDataEdit WriteMetaDataExtensional()
        {
            IMetaDataExtensionalEdit metaDataExtensionalEdit = null;
            IMetaDataExtensionalDZEdit metaDataExtensionalDzEdit = null;
            InvokeTaskDataProcessInfo(this, "元数据入库...");
            bool successed = false;
            try
            {
                if (string.IsNullOrEmpty(_metadataFullName))
                {
                    InvokeTaskDataProcessInfo(this, "指定的位置下未找到元数据文件");
                }
                else
                {
                    //写元数据表扩展属性
                    _metaDataDBOper = MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper,EnumMetaDataType.EnumExtensional);
                    metaDataExtensionalEdit = _metaDataDBOper as IMetaDataExtensionalEdit;
                    metaDataExtensionalEdit.CatalogId = (this.BelongtoTask as UpLoadTask).CatalogID;
                    metaDataExtensionalEdit.TableName = _metaTableName;
                    metaDataExtensionalEdit.EnumMetaDatumType=EnumMetaDatumType.enumSTELEDatum;
                    
                    metaDataExtensionalDzEdit = metaDataExtensionalEdit as IMetaDataExtensionalDZEdit;
                    metaDataExtensionalDzEdit.MetaFilePath = _metadataFullName;
                    
                    _metaDataDBOper = metaDataExtensionalEdit as IMetaData;
                    successed = _metaDataDBOper.Insert();
                    _dataID = metaDataExtensionalEdit.DataId;
                    if (!successed)
                    {
                        string msg = string.Format("元数据记录写入数据库失败！");
                        InvokeTaskDataProcessInfo(this, "元数据记录写入数据库失败");
                        metaDataExtensionalEdit = null;
                    }

                }
            }
            catch (Exception ex)
            {
                successed = false;
#if DEBUG
                InvokeTaskDataProcessInfo(this, "元数据入库出错：" + ex.Message);
#endif
                LogHelper.Error.Append(ex);
                _dataID = -1;
                metaDataExtensionalEdit = null;
            }
            finally
            {
                if (successed)
                {
                    InvokeTaskDataProcessInfo(this, "元数据入库成功");
                }
                else
                {
                    InvokeTaskDataProcessInfo(this, "元数据入库失败");
                    metaDataExtensionalEdit = null;
                }
            }
            return metaDataExtensionalEdit;
        }

        public IMetaDataEdit WriteMetaDataSys()
        {
            IMetaDataSysEdit coreMetaDateEdit = null;
            try
            {
                IMetaData coreMetaDate =
                    MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper,EnumMetaDataType.EnumSystem,EnumMetaDatumType.enumDefault, _dataID);
                
                //更新核心元数据信息
                if (coreMetaDate != null)
                {
                    coreMetaDateEdit = coreMetaDate as IMetaDataSysEdit;
                    coreMetaDateEdit.Name = this.TaskDataName;
                    coreMetaDateEdit.ImportDate = DateTime.Now;
                    coreMetaDateEdit.ImportUser = LoginControl.userName;
                    coreMetaDateEdit.Flag = -1;
                    coreMetaDate.Update();
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return coreMetaDateEdit;
        }

        public IMetaDataEdit WriteMetaDataFixed()
        {
            bool bSuccess = false;
            IMetaDataFixedDZEdit metadataFixeddzEdit = null;
            IMetaDataFixedDZ metaDataFixedDZ = null;
            try
            {

                metaDataFixedDZ = MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper, EnumMetaDataType.EnumFixed,EnumMetaDatumType.enumSTELEDatum, _dataID) as IMetaDataFixedDZ;
                metadataFixeddzEdit = metaDataFixedDZ as IMetaDataFixedDZEdit;
                metadataFixeddzEdit.DataSize = base.DataSize;
                metadataFixeddzEdit.DataTypeName = base.DataTypeName;
                metadataFixeddzEdit.DataUnit = base.DataUnit;
                metadataFixeddzEdit.Location = base.Location;
                metadataFixeddzEdit.ServerId = base.ServerId;
                bSuccess = metaDataFixedDZ.Update();
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return metadataFixeddzEdit;
        }

        /// <summary>
        /// 创建元数据文件路径
        /// </summary>
        /// <param name="metadataFileSetting">元数据文件上传设置</param>
        private void CreateMetadataFileFullName(FileUploadSetting metadataFileSetting)
        {
            try
            {
                _metadataFullName = GetSpecificFileFullName(metadataFileSetting,
                                                            _package.GetSpecificDataFile("Metadata")[0], _metadataFiles);
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex); 
            }
        }
        #endregion
    }

    /// <summary>
    /// 任务数据验证结果
    /// </summary>
    [Serializable]
    public class TaskDataValidateInfo
    {
        public string DataFileInfo = string.Empty;
        public string SanpshotInfo = string.Empty;
        public string MetadataInfo = string.Empty;
        public string ThumbInfo = string.Empty;
        public string ExtentInfo = string.Empty;
    }
}
