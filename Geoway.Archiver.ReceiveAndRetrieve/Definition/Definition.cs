using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.Query.Definition;
using Geoway.ADF.MIS.CatalogDataModel.Public;


namespace Geoway.Archiver.ReceiveAndRetrieve.Definition
{
    
    public class AchiverParams
    {
        public const string Alias_F_REDATAID = "��������ID";
    }

    public delegate void BeginImportEventHandler();

    public delegate void EndImportEventHandler();

    //public delegate void ShowDataInfoEventHandler(Task task);
    #region ö�ٶ���
    /// <summary>
    /// ����������������
    /// </summary>
    public enum EnumMediumType
    {
        enumUnknown = 0,
        /// <summary>
        /// CD����
        /// </summary>
        EnumCD,
        /// <summary>
        /// DVD����
        /// </summary>
        EnumDVD,
        /// <summary>
        /// ֽ��
        /// </summary>
        EnumPaper,
    }
    
    public enum EnumMetaDataType
    {
        /// <summary>
        /// ϵͳά����Ϣ
        /// </summary>
        EnumSystem,
        /// <summary>
        /// ��������
        /// </summary>
        EnumFixed,
        /// <summary>
        /// ��չ����
        /// </summary>
        EnumExtensional,
    }
    /// <summary>
    /// ���ݶ�������
    /// </summary>
    //public enum EnumObjectType : int
    //{
    //    /// <summary>
    //    /// �ļ�
    //    /// </summary>
    //    DataFile = 0,
    //    /// <summary>
    //    /// �ļ���
    //    /// </summary>
    //    DataFolder = 1,
    //    /// <summary>
    //    /// ���ݰ�
    //    /// </summary>
    //    DataPackage = 2,
    //    /// <summary>
    //    /// ���ݰ�����
    //    /// </summary>
    //    DataPackageType = 3,
    //    /// <summary>
    //    /// �������ݰ�
    //    /// </summary>
    //    VirtualDataPackage = 4
    //}
    public enum EnumMetaFormat
    {
        none = -1,
        mat = 0,
        excel = 1,
        xml = 2,
    }

    /// <summary>
    /// ��֤���
    /// </summary>
    public enum EnumValidateResult
    {
        /// <summary>
        /// δ���
        /// </summary>
        None = 0,
        /// <summary>
        /// ͨ��
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// ��ͨ��
        /// </summary>
        Faild = 2
    }

    /// <summary>
    /// ��������ɨ��ģʽ
    /// </summary>
    public enum EnumScanMode
    {
        /// <summary>
        /// ������
        /// </summary>
        None = 0,
        /// <summary>
        /// ��ǰĿ¼
        /// </summary>
        CurrentDirectory = 1,
        /// <summary>
        /// ��Ŀ¼
        /// </summary>
        ChildDirectory = 2
    }

    public enum enumRasterFileFormat
    {
        enumRasterFileFormatGeoTIFF = 0,
        enumRasterFileFormatGRID = 1,
        enumRasterFileFormatIMAGE = 2,
    }

    //public enum EnumExecuteState
    //{
    //    None = -1,
    //    NoExecute = 0, //δִ��
    //    Executing = 1, //ִ����
    //    ExecuteFailed = 2,       //ִ��ʧ��
    //    ExecuteSuccessful = 3,   //ִ�гɹ� 
    //}

    public enum EnumDataExecuteState
    {
        None = -1,    //��
        NoDone = 0,  //δִ��
        Successed = 1,//ִ�гɹ�
        Failed = 2, //ִ��ʧ��
    }

    public enum EnumTransferMode
    {
        None = -1,
        Synchronization = 0,
        Asynchronism = 1,
    }

    public enum EnumImageMetaFormat
    {
        None = -1,
        Excel = 0,
        Text = 1,
        Xml=2,
    }

    public enum EnumTaskType
    {
        Upload = 1,            //���
        Download = 2,            //����
    }

    public class TreeColumnName
    {
        public static string COLUMNNAME_NAME = "nodeName";
        public static string COLUMNNAME_ID = "nodeID";
    }

    public enum EnumInputTaskType
    {
        FtpSchemeUpload = 1,     //FTP�������
        FtpCommonUpload = 2,     //SDE���
        SdeUpload = 3,           //FTP�������
    }

