using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    /// <summary>
    /// �����������(��С��Ԫ)
    /// </summary>
    public abstract class TaskData : IDBOper<TaskData>, IDataExecuteProgress,IMetaDataFixedDZEdit
    {

        #region ����
        protected int _taskdataID;
        /// <summary>
        /// ��������ID
        /// </summary>
        public int TaskDataID
        {
            get { return _taskdataID; }
            set { _taskdataID = value; }
        }

        protected string _taskdataName;
        /// <summary>
        /// ������������
        /// </summary>
        public string TaskDataName
        {
            get { return _taskdataName; }
            set { _taskdataName = value; }
        }

        protected int _taskID;
        /// <summary>
        /// ��������ID
        /// </summary>
        public int TaskID
        {
            get { return _taskID; }
            set { _taskID = value; }
        }

        protected Int64 _size;
        /// <summary>
        /// ��������������
        /// </summary>
        public Int64 Size
        {
            get { return _size; }
            set { _size = value; }
        }

        protected string _pacPath;
        /// <summary>
        /// ��������·��
        /// </summary>
        public string PacPath
        {
            get { return _pacPath; }
            set { _pacPath = value; }
        }

        protected int _dataId;
        /// <summary>
        /// ��������ע����ϢID����ӦԪ���ݱ��е�F_DATAID
        /// </summary>
        public int DataId
        {
            get { return _dataId; }
            set { _dataId = value; }
        }

        protected string _tableName = string.Empty;
        /// <summary>
        /// ע��ͼ������
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set{_tableName = value;}
        }


        protected StringBuilder _log;
        /// <summary>
        /// �������ݲ�����־
        /// </summary>
        public StringBuilder Log
        {
            get { return _log; }
        }

        protected DataExecuteState _state;//ִ��״̬
        /// <summary>
        /// ��������ִ��״̬
        /// </summary>
        public DataExecuteState State
        {
            get { return _state; }
            set { _state = value; }
        }

        protected bool _isTransferFile = false;
        /// <summary>
        /// ��ȡ�������Ƿ����ڴ����ļ�
        /// </summary>
        public bool IsTransferFile
        {
            get { return _isTransferFile; }
            set { _isTransferFile = value; }
        }

        private Task _task = null;
        /// <summary>
        /// ��������
        /// </summary>
        public Task BelongtoTask
        {
            get
            {
                if (_task == null)
                {
                    EnumTaskType type = this is UpLoadTaskData ? EnumTaskType.Upload : EnumTaskType.Download;
                    Task myTask = TaskFactory.CreateTask(type);
                    _task = myTask.Select(_taskID);
                }
                return _task;
            }
            set { _task = value; }
        }

        #endregion

        #region ˽�к���
        protected void InvokeBeginGet(object o, ServerFileEventArgs e)
        {
            if (beginGet != null)
            {
                beginGet.Invoke(this, e);
            }
        }

        protected void InvokeProgress(object o, ServerProgressEventArgs e)
        {
            if (progress != null)
            {
                progress.Invoke(this, e);
            }
        }

        protected void InvokeEndGet(object o, ServerFileEventArgs e)
        {
            if (endGet != null)
            {
                endGet.Invoke(this, e);
            }
        }

        protected void InvokeBeginExecuteData()
        {
            if (this.beginExecuteData != null)
            {
                this.beginExecuteData(this, "");
            }
        }

        protected void InvokeEndExecuteData()
        {
            if (this.endExecuteData != null)
            {
                this.endExecuteData(this, "");
            }
        }

        protected void InvokeTaskDataProcessInfo(TaskData taskData, string msg)
        {
            if (this.taskDataProcessInfo != null)
            {
                taskDataProcessInfo(taskData, msg);
            }
        }
        #endregion

        #region IDBOper<TaskData> ��Ա
        public abstract bool Add();

        public abstract bool Delete();

        public abstract bool Update();

        public abstract IList<TaskData> Select();

        public abstract TaskData Select(int id);

        public abstract int GetNextID();

        public abstract IList<TaskData> Select2(int taskID);

        #endregion

        #region IDataExecuteProgress ��Ա
        protected event TaskDataExecuteEventHandler beginExecuteData;
        public event TaskDataExecuteEventHandler BeginExecuteData
        {
            add{beginExecuteData += value;}
            remove{beginExecuteData -= value;}
        }

        protected event TaskFileTransferEventHandler beginGet;
        public event TaskFileTransferEventHandler BeginGet
        {
            add{beginGet += value;}
            remove{beginGet -= value;}
        }

        protected event TaskFileTransferProgressEventHandler progress;
        public event TaskFileTransferProgressEventHandler Progress
        {
            add{progress += value;}
            remove{progress -= value;}
        }

        protected event TaskFileTransferEventHandler endGet;
        public event TaskFileTransferEventHandler EndGet
        {
            add{endGet += value;}
            remove{endGet -= value;}
        }

        protected event TaskDataExecuteEventHandler endExecuteData;
        public event TaskDataExecuteEventHandler EndExecuteData
        {
            add{endExecuteData += value;}
            remove{endExecuteData -= value;}
        }

        protected event TaskDataProcessInfoEventHandler taskDataProcessInfo;
        public event TaskDataProcessInfoEventHandler TaskDataProcessInfo
        {
            add { taskDataProcessInfo += value; }
            remove { taskDataProcessInfo -= value; }
        }

        #endregion

        /// <summary>
        /// ֹͣ�����ļ�
        /// </summary>
        public abstract void StopTransferFile();

        #region IMetaDataFixedDZEdit ��Ա
        protected double _DataSize;
        public double DataSize
        {
            get { return _DataSize; }
            set { _DataSize = value; }
        }

        protected string _dataTypeName;
        public string DataTypeName
        {
            get { return _dataTypeName; }
            set { _dataTypeName = value; }
        }

        protected string _DataUnit;
        public string DataUnit
        {
            get { return _DataUnit; }
            set { _DataUnit = value; }
        }

        protected long _serverId;
        public long ServerId
        {
            get { return _serverId; }
            set { _serverId = value; }
        }

        protected string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        #endregion
    }
}
