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
    /// �������������
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
        /// �洢������
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

        #region ���غ���
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

        #region ��������
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

        #region  ˽�к���

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

        #region TaskFile����

        private IList<TaskFileDAL> _taskFiles = null;

        private string _realPkgPath = string.Empty;
        /// <summary>
        /// ��Ӧ�ڵ�ǰ�������ݵ����ݰ�·��
        /// </summary>
        public string RealPackagePath
        {
            get { return _realPkgPath; }
            set { _realPkgPath = value; }
        }

        private DataPackage _package = null;
        /// <summary>
        /// ��������
        /// </summary>
        public DataPackage Package
        {
            get { return _package; }
            set { _package = value; }
        }

        string _mainDataPath = string.Empty;
        /// <summary>
        /// �������ļ�·��
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
        /// ���ļ�ϵͳ���������ļ��б�
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
                //Ԫ����
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
                //����ͼ
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

                //ʣ���ļ�
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
        /// ����TaskFile
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
        /// �����ݿⴴ�������ļ��б�
        /// </summary>
        public void CreateTaskFileListFromDB()
        {
            if (_taskFiles == null)
            {
                _taskFiles = TaskFileDAL.Singleton.SelectByTaksDataID(_taskdataID);
            }
        }

        /// <summary>
        /// ��ȡRealPakcageName
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
        /// �������������ļ�
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
        /// ��֤�����ļ��Ƿ����
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
                    result.Append("�ļ�");
                    result.Append(file.FileLocation);
                    result.Append("������");
                    result.Append("\r\n");
                }
            }
            return result.ToString().TrimEnd(new char[] { '\r', '\n' });
        }

        /// <summary>
        /// �Ƿ��Ѿ�����ָ���ļ����Ƚ��ļ���ַ��
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


        #region ��֤

        private EnumValidateResult _validateResult = EnumValidateResult.None;
        /// <summary>
        /// �Ƿ�����֤�Ϸ���
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private TaskDataValidateInfo _validateInfo = new TaskDataValidateInfo();
        /// <summary>
        /// ��֤���
        /// </summary>
        public TaskDataValidateInfo ValidateInfo
        {
            get { return _validateInfo; }
            set { _validateInfo = value; }
        }

        #endregion

        #region ˽�з���

        /// <summary>
        /// ��ȡ�����ļ����ļ��������ڿ���ͼ��Ԫ�����ļ���
        /// </summary>
        /// <param name="fileSetting">�ļ��ϴ�����</param>
        /// <param name="dataFile">DataFile����</param>
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
                //    //TODO:��Ҫ�Ľ��Ļ����ڴ˴�����������������

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

        #region IUploadDataPackage ��Ա
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
        /// �ϴ����ݵ�������
        /// </summary>
        /// <param name="registerID">������Ԫ���ݱ��ж�ӦF_DATAID</param>
        /// <param name="serverPath"></param>
        /// <param name="isUpLoad"></param>
        /// <returns></returns>
        public bool UpLoadDataPackageToServer(int registerID, string serverPath, bool isUpLoad)
        {
            if (registerID < 0)
            {
                this.State.ServerState = Definition.EnumDataExecuteState.Failed;
                Update();//�����������ݱ�TBIMG_TASKDATA
                return false;
            }
            if (_server == null)
            {
                _server = base.BelongtoTask.StorageServer;
            }
            if (_server == null || !_server.Connected())
            {
                this.State.ServerState = Definition.EnumDataExecuteState.Failed;
                Update();//�����������ݱ�TBIMG_TASKDATA
                InvokeTaskDataProcessInfo(this, "��������ļ�ʧ�ܣ�ԭ�����Ӵ洢������ʧ��");
                return false;
            }

            if (isUpLoad)
            {
                InvokeTaskDataProcessInfo(this, "��������ļ�...");
            }

            //string msg = "�����ϴ�����:" + name;
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
                        //�ϴ�ʵ������
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
                    //�ļ���¼д�����ݿ�
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
            //msg = "�ϴ�����:" + name + "����";
            //InvokeEndUploadData(id, msg);

            if (isUpLoad)
            {
                InvokeTaskDataProcessInfo(this, "�����ļ����" + (succeed ? "�ɹ�" : "ʧ��"));
            }

            return succeed;
        }

        public int WritePackageInfoToRegister(int dataPackageID)
        {
            
            //InvokeTaskDataProcessInfo(this, "д��ע����Ϣ...");

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
            //_curHeadInfo.Flag = EnumObjectState.Normal;     //״̬
            //_curHeadInfo.DataPackageID = dataPackageID;     //���ݰ�
            //_curHeadInfo.CatalogType = uploadTask.CatalogTreeID;      //Ŀ¼��ID
            //_curHeadInfo.ObjectTypeName = _package.Name;    //������������
            //_curHeadInfo.RealPackageName = GetRealPackageName();
            ////�ؼ���
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
            //            InvokeTaskDataProcessInfo(this, "д��ע����Ϣ�ɹ�");
            //            return _curHeadInfo.ID;
            //        }
            //    }
            //    else
            //    {
            //        if (_curHeadInfo.Add())
            //        {
            //            InvokeTaskDataProcessInfo(this, "д��ע����Ϣ�ɹ�");
            //            return _curHeadInfo.ID;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //   LogHelper.Error.Append(ex);
            //}
            //InvokeTaskDataProcessInfo(this, "д��ע����Ϣʧ��");
            return 1;
        }

        #endregion

        #region IUploadSnapShot ��Ա
        private List<string> _snapshotFiles = null; //�ⲿ����ͼ��ѡ�ļ��б�

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
        /// ��ȡ�������Ƿ��ϴ�����ͼ�����ļ�
        /// </summary>
        public bool IsUploadSnapshotFile
        {
            get { return _isUploadSnapshotFile; }
            set { _isUploadSnapshotFile = value; }
        }

        public int UpLoadSnapShotToServer()
        {
            InvokeTaskDataProcessInfo(this, "������ͼ...");
            int snapshotID = -1;
            if (string.IsNullOrEmpty(_snapshotFullName))
            {
                InvokeTaskDataProcessInfo(this, "δ�ҵ�����ͼ�ļ�");
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
                //    InvokeTaskDataProcessInfo(this, "������ͼʧ�ܣ�ԭ��δָ������ͼ�洢Ŀ¼");
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
                //    InvokeTaskDataProcessInfo(this, "������ͼ��" + rasterLoadOperater.LogMessage.ToString().TrimEnd("\r\n".ToCharArray()));
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
                InvokeTaskDataProcessInfo(this, "������ͼ��" + ex.Message);
#endif
                snapshotID = -1;
            }
            finally
            {
                if (snapshotID > 0)
                {
                    InvokeTaskDataProcessInfo(this, "������ͼ�ɹ�");
                }
                else
                {
                    InvokeTaskDataProcessInfo(this, "������ͼʧ��");
                }
            }
            return snapshotID;
        }

        /// <summary>
        /// ��������ͼ�ļ�·��
        /// </summary>
        /// <param name="snapshotFileSetting">����ͼ�ļ��ϴ�����</param>
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
        /// ��ȡĴָͼ·��
        /// </summary>
        public void CreateThumbFileFullName()
        {
            //�ⲿ����ʱ��ֵ
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

        #region IWriteImageInfo ��Ա
        private List<string> errors = null;
        private List<string> warnings = null;

        public bool WriteImageRegisteInfo(int registerID)
        {
            errors = new List<string>();
            warnings = new List<string>();
            bool flg = false;
            //try
            //{

            //    //InvokeTaskDataProcessInfo(this, "Ӱ��ע����Ϣ...");
            //    if (_curHeadInfo == null)
            //    {
            //        DataRegisterInfo dataRegisterInfo = new DataRegisterInfo(base.RegisterLayerName);
            //        _curHeadInfo = (DataRegisterInfo)dataRegisterInfo.Select(registerID);
            //    }
            //    if (_curHeadInfo == null)
            //    {
            //        errors.Add("δ�ҵ�ָ����ע����Ϣ");
            //        flg = false;
            //    }
            //    else
            //    {
            //        // _curHeadInfo.MetaTableName = metaTableName;
            //        // _curHeadInfo.MetaID = metaID;
            //        //if (_curHeadInfo.MetaTableName == null || _curHeadInfo.MetaTableName == string.Empty)
            //        //{
            //        //    //warnings.Add("δ�ϴ�Ԫ����");
            //        //}
            //        DataTable meta = DBHelper.GlobalDBHelper.DoQueryEx("meta", "SELECT * FROM " + _curHeadInfo.MetaTableName + " WHERE F_OID =" + _curHeadInfo.MetaID, true);
            //        MetaRegisterConfigDAL config = MetaRegisterConfigDAL.Singleton.Select(_curHeadInfo.MetaTableName);

            //        if (meta == null || meta.Rows.Count == 0)
            //        {
            //            errors.Add("δ�ҵ���ص�Ԫ����");
            //            flg = false;
            //        }
            //        else
            //        {
            //            if (config == null)
            //            {
            //                errors.Add("δ����ע���ֶ�");
            //                return false;
            //            }

            //            if (meta.Columns.Contains(config.DataTime) == false)
            //            {
            //                errors.Add("Ԫ���ݱ����������ڡ���Ϣ");
            //            }
            //            else
            //            {
            //                DateTime year_n;
            //                try
            //                {
            //                    year_n = GetSafeDataUtility.ValidateDataRow_T(meta.Rows[0], config.DataTime);
            //                    if (year_n.Equals(DateTime.MinValue))
            //                    {
            //                        warnings.Add("������Ч������ֵ");
            //                    }
            //                    _curHeadInfo.DataTime = year_n;
            //                    _curHeadInfo.Year = year_n.Year;
            //                    //_curHeadInfo.Keywords += "," + _curHeadInfo.Year.ToString();
            //                    KeywordIndexHelper.AddKeywordIndex(_curHeadInfo.Year.ToString(), EnumKeywordIndexType.Year);
            //                }
            //                catch
            //                {
            //                    warnings.Add("������Ч������ֵ");
            //                }
            //            }

            //            if (meta.Columns.Contains(config.Scale) == false)
            //            {
            //                errors.Add("Ԫ���ݱ������������ߡ���Ϣ");
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    string scale_str = GetSafeDataUtility.ValidateDataRow_S(meta.Rows[0], config.Scale);
            //                    if (scale_str == null || scale_str == string.Empty)
            //                    {
            //                        warnings.Add("������Ч�ı�������ֵ");
            //                    }
            //                    _curHeadInfo.Scale = scale_str;
            //                    //_curHeadInfo.Keywords += "," + scale_str;
            //                    KeywordIndexHelper.AddKeywordIndex(scale_str, EnumKeywordIndexType.Scale);
            //                }
            //                catch
            //                {
            //                    warnings.Add("������Ч�ı�������ֵ");
            //                }
            //            }

            //            if (meta.Columns.Contains(config.Resulation) == false)
            //            {
            //                errors.Add("Ԫ���ݱ��������ֱ��ʡ���Ϣ");
            //            }
            //            else
            //            {
            //                try
            //                {
            //                    string resolution_str = GetSafeDataUtility.ValidateDataRow_S(meta.Rows[0], config.Resulation);
            //                    if (resolution_str == null || resolution_str == string.Empty)
            //                    {
            //                        warnings.Add("������Ч�ķֱ�����ֵ");
            //                    }
            //                    _curHeadInfo.Resolution = resolution_str;
            //                    //_curHeadInfo.Keywords += "," + resolution_str;
            //                    KeywordIndexHelper.AddKeywordIndex(resolution_str, EnumKeywordIndexType.Resolution);
            //                }
            //                catch
            //                {
            //                    warnings.Add("������Ч�ķֱ�����ֵ");
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

        #region IRollback ��Ա

        /// <summary>
        /// �ع�
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            InvokeBeginRollbackTaskData();
            bool succeed = true;
            //����Ԫ������Ϣ
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
                InvokeRollbackTaskDataProgress(0, 0, "�����Ѿ���ɾ��");
                Update();
            }
            else
            {

                #region ɾ��ʵ���ļ�
                if (this._state.ServerState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    tmpSucceed = DeleteDataFiles();
                    if (tmpSucceed)
                    {
                        InvokeRollbackTaskDataProgress(70, 100, "ɾ���ļ��ɹ�");
                        this._state.ServerState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone;
                    }
                    else
                    {
                        InvokeRollbackTaskDataProgress(70, 100, "ɾ���ļ�ʧ��");
                    }
                    succeed = succeed && tmpSucceed;
                }
                #endregion

                #region ɾ������ͼ

                if (this._state.SnapShotState !=
                    Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    //InvokeRollbackTaskDataProgress(90, 100, "��ʼɾ������ͼ");
                    //if (string.IsNullOrEmpty(_curHeadInfo.SnapLayer))
                    //{
                    //    InvokeRollbackTaskDataProgress(95, 100, "����ͼδ���");
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
                    //        InvokeRollbackTaskDataProgress(92, 100, "ɾ������ͼ����" + exp.Message);
                    //    }
                    //    succeed = succeed && tmpSucceed;
                    //    InvokeRollbackTaskDataProgress(95, 100, "ɾ������ͼ" + (tmpSucceed ? "�ɹ�" : "ʧ��"));
                    //}
                }

                #endregion

                #region ɾ��Ԫ����

                if (this._state.MetaState != Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.NoDone)
                {
                    InvokeRollbackTaskDataProgress(80, 100, "��ʼɾ��Ԫ����");

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
                        InvokeRollbackTaskDataProgress(90, 100, "ɾ��Ԫ���ݳ���" + exp.Message);
                    }
                    succeed = succeed && tmpSucceed;
                    InvokeRollbackTaskDataProgress(100, 100, "ɾ��Ԫ����" + (tmpSucceed ? "�ɹ�" : "ʧ��"));
                }

                #endregion

                #region ɾ��ע����Ϣ��Ĵָͼ���ռ䷶Χ

                //InvokeRollbackTaskDataProgress(97, 100, "��ʼɾ��ע����Ϣ��Ĵָͼ���ռ䷶Χ");
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
                //    InvokeRollbackTaskDataProgress(98, 100, "ɾ��ע����Ϣ��Ĵָͼ���ռ䷶Χ����" + exp.Message);
                //}
                //succeed = succeed && tmpSucceed;
                //InvokeRollbackTaskDataProgress(100, 100, "ɾ��ע����Ϣ��Ĵָͼ���ռ䷶Χ" + (tmpSucceed ? "�ɹ�" : "ʧ��"));
                //Update();

                #endregion
            }
            InvokeEndRollbackTaskData(succeed);
            return succeed;
        }

        /// <summary>
        /// ɾ�������ļ�
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
                msg = "����ɾ���ļ� " + path;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
                try
                {
                    if (DataPathInfoDAL.GetDataPathCountByServerPath(path.FileLocation, _server.ServerParameter.ID) > 1)
                    {
                        path.Delete();
                        msg = "�ļ�" + path + "�����������ݹ�����ʱ����ɾ��";
                    }
                    else
                    {
                        tmpSucceed = _server.DeleteFile(path.FileLocation);
                        succeed = succeed && tmpSucceed;
                        if (tmpSucceed)
                        {
                            path.Delete();
                            msg = "ɾ���ļ� " + path + "�ɹ�";
                        }
                        else
                        {
                            msg = "ɾ���ļ� " + path + "ʧ��";
                        }
                    }
                }
                catch (Exception exp)
                {
                    msg = "ɾ���ļ� " + path + "ʧ�ܣ�ԭ��" + exp.Message;
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
        /// ɾ�������ļ�
        /// </summary>
        /// <returns></returns>
        private bool DeleteDataFiles()
        {
            bool succeed = true;
            bool tmpSucceed = true;

            IList<DataPathInfoDAL> lstPath = DataPathInfoDAL.Singleton.SeletByObjectID(
                this.DataId.ToString(), EnumDataFileSourceType.DataUnit);//��ȡ�������ݶ�Ӧ���ļ��ڷ������ϵ�·��


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
                msg = "����ɾ���ļ� " + path;
                InvokeRollbackTaskDataProgress(pos, 100, msg);
                try
                {
                    if (DataPathInfoDAL.GetDataPathCountByServerPath(path.FileLocation, _server.ServerParameter.ID) > 1)
                    {
                        path.Delete();
                        msg = "�ļ�" + path + "�����������ݹ�����ʱ����ɾ��";
                    }
                    else
                    {
                        tmpSucceed = _server.DeleteFile(path.FileLocation);
                        succeed = succeed && tmpSucceed;
                        if (tmpSucceed)
                        {
                            path.Delete();
                            msg = "ɾ���ļ� " + path + "�ɹ�";
                        }
                        else
                        {
                            msg = "ɾ���ļ� " + path + "ʧ��";
                        }
                    }
                }
                catch (Exception exp)
                {
                    msg = "ɾ���ļ� " + path + "ʧ�ܣ�ԭ��" + exp.Message;
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

        #region IWriteMetaData��Ա
        private List<string> _metadataFiles = null; //�ⲿԪ���ݺ�ѡ�ļ��б�
        string _metadataFullName = string.Empty;
        /// <summary>
        /// Ԫ�����ļ���
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
        /// ��ȡ�������Ƿ��ϴ�Ԫ�����ļ�
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
            InvokeTaskDataProcessInfo(this, "Ԫ�������...");
            bool successed = false;
            try
            {
                if (string.IsNullOrEmpty(_metadataFullName))
                {
                    InvokeTaskDataProcessInfo(this, "ָ����λ����δ�ҵ�Ԫ�����ļ�");
                }
                else
                {
                    //дԪ���ݱ���չ����
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
                        string msg = string.Format("Ԫ���ݼ�¼д�����ݿ�ʧ�ܣ�");
                        InvokeTaskDataProcessInfo(this, "Ԫ���ݼ�¼д�����ݿ�ʧ��");
                        metaDataExtensionalEdit = null;
                    }

                }
            }
            catch (Exception ex)
            {
                successed = false;
#if DEBUG
                InvokeTaskDataProcessInfo(this, "Ԫ����������" + ex.Message);
#endif
                LogHelper.Error.Append(ex);
                _dataID = -1;
                metaDataExtensionalEdit = null;
            }
            finally
            {
                if (successed)
                {
                    InvokeTaskDataProcessInfo(this, "Ԫ�������ɹ�");
                }
                else
                {
                    InvokeTaskDataProcessInfo(this, "Ԫ�������ʧ��");
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
                
                //���º���Ԫ������Ϣ
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
        /// ����Ԫ�����ļ�·��
        /// </summary>
        /// <param name="metadataFileSetting">Ԫ�����ļ��ϴ�����</param>
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
    /// ����������֤���
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
