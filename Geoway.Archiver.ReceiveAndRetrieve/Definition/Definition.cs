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
        public const string Alias_F_REDATAID = "关联数据ID";
    }

    public delegate void BeginImportEventHandler();

    public delegate void EndImportEventHandler();

    //public delegate void ShowDataInfoEventHandler(Task task);
    #region 枚举定义
    /// <summary>
    /// 介质资料载体类型
    /// </summary>
    public enum EnumMediumType
    {
        enumUnknown = 0,
        /// <summary>
        /// CD光盘
        /// </summary>
        EnumCD,
        /// <summary>
        /// DVD光盘
        /// </summary>
        EnumDVD,
        /// <summary>
        /// 纸质
        /// </summary>
        EnumPaper,
    }
    
    public enum EnumMetaDataType
    {
        /// <summary>
        /// 系统维护信息
        /// </summary>
        EnumSystem,
        /// <summary>
        /// 固有属性
        /// </summary>
        EnumFixed,
        /// <summary>
        /// 扩展属性
        /// </summary>
        EnumExtensional,
    }
    /// <summary>
    /// 数据对象类型
    /// </summary>
    //public enum EnumObjectType : int
    //{
    //    /// <summary>
    //    /// 文件
    //    /// </summary>
    //    DataFile = 0,
    //    /// <summary>
    //    /// 文件夹
    //    /// </summary>
    //    DataFolder = 1,
    //    /// <summary>
    //    /// 数据包
    //    /// </summary>
    //    DataPackage = 2,
    //    /// <summary>
    //    /// 数据包类型
    //    /// </summary>
    //    DataPackageType = 3,
    //    /// <summary>
    //    /// 虚拟数据包
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
    /// 验证结果
    /// </summary>
    public enum EnumValidateResult
    {
        /// <summary>
        /// 未检查
        /// </summary>
        None = 0,
        /// <summary>
        /// 通过
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// 不通过
        /// </summary>
        Faild = 2
    }

    /// <summary>
    /// 创建任务扫描模式
    /// </summary>
    public enum EnumScanMode
    {
        /// <summary>
        /// 无限制
        /// </summary>
        None = 0,
        /// <summary>
        /// 当前目录
        /// </summary>
        CurrentDirectory = 1,
        /// <summary>
        /// 子目录
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
    //    NoExecute = 0, //未执行
    //    Executing = 1, //执行中
    //    ExecuteFailed = 2,       //执行失败
    //    ExecuteSuccessful = 3,   //执行成功 
    //}

    public enum EnumDataExecuteState
    {
        None = -1,    //无
        NoDone = 0,  //未执行
        Successed = 1,//执行成功
        Failed = 2, //执行失败
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
        Upload = 1,            //入库
        Download = 2,            //出库
    }

    public class TreeColumnName
    {
        public static string COLUMNNAME_NAME = "nodeName";
        public static string COLUMNNAME_ID = "nodeID";
    }

    public enum EnumInputTaskType
    {
        FtpSchemeUpload = 1,     //FTP方案入库
        FtpCommonUpload = 2,     //SDE入库
        SdeUpload = 3,           //FTP常规入库
    }

    public enum EnumSchemeType
    {
        FtpInputScheme = 1,
        SdeInputScheme = 2,
    }

    public enum EnumServerType
    {
        None = -1,
        FTP = 0, //FTP服务器
        File = 1,//文件服务器
    }

    /// <summary>
    /// 关键字索引类型
    /// </summary>
    public enum EnumKeywordIndexType
    {
        /// <summary>
        /// 年份
        /// </summary>
        Year = 0,
        /// <summary>
        /// 比例尺
        /// </summary>
        Scale = 1,
        /// <summary>
        /// 分辨率
        /// </summary>
        Resolution = 2
    }
    
    /// <summary>
    /// 数据文件状态。若针对元数据表：对应表中的F_Flag值 
    /// </summary>
    public enum EnumDataState
    {
        /// <summary>
        /// -1：入库注册元数据表，未上传实体数据（或实体数据未上传成功）
        /// </summary>
        MetaDataRegister = -1,
        /// <summary>
        ///0：入库成功
        /// </summary>
        MetaDataArchiver=0,
        /// <summary>
        ///1：删除数据（回收站数据）
        /// </summary>
        MetaDataDelete=1,
    }

    /// <summary>
    /// 数据文件属性
    /// </summary>
    public enum EnumDataFileProperty
    {
        /// <summary>
        /// 默认
        /// </summary>
        NormalFile = 0,

        /// <summary>
        /// 主数据文件
        /// </summary>
        MainDataFile = 1,

        /// <summary>
        /// 快视图文件
        /// </summary>
        QuickImageFile = 2,

        /// <summary>
        /// 元数据文件
        /// </summary>
        MetadataFile = 3,

        /// <summary>
        /// 拇指图
        /// </summary>
        MZTFILE = 4,

        /// <summary>
        /// 索引文件
        /// </summary>
        IndexFile = 5
    }

    #endregion

    public class FileAttribute
    {
        /// <summary>
        /// 主文件
        /// </summary>
        public  const string FILE_MAIN = "MainFile";
        /// <summary>
        /// 快视图
        /// </summary>
        public const string SNAPSHOT = "Snapshot";
        /// <summary>
        /// 元数据
        /// </summary>
        public const string METADATA = "Metadata";
        /// <summary>
        /// 拇指图
        /// </summary>
        public const string MZTFILE = "MZTFile";
        /// <summary>
        /// 索引文件
        /// </summary>
        public const string INDEXFILE = "IndexFile";
        /// <summary>
        /// 参考元数据文件
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

    #region 任务事件定义

    /// <summary>
    /// 任务执行事件
    /// </summary>
    /// <param name="task"></param>
    /// <param name="msg"></param>
    //public delegate void TaskExecuteEventHandler(Task task, string msg);

    #endregion

    #region 任务数据事件定义

    /// <summary>
    /// 任务数据执行事件
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskDataExecuteEventHandler(TaskData taskData, string msg);

    /// <summary>
    /// 任务数据文件传输事件
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskFileTransferEventHandler(TaskData taskData, ServerFileEventArgs e);
    //public delegate void TaskPublicFileTransferEventHandler(UploadTaskPackage taskPackage, ServerFileEventArgs e);

    /// <summary>
    /// 任务数据文件传输进度
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskFileTransferProgressEventHandler(TaskData taskData, ServerProgressEventArgs e);
    //public delegate void TaskPublicFileTransferProgressEventHandler(UploadTaskPackage taskPackage, ServerProgressEventArgs e);

    /// <summary>
    /// 任务数据操作信息
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskDataProcessInfoEventHandler(TaskData taskData, string msg);

    /// <summary>
    /// 任务数据包操作信息
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="msg"></param>
    //public delegate void TaskPackageProcessInfoEventHandler(UploadTaskPackage taskPackage, string msg);

    /// <summary>
    /// 任务数据回滚开始
    /// </summary>
    /// <param name="taskData"></param>
    //public delegate void BeginRollbackTaskDataEventHandler(TaskData taskData);
    /// <summary>
    /// 任务数据回滚结束
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="succeed">是否成功</param>
    //public delegate void EndRollbackTaskDataEventHandler(TaskData taskData, bool succeed);
    /// <summary>
    /// 任务数据回滚进度
    /// </summary>
    /// <param name="taskData"></param>
    /// <param name="e">进度信息</param>
    //public delegate void RollbackTaskDataProgressEventHandler(TaskData taskData, int position, int max, string msg);

    /// <summary>
    /// 任务回滚完成事件
    /// </summary>
    /// <param name="task">任务</param>
    /// <param name="totalCount">需撤销数据总数</param>
    /// <param name="failedCount">失败数据数</param>
    //public delegate void RollbackTaskFinishEventHandler(Task task, int totalCount, int failedCount);

    #endregion


    public delegate void AfterShowTaskListEventHandler(EnumExecuteState state, int taskCount, int runningTaskCount);

    public delegate void SetButtonsEnabledEventHandler(bool start, bool pause, bool stop, bool delete, bool rollback, bool rollbackFailed, bool rollbackAll, bool validate, bool rescan);
    
    //public delegate void ServerFileEventHandler(object o, ServerFileEventArgs e);
    /// <summary>
    /// 服务器上传或下载事件
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    /// <summary>
    /// 服务器进度事件
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    /// <param name="state"> </param>
    //public delegate void ServerProgressHandler(object o, ServerProgressEventArgs e);

    ///// <summary>
    ///// 进度最大值代理
    ///// </summary>
    ///// <param name="Count">最大值</param>
    //public delegate void ProgressCount(ProgressEventArgs eventArgs);

    ///// <summary>
    ///// 进度位置代理
    ///// </summary>
    ///// <param name="Position">当前进度值</param>
    //public delegate void ProgressPosition(ProgressEventArgs eventArgs);

    ///// <summary>
    ///// 进度信息代理
    ///// </summary>
    ///// <param name="Info">进度信息</param>
    //public delegate void ProgressInfo(string info);
    public delegate void ShowTaskListDelegate(EnumExecuteState state);
    /// <summary>
    /// 日志信息发生变化
    /// </summary>
    /// <param name="newLogString"></param>
    public delegate void OutputLogsChange(string newLogString);

    /// <summary>
    /// 开始执行
    /// </summary>
    public delegate void BeginExecuteDelegate(int id, string msg);

    /// <summary>
    /// 执行结束
    /// </summary>
    public delegate void EndExecuteDelegate(int id, string msg);

    /// <summary>
    /// 验证是否替换已有数据
    /// </summary>
    /// <returns></returns>
   // public delegate bool CheckReplaceDataDelegate(int taskID, string dataName);

    /// <summary>
    /// 任务数据执行状态发生变化
    /// </summary>
    /// <param name="enumDataExecuteState"></param>
    public delegate void DataStateChangedDelegate(int dataid, EnumDataExecuteState enumDataExecuteState);

    public class FTPConnectionException : Exception
    {
        public override string Message
        {
            get
            {
                return "FTP连接失败";
            }
        }
    }

    //删除数据事件
    public delegate void DeleteProgressEventHandler(DeleteDataProgressEventArgs e);
    public delegate void AfterDeleteDatasEventHandler(IList<RegisterKey> succeedIDs);
    public delegate void BeginDeleteDataEventHandler(string dataName);
    public delegate void EndDeleteDataEventHandler(string dataName, bool succeed);

    /// <summary>
    /// 删除进度事件
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

    ////密级枚举
    //public enum EConfidentialClass
    //{
    //    公开 = 0, //UnKnownConfidential = 0,//未知
    //    秘密长期,
    //    秘密, //LitileConfidential = 1,//秘密
    //    机密, //Confidential = 2,//机密
    //    绝密, //StrictlyConfidential = 3,//绝密  
    //    内部,
    //}

    ///// <summary>
    ///// 数据资料类型
    ///// </summary>
    //public enum EnumDataType
    //{
    //    None = 0,
    //    DEM = 1,
    //    DOM,
    //    DLG,
    //    DRG,
    //    DTM = 5,
    //    DigitalNavi,     //数码航片
    //    SatelliteImages, //卫星影像
    //    ScanDiaoHuiPian = 8, //扫描调绘
    //    ControlPoint,     //控制点成果
    //    /// <summary>
    //    /// 纸质调绘片
    //    /// </summary>
    //    ZZDHP = 10,
    //    /// <summary>
    //    /// 航片筒
    //    /// </summary>
    //    ImageTube = 11,
    //    /// <summary>
    //    /// 纸质地形图
    //    /// </summary>
    //    ZZDXT = 12,
    //    /// <summary>
    //    /// 外业控制资料
    //    /// </summary>
    //    KZZL = 13,
    //    /// <summary>
    //    /// 磁盘
    //    /// </summary>
    //    HD = 14,
    //}   

    public enum EnumDeleteItem
    {
        None = 0,
        /// <summary>
        /// 注册信息
        /// </summary>
        RegisterInfo = 1,
        /// <summary>
        /// 数据文件
        /// </summary>
        DataFile = 2,
        /// <summary>
        /// 元数据

        /// </summary>
        Metadata = 3,
        /// <summary>
        /// 快视图

        /// </summary>
        SnapShot = 4
    }

    public enum EnumDeleteAction
    {
        None = 0,
        /// <summary>
        /// 开始
        /// </summary>
        Beginning = 1,
        /// <summary>
        /// 正在进行
        /// </summary>
        Processing = 2,
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 3,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 4
    }


    /// <summary>
    /// 存储数据模型数据节点的图层类型
    /// </summary>
    public enum EnumDataTypeLayer
    {
        layer_point,    //点图层
        layer_polygon,  //面图层
        layer_none      //无
    }


    ///// <summary>
    ///// 扩展消息框返回结果
    ///// </summary>
    //public enum EnumMessageBoxExResult
    //{
    //    /// <summary>
    //    /// 未定义
    //    /// </summary>
    //    Undefined = 0,
    //    /// <summary>
    //    /// 是
    //    /// </summary>
    //    Yes = 1,
    //    /// <summary>
    //    /// 全是
    //    /// </summary>
    //    YesAll = 2,
    //    /// <summary>
    //    /// 否
    //    /// </summary>
    //    No = 3,
    //    /// <summary>
    //    /// 全否
    //    /// </summary>
    //    NoAll = 4,
    //    /// <summary>
    //    /// 取消
    //    /// </summary>
    //    Cancel = 5
    //}

    public enum EnumExecuteState
    {
        None = -1,
        NoExecute = 0, //未执行
        Executing = 1, //执行中
        ExecuteFailed = 2,       //执行失败
        ExecuteSuccessful = 3,   //执行成功 
    }
}
