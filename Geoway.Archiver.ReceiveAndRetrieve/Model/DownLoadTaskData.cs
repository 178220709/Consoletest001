using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.Query.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.DownLoad;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    using Modeling.Definition;


    public class DownLoadTaskData : TaskData, IDownloadPackage, IDownloadMetaData, IDownloadSnapShot
    {
        private StorageServer _server;

        public DownLoadTaskData()
        {
            _state = new DataExecuteState();
            _log = new StringBuilder();
        }

        #region 重载函数
        public override bool Add()
        {
            TaskDataDAL taskDataDAL = Translate(this);
            return taskDataDAL.Insert();
        }

        public override bool Delete()
        {
            TaskDataDAL taskDataDAL = Translate(this);
            return taskDataDAL.Insert();
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

        public override TaskData Select(int id)
        {
            TaskDataDAL taskDataDAL = TaskDataDAL.Singleton.Select(id);
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
            DownLoadTaskData taskData = new DownLoadTaskData();
            taskData.TaskDataID = taskDataDAL.TaskDataID;
            taskData.TaskDataName = taskDataDAL.TaskDataName;
            taskData.TaskID = taskDataDAL.TaskID;
            taskData.State = taskDataDAL.State;
            taskData.Size = taskDataDAL.DataSize;
            taskData.DataId = taskDataDAL.ObjectID;
            taskData.State = taskDataDAL.State;
            taskData.TableName = taskDataDAL.RegisterLayerName;
            return taskData;
        }

        public static TaskDataDAL Translate(TaskData taskData)
        {
            DownLoadTaskData downLoadTaskData = (DownLoadTaskData)taskData;

            TaskDataDAL taskDataDAL = new TaskDataDAL();
            taskDataDAL.TaskDataID = downLoadTaskData.TaskDataID;
            taskDataDAL.TaskDataName = downLoadTaskData.TaskDataName;
            taskDataDAL.DataSize = downLoadTaskData.Size;
            taskDataDAL.TaskID = downLoadTaskData.TaskID;
            taskDataDAL.DataSize = downLoadTaskData.Size;
            taskDataDAL.InOrOut = (int)EnumTaskType.Download;
            taskDataDAL.ObjectID = downLoadTaskData._dataId;
            taskDataDAL.State = downLoadTaskData.State;
            taskDataDAL.RegisterLayerName = downLoadTaskData.TableName;
            return taskDataDAL;
        }
        #endregion

        #region  私有函数
        private void BeforeDownloadData()
        {
            _server.BeginGet += Server_BeginGet;
            _server.EndGet += Server_EndGet;
            _server.Progress += Server_Progress;
        }

        private void AfterDownloadData(DownLoadTaskData taskData)
        {
            _server.BeginGet -= Server_BeginGet;
            _server.EndGet -= Server_EndGet;
            _server.Progress -= Server_Progress;
        }

        void Server_BeginGet(object o, ServerFileEventArgs e)
        {
            InvokeBeginGet(o, e);
        }

        void Server_Progress(object o, ServerProgressEventArgs e)
        {
            InvokeProgress(o, e);
        }

        void Server_EndGet(object o, ServerFileEventArgs e)
        {
            InvokeEndGet(o, e);
        }

        #endregion

        #region IDownloadPackage 成员
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

        public bool DownloadDataPackageToLocal(string localDirectory)
        {
            base.IsTransferFile = true;
            InvokeBeginExecuteData();
            bool successed = true;

            IMetaDataSys metaData = MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper,EnumMetaDataType.EnumSystem,  EnumMetaDatumType.enumDefault, DataId) as IMetaDataSys;
            IMetaDataSysEdit headInfo = metaData as IMetaDataSysEdit;
            IMetaDataFixedDZ metaDataFixedDz = MetaDataFactory.CreateMetaData(DBHelper.GlobalDBHelper, EnumMetaDataType.EnumFixed, EnumMetaDatumType.enumSTELEDatum, DataId) as IMetaDataFixedDZ;
            IMetaDataFixedDZEdit metaDataFixedDzEdit = metaDataFixedDz as IMetaDataFixedDZEdit;
            

            if (headInfo == null || headInfo.Flag == 1)
            {
                this.State.ServerState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Failed;
                successed = false;
                _errorMassage = "该影像数据已被删除";
                //Update();
            }
            else
            {
                if (_server == null)
                {
                    _server =
                        CatalogModelEngine.CreateCatalogDataSource(
                            CatalogModelEngine.GetStorageNodeByID(DBHelper.GlobalDBHelper, (int)metaDataFixedDzEdit.ServerId));
                }
                BeforeDownloadData();

                if (_server == null)
                {
                    this.State.ServerState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Failed;
                    _errorMassage = "存储服务器连接失败";
                    InvokeTaskDataProcessInfo(this, _errorMassage);
                    return false;
                }
                if (!_server.Connected())
                {
                    _errorMassage = "存储服务器[" + _server.ServerParameter.Name + "]连接失败";
                    InvokeTaskDataProcessInfo(this, _errorMassage);
                    return false;
                }

                bool tmpsuccessed = true;

                //if (_dataInstance == null)
                //{
                //    _dataInstance = DataInstance.SelectByHeadInfoID(DBHelper.GlobalDBHelper, (int)headInfo.DataId);
                //}

                IList<DataPathInfoDAL> lstDataPathDAL = DataPathInfoDAL.Singleton.SeletByObjectID(DataId.ToString(),EnumDataFileSourceType.DataUnit);

                if (lstDataPathDAL.Count > 0)
                {
                    string localPath = string.Empty;
                    string localFile = string.Empty;
                    string tempPath = string.Empty;
                    foreach (DataPathInfoDAL dataPathDAL in lstDataPathDAL)
                    {
                        if (!base.IsTransferFile)
                        {
                            successed = false;
                            break;
                        }
                        if (_server.FileExist(dataPathDAL.FileLocation))
                        {
                            localFile = DataInstanceHelper.GetLocalFilePath(localDirectory, dataPathDAL.FileLocation,
                                                                            _server.ServerParameter.FtpPath);
                            tmpsuccessed = _server.GetSingleFile(dataPathDAL.FileLocation, localFile);
                            successed = successed && tmpsuccessed;
                        }
                        else
                        {
                            InvokeTaskDataProcessInfo(this, "服务器文件 " + dataPathDAL.FileLocation + " 不存在");
                            if (string.IsNullOrEmpty(_errorMassage))
                            {
                                _errorMassage += "部分文件不存在;";
                            }
                            successed = false;
                        }
                    }
                }

                this.State.ServerState = successed ? Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Successed : Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumDataExecuteState.Failed;
                //successed = Update() && successed;

                AfterDownloadData(this);

                InvokeEndExecuteData();
            }
            return successed;
        }

        private string _errorMassage = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMassage
        {
            get { return _errorMassage; }
        }

     

        #endregion

        #region IDownloadMetaData 成员

        public bool DownloadMetaToLocal(int registerID, EnumMetaFormat enumMetaFormat, string localPath)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDownloadSnapShot 成员
        public bool DownLoadSnapShotToLocal(int registerID, string localPath)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


    }
}
