using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    public class TaskFileDAL : DALBase<TaskFileDAL>
    {
        #region 数据库结构


        public const string TABLE_NAME = "TBARC_TASKFILE";
        public const string FLD_NAME_F_ID = "F_ID";
        public const string FLD_NAME_F_DATAID = "F_TASKDATAID";
        public const string FLD_NAME_F_FILELOCATION = "F_FILELOCATION";
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_FILEATTRIBUTE = "F_FILEATTRIBUTE";
        public const string FLD_NAME_F_FILECUSTOM = "F_CUSTOM";
        public const string FLD_NAME_F_PACKAGEPATH = "F_PACKAGEPATH"; //数据包、文件夹、文件 路径
        public const string FLD_NAME_F_SOURCETYPE = "F_SOURCETYPE"; //数据来源

        #endregion

        #region 单例模式
        public static TaskFileDAL Singleton = new TaskFileDAL();
        #endregion

        #region 属性


        private int _id;
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _taskDataID;
        /// <summary>
        /// 任务数据ID
        /// </summary>
        public int TaskDataID
        {
            get { return _taskDataID; }
            set { _taskDataID = value; }
        }

        private Int64 _dataSize;
        /// <summary>
        /// 文件大小（KB）

        /// </summary>
        public Int64 DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        private string _fileLocation;
        /// <summary>
        /// 文件位置，绝对路径

        /// </summary>
        public string FileLocation
        {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        private string _packagePath = string.Empty;
        /// <summary>
        /// 数据包内部x路径
        /// </summary>
        public string PackagePath
        {
            get { return _packagePath; }
            set { _packagePath = value; }
        }

        private int _fileAttribute = 0;
        /// <summary>
        /// 文件属性

        /// </summary>
        public int FileAttribute
        {
            get { return _fileAttribute; }
            set { _fileAttribute = value; }
        }

        private string _fileCustom = string.Empty;
        /// <summary>
        /// 自定义属性

        /// </summary>
        public string FileCustom
        {
            get { return _fileCustom; }
            set { _fileCustom = value; }
        }

        private EnumDataFileSourceType _sourceType = EnumDataFileSourceType.DataUnit;
        /// <summary>
        /// 获取或设置来源

        /// </summary>
        public EnumDataFileSourceType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        #endregion

        #region 重载函数
        public override bool Insert()
        {
            string sqlStatement = string.Empty;

            IList<DBFieldItem> items = new List<DBFieldItem>();

            _id = GetNextID(TABLE_NAME, FLD_NAME_F_ID);

            items.Add(new DBFieldItem(FLD_NAME_F_ID, _id, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _taskDataID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FILELOCATION, _fileLocation, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEPATH, _packagePath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_FILEATTRIBUTE, _fileAttribute, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FILECUSTOM, _fileCustom, EnumDBFieldType.FTString));
            
            items.Add(new DBFieldItem(FLD_NAME_F_SOURCETYPE, (int)_sourceType, EnumDBFieldType.FTNumber));

            try
            {
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }

            bool bSuccess = DoSQL(sqlStatement);

            return bSuccess;
        }

        public override bool Delete()
        {
            string sqlStatement;
            sqlStatement = "DELETE FROM " + TABLE_NAME + " WHERE " + FLD_NAME_F_ID + " = " + "'" + this._id + "'";

            bool bSuccess = DoSQL(sqlStatement);
            return bSuccess;
        }

        public override bool Update()
        {
            string sqlStatement;
            string strFilter = FLD_NAME_F_ID + " = " + "'" + this._id + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _taskDataID, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FILELOCATION, _fileLocation, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEPATH, _packagePath, EnumDBFieldType.FTString));

            items.Add(new DBFieldItem(FLD_NAME_F_FILEATTRIBUTE, _fileAttribute, EnumDBFieldType.FTNumber));

            items.Add(new DBFieldItem(FLD_NAME_F_FILECUSTOM, _fileCustom, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_SOURCETYPE, (int)_sourceType, EnumDBFieldType.FTNumber));

            sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter, DBHelper.GlobalDBHelper);

            bool bSuccess = DoSQL(sqlStatement);

            return bSuccess;
        }

        // <summary>
        /// 选择
        /// </summary>
        /// <returns>所有数据路径列表</returns>
        public override IList<TaskFileDAL> Select()
        {
            IList<TaskFileDAL> pList = new List<TaskFileDAL>();
            DataTable dtResult = DoQuery();
            pList = Translate(dtResult);
            return pList;
        }

        public override string ToString()
        {
            return _fileLocation;
        }

        #endregion

        #region 扩展属性查询


        public TaskFileDAL Select(int fid)
        {
            IList<TaskFileDAL> pList = new List<TaskFileDAL>();

            string strFilter = FLD_NAME_F_ID + " = " + fid;
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            if (pList.Count > 0)
            {
                return pList[0];
            }
            else
                return null;
        }
        /// <summary>
        /// 根据TASKDataID查找制定文件路径
        /// </summary>
        /// <param name="taskDataID"></param>
        /// <param name="enumDataFileProperty"></param>
        /// <returns></returns>
        public TaskFileDAL Select(int taskDataID, EnumDataFileProperty enumDataFileProperty)
        {
            IList<TaskFileDAL> pList = new List<TaskFileDAL>();

            string strFilter = string.Format(" {0} = {1} AND {2} = {3}", FLD_NAME_F_DATAID, taskDataID,FLD_NAME_F_FILEATTRIBUTE, ((int) enumDataFileProperty));
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            if (pList.Count > 0)
            {
                return pList[0];
            }
            else
                return null;
        }

        public IList<TaskFileDAL> SelectByTaksDataID(int taskDataID)
        {
            string strFilter = FLD_NAME_F_DATAID + " = " + taskDataID + " AND " + FLD_NAME_F_SOURCETYPE + "=" + (int)EnumDataFileSourceType.DataUnit;
            DataTable dtResult = DoQuery(strFilter);
            return Translate(dtResult);
        }

        public IList<TaskFileDAL> SelectByTaksDataID(int taskDataID, string xPath)
        {
            string strFilter = string.Format("{0}={1} AND {2}={3} AND {4}='{5}'",
                                             FLD_NAME_F_DATAID, taskDataID,
                                             FLD_NAME_F_SOURCETYPE, (int)EnumDataFileSourceType.DataUnit,
                                             FLD_NAME_F_PACKAGEPATH, xPath);
            DataTable dtResult = DoQuery(strFilter);
            return Translate(dtResult);
        }

        public IList<TaskFileDAL> SelectByTaskPackageID(int taskPackageID)
        {
            string strFilter = FLD_NAME_F_DATAID + " = " + taskPackageID + " AND " + FLD_NAME_F_SOURCETYPE + "=" + (int)EnumDataFileSourceType.DataPackage;
            DataTable dtResult = DoQuery(strFilter);
            return Translate(dtResult);
        }

        public IList<TaskFileDAL> SelectByTaskPackageID(int taskPackageID, string xPath)
        {
            string strFilter = string.Format("{0}={1} AND {2}={3} AND {4}='{5}'",
                                             FLD_NAME_F_DATAID, taskPackageID,
                                             FLD_NAME_F_SOURCETYPE, (int)EnumDataFileSourceType.DataPackage,
                                             FLD_NAME_F_PACKAGEPATH, xPath);
            DataTable dtResult = DoQuery(strFilter);
            return Translate(dtResult);
        }

        public DataTable DoQuery()
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        public DataTable DoQuery(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " where " + strFilter;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="svrPath"></param>
        /// <returns></returns>
        public DataTable GetSameDataPathByServerIDAndPath(int serverID, string svrPath)
        {
            string sql = string.Format("SELECT S.{0}, P.{1} FROM {2} S, {3} P WHERE S.{4}={5} AND CONCAT(TRIM('/' FROM S.{6}),'{7}')=P.{1}",
                                       StorageFieldDefinition.FLD_NAME_F_NAME, FLD_NAME_F_FILELOCATION,
                                       StorageFieldDefinition.TABLE_NAME, TABLE_NAME,
                                       StorageFieldDefinition.FLD_NAME_F_ID, serverID,
                                       StorageFieldDefinition.FLD_NAME_F_PRE_PATH, svrPath);
            return DBHelper.GlobalDBHelper.DoQueryEx("TEMP", sql, true);
        }

        #endregion

        #region translate
        /// <summary>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<TaskFileDAL> Translate(DataTable dtResult)
        {
            IList<TaskFileDAL> list = new List<TaskFileDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    TaskFileDAL info = new TaskFileDAL();
                    info.ID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ID);
                    info.TaskDataID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);
                    info.FileLocation = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_FILELOCATION);
                    size = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATASIZE);
                    if (string.IsNullOrEmpty(size))
                    {
                        info.DataSize = 0;
                    }
                    else
                    {
                        info.DataSize = Convert.ToInt64(size);
                    }
                    info.PackagePath = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_PACKAGEPATH);
                    info.FileAttribute = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_FILEATTRIBUTE);
                    info.FileCustom = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_FILECUSTOM);
                    info.SourceType = (EnumDataFileSourceType)GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SOURCETYPE);
                    list.Add(info);
                }
            }
            return list;
        }
        #endregion

        #region 内部方法

        private string getSelectField()
        {
            string fields = FLD_NAME_F_ID + "," +
                FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_FILELOCATION + "," +
                FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_FILEATTRIBUTE + "," +
                FLD_NAME_F_FILECUSTOM + "," +
                FLD_NAME_F_PACKAGEPATH + "," +
                FLD_NAME_F_SOURCETYPE;
            return fields;
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除未执行的任务数据相关的记录

        /// </summary>
        /// <param name="taskDataID">任务数据ID</param>
        /// <returns></returns>
        public bool DeleteDataPathByTaskDataID(int taskDataID)
        {
            string sql = string.Format("delete {0} where {1}={2}", TABLE_NAME, FLD_NAME_F_DATAID, taskDataID);
            return DoSQL(sql);
        }

        #endregion
    }
}