    public enum EnumSchemeType
    {
        FtpInputScheme = 1,
        SdeInputScheme = 2,
    }

    public enum EnumServerType
    {
        None = -1,
        FTP = 0, //FTP������
        File = 1,//�ļ�������
    }

    /// <summary>
    /// �ؼ�����������
    /// </summary>
    public enum EnumKeywordIndexType
    {
        /// <summary>
        /// ���
        /// </summary>
        Year = 0,
        /// <summary>
        /// ������
        /// </summary>
        Scale = 1,
        /// <summary>
        /// �ֱ���
        /// </summary>
        Resolution = 2
    }
    
    /// <summary>
    /// �����ļ�״̬�������Ԫ���ݱ���Ӧ���е�F_Flagֵ 
    /// </summary>
    public enum EnumDataState
    {
        /// <summary>
        /// -1�����ע��Ԫ���ݱ�δ�ϴ�ʵ�����ݣ���ʵ������δ�ϴ��ɹ���
        /// </summary>
        MetaDataRegister = -1,
        /// <summary>
        ///0�����ɹ�
        /// </summary>
        MetaDataArchiver=0,
        /// <summary>
        ///1��ɾ�����ݣ�����վ���ݣ�
        /// </summary>
        MetaDataDelete=1,
    }

    /// <summary>
    /// �����ļ�����
    /// </summary>
    public enum EnumDataFileProperty
    {
        /// <summary>
        /// Ĭ��
        /// </summary>
        NormalFile = 0,

        /// <summary>
        /// �������ļ�
        /// </summary>
        MainDataFile = 1,

        /// <summary>
        /// ����ͼ�ļ�
        /// </summary>
        QuickImageFile = 2,

        /// <summary>
        /// Ԫ�����ļ�
        /// </summary>
        MetadataFile = 3,

        /// <summary>
        /// Ĵָͼ
        /// </summary>
        MZTFILE = 4,

        /// <summary>
        /// �����ļ�
        /// </summary>
        IndexFile = 5
    }

    #endregion

    public class FileAttribute
    {
        /// <summary>
        /// ���ļ�
        /// </summary>
        public  const string FILE_MAIN = "MainFile";
        /// <summary>
        /// ����ͼ
        /// </summary>
        public const string SNAPSHOT = "Snapshot";
        /// <summary>
        /// Ԫ����
        /// </summary>
        public const string METADATA = "Metadata";
        /// <summary>
        /// Ĵָͼ
        /// </summary>
        public const string MZTFILE = "MZTFile";
        /// <summary>
        /// �����ļ�
        /// </summary>
        public const string INDEXFILE = "IndexFile";
        /// <summary>
        /// �ο�Ԫ�����ļ�
        /// </summary>
        public const string CKMETAFILE = "CKMetaFile";

    }

    public class ProgressEventArgs : EventArgs
    {
        public int taskid;
        public int count;

        public ProgressEventArgs(int taskid, int count)
        {
            this.taskid = taskid;
            this.count = count;
        }
    }

    //public class ServerFileEventArgs : EventArgs
    //{
    //    public EnumDataExecuteState Status;
    //    public string filename;
    //    public Int64 size;

    //    public ServerFileEventArgs(EnumDataExecuteState status, string filename, Int64 size)
    //    {
    //        this.Status = status;
    //        this.filename = filename;
    //        this.size = size;
    //    }
    //}

    //public class ServerProgressEventArgs : EventArgs
    //{
    //    public long Count;
    //    public long CurrentFile;
    //    public string FileName;
    //    public long Length;
    //    public long Position;
    //    public EnumDataExecuteState Status;
    //    public long TotalFiles;
    //    public string TransferRate;

    //    public ServerProgressEventArgs()
    //    {

    //    }
    //}

    #region �����¼�����

    /// <summary>
    /// ����ִ���¼�
    /// </summary>
    /// <param name="task"></param>
    /// <param name="msg"></param>
    //public delegate void TaskExecuteEventHandler(Task task, string msg);

    #endregion

    #region ���������¼�����

    /// <summary>
    /// ��������ִ���¼�
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskDataExecuteEventHandler(TaskData taskData, string msg);

