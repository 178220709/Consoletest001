using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using System.IO;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Interface;
using System.Windows.Forms;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 任务信息数据库操作类
    /// </summary>
    public class TaskInfoDAL : DALBase<TaskInfoDAL>
    {
        #region 任务信息表结构
        public const string TABLE_NAME = "TBARC_TASK";
        public const string FLD_NAME_F_TASKID = "F_TASKID";
        public const string FLD_NAME_F_TASKNAME = "F_TASKNAME";
        public const string FLD_NAME_F_TASKDESCRIBE = "F_TASKDESCRIBE";
        public const string FLD_NAME_F_TASKDIRECTORY = "F_TASKDIRECTORY";
        public const string FLD_NAME_F_INPUTOROUTPUT = "F_INPUTOROUTPUT";
        public const string FLD_NAME_F_SERVERID = "F_SERVERID";
        public const string FLD_NAME_F_SERVERPREPATH = "F_SERVERPREPATH";
        public const string FLD_NAME_F_USERID = "F_USERID";
        public const string FLD_NAME_F_DEFINETIME = "F_DEFINETIME";
        public const string FLD_NAME_F_BEGINTIME = "F_BEGINTIME";
        public const string FLD_NAME_F_ENDTIME = "F_ENDTIME";
        public const string FLD_NAME_F_CATALOGID = "F_CATALOGID";
        public const string FLD_NAME_F_TASKSTATE = "F_TASKSTATE";
        public const string FLD_NAME_F_TASKCOUNT = "F_TASKCOUNT";
        public const string FLD_NAME_F_DATAAMOUNT = "F_DATAAMOUNT";
        public const string FLD_NAME_F_ISWITHMETA = "F_ISWITHMETA";
        public const string FLD_NAME_F_ISWITHDATA = "F_ISWITHDATA";
        public const string FLD_NAME_F_ISWITHSNAPSHOT = "F_ISWITHSNAPSHOT";
        public const string FLD_NAME_F_SNAPSHOTSETTING = "F_SNAPSHOTSETTING";
        public const string FLD_NAME_F_METADATASETTING = "F_METADATASETTING";
        public const string FLD_NAME_F_ISTIMING = "F_ISTIMING";
        public const string FLD_NAME_F_FLAG = "F_FLAG";
        public const string FLD_NAME_F_PROGRESSCOUNT = "F_PROGRESSCOUNT";
        public const string FLD_NAME_F_SDEPROGRESSCOUNT = "F_SDEPROGRESSCOUNT";
        public const string FLD_NAME_F_LOCALPATH = "F_LOCALPATH";
        public const string FLD_NAME_F_ISHANDMETA = "F_ISHANDMETA";   //李海永添加 是否手动录入元数据
        public const string FLD_NAME_F_EXECUTELOG = "F_EXECUTELOG";   //任务日志
        public const string FLD_NAME_F_VALIDATERESULT = "F_VALIDATERESULT";   //验证
        public const string FLD_NAME_F_SCANMODE = "F_SCANMODE";    //扫描模式

        #endregion

        public static TaskInfoDAL singleton = new TaskInfoDAL();

        #region 属性
        private int taskID = -9999;
        /// <summary>
        /// 任务ID
        /// </summary>
        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        private string taskName;
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        private string taskDescribe;
        /// <summary>
        /// 任务描述
        /// </summary>
        public string TaskDescribe
        {
            get { return taskDescribe; }
            set { taskDescribe = value; }
        }

        private string taskDirectory = string.Empty;
        /// <summary>
        /// 任务目录
        /// </summary>
        public string Directory
        {
            get { return taskDirectory; }
            set { taskDirectory = value; }
        }


        private int inputOrOutput;
        /// <summary>
        /// 入库/出库
        /// </summary>
        public int InputOrOutput
        {
            get { return inputOrOutput; }
            set { inputOrOutput = value; }
        }

        private int serverID;
        /// <summary>
        /// 存储节点ID
        /// </summary>
        public int ServerID
        {
            get { return serverID; }
            set { serverID = value; }
        }

        private string prePath;
        /// <summary>
        /// 服务器前缀路径
        /// </summary>
        public string PrePath
        {
            get { return prePath; }
            set { prePath = value; }
        }

        private string localPath;
        /// <summary>
        /// 任务路径
        /// </summary>
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; }
        }

        private int userID;
        /// <summary>
        /// 创建用户
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private DateTime defineTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DefineTime
        {
            get { return defineTime; }
            set { defineTime = value; }
        }

        private DateTime beginTime;
        /// <summary>
        /// 开始执行时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; }
        }

        private DateTime endTime;
        /// <summary>
        /// 任务完成时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private int isTiming = 0;
        /// <summary>
        /// 是否立即执行
        /// </summary>
        public int IsTiming
        {
            get { return isTiming; }
            set { isTiming = value; }
        }

        private int taskState;
        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }

        private Int64 dataAmount;
        /// <summary>
        /// 任务数据量（Byte）
        /// </summary>
        public Int64 DataAmount
        {
            get { return dataAmount; }
            set { dataAmount = value; }
        }

        private int dataCount;
        /// <summary>
        /// ？？？
        /// </summary>
        public int DataCount
        {
            get { return dataCount; }
            set { dataCount = value; }
        }

        private int catalogID;
        /// <summary>
        /// 归档任务数据节点/订单下载任务关联订单ID
        /// </summary>
        public int CatalogID
        {
            get { return catalogID; }
            set { catalogID = value; }
        }

        private int isWithMeta;
        /// <summary>
        /// 是否入库元数据
        /// </summary>
        public int IsWithMeta
        {
            get { return isWithMeta; }
            set { isWithMeta = value; }
        }
        //是否手动录入元数据
        private int isHandMeta;
        /// <summary>
        /// 是否手动录入元数据
        /// </summary>
        public int IsHandMeta
        {
            get { return isHandMeta; }
            set { isHandMeta = value; }
        }

        private int isWithData;
        /// <summary>
        /// 是否入库数据文件
        /// </summary>
        public int IsWithData
        {
            get { return isWithData; }
            set { isWithData = value; }
        }

        private int isWithSnapShot;
        /// <summary>
        /// 是否入库快视图
        /// </summary>
        public int IsWithSnapShot
        {
            get { return isWithSnapShot; }
            set { isWithSnapShot = value; }
        }

        private StringBuilder log;
        /// <summary>
        /// 任务执行日志
        /// </summary>
        public StringBuilder Log
        {
            get { return log; }
            set { log = value; }
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


        private int flag = 1;
        /// <summary>
        /// 状态标志
        /// </summary>
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        int progressCount = 0;
        /// <summary>
        /// 任务进度（0～100）
        /// </summary>
        public int ProgressCount
        {
            get { return progressCount; }
            set { progressCount = value; }
        }

        int sdeProgressCount = 0;
        /// <summary>
        /// 
        /// </summary>
        public int SDEProgressCount
        {
            get { return sdeProgressCount; }
            set { sdeProgressCount = value; }
        }

        private EnumValidateResult _validateResult = EnumValidateResult.None;
        /// <summary>
        /// 验证结果
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private EnumScanMode _scanMode = 0;
        /// <summary>
        /// 入库任务扫描模式
        /// </summary>
        public EnumScanMode ScanMode
        {
            get { return _scanMode; }
            set { _scanMode = value; }
        }

        #endregion

        #region 重载函数
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool Insert()
        {
            if (taskID <= 0)
            {
                taskID = DBHelper.GlobalDBHelper.GetNextValidID(TABLE_NAME, FLD_NAME_F_TASKID);
            }

            string sqlStatement = string.Empty;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, taskID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKNAME, taskName, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKDESCRIBE, taskDescribe, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKDIRECTORY, taskDirectory, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_INPUTOROUTPUT, inputOrOutput, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, serverID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SERVERPREPATH, prePath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_LOCALPATH, localPath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_USERID, userID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_CATALOGID, catalogID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_BEGINTIME, beginTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_ENDTIME, endTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_DEFINETIME, defineTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_ISTIMING, isTiming, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKSTATE, taskState, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_DATAAMOUNT, dataAmount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHMETA, isWithMeta, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_ISHANDMETA, isHandMeta, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHDATA, isWithData, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHSNAPSHOT, isWithSnapShot, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FLAG, flag, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_PROGRESSCOUNT, progressCount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SDEPROGRESSCOUNT, sdeProgressCount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_VALIDATERESULT, (int)_validateResult, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SCANMODE, (int)_scanMode, EnumDBFieldType.FTNumber));

            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
            }
            catch (Exception ex)
            {
                return false;
            }
            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0 ? true : false;

            if (bSuccess)
            {
                string strFilter = FLD_NAME_F_TASKID + " = " + taskID;

                Byte[] bytes = null;
                //快视图
                bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_snapshotSetting);
                if (bytes != null && bytes.Length > 0)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_SNAPSHOTSETTING, ref bytes);
                }

                //元数据
                bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_metadataSetting);
                if (bytes != null && bytes.Length > 0)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_METADATASETTING, ref bytes);
                }

            }

            return bSuccess;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            string sqlStatement;
            sqlStatement = "DELETE FROM " + TABLE_NAME + " WHERE " + FLD_NAME_F_TASKID + " = " + "'" + taskID + "'";

            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) == 1 ? true : false;
            return bSuccess;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public override bool Update()
        {
            bool bSuccess;
            string sqlStatement;
            string strFilter = FLD_NAME_F_TASKID + " = " + "'" + taskID + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, taskID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKNAME, taskName, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKDESCRIBE, taskDescribe, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKDIRECTORY, taskDirectory, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_INPUTOROUTPUT, inputOrOutput, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, serverID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SERVERPREPATH, prePath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_LOCALPATH, localPath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_USERID, userID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_CATALOGID, catalogID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_BEGINTIME, beginTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_ENDTIME, endTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_DEFINETIME, defineTime, EnumDBFieldType.FTDatetime));

            items.Add(new DBFieldItem(FLD_NAME_F_ISTIMING, isTiming, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_TASKSTATE, TaskState, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_DATAAMOUNT, dataAmount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHMETA, isWithMeta, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ISHANDMETA, isHandMeta, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHDATA, isWithData, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ISWITHSNAPSHOT, isWithSnapShot, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FLAG, flag, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_PROGRESSCOUNT, progressCount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SDEPROGRESSCOUNT, sdeProgressCount, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_VALIDATERESULT, (int)_validateResult, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_SCANMODE, (int)_scanMode, EnumDBFieldType.FTNumber));

            sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter,DBHelper.GlobalDBHelper);

            try
            {
                bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) == 1 ? true : false;

                if (bSuccess)
                {
                    strFilter = FLD_NAME_F_TASKID + " = " + taskID;

                    Byte[] bytes = null;
                    bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_snapshotSetting);
                    if (bytes != null && bytes.Length > 0)
                    {
                        DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_SNAPSHOTSETTING, ref bytes);
                    }

                    bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_metadataSetting);
                    if (bytes != null && bytes.Length > 0)
                    {
                        DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_METADATASETTING, ref bytes);
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return bSuccess;
        }


        // <summary>
        /// 选择
        /// </summary>
        /// <returns>所有任务列表</returns>
        public override IList<TaskInfoDAL> Select()
        {
            IList<TaskInfoDAL> pList = new List<TaskInfoDAL>();
            DataTable dtResult = SelectEx();
            pList = Translate(dtResult);
            return pList;
        }

        public override string ToString()
        {
            return taskName;
        }
        #endregion

        #region 扩展属性查询

        public TaskInfoDAL Select(int taskid)
        {
            IList<TaskInfoDAL> pList = new List<TaskInfoDAL>();

            string strFilter = FLD_NAME_F_TASKID + " = " + taskid;
            DataTable dtResult = Select2(strFilter);
            pList = Translate(dtResult);
            if (pList.Count > 0)
            {
                return pList[0];
            }
            else
                return null;
        }

        public DataTable SelectEx()
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " order by " + FLD_NAME_F_TASKID;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        public DataTable Select2(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " where " + strFilter;
            try
            {
                DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
                return dtResult;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }


        }
        #endregion

        #region 日志管理

        /// <summary>
        /// 读取任务日志到文件
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <param name="logFileName">日志文件名</param>
        public void ReadTaskLog(int id, string logFileName)
        {
            string filter = FLD_NAME_F_TASKID + "=" + id;
            try
            {
                byte[] buf = DBHelper.GlobalDBHelper.ReadBlob2Bytes(filter, TABLE_NAME, FLD_NAME_F_EXECUTELOG);
                if (buf != null && buf.Length > 0)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(logFileName, FileMode.Create);
                        fs.Write(buf, 0, buf.Length);
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                            fs.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// 存储任务日志
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <param name="logFileName">日志文件名</param>
        public void SaveTaskLog(int id, string logFileName)
        {
            string filter = FLD_NAME_F_TASKID + "=" + id;
            try
            {
                DBHelper.GlobalDBHelper.SaveFile2Blob(logFileName, filter, TABLE_NAME, FLD_NAME_F_EXECUTELOG, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        #endregion

        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<TaskInfoDAL> Translate(DataTable dtResult)
        {
            string fullName = string.Empty;
            string path = Application.StartupPath;
            byte[] bytes = null;
            IList<TaskInfoDAL> list = new List<TaskInfoDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    TaskInfoDAL info = new TaskInfoDAL();
                    info.TaskID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_TASKID);
                    info.TaskName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TASKNAME);
                    info.taskDescribe = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TASKDESCRIBE);
                    info.taskDirectory = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TASKDIRECTORY);
                    info.ServerID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                    info.PrePath = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SERVERPREPATH);
                    info.LocalPath = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCALPATH);
                    info.UserID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_USERID);
                    info.DefineTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_DEFINETIME);
                    info.TaskState = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_TASKSTATE);
                    info.DataAmount = Int64.Parse(GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAAMOUNT));
                    info.CatalogID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_CATALOGID);
                    info.BeginTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_BEGINTIME);
                    info.EndTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_ENDTIME);
                    info.isTiming = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISTIMING);
                    info.isWithMeta = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISWITHMETA);
                    info.isHandMeta = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISHANDMETA);
                    info.isWithData = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISWITHDATA);
                    info.IsWithSnapShot = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISWITHSNAPSHOT);
                    info.Flag = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_FLAG);
                    info.ProgressCount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_PROGRESSCOUNT);
                    info.SDEProgressCount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SDEPROGRESSCOUNT);
                    info.ValidateResult = (EnumValidateResult)GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_VALIDATERESULT);
                    info.ScanMode = (EnumScanMode)GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SCANMODE);

                    bytes = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_TASKID + "=" + info.TaskID, TABLE_NAME, FLD_NAME_F_SNAPSHOTSETTING);
                    if (bytes != null && bytes.Length > 0)
                    {
                        info.SnapshotSetting = (FileUploadSetting)Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(bytes);
                    }

                    bytes = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_TASKID + "=" + info.TaskID, TABLE_NAME, FLD_NAME_F_METADATASETTING);
                    if (bytes != null && bytes.Length > 0)
                    {
                        info.MetadataSetting = (FileUploadSetting)Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(bytes);
                    }

                    list.Add(info);
                }
            }
            return list;
        }
        #endregion

        #region 内部方法

        private int GetByteLength(string str)
        {
            byte[] bytestr = System.Text.Encoding.Unicode.GetBytes(str);
            int len = 0;
            for (int i = 0; i < bytestr.GetLength(0); i++)
            {
                if (i % 2 == 0)
                {
                    len++;
                }
                else
                {
                    if (bytestr[i] > 0)
                    {
                        len++;
                    }
                }
            }
            return len;
        }

        /// <summary>
        /// 获取指定字节长度的中英文混合字符串
        /// </summary>
        private string GetString(string strValue, int maxlen)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(strValue);// 单字节字符长度
            int charLen = strValue.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > maxlen)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(strValue.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > maxlen)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == maxlen)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = strValue.Substring(0, pos);
            }
            else
                result = strValue;

            return result;
        }

        private string getSelectField()
        {
            string fields = FLD_NAME_F_TASKID + "," +
                FLD_NAME_F_TASKNAME + "," +
                FLD_NAME_F_TASKDESCRIBE + "," +
                FLD_NAME_F_TASKDIRECTORY + "," +
                FLD_NAME_F_SERVERID + "," +
                FLD_NAME_F_SERVERPREPATH + "," +
                FLD_NAME_F_USERID + "," +
                FLD_NAME_F_LOCALPATH + "," +
                //FLD_NAME_F_SCHEMEID + "," +
                FLD_NAME_F_INPUTOROUTPUT + "," +
                FLD_NAME_F_DEFINETIME + "," +
                FLD_NAME_F_ISTIMING + "," +
                FLD_NAME_F_CATALOGID + "," +
                FLD_NAME_F_TASKSTATE + "," +
                FLD_NAME_F_BEGINTIME + "," +
                FLD_NAME_F_DATAAMOUNT + "," +
                FLD_NAME_F_ENDTIME + "," +
                FLD_NAME_F_ISWITHMETA + "," +
                FLD_NAME_F_ISHANDMETA + "," +
                FLD_NAME_F_ISWITHDATA + "," +
                FLD_NAME_F_ISWITHSNAPSHOT + "," +
                FLD_NAME_F_SNAPSHOTSETTING + "," +
                FLD_NAME_F_METADATASETTING + "," +
                FLD_NAME_F_FLAG + "," +
                FLD_NAME_F_SDEPROGRESSCOUNT + "," +
                FLD_NAME_F_PROGRESSCOUNT + "," +
                FLD_NAME_F_DATAAMOUNT + "," +
                FLD_NAME_F_VALIDATERESULT + ',' + 
                FLD_NAME_F_SCANMODE;
            return fields;
        }

        #endregion
    }
}
