using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.DB.Public.Enum;
using EnumExecuteState = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 任务数据
    /// </summary>
    public class TaskDataDAL : DALBase<TaskDataDAL>
    {
        #region 任务数据信息表结构
        public const string TABLE_NAME = "TBIMG_TASKDATA";
        public const string FLD_NAME_F_DATAID = "F_DATAID";
        public const string FLD_NAME_F_DATANAME = "F_DATANAME";
        public const string FLD_NAME_F_TASKID = "F_TASKID";
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_INPUTSTATE = "F_DATASTATE";
        public const string FLD_NAME_F_INOROUT = "F_INOROUT";
        public const string FLD_NAME_F_PACPATH = "F_PACPATH";
        public const string FLD_NAME_F_PACKAGEID = "F_PACKAGEID";
        public const string FLD_NAME_F_OBJECTID = "F_OBJECTID";

        public const string FLD_NAME_F_VALIDATERESULT = "F_VALIDATERESULT";
        public const string FLD_NAME_F_VALIDATEINFO = "F_VALIDATEINFO";
        public const string FLD_NAME_F_METAFIELDVALUE = "F_METAFIELDVALUE";//记录元数据信息
        public const string FLD_NAME_F_TARGETREGISTERLAYERNAME = "f_registerlayername";

        public const string FLD_NAME_F_SNAPSHOTFILE = "F_SNAPSHOTFILE";
        public const string FLD_NAME_F_METADATAFILE = "F_METADATAFILE";
        public const string FLD_NAME_F_ISUPLOADSNAPSHOTFILE = "F_ISUPLOADSNAPSHOTFILE";
        public const string FLD_NAME_F_ISUPLOADMETADATAFILE = "F_ISUPLOADMETADATAFILE";
        //zqq+
        public const string FLD_NAME_F_METATABLENAME = "F_METATABLENAME";
        //
        #endregion

        public static TaskDataDAL Singleton = new TaskDataDAL();

        #region 私有字段
        private int _taskdataID;
        public int TaskDataID
        {
            get { return _taskdataID; }
            set { _taskdataID = value; }
        }
        private string _taskdataName;
        public string TaskDataName
        {
            get { return _taskdataName; }
            set { _taskdataName = value; }
        }
        private int _taskID;
        public int TaskID
        {
            get { return _taskID; }
            set { _taskID = value; }
        }

        private Int64 _dataSize;
        public Int64 DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        private string _pacPath;

        public string PacPath
        {
            get { return _pacPath; }
            set { _pacPath = value; }
        }

        private DataExecuteState _state;

        internal DataExecuteState State
        {
            get { return _state; }
            set { _state = value; }
        }

        private int _inOrOut;

        public int InOrOut
        {
            get { return _inOrOut; }
            set { _inOrOut = value; }
        }

        private int _objectID;

        public int ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        private int _packageID = 0;
        /// <summary>
        /// 任务数据包ID
        /// </summary>
        public int PackageID
        {
            get { return _packageID; }
            set { _packageID = value; }
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

        private string _snapshotFileFullName = string.Empty;
        /// <summary>
        /// 快视图文件名
        /// </summary>
        public string SnapshotFileFullName
        {
            get { return _snapshotFileFullName; }
            set { _snapshotFileFullName = value; }
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

        private string _metadataFileFullName = string.Empty;
        /// <summary>
        /// 元数据文件名
        /// </summary>
        public string MetadataFileFullName
        {
            get { return _metadataFileFullName; }
            set { _metadataFileFullName = value; }
        }

        private string _metaTableName;
        
        
        private EnumValidateResult _validateResult = EnumValidateResult.None;
        /// <summary>
        /// 验证结果
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private TaskDataValidateInfo _validateInfo = new TaskDataValidateInfo();
        /// <summary>
        /// 验证详细信息
        /// </summary>
        public TaskDataValidateInfo ValidateInfo
        {
            get { return _validateInfo; }
            set { _validateInfo = value; }
        }

        private string _registerLayerName = string.Empty;
        /// <summary>
        /// 注册图层名称
        /// </summary>
        public string RegisterLayerName
        {
            get { return _registerLayerName; }
            set { _registerLayerName = value; }
        }
        private Dictionary<string, string> metaFieldContent;

        public Dictionary<string, string> MetaFieldContent
        {
            get { return metaFieldContent; }
            set { metaFieldContent = value; }
        }

        public string MetaTableName
        {
            get { return _metaTableName; }
            set { _metaTableName = value; }
        }

        #endregion

        #region 重载函数
        /// <summary>
        /// 增加一条任务数据记录
        /// </summary>
        /// <returns></returns>
        public override bool Insert()
        {
            bool bSuccess = false;
            string sqlStatement;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            _taskdataID = DBHelper.GlobalDBHelper.GetNextValidID(TABLE_NAME, FLD_NAME_F_DATAID);

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _taskdataID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATANAME, _taskdataName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, _taskID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_INOROUT, _inOrOut, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_PACPATH, _pacPath, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEID, _packageID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_VALIDATERESULT, (int)_validateResult, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_TARGETREGISTERLAYERNAME, _registerLayerName, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_SNAPSHOTFILE, _snapshotFileFullName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_METADATAFILE, _metadataFileFullName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_ISUPLOADSNAPSHOTFILE, _isUploadSnapshotFile ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ISUPLOADMETADATAFILE, _isUploadMetadataFile ? 1 : 0, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_METATABLENAME,_metaTableName,EnumDBFieldType.FTString));

            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
                bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) == 1 ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex); ;
                return bSuccess;
            }

            if (bSuccess && _state != null)
            {
                string strFilter = FLD_NAME_F_DATAID + " = " + _taskdataID;

                byte[] bytes = null;
                //状态
                bytes = Archiver.Utility.Class.SerializationUtility.Serialize(_state);
                if (bytes != null)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_INPUTSTATE, ref bytes);
                }
                //验证信息
                bytes = Archiver.Utility.Class.SerializationUtility.Serialize(_validateInfo);
                if (bytes != null)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_VALIDATEINFO, ref bytes);
                }
                //元数据信息
                bytes = Archiver.Utility.Class.SerializationUtility.Serialize(metaFieldContent);
                if (bytes != null)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_METAFIELDVALUE, ref bytes);
                }
            }

            return bSuccess;
        }
        /// <summary>
        /// 删除指定任务ID的所有数据
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            string sqlStatement;
            sqlStatement = "DELETE FROM " + TABLE_NAME + " WHERE " + FLD_NAME_F_DATAID + " = " + "'" + _taskdataID + "'";

            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) == 1 ? true : false;
            return bSuccess;
        }

        /// <summary>
        /// 更新指定数据ID的数据信息
        /// </summary>
        /// <returns></returns>
        public override bool Update()
        {
            string sqlStatement;
            bool bSuccess;
            string strFilter = FLD_NAME_F_DATAID + " = " + "'" + _taskdataID + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _taskdataID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATANAME, _taskdataName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, _taskID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_INOROUT, _inOrOut, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_PACPATH, _pacPath, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEID, _packageID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_VALIDATERESULT, (int)_validateResult, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_TARGETREGISTERLAYERNAME, _registerLayerName, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_SNAPSHOTFILE, _snapshotFileFullName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_METADATAFILE, _metadataFileFullName, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_ISUPLOADSNAPSHOTFILE, _isUploadSnapshotFile ? 1 : 0, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_ISUPLOADMETADATAFILE, _isUploadMetadataFile ? 1 : 0, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_METATABLENAME, _metaTableName, EnumDBFieldType.FTString));

            sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter, DBHelper.GlobalDBHelper);

            try
            {
                bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) == 1 ? true : false;
                if (bSuccess)
                {
                    byte[] bytes = null;
                    //状态
                    bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_state);
                    if (bytes != null)
                    {
                        DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_INPUTSTATE, ref bytes);
                    } //验证信息
                    bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_validateInfo);
                    if (bytes != null)
                    {
                        DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_VALIDATEINFO, ref bytes);
                    }    //元数据信息
                    bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(metaFieldContent);
                    if (bytes != null)
                    {
                        DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_METAFIELDVALUE, ref bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex); ;
                return false;
            }
            return bSuccess;
        }

        // <summary>
        /// 选择
        /// </summary>
        /// <returns>所有数据列表</returns>
        public override IList<TaskDataDAL> Select()
        {
            IList<TaskDataDAL> pList = new List<TaskDataDAL>();
            DataTable dtResult = SelectEx();
            pList = Translate(dtResult);
            return pList;
        }
        #endregion

        #region 扩展属性查询
        public TaskDataDAL Select(int dataid)
        {
            IList<TaskDataDAL> pList = new List<TaskDataDAL>();

            string strFilter = FLD_NAME_F_DATAID + " = " + dataid;
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
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " order by " + FLD_NAME_F_DATAID;
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
                return null;
            }

        }

        public IList<TaskDataDAL> Select2(int taskID)
        {
            IList<TaskDataDAL> pList = null;
            string strFilter = FLD_NAME_F_TASKID + " = " + taskID;
            DataTable dtResult = Select2(strFilter);
            pList = Translate(dtResult);
            return pList;

        }

        public IList<TaskDataDAL> SelectByPackageID(int packageID)
        {
            DataTable dt = Select2(FLD_NAME_F_PACKAGEID + "=" + packageID);
            if (dt == null)
            {
                return null;
            }
            else
            {
                return Translate(dt);
            }
        }

        #endregion

        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<TaskDataDAL> Translate(DataTable dtResult)
        {
            IList<TaskDataDAL> list = new List<TaskDataDAL>();
            string xmlFileName = string.Empty;
            string path = Application.StartupPath;
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow pRow = null;
                string sLayers = string.Empty;
                byte[] bytes = null;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    TaskDataDAL info = new TaskDataDAL();
                    info.TaskDataID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);
                    info.TaskDataName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATANAME);
                    info.TaskID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_TASKID);
                    info.DataSize = Int64.Parse(GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATASIZE));
                    info.InOrOut = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_INOROUT);
                    info.PacPath = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_PACPATH);
                    info.PackageID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_PACKAGEID);
                    info.ObjectID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_OBJECTID);
                    info.ValidateResult = (EnumValidateResult)GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_VALIDATERESULT);
                    info.RegisterLayerName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TARGETREGISTERLAYERNAME);
                    info.SnapshotFileFullName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SNAPSHOTFILE);
                    info.MetadataFileFullName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_METADATAFILE);
                    //zqq+
                    info.MetaTableName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_METATABLENAME);
                    
                    info.IsUploadSnapshotFile = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISUPLOADSNAPSHOTFILE) > 0;
                    info.IsUploadMetadataFile = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISUPLOADMETADATAFILE) > 0;

                    //状态
                    bytes = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_DATAID + "=" + info.TaskDataID, TABLE_NAME, FLD_NAME_F_INPUTSTATE);
                    if (bytes != null && bytes.Length > 0)
                    {
                        info.State = (DataExecuteState)Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(bytes);
                    }
                    //验证信息
                    bytes = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_DATAID + "=" + info.TaskDataID, TABLE_NAME, FLD_NAME_F_VALIDATEINFO);
                    if (bytes != null && bytes.Length > 0)
                    {
                        info.ValidateInfo = (TaskDataValidateInfo)Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(bytes);
                    }
                    //元数据信息
                    bytes = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_DATAID + "=" + info.TaskDataID, TABLE_NAME, FLD_NAME_F_METAFIELDVALUE);
                    if (bytes != null && bytes.Length > 0)
                    {
                        info.MetaFieldContent = (Dictionary<string, string>)Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(bytes);
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
            string fields = FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_DATANAME + "," +
                FLD_NAME_F_TASKID + "," +
                FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_PACPATH + "," +
                //FLD_NAME_F_INPUTSTATE + "," +
                FLD_NAME_F_INOROUT + "," +
                FLD_NAME_F_OBJECTID + "," +
                FLD_NAME_F_VALIDATERESULT + "," +
                FLD_NAME_F_TARGETREGISTERLAYERNAME + "," +
                //FLD_NAME_F_VALIDATEINFO + "," + 
                FLD_NAME_F_PACKAGEID + "," +
                FLD_NAME_F_SNAPSHOTFILE + "," +
                FLD_NAME_F_METADATAFILE + "," +
                FLD_NAME_F_METATABLENAME + "," +
                FLD_NAME_F_ISUPLOADSNAPSHOTFILE + "," +
                FLD_NAME_F_ISUPLOADMETADATAFILE;
            return fields;
        }

        #endregion

        #region 扩展增删改操作

        /// <summary>
        /// 删除任务相关的所有数据
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <returns></returns>
        public bool DeleteTaskDatasByTaskID(int taskID)
        {
            string sqlStatement = "DELETE FROM " + TABLE_NAME + " WHERE " + FLD_NAME_F_TASKID + " = " + taskID;
            return DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0;
        }

        public DataTable GetTaskDataByMainData(string mainDataFile, int userID)
        {
            string sql = string.Format("SELECT {0} FROM {1} T,{2} D,{3} P WHERE T.{4}=D.{5} AND D.{6}=P.{7} AND P.{8}='{9}' AND T.{10}={11} AND T.{12}={13} AND P.{14}='{15}' ",
                                       TaskInfoDAL.FLD_NAME_F_TASKNAME,
                                       TaskInfoDAL.TABLE_NAME, TABLE_NAME, TaskFileDAL.TABLE_NAME,
                                       TaskInfoDAL.FLD_NAME_F_TASKID, FLD_NAME_F_TASKID,
                                       FLD_NAME_F_DATAID, TaskFileDAL.FLD_NAME_F_DATAID,
                                       TaskFileDAL.FLD_NAME_F_FILELOCATION, mainDataFile,
                                       TaskInfoDAL.FLD_NAME_F_USERID, userID,
                                       TaskInfoDAL.FLD_NAME_F_TASKSTATE, (int)EnumExecuteState.NoExecute,
                                       TaskFileDAL.FLD_NAME_F_FILECUSTOM, FileAttribute.FILE_MAIN);
            return DBHelper.GlobalDBHelper.DoQueryEx("TEMP", sql, true);
        }

        #endregion
    }
}