    /// <summary>
    /// ���������ļ������¼�
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskFileTransferEventHandler(TaskData taskData, ServerFileEventArgs e);
    //public delegate void TaskPublicFileTransferEventHandler(UploadTaskPackage taskPackage, ServerFileEventArgs e);

    /// <summary>
    /// ���������ļ��������
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskFileTransferProgressEventHandler(TaskData taskData, ServerProgressEventArgs e);
    //public delegate void TaskPublicFileTransferProgressEventHandler(UploadTaskPackage taskPackage, ServerProgressEventArgs e);

    /// <summary>
    /// �������ݲ�����Ϣ
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskDataProcessInfoEventHandler(TaskData taskData, string msg);

    /// <summary>
    /// �������ݰ�������Ϣ
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskPackageProcessInfoEventHandler(UploadTaskPackage taskPackage, string msg);

    /// <summary>
    /// �������ݻع���ʼ
    /// </summary>
    /// <param name="taskData"></param>
    //public delegate void BeginRollbackTaskDataEventHandler(TaskData taskData);
    /// <summary>
    /// �������ݻع�����
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="succeed">�Ƿ�ɹ�</param>
    //public delegate void EndRollbackTaskDataEventHandler(TaskData taskData, bool succeed);
    /// <summary>
    /// �������ݻع�����
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="e">������Ϣ</param>
    //public delegate void RollbackTaskDataProgressEventHandler(TaskData taskData, int position, int max, string msg);

    /// <summary>
    /// ����ع�����¼�
    /// </summary>
    /// <param name="task">����</param>
    /// <param name="totalCount">�賷����������</param>
    /// <param name="failedCount">ʧ��������</param>
    //public delegate void RollbackTaskFinishEventHandler(Task task, int totalCount, int failedCount);

    #endregion


    public delegate void AfterShowTaskListEventHandler(EnumExecuteState state, int taskCount, int runningTaskCount);

    public delegate void SetButtonsEnabledEventHandler(bool start, bool pause, bool stop, bool delete, bool rollback, bool rollbackFailed, bool rollbackAll, bool validate, bool rescan);
    
    //public delegate void ServerFileEventHandler(object o, ServerFileEventArgs e);
    /// <summary>
    /// �������ϴ��������¼�
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    /// <summary>
    /// �����������¼�
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    /// <param name="state"> </param>
    //public delegate void ServerProgressHandler(object o, ServerProgressEventArgs e);

    ///// <summary>
    ///// �������ֵ����
    ///// </summary>
    ///// <param name="Count">���ֵ</param>
    //public delegate void ProgressCount(ProgressEventArgs eventArgs);

    ///// <summary>
    ///// ����λ�ô���
    ///// </summary>
    ///// <param name="Position">��ǰ����ֵ</param>
    //public delegate void ProgressPosition(ProgressEventArgs eventArgs);

    ///// <summary>
    ///// ������Ϣ����
    ///// </summary>
    ///// <param name="Info">������Ϣ</param>
    //public delegate void ProgressInfo(string info);
    public delegate void ShowTaskListDelegate(EnumExecuteState state);
    /// <summary>
    /// ��־��Ϣ�����仯
    /// </summary>
    /// <param name="newLogString"></param>
    public delegate void OutputLogsChange(string newLogString);

    /// <summary>
    /// ��ʼִ��
    /// </summary>
    public delegate void BeginExecuteDelegate(int id, string msg);

    /// <summary>
    /// ִ�н���
    /// </summary>
    public delegate void EndExecuteDelegate(int id, string msg);

    /// <summary>
    /// ��֤�Ƿ��滻��������
    /// </summary>
    /// <returns></returns>
   // public delegate bool CheckReplaceDataDelegate(int taskID, string dataName);

    /// <summary>
    /// ��������ִ��״̬�����仯
    /// </summary>
    /// <param name="enumDataExecuteState"></param>
    public delegate void DataStateChangedDelegate(int dataid, EnumDataExecuteState enumDataExecuteState);

    public class FTPConnectionException : Exception
    {
        public override string Message
        {
            get
            {
                return "FTP����ʧ��";
            }
        }
    }

    //ɾ�������¼�
    public delegate void DeleteProgressEventHandler(DeleteDataProgressEventArgs e);
    public delegate void AfterDeleteDatasEventHandler(IList<RegisterKey> succeedIDs);
    public delegate void BeginDeleteDataEventHandler(string dataName);
    public delegate void EndDeleteDataEventHandler(string dataName, bool succeed);

