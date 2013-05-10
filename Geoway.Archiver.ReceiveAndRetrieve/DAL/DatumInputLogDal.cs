using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// ���������־������
    /// </summary>
    internal class DatumInputLogDal
    {
        #region ��ʵ��

        private static DatumInputLogDal _instance = null;
        /// <summary>
        /// ����
        /// </summary>
        public static DatumInputLogDal SingleInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatumInputLogDal();
                }
                return _instance;
            }
        }



        #endregion ��ʵ��

        #region ���ݿ��ṹ

        private const string TABLE_NAME = "TBARC_DATUMINPUTLOG";
        private const string F_LOGID = "F_LOGID"; // ��־ID
        private const string F_LOGNAME = "F_LOGNAME"; // ��־����
        private const string F_LOGDATE = "F_LOGDATE"; // ��־����
        private const string F_LOGINFO = "F_LOGINFO"; // ��־��Ϣ

        public int LogId = -1;
        public string LogName = string.Empty;
        public DateTime LogDate = DateTime.Now;
        public string FullPath = string.Empty;
        public byte[] LogInfo = null;

        #endregion

        #region public ����

        /// <summary>
        /// �½�
        /// </summary>
        /// <returns></returns>
        public bool Insert(IDBHelper db)
        {
            try
            {
                if (LogId <= 0)
                {
                    LogId = db.GetNextValidID(TABLE_NAME, F_LOGID);
                }
                string sqlStatement = string.Empty;
                IList<DBFieldItem> items = new List<DBFieldItem>();
                items.Add(new DBFieldItem(F_LOGID, LogId, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(F_LOGNAME, LogName, EnumDBFieldType.FTString));
                items.Add(new DBFieldItem(F_LOGDATE, LogDate, EnumDBFieldType.FTDatetime));
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, db);
                bool bSuccess = db.DoSQL(sqlStatement) > 0 ? true : false;
                if (bSuccess)
                {
                    //if (LogInfo != null)
                    //{
                    //    string strFilter = F_LOGID + " = " + LogId;
                    //    //db.SaveBytes2Blob(strFilter, TABLE_NAME, F_LOGINFO, ref LogInfo);
                    //    db.SaveFile2Blob(FullPath, strFilter, TABLE_NAME, F_LOGINFO, true);
                    //}
                    //else
                    //{
                    string strFilter = F_LOGID + " = " + LogId;
                    //db.SaveBytes2Blob(strFilter, TABLE_NAME, F_LOGINFO, ref LogInfo);
                    db.SaveFile2Blob(FullPath, strFilter, TABLE_NAME, F_LOGINFO, true);
                   
                    // }
                }
                return bSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Select(IDBHelper db, ref DataTable dt)
        {
            try
            {
                string sqlStatement = string.Format("select {0},{1},{2} from {3} ", F_LOGID, F_LOGNAME, F_LOGDATE, TABLE_NAME);
                dt = db.DoQueryEx(sqlStatement);
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool SelectFile(IDBHelper db, ref DataTable dt)
        {
            try
            {
                string strFilter = F_LOGNAME + " = '" + LogName+"'";
                db.ReadBlob2File(FullPath, strFilter, TABLE_NAME, F_LOGINFO);
                string sqlStatement = string.Format("select {0} from {1} where {2}='{3}'", F_LOGINFO, TABLE_NAME, F_LOGNAME, LogName);
                dt = db.DoQueryEx(sqlStatement);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        #endregion

        #region public static ����
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        public bool Delete(IDBHelper db, int logId)
        {
            string sqlStatement;
            sqlStatement = string.Format("delete from {0} where {1}={2}", TABLE_NAME, F_LOGID, LogId);

            return db.DoSQL(sqlStatement) >= 0;
        }

        // <summary>
        /// ѡ��
        /// </summary>
        /// <returns>���������б�</returns>
        public static IList<DatumInputLogDal> Select(IDBHelper db)
        {
            IList<DatumInputLogDal> pList = new List<DatumInputLogDal>();

            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " order by " + F_LOGID;
            DataTable dtResult = db.DoQueryEx(TABLE_NAME, sqlStatement, true);
            pList = Translate(db, dtResult);
            return pList;
        }

        #endregion

        #region private static
        /// <summary>
        /// ����DataTable
        /// <param name="dtResult">Դ����</param>
        /// <returns>ʵ�弯��</returns>
        public static IList<DatumInputLogDal> Translate(IDBHelper db, DataTable dtResult)
        {
            IList<DatumInputLogDal> list = new List<DatumInputLogDal>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    DatumInputLogDal info = new DatumInputLogDal();
                    info.LogId = GetSafeDataUtility.ValidateDataRow_N(pRow, F_LOGID);
                    info.LogName = GetSafeDataUtility.ValidateDataRow_S(pRow, F_LOGNAME);
                    info.LogDate = GetSafeDataUtility.ValidateDataRow_T(pRow, F_LOGDATE);

                    string filter = string.Format("{0}={1}", F_LOGID, info.LogId);
                    if (db.IsBlobNull(filter, TABLE_NAME, F_LOGINFO))
                    {
                        info.LogInfo = null;
                    }
                    else
                    {
                        info.LogInfo = db.ReadBlob2Bytes(filter, TABLE_NAME, F_LOGINFO);
                    }

                    list.Add(info);
                }
            }
            return list;
        }

        private static string getSelectField()
        {
            string fields = F_LOGID + "," +
                F_LOGNAME + "," +
                F_LOGDATE;
            return fields;
        }

        #endregion
    }
}
