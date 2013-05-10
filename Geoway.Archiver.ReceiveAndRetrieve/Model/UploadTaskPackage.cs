using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.Core.Public.security;
using Geoway.ADF.MIS.DB.Public;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    using Geoway.Archiver.Catalog;
    using EnumExecuteState=Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState;

    /// <summary>
    ///目的：任务数据包上传操作类


    ///创建人：王金玉


    ///创建日期：2010-11-17
    ///修改描述：


    ///修改人：
    ///修改日期：


    ///备注：


    /// </summary>
    public class UploadTaskPackage
    {
        public event TaskPackageProcessInfoEventHandler TaskPackageProcessInfo;
        public event TaskPublicFileTransferEventHandler BeginPutPublicFile;
        public event TaskPublicFileTransferEventHandler EndPutPublicFile;
        public event TaskPublicFileTransferProgressEventHandler PutPublicFileProgress;

        private IList<TaskFileDAL> _taskFiles = null;
        private TaskPackageDAL _taskPackage = null;
        private StorageServer _server = null;

        #region 属性



        private IList<TaskData> _taskDatas = null;
        /// <summary>
        /// 包含的任务数据


        /// </summary>
        public IList<TaskData> TaskDatas
        {
            get
            {
                if (_taskDatas == null)
                {
                    _taskDatas = new List<TaskData>();
                    IList<TaskDataDAL> lst = TaskDataDAL.Singleton.SelectByPackageID(_taskPackage.ID);
                    foreach (TaskDataDAL dal in lst)
                    {
                        _taskDatas.Add(UpLoadTaskData.Translate(dal));
                    }
                }
                return _taskDatas;
            }
            set { _taskDatas = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            get { return _taskPackage.ID; }
            set { _taskPackage.ID = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _taskPackage.Name; }
            set { _taskPackage.Name = value; }
        }

        /// <summary>
        /// 获取或设置状态


        /// </summary>
        public Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState State
        {
            get { return _taskPackage.State; }
            set { _taskPackage.State = value; }
        }

        /// <summary>
        /// 获取或设置所属任务ID
        /// </summary>
        public int TaskID
        {
            get { return _taskPackage.TaskID; }
            set { _taskPackage.TaskID = value; }
        }

        /// <summary>
        /// 获取或设置数据量大小
        /// </summary>
        public Int64 DataSize
        {
            get { return _taskPackage.DataSize; }
            set { _taskPackage.DataSize = value; }
        }

        /// <summary>
        /// 获取或设置数据包路径
        /// </summary>
        public string PackagePath
        {
            get { return _taskPackage.PackagePath; }
            set { _taskPackage.PackagePath = value; }
        }

        private int _catalogID = 0;
        /// <summary>
        /// 数据目录节点ID
        /// </summary>
        public int CatalogID
        {
            get { return _catalogID; }
            set
            { 
                _catalogID = value;

                _registerLayerName = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, _catalogID).MetaTableName;
            }
        }

        private string _registerLayerName = string.Empty;

        public string RegisterLayerName
        {
            get { return _registerLayerName; }
        }
        

        private int _serverID = 0;
        /// <summary>
        /// 存储节点ID
        /// </summary>
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }

        private DataPackageType _packageType = null;
        /// <summary>
        /// 数据包类型
        /// </summary>
        public DataPackageType PackageType
        {
            get { return _packageType; }
            set { _packageType = value; }
        }


        #endregion

        #region 初始化



        public UploadTaskPackage(DataPackageType packageType)
        {
            _packageType = packageType;
            _taskPackage = new TaskPackageDAL();
        }

        public UploadTaskPackage(int id)
        {
            if (id > 0)
            {
                _taskPackage = TaskPackageDAL.Singleton.Select(id);
            }
            else
            {
                _taskPackage = new TaskPackageDAL();
            }
        }

        public UploadTaskPackage(TaskPackageDAL taskPackage, StorageServer server)
        {
            _taskPackage = taskPackage;
            _server = server;
        }

        #endregion

        public bool Add()
        {
            return _taskPackage.Insert();
        }

        public bool Update()
        {
            return _taskPackage.Update();
        }

        public bool Delete()
        {
            return _taskPackage.Delete();
        }

        /// <summary>
        /// 从数据库创建数据包公共文件列表

        /// </summary>
        public void CreateTaskPublicFileListFromDB()
        {
            if (_taskFiles == null)
            {
                _taskFiles = TaskFileDAL.Singleton.SelectByTaskPackageID(_taskPackage.ID);
            }
        }

        /// <summary>
        /// 从文件系统创建数据包公共文件列表
        /// </summary>
        public void CreateTaskPublicFileListFromFS(IList<UpLoadTaskData> packageDatas)
        {
            if (_taskFiles == null)
            {
                _taskFiles = new List<TaskFileDAL>();
                DataSize += DataInstanceHelper.GetDataFileByProperty(ID, _packageType, PackagePath, EnumDataFileProperty.NormalFile, "", _taskFiles, EnumDataFileSourceType.DataPackage);
                for (int i = 0; i < _taskFiles.Count; i++)
                {
                    foreach (UpLoadTaskData data in packageDatas)
                    {
                        if (data.ContainsFile(_taskFiles[i]))
                        {
                            _taskFiles.RemoveAt(i--);
                            DataSize -= _taskFiles[i].DataSize;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存任务公共数据文件到数据库
        /// </summary>
        public void SaveTaskPublicFiles()
        {
            foreach (TaskFileDAL dal in _taskFiles)
            {
                dal.Insert();
            }
        }

        /// <summary>
        /// 上传公共文件
        /// </summary>
        /// <param name="svrPrixPath"></param>
        /// <returns></returns>
        public bool UploadPublicFiles(string svrPrixPath, int packageID)
        {
            InvokeTaskDataProcessInfo("开始上传数据包公共文件...");

            CreateTaskPublicFileListFromDB();

            bool succeed = true;

            if (_taskFiles != null && _taskFiles.Count > 0)
            {
                BeforeUploadPackage();

                DataPathInfoDAL dataPathDAL = null;
                string localPath = string.Empty;
                string serverFile = string.Empty;
                string tempPath = string.Empty;
                bool tmpsuccessed = false;
                foreach (TaskFileDAL file in _taskFiles)
                {
                    serverFile = DataInstanceHelper.GetServerFilePath(_server.ServerParameter.FtpPath,
                                                                      svrPrixPath,
                                                                      _taskPackage.PackagePath,
                                                                      file.FileLocation);
                    tmpsuccessed = _server.PutSingleFile(file.FileLocation, serverFile);

                    if (tmpsuccessed)
                    {
                        //文件记录写入数据库


                        dataPathDAL = new DataPathInfoDAL();
                        dataPathDAL.RegisterLayerName = _registerLayerName;
                        dataPathDAL.ObjectID = packageID.ToString();
                        dataPathDAL.PackagePath = file.PackagePath;
                        dataPathDAL.DataSize = file.DataSize;
                        dataPathDAL.FileLocation = serverFile;
                        dataPathDAL.SourceType = EnumDataFileSourceType.DataPackage;
                        tmpsuccessed = dataPathDAL.Insert();
                    }
                    succeed = succeed && tmpsuccessed;
                }

                AfterUploadPackage();
                InvokeTaskDataProcessInfo("数据包公共文件上传结束");
            }
            else
            {
                InvokeTaskDataProcessInfo("没有公共文件需要上传");
            }
            _taskPackage.State = (succeed ? EnumExecuteState.ExecuteSuccessful : EnumExecuteState.ExecuteFailed);
            _taskPackage.Update();
            return succeed;
        }

        /// <summary>
        /// 记录登记信息
        /// </summary>
        /// <returns></returns>
        public int WriteRegisterInfo()
        {
            //DataPackageInfoDAL dataPackage = new DataPackageInfoDAL();
            //dataPackage.Name = _taskPackage.Name;
            //dataPackage.PackageType = _packageType.Name;
            //dataPackage.ImportUserName = LoginControl.userName;
            //dataPackage.ImportDateTime = DBHelper.GlobalDBHelper.getServerDate();
            //dataPackage.CatalogID = _catalogID;
            //dataPackage.ServerID = _serverID;
            //dataPackage.DataSize = _taskPackage.DataSize;
            //if (dataPackage.Insert())
            //{
            //    return dataPackage.ID;
            //}
            //else
            {
                return -1;
            }
        }

        #region 内部方法

        private void InvokeTaskDataProcessInfo(string msg)
        {
            if (TaskPackageProcessInfo != null)
            {
                TaskPackageProcessInfo(this, msg);
            }
        }

        private void BeforeUploadPackage()
        {
            _server.BeginPut += new ServerFileEventHandler(_server_BeginPut);
            _server.Progress += new ServerProgressEventHandler(_server_Progress);
            _server.EndPut += new ServerFileEventHandler(_server_EndPut);
        }

        private void AfterUploadPackage()
        {
            _server.BeginPut -= new ServerFileEventHandler(_server_BeginPut);
            _server.Progress -= new ServerProgressEventHandler(_server_Progress);
            _server.EndPut -= new ServerFileEventHandler(_server_EndPut);
        }

        void _server_Progress(object o, ServerProgressEventArgs e)
        {
            if (PutPublicFileProgress != null)
            {
                PutPublicFileProgress(this, e);
            }
        }

        void _server_EndPut(object o, ServerFileEventArgs e)
        {
            if (EndPutPublicFile != null)
            {
                EndPutPublicFile(this, e);
            }
        }

        void _server_BeginPut(object o, ServerFileEventArgs e)
        {
            if (BeginPutPublicFile != null)
            {
                BeginPutPublicFile(this, e);
            }
        }

        #endregion
    }
}