    /// <summary>
    /// ɾ�������¼�
    /// </summary>
    public class DeleteDataProgressEventArgs : EventArgs
    {
        public DeleteDataProgressEventArgs()
        {
        }

        public int TotalCount = 0;
        public int CurrentData = 0;
        public string CurrentDataName = string.Empty;
        public EnumDeleteItem CurrentItem = EnumDeleteItem.None;
        public EnumDeleteAction CurrentAction = EnumDeleteAction.None;
        public string FileName = string.Empty;
        public int Postion = 0;
        public int Maximum = 0;
    }

    ////�ܼ�ö��
    //public enum EConfidentialClass
    //{
    //    ���� = 0, //UnKnownConfidential = 0,//δ֪
    //    ���ܳ���,
    //    ����, //LitileConfidential = 1,//����
    //    ����, //Confidential = 2,//����
    //    ����, //StrictlyConfidential = 3,//����  
    //    �ڲ�,
    //}

    ///// <summary>
    ///// ������������
    ///// </summary>
    //public enum EnumDataType
    //{
    //    None = 0,
    //    DEM = 1,
    //    DOM,
    //    DLG,
    //    DRG,
    //    DTM = 5,
    //    DigitalNavi,     //���뺽Ƭ
    //    SatelliteImages, //����Ӱ��
    //    ScanDiaoHuiPian = 8, //ɨ�����
    //    ControlPoint,     //���Ƶ�ɹ�
    //    /// <summary>
    //    /// ֽ�ʵ���Ƭ
    //    /// </summary>
    //    ZZDHP = 10,
    //    /// <summary>
    //    /// ��ƬͲ
    //    /// </summary>
    //    ImageTube = 11,
    //    /// <summary>
    //    /// ֽ�ʵ���ͼ
    //    /// </summary>
    //    ZZDXT = 12,
    //    /// <summary>
    //    /// ��ҵ��������
    //    /// </summary>
    //    KZZL = 13,
    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    HD = 14,
    //}   

    public enum EnumDeleteItem
    {
        None = 0,
        /// <summary>
        /// ע����Ϣ
        /// </summary>
        RegisterInfo = 1,
        /// <summary>
        /// �����ļ�
        /// </summary>
        DataFile = 2,
        /// <summary>
        /// Ԫ����

        /// </summary>
        Metadata = 3,
        /// <summary>
        /// ����ͼ

        /// </summary>
        SnapShot = 4
    }

    public enum EnumDeleteAction
    {
        None = 0,
        /// <summary>
        /// ��ʼ
        /// </summary>
        Beginning = 1,
        /// <summary>
        /// ���ڽ���
        /// </summary>
        Processing = 2,
        /// <summary>
        /// �ɹ�
        /// </summary>
        Succeed = 3,
        /// <summary>
        /// ʧ��
        /// </summary>
        Failed = 4
    }


    /// <summary>
    /// �洢����ģ�����ݽڵ��ͼ������
    /// </summary>
    public enum EnumDataTypeLayer
    {
        layer_point,    //��ͼ��
        layer_polygon,  //��ͼ��
        layer_none      //��
    }


    ///// <summary>
    ///// ��չ��Ϣ�򷵻ؽ��
    ///// </summary>
    //public enum EnumMessageBoxExResult
    //{
    //    /// <summary>
    //    /// δ����
    //    /// </summary>
    //    Undefined = 0,
    //    /// <summary>
    //    /// ��
    //    /// </summary>
    //    Yes = 1,
    //    /// <summary>
    //    /// ȫ��
    //    /// </summary>
    //    YesAll = 2,
    //    /// <summary>
    //    /// ��
    //    /// </summary>
    //    No = 3,
    //    /// <summary>
    //    /// ȫ��
    //    /// </summary>
    //    NoAll = 4,
    //    /// <summary>
    //    /// ȡ��
    //    /// </summary>
    //    Cancel = 5
    //}

    public enum EnumExecuteState
    {
        None = -1,
        NoExecute = 0, //δִ��
        Executing = 1, //ִ����
        ExecuteFailed = 2,       //ִ��ʧ��
        ExecuteSuccessful = 3,   //ִ�гɹ� 
    }
}
