using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using ESRI.ArcGIS.Geodatabase;
using EnumExecuteState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public abstract class Task : IDBOper<Task>, IExecuteControler, IExecuteLog, ITaskExecuteProgress, IDataExecuteProgress
    {
        #region 字段
        protected int _id;
        protected string _name;
        protected string _description;
        protected string _taskDirectory;
        protected int _userID;
        protected Int64 _dataAmount;
        protected DateTime _defineTime;
        protected DateTime _beginTime;
        protected DateTime _endTime;
        protected EnumExecuteState _enumState = EnumExecuteState.NoExecute;
        protected int _serverID;
        protected StorageServer _server;
        protected bool _isTiming;
        protected int _curProgressCount = 0;
        protected bool _flag;
        #endregion

        #region 属性
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 任务目录
        /// </summary>
        public string Directory
        {
            get { return _taskDirectory; }
            set { _taskDirectory = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public Int64 DataAmount
        {
            get { return _dataAmount; }
            set { _dataAmount = value; }
        }

        public DateTime DefineTime
        {
            get { return _defineTime; }
            set { _defineTime = value; }
        }

        public DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public bool IsTiming
        {
            get { return _isTiming; }
            set { _isTiming = value; }
        }

        /// <summary>
        /// 当前操作进度（任务数据个数转换为百分比）
        /// </summary>
        public int CurProgressCount
        {
            get { return _curProgressCount; }
            set { _curProgressCount = value; }
        }

        public EnumExecuteState EnumState
        {
            get { return _enumState; }
            set { _enumState = value; }
        }

        public int ServerID
        {
            get { return _serverID; }
            set
            {
                if (_serverID != value)
                {
                    _serverID = value;
                    _server = null;
                }
            }
        }

        public StorageServer StorageServer
        {
            get
            {
                if (_server == null)
                {
                    _server = CatalogModelEngine.CreateCatalogDataSource(CatalogModelEngine.GetStorageNodeByID(DBHelper.GlobalDBHelper, _serverID));
                }
                return _server;
            }
            set { _server = value; }
        }

        public bool Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        protected IList<TaskData> _datas = null;
        /// <summary>
        /// 获取或设置任务数据
        /// </summary>
        public IList<TaskData> Datas
        {
            get
            {
                return GetTaskDatas();
            }
            set { _datas = value; }
        }

        #endregion

        ~Task()
        {
            if (_server != null)
            {
                _server.ReleaseConnect();
            }
        }
        #region 虚函数


        #endregion

        #region 重载函数
        public override bool Equals(object obj)
        {
            Task t = obj as Task;
            if (t == null)
            {
                return false;
            }
            return this._id == t.ID;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual string ToState()
        {
            string state = string.Empty;
            switch (_enumState)
            {
                case EnumExecuteState.NoExecute:
                    state = "未执行";
                    break;
                case EnumExecuteState.Executing:
                    state = "执行中";
                    break;
                case EnumExecuteState.ExecuteSuccessful:
                    state = "成功";
                    break;
                case EnumExecuteState.ExecuteFailed:
                    state = "失败";
                    break;
                default:
                    break;
            }
            return state;
        }

        public abstract IList<TaskData> GetTaskDatas();
        #endregion

        #region 私有函数
        /// <summary>
        /// 设置执行过程信息
        /// </summary>
        /// <param name="info"></param>
        protected void InvokeProcessInfo(string info)
        {
            if (this.progressInfoNotify != null)
            {
                this.progressInfoNotify.Invoke(info);
            }
        }

        /// <summary>
        /// 设置进度条的状态
        /// </summary>
        /// <param name="position"></param>
        protected void InvokeProgressPosition(int position)
        {
            if (this.positionNotify != null)
            {
                TaskProgressEventArgs progressEventArgs = new TaskProgressEventArgs(this._id, position);
                this.positionNotify.Invoke(progressEventArgs);
            }
        }

        /// <summary>
        /// 设置进度条的最大值
        /// </summary>
        /// <param name="count"></param>
        protected void InvokeProgressCount(int count)
        {
            if (this.countNotify != null)
            {
                TaskProgressEventArgs progressEventArgs = new TaskProgressEventArgs(this._id, count);
                this.countNotify.Invoke(progressEventArgs);
            }
        }

        /// <summary>
        /// 通知开始执行上传
        /// </summary>
        protected void InvokeBeginExecute(string msg)
        {
            if (this.beginExecute != null)
            {
                this.beginExecute.Invoke(this, msg);
            }
        }

        /// <summary>
        /// 通知结束上传
        /// </summary>
        protected void InvokeEndExecute(string msg)
        {
            if (this.endExecute != null)
            {
                this.endExecute.Invoke(this, msg);
            }
        }

        protected void BeforeExecuteData(TaskData taskData)
        {
            //taskData.BeginExecuteData += new TaskDataExecuteEventHandler(taskData_BeginExecuteData);
            //taskData.EndExecuteData += new TaskDataExecuteEventHandler(taskData_EndExecuteData);
            taskData.BeginGet += taskData_BeginGet;
            taskData.EndGet += taskData_EndGet;
            taskData.Progress += taskData_Progress;
            taskData.TaskDataProcessInfo += taskData_TaskDataProcessInfo;
        }

        protected void AfterExecuteData(TaskData taskData)
        {
            //taskData.BeginExecuteData -= new TaskDataExecuteEventHandler(taskData_BeginExecuteData);
            //taskData.EndExecuteData -= new TaskDataExecuteEventHandler(taskData_EndExecuteData);
            taskData.BeginGet -= taskData_BeginGet;
            taskData.EndGet -= taskData_EndGet;
            taskData.Progress -= taskData_Progress;
            taskData.TaskDataProcessInfo -= taskData_TaskDataProcessInfo;
        }


        protected void InvokeBeginGet(TaskData taskData, ServerFileEventArgs e)
        {
            if (BeginGet != null)
            {
                BeginGet.Invoke(taskData, e);
            }
        }

        protected void InvokeProgress(TaskData taskData, ServerProgressEventArgs e)
        {
            if (Progress != null)
            {
                Progress.Invoke(taskData, e);
            }
        }

        protected void InvokeTaskDataProcessInfo(TaskData taskData, string msg)
        {
            if (TaskDataProcessInfo != null)
            {
                TaskDataProcessInfo(taskData, msg);
            }
        }

        protected void InvokeEndGet(TaskData taskData, ServerFileEventArgs e)
        {
            if (EndGet != null)
            {
                EndGet.Invoke(taskData, e);
            }
        }

        protected void InvokeBeginUploadData(TaskData taskData, string msg)
        {
            if (BeginExecuteData != null)
            {
                BeginExecuteData.Invoke(taskData, msg);
            }
        }

        protected void InvokeEndUploadData(TaskData taskData, string msg)
        {
            if (EndExecuteData != null)
            {
                EndExecuteData.Invoke(taskData, msg);
            }
        }
        #endregion

        #region 关联事件

        void taskData_BeginExecuteData(TaskData taskData, string msg)
        {
            InvokeBeginUploadData(taskData, msg);
        }

        void taskData_BeginGet(TaskData taskData, ServerFileEventArgs e)
        {
            InvokeBeginGet(taskData, e);
        }

        void taskData_Progress(TaskData taskData, ServerProgressEventArgs e)
        {
            InvokeProgress(taskData, e);
        }

        void taskData_EndGet(TaskData taskData, ServerFileEventArgs e)
        {
            InvokeEndGet(taskData, e);
        }

        void taskData_EndExecuteData(TaskData taskData, string msg)
        {
            InvokeEndUploadData(taskData, msg);
        }

        void taskData_TaskDataProcessInfo(TaskData taskData, string msg)
        {
            InvokeTaskDataProcessInfo(taskData, msg);
        }
        #endregion

        #region IExecuteControler 成员
        protected bool isPaused = false;
        public bool IsPaused
        {
            get
            {
                return isPaused;
            }
            set
            {
                isPaused = value;
            }
        }

        protected bool isCancel = false;
        public bool IsCancel
        {
            get
            {
                return isCancel;
            }
            set
            {
                isCancel = value;
            }
        }

        public abstract void Start();

        public abstract void Continue();

        public abstract void Pause();

        public abstract void Stop();

        #endregion

        #region IExecuteLog 成员

        protected StringBuilder logInfo;
        public StringBuilder LogInfo
        {
            get
            {
                return logInfo;
            }
            set
            {
                logInfo = value;
            }
        }

        public abstract bool ExportToXML(string XMLPath);

        public abstract bool ExportToText(string TextFilePath);

        public abstract bool ExportToDatatable(out System.Data.DataTable table);

        public abstract bool WriteLogToSystem(int SubSysID);

        #endregion

        #region IDBOper<Task> 成员
        /// <summary>
        /// 新建任务
        /// </summary>
        /// <returns></returns>
        public abstract bool Add();
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public abstract bool Delete();

        /// <summary>
        /// 编辑任务
        /// </summary>
        public abstract bool Update();


        public abstract int GetNextID();

        public abstract IList<Task> Select();

        public abstract Task Select(int id);

        public abstract IList<Task> Select(string filter);

        #endregion

        #region ITaskExecuteProgress 成员

        protected event ProgressCount countNotify;
        public event ProgressCount ProgressCount
        {
            add
            {
                countNotify += value;
            }
            remove
            {
                countNotify -= value;

            }

        }

        protected event ProgressPosition positionNotify;
        public event ProgressPosition ProgressPosition
        {
            add
            {
                positionNotify += value;
            }
            remove
            {
                positionNotify -= value;
            }
        }

        protected event ProgressInfo progressInfoNotify;
        public event ProgressInfo ProgressInfo
        {
            add
            {
                progressInfoNotify += value;
            }
            remove
            {
                progressInfoNotify -= value;
            }
        }

        protected event TaskExecuteEventHandler beginExecute;
        public event TaskExecuteEventHandler BeginTaskExecute
        {
            add
            {
                beginExecute += value;
            }
            remove
            {
                beginExecute -= value;
            }
        }

        protected event TaskExecuteEventHandler endExecute;
        public event TaskExecuteEventHandler EndTaskExecute
        {
            add
            {
                endExecute += value;
            }
            remove
            {
                endExecute -= value;
            }
        }
        #endregion

        #region IDataExecuteProgress 成员

        public event TaskDataExecuteEventHandler BeginExecuteData;

        public event TaskFileTransferEventHandler BeginGet;

        public event TaskFileTransferProgressEventHandler Progress;

        public event TaskFileTransferEventHandler EndGet;

        public event TaskDataExecuteEventHandler EndExecuteData;

        public event TaskDataProcessInfoEventHandler TaskDataProcessInfo;

        #endregion
    }
}
