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
    /// ������Ϣ���ݿ������
    /// </summary>
    public class TaskInfoDAL : DALBase<TaskInfoDAL>
    {
        #region ������Ϣ��ṹ
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
        public const string FLD_NAME_F_ISHANDMETA = "F_ISHANDMETA";   //������ �Ƿ��ֶ�¼��Ԫ����
        public const string FLD_NAME_F_EXECUTELOG = "F_EXECUTELOG";   //������־
        public const string FLD_NAME_F_VALIDATERESULT = "F_VALIDATERESULT";   //��֤
        public const string FLD_NAME_F_SCANMODE = "F_SCANMODE";    //ɨ��ģʽ

        #endregion

        public static TaskInfoDAL singleton = new TaskInfoDAL();

        #region ����
        private int taskID = -9999;
        /// <summary>
        /// ����ID
        /// </summary>
        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        private string taskName;
        /// <summary>
        /// ��������
        /// </summary>
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        private string taskDescribe;
        /// <summary>
        /// ��������
        /// </summary>
        public string TaskDescribe
        {
            get { return taskDescribe; }
            set { taskDescribe = value; }
        }

        private string taskDirectory = string.Empty;
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public string Directory
        {
            get { return taskDirectory; }
            set { taskDirectory = value; }
        }


        private int inputOrOutput;
        /// <summary>
        /// ���/����
        /// </summary>
        public int InputOrOutput
        {
            get { return inputOrOutput; }
            set { inputOrOutput = value; }
        }

        private int serverID;
        /// <summary>
        /// �洢�ڵ�ID
        /// </summary>
        public int ServerID
        {
            get { return serverID; }
            set { serverID = value; }
        }

        private string prePath;
        /// <summary>
        /// ������ǰ׺·��
        /// </summary>
        public string PrePath
        {
            get { return prePath; }
            set { prePath = value; }
        }

        private string localPath;
        /// <summary>
        /// ����·��
        /// </summary>
        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; }
        }

        private int userID;
        /// <summary>
        /// �����û�
        /// </summary>
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private DateTime defineTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime DefineTime
        {
            get { return defineTime; }
            set { defineTime = value; }
        }

        private DateTime beginTime;
        /// <summary>
        /// ��ʼִ��ʱ��
        /// </summary>
        public DateTime BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; }
        }

        private DateTime endTime;
        /// <summary>
        /// �������ʱ��
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private int isTiming = 0;
        /// <summary>
        /// �Ƿ�����ִ��
        /// </summary>
        public int IsTiming
        {
            get { return isTiming; }
            set { isTiming = value; }
        }

        private int taskState;
        /// <summary>
        /// ����״̬
        /// </summary>
        public int TaskState
        {
            get { return taskState; }
            set { taskState = value; }
        }

        private Int64 dataAmount;
        /// <summary>
        /// ������������Byte��
        /// </summary>
        public Int64 DataAmount
        {
            get { return dataAmount; }
            set { dataAmount = value; }
        }

        private int dataCount;
        /// <summary>
        /// ������
        /// </summary>
        public int DataCount
        {
            get { return dataCount; }
            set { dataCount = value; }
        }

        private int catalogID;
        /// <summary>
        /// �鵵�������ݽڵ�/�������������������ID
        /// </summary>
        public int CatalogID
        {
            get { return catalogID; }
            set { catalogID = value; }
        }

        private int isWithMeta;
        /// <summary>
        /// �Ƿ����Ԫ����
        /// </summary>
        public int IsWithMeta
        {
            get { return isWithMeta; }
            set { isWithMeta = value; }
        }
        //�Ƿ��ֶ�¼��Ԫ����
        private int isHandMeta;
        /// <summary>
        /// �Ƿ��ֶ�¼��Ԫ����
        /// </summary>
        public int IsHandMeta
        {
            get { return isHandMeta; }
            set { isHandMeta = value; }
        }

        private int isWithData;
        /// <summary>
        /// �Ƿ���������ļ�
        /// </summary>
        public int IsWithData
        {
            get { return isWithData; }
            set { isWithData = value; }
        }

        private int isWithSnapShot;
        /// <summary>
        /// �Ƿ�������ͼ
        /// </summary>
        public int IsWithSnapShot
        {
            get { return isWithSnapShot; }
            set { isWithSnapShot = value; }
        }

        private StringBuilder log;
        /// <summary>
        /// ����ִ����־
        /// </summary>
        public StringBuilder Log
        {
            get { return log; }
            set { log = value; }
        }

        private FileUploadSetting _snapshotSetting = null;
        /// <summary>
        /// ����ͼ�������
        /// </summary>
        public FileUploadSetting SnapshotSetting
        {
            get { return _snapshotSetting; }
            set { _snapshotSetting = value; }
        }

        private FileUploadSetting _metadataSetting = null;
        /// <summary>
        /// Ԫ�����������
        /// </summary>
        public FileUploadSetting MetadataSetting
        {
            get { return _metadataSetting; }
            set { _metadataSetting = value; }
        }


        private int flag = 1;
        /// <summary>
        /// ״̬��־
        /// </summary>
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        int progressCount = 0;
        /// <summary>
        /// ������ȣ�0��100��
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
        /// ��֤���
        /// </summary>
        public EnumValidateResult ValidateResult
        {
            get { return _validateResult; }
            set { _validateResult = value; }
        }

        private EnumScanMode _scanMode = 0;
        /// <summary>
        /// �������ɨ��ģʽ
        /// </summary>
        public EnumScanMode ScanMode
        {
            get { return _scanMode; }
            set { _scanMode = value; }
        }

        #endregion

        #region ���غ���
        /// <summary>
        /// �½�
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
                //����ͼ
                bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_snapshotSetting);
                if (bytes != null && bytes.Length > 0)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_SNAPSHOTSETTING, ref bytes);
                }

                //Ԫ����
                bytes = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(_metadataSetting);
                if (bytes != null && bytes.Length > 0)
                {
                    DBHelper.GlobalDBHelper.SaveBytes2Blob(strFilter, TABLE_NAME, FLD_NAME_F_METADATASETTING, ref bytes);
                }

            }

            return bSuccess;
        }

        /// <summary>
        /// ɾ��
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
        /// ����
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
        /// ѡ��
        /// </summary>
        /// <returns>���������б�</returns>
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

        #region ��չ���Բ�ѯ

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

        #region ��־����

        /// <summary>
        /// ��ȡ������־���ļ�
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="logFileName">��־�ļ���</param>
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
        /// �洢������־
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="logFileName">��־�ļ���</param>
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
        /// ����DataTable
        /// <param name="dtResult">Դ����</param>
        /// <returns>ʵ�弯��</returns>
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

        #region �ڲ�����

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
        /// ��ȡָ���ֽڳ��ȵ���Ӣ�Ļ���ַ���
        /// </summary>
        private string GetString(string strValue, int maxlen)
        {
            string result = string.Empty;// ���շ��صĽ��
            int byteLen = System.Text.Encoding.Default.GetByteCount(strValue);// ���ֽ��ַ�����
            int charLen = strValue.Length;// ���ַ�ƽ�ȶԴ�ʱ���ַ�������
            int byteCount = 0;// ��¼��ȡ����
            int pos = 0;// ��¼��ȡλ��
            if (byteLen > maxlen)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(strValue.ToCharArray()[i]) > 255)// �������ַ������2
                        byteCount += 2;
                    else// ��Ӣ���ַ������1
                        byteCount += 1;
                    if (byteCount > maxlen)// ����ʱֻ������һ����Чλ��
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == maxlen)// ���µ�ǰλ��
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
