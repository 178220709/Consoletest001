using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using System.Data;
using System.Xml;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using System.Threading;
using ESRI.ArcGIS.Geometry;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.ADF.MIS.Utility.Core;
using System.Windows.Forms;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.ADF.MIS.Core.Public.security;
using Geoway.Archiver;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using EnumExecuteState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Modeling.Definition;


namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public class UpLoadTask : Task, IUpLoadSettingOld
    {

        #region 字段定义
        private UpLoadTaskData _curUploadTaskData = null;
        private DataDeleteHelper _dataDeleteHelper = null;
        private string _thumbFileName;

        /// <summary>
        /// 拇指图文件路径
        /// </summary>
        public string ThumbFileName
        {
            get { return _thumbFileName; }
            set { _thumbFileName = value; }
        }
        
        private bool _ignoreErrorData = true;
        /// <summary>
        /// 获取或设置是否忽略检查失败的数据
        /// </summary>
        public bool IgnoreErrorData
        {
            get { return _ignoreErrorData; }
            set { _ignoreErrorData = value; }
        }

        private bool _isParentDir = true;
        /// <summary>
        /// 获取或设置是否上级目录
        /// </summary>
        public bool IsParentDir
        {
            get { return _isParentDir; }
            set { _isParentDir = value; }
        }

        private EnumValidateResult _validateResult = EnumValidateResult.None;
        /// <summary>
        /// 获取或设置是否应验证合法性
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private IList<UploadTaskPackage> _taskPackages = null;
        /// <summary>
        /// 获取或设置包含数据包
        /// </summary>
        public IList<UploadTaskPackage> TaskPackages
        {
            get
            {
                if (_taskPackages == null && TaskDataType.ObjectType == EnumObjectType.DataPackageType)
                {
                    _taskPackages = new List<UploadTaskPackage>();
                    IList<TaskPackageDAL> taskPackageDALs = TaskPackageDAL.Singleton.SelectByTaskID(base.ID);
                    if (taskPackageDALs != null && taskPackageDALs.Count > 0)
                    {
                        UploadTaskPackage tmp = null;
                        foreach (TaskPackageDAL dal in taskPackageDALs)
                        {
                            tmp = new UploadTaskPackage(dal, base.StorageServer);
                            _taskPackages.Add(tmp);
                        }
                    }
                }
                return _taskPackages;
            }
            set { _taskPackages = value; }
        }

        private int _catalogID;
        /// <summary>
        /// 获取或设置数据目录ID
        /// </summary>
        public int CatalogID
        {
            get { return _catalogID; }
            set
            {
                _catalogID = value;
                try
                {
                    //原数据表名
                    _metaTableName = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, _catalogID).MetaTableName;
                }
                catch (Exception ex)
                {
                    LogHelper.Error.Append(ex);
                }
            }
        }

        private string _metaTableName;

        public string MetaTableName
        {
            get { return _metaTableName; }
        }

        private int _catalogTreeID = -1;
        /// <summary>
        /// 当前节点上级目录树ID
        /// </summary>
        public int CatalogTreeID
        {
            get
            {
                if (_catalogTreeID < 0)
                {
                    ICatalogNode dal = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, _catalogID);

                    if (dal != null)
                    {
                        _catalogTreeID = dal.CatalogID;//上级目录树ID
                    }
                }
                return _catalogTreeID;
            }
            set { _catalogTreeID = value; }
        }

        private string _prePath;
        /// <summary>
        /// 获取或设置本地数据目录
        /// </summary>
        public string PrePath
        {
            get { return _prePath; }
            set { _prePath = value; }
        }

        private GwDataObject _taskDataType = null;
        /// <summary>
        /// 获取数据类型
        /// </summary>
        public GwDataObject TaskDataType
        {
            get
            {
                if (_taskDataType == null)
                {
                    ICatalogNode dal = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, _catalogID);
                    if (dal != null)
                    {
                        return dal.NodeExInfo.DataType;
                    }
                }
                return _taskDataType;
            }
        }

        private FileUploadSetting _snapshotSetting = null;
        /// <summary>
        /// 快视图入库设置
        /// </summary>
        public FileUploadSetting SnapshotSetting
        {
            get { return _snapshotSetting; }
            set { _snapshotSetting = value; }
        }

        private FileUploadSetting _metadataSetting = null;
        /// <summary>
        /// 元数据入库设置
        /// </summary>
        public FileUploadSetting MetadataSetting
        {
            get { return _metadataSetting; }
            set { _metadataSetting = value; }
        }
        #endregion

        #region 事件定义
        private CheckReplaceDataDelegate _checkReplaceData = null;
        /// <summary>
        /// 验证是否替换已有数据
        /// </summary>
        public event CheckReplaceDataDelegate CheckReplaceData
        {
            add { _checkReplaceData += value; }
            remove { _checkReplaceData -= value; }
        }

        private bool InvokeCheckReplaceData(int taskID, string dataName)
        {
            return _checkReplaceData == null ? true : _checkReplaceData(taskID, dataName);
        }

        #endregion

        #region 重载函数
        /// <summary>
        /// 增加一条记录到TBIMG_TASK表
        /// </summary>
        /// <returns></returns>
        public override bool Add()
        {
            TaskInfoDAL taskInfoDAL = Translate(this);
            if (taskInfoDAL.Insert())
            {
                this._id = taskInfoDAL.TaskID;
                return true;
            }
            return false;
        }

        public override bool Delete()
        {
            TaskInfoDAL taskInfoDAL = Translate(this);
            return taskInfoDAL.Delete();
        }

        public override bool Update()
        {
            TaskInfoDAL taskInfoDAL = Translate(this);
            return taskInfoDAL.Update();
        }

        public override int GetNextID()
        {
            return TaskInfoDAL.singleton.GetNextID(TaskInfoDAL.TABLE_NAME, TaskInfoDAL.FLD_NAME_F_TASKID);
        }

        public override IList<Task> Select()
        {
            IList<Task> result = new List<Task>();
            string filter = string.Empty;
            filter += TaskInfoDAL.FLD_NAME_F_INPUTOROUTPUT + "=" + (int)EnumTaskType.Upload;
            IList<TaskInfoDAL> taskInfoDALs = TaskInfoDAL.singleton.Select();
            foreach (TaskInfoDAL taskInfoDAL in taskInfoDALs)
            {
                result.Add(Translate(taskInfoDAL));

            }
            return result;
        }

        public override Task Select(int id)
        {
            TaskInfoDAL taskInfoDAL = TaskInfoDAL.singleton.Select(id);
            return Translate(taskInfoDAL);
        }

        public override IList<Task> Select(string filter)
        {
            IList<Task> result = new List<Task>();
            if (filter.Trim(' ') != string.Empty)
            {
                filter = "( " + filter + " ) and ";
            }
            filter += TaskInfoDAL.FLD_NAME_F_INPUTOROUTPUT + "=" + (int)EnumTaskType.Upload;
            IList<TaskInfoDAL> taskInfoDALs = TaskInfoDAL.singleton.Translate(TaskInfoDAL.singleton.Select2(filter));
            foreach (TaskInfoDAL taskInfoDAL in taskInfoDALs)
            {
                result.Add(Translate(taskInfoDAL));

            }
            return result;

        }

        public override bool ExportToXML(string XMLPath)
        {
            return false;

        }

        public override bool ExportToText(string TextFilePath)
        {
            return false;
        }

        public override bool ExportToDatatable(out DataTable table)
        {
            table = null;
            return false;

        }

        public override bool WriteLogToSystem(int SubSysID)
        {
            try
            {
                foreach (TaskData pLog in _datas)
                {
                    //LoginControl.WriteToLog(pLog.Log);
                }
                return true;
            }
            catch { return false; }
        }

        public override void Start() 
        {
            isCancel = false;
            this._beginTime = InitPara.DBHelper.getServerDate();
            this.IsTiming = false;
            this.EnumState = EnumExecuteState.Executing;
            int currentCount = 0;
            int totalCount = Datas.Count;
            _curProgressCount = 0;
            bool successed = true;

            try
            {
                Update();//更新任务表：tbimg_task
                InvokeBeginExecute(this._name);

                try
                {
                    string targetCatalogName = string.Empty;
                    if (TaskDataType.ObjectType == EnumObjectType.DataPackage)   //数据单元
                    {
                        foreach (UpLoadTaskData taskdata in Datas)//从TBIMG_TASKDATA表获取此次任务的数据信息
                        {
                            taskdata.CreateThumbFileFullName();//获取拇指图文件路径
                            if (IsCancel)
                            {
                                break;
                            }
                            currentCount++;
                            taskdata.StorageServer = base.StorageServer;//获取存储节点信息
                            successed = DoUploadTaskData(taskdata, targetCatalogName, TaskDataType as DataPackage, 0, "");    //执行数据入库
                            _curProgressCount = (int)(100.0 * currentCount / totalCount);
                            InvokeProgressPosition(_curProgressCount);
                            base.CurProgressCount = _curProgressCount;

                            if (IsCancel)
                            {
                                break;
                            }
                            Update();
                            successed = taskdata.State.IsSuccessed() && successed;
                        }
                    }
                    else//数据包
                    {
                        #region 暂不考虑
                        int newPackageID = 0;
                        foreach (UploadTaskPackage pkg in TaskPackages)
                        {
                            if (IsCancel)
                            {
                                break;
                            }

                            pkg.CatalogID = _catalogID;
                            pkg.ServerID = base.ServerID;
                            pkg.PackageType = TaskDataType as DataPackageType;
                            newPackageID = pkg.WriteRegisterInfo();
                            if (newPackageID > 0)
                            {
                                if (pkg.UploadPublicFiles(_prePath, newPackageID))
                                {
                                    foreach (UpLoadTaskData taskData in pkg.TaskDatas)
                                    {
                                        if (isCancel)
                                        {
                                            break;
                                        }
                                        currentCount++;

                                        taskData.BelongtoTask = this;
                                        taskData.StorageServer = base.StorageServer;
                                        successed = DoUploadTaskData(taskData, targetCatalogName, (TaskDataType as DataPackageType).SubPackage, newPackageID, pkg.Name);    //执行数据入库

                                        _curProgressCount = (int)(100.0 * currentCount / totalCount);
                                        InvokeProgressPosition(_curProgressCount);

                                        base.CurProgressCount = _curProgressCount;

                                        if (IsCancel)
                                        {
                                            break;
                                        }
                                        Update();

                                        successed = taskData.State.IsSuccessed() && successed;
                                    }
                                }
                            }
                        }
#endregion
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error.Append(ex);
                    successed = false;
                }

                this._endTime = DBHelper.GlobalDBHelper.getServerDate();
                this.EnumState = successed == true ? EnumExecuteState.ExecuteSuccessful : EnumExecuteState.ExecuteFailed;
                if (IsCancel)
                {
                    InvokeTaskDataProcessInfo(_curUploadTaskData, "用户暂停任务");
                    InvokeEndExecute(this._name);
                }
                else
                {
                    Update();
                    InvokeEndExecute(this._name);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        public override void Continue()
        {
            InvokeProcessInfo("...");

            if (isPaused)
                isPaused = false;
        }

        public override void Pause()
        {
            InvokeProcessInfo("暂停...");

            isPaused = true;
            while (isPaused)
            {
                Thread.Sleep(100);
                Application.DoEvents();
            }
        }

        public override void Stop()
        {
            InvokeProcessInfo("用户停止任务...");
            IsCancel = true;
            IsPaused = false;

            _curUploadTaskData.StopTransferFile();

            this.EnumState = EnumExecuteState.Executing;
            Update();
        }

        public override IList<TaskData> GetTaskDatas()
        {
            try
            {
                if (_datas == null || _datas.Count <= 0)
                {
                    _datas = new List<TaskData>();
                    TaskData taskData = TaskDataFactory.CreateTaskData(EnumTaskType.Upload);
                    if (TaskDataType.ObjectType == Geoway.ADF.MIS.CatalogDataModel.Public.Definition.EnumObjectType.DataPackage)
                    {
                        _datas = taskData.Select2(_id);
                    }
                    else
                    {
                        foreach (UploadTaskPackage pkg in TaskPackages)
                        {
                            foreach (TaskData d in pkg.TaskDatas)
                            {
                                _datas.Add(d);
                            }
                        }
                    }
                }
                return _datas;
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                return new List<TaskData>();
            }
        }

        #endregion

        #region 公共方法
        public TaskInfoDAL Translate(Task task)
        {
            UpLoadTask upLoadTask = (UpLoadTask)task;
            TaskInfoDAL info = new TaskInfoDAL();

            info.TaskID = upLoadTask.ID;
            info.TaskName = upLoadTask.Name;
            info.TaskDescribe = upLoadTask.Description;
            info.Directory = upLoadTask.Directory;
            //if (server != null)
            //{
            //    info.ServerID = server.ServerParameter.ID;
            //}
            info.ServerID = upLoadTask.ServerID;
            info.UserID = upLoadTask.UserID;
            info.DefineTime = upLoadTask.DefineTime;
            info.BeginTime = upLoadTask.BeginTime;
            info.EndTime = upLoadTask.EndTime;
            info.IsTiming = upLoadTask.IsTiming == true ? 1 : 0;
            info.TaskState = (int)upLoadTask.EnumState;
            info.DataAmount = upLoadTask.DataAmount;
            info.CatalogID = upLoadTask.CatalogID;
            info.PrePath = upLoadTask.PrePath;
            info.Flag = upLoadTask._flag == true ? 1 : 0;
            info.SnapshotSetting = _snapshotSetting;
            info.MetadataSetting = _metadataSetting;
            info.InputOrOutput = (int)EnumTaskType.Upload;
            info.ProgressCount = upLoadTask.CurProgressCount;

            info.IsWithData = upLoadTask.IsUpLoadDataPackage ? 1 : 0;
            info.IsWithMeta = upLoadTask.IsUpLoadMeta ? 1 : 0;
            info.IsHandMeta = upLoadTask.IsHandEditMeta ? 1 : 0;
            info.IsWithSnapShot = upLoadTask.IsUpLoadSnapShot ? 1 : 0;

            info.ValidateResult = upLoadTask.ValidateResult;

            info.ScanMode = upLoadTask.IsParentDir ? EnumScanMode.ChildDirectory : EnumScanMode.CurrentDirectory;

            return info;
        }

        public Task Translate(TaskInfoDAL taskInfoDAL)
        {
            if (taskInfoDAL == null)
                return null;
            UpLoadTask info = new UpLoadTask();

            info.ID = taskInfoDAL.TaskID;
            info.Name = taskInfoDAL.TaskName;
            info.Description = taskInfoDAL.TaskDescribe;
            info.Directory = taskInfoDAL.Directory;
            info.UserID = taskInfoDAL.UserID;
            info.DefineTime = taskInfoDAL.DefineTime;
            info.BeginTime = taskInfoDAL.BeginTime;
            info.EndTime = taskInfoDAL.EndTime;
            info.IsTiming = taskInfoDAL.IsTiming == 0 ? false : true;
            info.EnumState = (EnumExecuteState)taskInfoDAL.TaskState;
            info._serverID = taskInfoDAL.ServerID;
            //info.server = StorageServerFactory.CreateStorageServer(taskInfoDAL.ServerID);

            info.DataAmount = taskInfoDAL.DataAmount;
            info.CatalogID = taskInfoDAL.CatalogID;
            info.PrePath = taskInfoDAL.PrePath;
            info._flag = taskInfoDAL.Flag == 0 ? false : true;

            info.IsUpLoadDataPackage = taskInfoDAL.IsWithData == 1;
            info.IsUpLoadMeta = taskInfoDAL.IsWithMeta == 1;
            info.IsHandEditMeta = taskInfoDAL.IsHandMeta == 1;
            info.IsUpLoadSnapShot = taskInfoDAL.IsWithSnapShot == 1;

            info.SnapshotSetting = taskInfoDAL.SnapshotSetting;
            info.MetadataSetting = taskInfoDAL.MetadataSetting;

            info.CurProgressCount = taskInfoDAL.ProgressCount;

            info.ValidateResult = taskInfoDAL.ValidateResult;

            info.IsParentDir = taskInfoDAL.ScanMode == EnumScanMode.ChildDirectory;

            return info;
        }

        /// <summary>
        /// 判断任务数据中是否包含指定的数据
        /// </summary>
        /// <param name="mainDataFileFullName">指定数据的主数据文件</param>
        /// <returns></returns>
        public bool ContainsData(string mainDataFileFullName)
        {
            foreach (UpLoadTaskData var in Datas)
            {
                if (var.MainDataPath.CompareTo(mainDataFileFullName) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加任务数据
        /// </summary>
        /// <param name="data"></param>
        public void AddTaskData(TaskData data)
        {
            if (_datas == null)
            {
                _datas = new List<TaskData>();
            }
            _datas.Add(data);
        }

        #endregion

        #region 私有方法
        private bool DoUploadTaskData(UpLoadTaskData taskdata, string targetCatalogName, DataPackage package, int dataPackageID, string dataPackageFolderName)
        {
            _curUploadTaskData = taskdata;
            bool successed = true;
            InvokeBeginUploadData(taskdata, "");
            if (IsFinished(taskdata))
            {
                InvokeTaskDataProcessInfo(taskdata, "任务数据“" + taskdata.TaskDataName + "”已成功入库");
            }
            else
            {
                if (_ignoreErrorData && taskdata.ValidateResult == EnumValidateResult.Faild)
                {
                    InvokeTaskDataProcessInfo(taskdata, "任务数据“" + taskdata.TaskDataName + "”不合格，取消入库");
                    return true;
                }
                bool isReplace = false;
                BeforeExecuteData(taskdata);
                try
                {
                    int metaID = -1;
                    taskdata.Package = package;
                    taskdata.CreateTaskFileListFromDB();
                    if (IsCancel)
                    {
                        return false;
                    }
                    #region 重名数据处理
                    string curPrePath = _prePath;
                    IList<IMetaDataEdit> lstMetaData = new List<IMetaDataEdit>();
                    if (!string.IsNullOrEmpty(dataPackageFolderName))
                    {
                        curPrePath = curPrePath.TrimEnd('/') + "/" + dataPackageFolderName;
                    }
                    string mainFileSvrPath=  DataInstanceHelper.GetServerFilePath(StorageServer.ServerParameter.FtpPath,
                                                                            curPrePath,taskdata.PacPath,taskdata.MainDataPath);
                        
                    if (taskdata.DataId <= 0)
                    {
                        //根据路径获取已有文件列表
                        lstMetaData = DataPathInfoDAL.Singleton.GetMetaDataByPath(DBHelper.GlobalDBHelper, mainFileSvrPath);
                        if (lstMetaData.Count > 0)
                        {
                            //询问是否替换已有文件
                            if (InvokeCheckReplaceData(_id, taskdata.TaskDataName))
                            {
                                isReplace = true;
                            }
                            else
                            {
                                InvokeTaskDataProcessInfo(taskdata, "用户取消入库");
                                return true;
                            }
                        }
                    }
                    else
                    {
                        isReplace = true;
                    }
                    if (isReplace)
                    {
                        if (_dataDeleteHelper == null)
                        {
                            _dataDeleteHelper = new DataDeleteHelper();
                        }
                    }
                    #endregion

                    if (IsCancel)
                    {
                        return false;
                    }
                    
                    #region 入库元数据
                    IMetaDataEdit metaDateEdit = null;
                    if (isUpLoadMeta && taskdata.State.MetaState != Definition.EnumDataExecuteState.Successed)
                    {
                        if (isReplace)
                        {
                            //0、替换则删除原元数据
                            _dataDeleteHelper.DeleteMetaData(lstMetaData);
                        }
                        //1、写元数据扩展信息
                        taskdata.WriteMetaDataExtensional();
                        metaID = taskdata.DataID;
                            
                        if (metaID > 0)
                        {
                            taskdata.State.MetaState = Definition.EnumDataExecuteState.Successed;
                            taskdata.State.ExtentState = Definition.EnumDataExecuteState.Successed;
                            //2、更新元数据系统维护信息
                            if ((metaDateEdit=taskdata.WriteMetaDataSys())!=null)
                            {
                                //3、更新元数据固有字段信息
                                taskdata.DataSize = this.DataAmount;
                                taskdata.DataUnit = "B";
                                taskdata.DataTypeName = this.TaskDataType.Name;
                                taskdata.ServerId = this.ServerID;
                                taskdata.Location = curPrePath;
                                taskdata.DataID = metaID;
                                if(taskdata.WriteMetaDataFixed()==null)
                                {
                                    taskdata.State.MetaState = Definition.EnumDataExecuteState.Failed;
                                }
                            }
                            else
                            {
                                taskdata.State.MetaState = Definition.EnumDataExecuteState.Failed;
                            }
                        }
                        else
                        {
                            taskdata.State.MetaState = Definition.EnumDataExecuteState.Failed;
                        }
                    }
                    #endregion
                        
                    if (IsCancel)
                    {
                        return false;
                    }
                        
                    #region 入库数据实体
                    taskdata.DataId = metaID;
                    if (!taskdata.Update())
                    {
                        return false;
                    }
                    
                    
                    if (taskdata.State.ServerState != Definition.EnumDataExecuteState.Successed)
                    {
                        if (!taskdata.UpLoadDataPackageToServer(metaID, curPrePath.TrimEnd('/'), isUpLoadDataPackage))
                        {
                            //上传失败则删除对应的元数据信息
                            InvokeTaskDataProcessInfo(taskdata, "数据文件登记失败");
                            DataOper.DeleteMetaData(DBHelper.GlobalDBHelper, taskdata.DataId);
                            return false;
                        }
                    }
                    #endregion

                    if (IsCancel)
                    {
                        return false;
                    }
                    //更新核心元数据信息
                    if (metaDateEdit != null)
                    {
                        DataOper.UpdateFlag(DBHelper.GlobalDBHelper, metaDateEdit.DataId, 0);
                    }

               

                    #region 维护数据日期、比例尺、分辨率注册信息

                    if (isUpLoadMeta && taskdata.State.MetaState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Failed)
                    {

                        //IWriteImageInfo pWriteImageInfo = taskdata as IWriteImageInfo;
                        //if (pWriteImageInfo != null)
                        //{
                        //    pWriteImageInfo.WriteImageRegisteInfo(registerID);
                        //    foreach (string error in pWriteImageInfo.GetImageInfoSaveErrors)
                        //    {
                        //        InvokeTaskDataProcessInfo(taskdata, "错误:" + error);
                        //    }

                        //    foreach (string waring in pWriteImageInfo.GetImageInfoSaveWarnings)
                        //    {
                        //        InvokeTaskDataProcessInfo(taskdata, "警告:" + waring);
                        //    }
                        //}
                        //else
                        //{
                        //    InvokeTaskDataProcessInfo(taskdata, "维护数据日期、比例尺、分辨率信息失败：其他原因");
                        //}
                    }

                    #endregion

                    if (IsCancel)
                    {
                        return false;
                    }

                    #region 入库快视图
                    if (isUpLoadSnapShot && taskdata.State.SnapShotState != Definition.EnumDataExecuteState.Successed)
                    {
                        int snapshotID = taskdata.UpLoadSnapShotToServer();
                        if (snapshotID > 0)
                        {
                            taskdata.State.SnapShotState = Definition.EnumDataExecuteState.Successed;
                            taskdata.State.ThumbImageState = Definition.EnumDataExecuteState.Successed;
                        }
                        else
                        {
                            taskdata.State.SnapShotState = Definition.EnumDataExecuteState.Failed;
                            taskdata.State.ThumbImageState = Definition.EnumDataExecuteState.Failed;
                        }
                    }
                    #endregion

                    if (IsCancel)
                    {
                        return false;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogHelper.Error.Append(ex);
                }
                finally
                {
                    taskdata.Update();
                }
                AfterExecuteData(taskdata);
            }
            InvokeEndUploadData(taskdata, taskdata.Log.ToString());
            return successed;
        }

        /// <summary>
        /// 任务数据是否已完成
        /// </summary>
        /// <param name="taskdata"></param>
        /// <returns></returns>
        private bool IsFinished(UpLoadTaskData taskdata)
        {
            return !(isUpLoadDataPackage && taskdata.State.ServerState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed ||
                isUpLoadSnapShot && taskdata.State.SnapShotState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed ||
                isUpLoadMeta && taskdata.State.MetaState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed ||
                taskdata.State.ExtentState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed ||
                taskdata.State.ThumbImageState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed);
        }

        public void AddLogs(string logInfo)
        {
            if (this.logInfo == null)
            {
                this.logInfo = new StringBuilder();
            }
            this.logInfo.AppendLine(logInfo);
        }


        #region IUpLoadSetting 成员

        private bool isUpLoadDataPackage = true;
        public bool IsUpLoadDataPackage
        {
            get
            {
                return isUpLoadDataPackage;
            }
            set
            {
                isUpLoadDataPackage = value;
            }
        }

        private bool isUpLoadMeta = true;
        public bool IsUpLoadMeta
        {
            get
            {
                return isUpLoadMeta;
            }
            set
            {
                isUpLoadMeta = value;
            }
        }
        private bool isHandEditMeta = false;
        public bool IsHandEditMeta
        {
            get
            {
                return isHandEditMeta;
            }
            set
            {
                isHandEditMeta = value;
            }
        }
        private bool isUpLoadSnapShot = true;
        public bool IsUpLoadSnapShot
        {
            get
            {
                return isUpLoadSnapShot;
            }
            set
            {
                isUpLoadSnapShot = value;
            }
        }

        #endregion

        #region 任务回滚

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="isOnlyBadData">是否只回滚错误数据</param>
        /// <returns></returns>
        public bool Rollback(bool isOnlyBadData)
        {
            bool succeed = true;
            bool tmpSucceed = false;

            int total = 0;
            int failed = 0;

            IList<TaskData> datas = base.Datas;
            //int dataCount = datas.Count;
            //int i = 1;
            MetaDataSysOS coreMetaDataInfoDAL = null;
            UpLoadTaskData taskData = null;
            foreach (TaskData var in datas)
            {
                taskData = var as UpLoadTaskData;
                if (taskData.DataId > 0)
                {
                    if ((isOnlyBadData && !taskData.State.IsSuccessed()) || !isOnlyBadData)
                    {
                        total++;
                        taskData = var as UpLoadTaskData;
                        taskData.BeginRollbackTaskData += BeginRollbackTaskData;
                        taskData.EndRollbackTaskData += EndRollbackTaskData;
                        taskData.RollbackTaskDataProgress += RollbackTaskDataProgress;

                        try
                        {
                            tmpSucceed = taskData.Rollback();
                        }
                        catch (Exception exp)
                        {
                            LogHelper.Error.Append(exp);
                            tmpSucceed = false;
                        }
                        if (!tmpSucceed)
                        {
                            failed++;
                        }
                        succeed = succeed && tmpSucceed;
                    }
                }
                //base.CurProgressCount = (i++ * 100) / dataCount;
                //Update();
            }
            InvokeRollbackTaskFinish(total, failed);
            return succeed;
        }

        public event BeginRollbackTaskDataEventHandler BeginRollbackTaskData;

        public event EndRollbackTaskDataEventHandler EndRollbackTaskData;

        public event RollbackTaskDataProgressEventHandler RollbackTaskDataProgress;

        public event RollbackTaskFinishEventHandler RollbackTaskFinish;
        private void InvokeRollbackTaskFinish(int totalCount, int failedCount)
        {
            if (this.RollbackTaskFinish != null)
            {
                this.RollbackTaskFinish(this, totalCount, failedCount);
            }
        }

        #endregion
    }
}
