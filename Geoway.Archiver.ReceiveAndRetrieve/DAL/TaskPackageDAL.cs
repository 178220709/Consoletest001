using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Core;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    using EnumExecuteState=Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState;
    using Geoway.ADF.MIS.Utility.Log;

    /// <summary>
    ///目的：任务数据包数据表操作类
    ///创建人：王金玉

    ///创建日期：2010-11-17
    ///修改描述：

    ///修改人：
    ///修改日期：

    ///备注：

    /// </summary>
    public class TaskPackageDAL : DALBase<TaskPackageDAL>
    {
        #region 数据结构

        private const string TABLE_NAME = "TBARC_TASKPACKAGE";

        private const string FLD_NAME_F_ID = "F_ID";
        private const string FLD_NAME_F_NAME = "F_NAME";
        private const string FLD_NAME_F_TASKID = "F_TASKID";
        private const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        private const string FLD_NAME_F_FLAG = "F_FLAG";
        private const string FLD_NAME_F_PATH = "F_PATH";

        #endregion

        #region 单例模式

        public static TaskPackageDAL Singleton = new TaskPackageDAL();

        #endregion

        #region 属性


        private int _id = 0;
        /// <summary>
        /// 获取或设置唯一标识
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name = string.Empty;
        /// <summary>
        /// 获取或设置数据包名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState _state = Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState.NoExecute;
        /// <summary>
        /// 获取或设置状态

        /// </summary>
        public Geoway.Archiver.ReceiveAndRetrieve.Definition.EnumExecuteState State
        {
            get { return _state; }
            set { _state = value; }
        }

        private int _taskID = 0;
        /// <summary>
        /// 获取或设置所属任务ID
        /// </summary>
        public int TaskID
        {
            get { return _taskID; }
            set { _taskID = value; }
        }

        private Int64 _dataSize = 0;
        /// <summary>
        /// 获取或设置数据量大小
        /// </summary>
        public Int64 DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        private string _packagePath = string.Empty;
        /// <summary>
        /// 获取或设置数据包路径
        /// </summary>
        public string PackagePath
        {
            get { return _packagePath; }
            set { _packagePath = value; }
        }


        #endregion

        #region 基本操作

        public override bool Insert()
        {
            int id = base.GetNextID(TABLE_NAME, FLD_NAME_F_ID);
            string sql = string.Empty;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            //开始设置参数

            items.Add(new DBFieldItem(FLD_NAME_F_ID, id, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_NAME, _name, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, _taskID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_FLAG, (int)_state, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_PATH, _packagePath, EnumDBFieldType.FTString));
            try
            {
                sql = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
                if (DoSQL(sql))
                {
                    _id = id;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        public override bool Update()
        {
            string strFilter = FLD_NAME_F_ID + "=" + _id;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            //开始设置参数

            items.Add(new DBFieldItem(FLD_NAME_F_NAME, _name, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_TASKID, _taskID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_FLAG, (int)_state, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_PATH, _packagePath, EnumDBFieldType.FTString));
            try
            {
                string sql = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter, DBHelper.GlobalDBHelper);
                return DoSQL(sql);
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        public override bool Delete()
        {
            string sql = string.Format("Delete {0} Where {1}={2}", TABLE_NAME, FLD_NAME_F_ID, _id);
            return base.DoSQL(sql);
        }

        public bool Delete(int taskID)
        {
            string sql = string.Format("delete {0} where {1}={2}", TABLE_NAME, FLD_NAME_F_TASKID, taskID);
            return DoSQL(sql);
        }

        public override IList<TaskPackageDAL> Select()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ToString()
        {
            return _name;
        }

        #endregion

        #region 综合查询

        public TaskPackageDAL Select(int id)
        {
            IList<TaskPackageDAL> lst = Select(FLD_NAME_F_ID + "=" + id);
            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }
            return null;
        }

        public IList<TaskPackageDAL> SelectByTaskID(int taskID)
        {
            return Select(FLD_NAME_F_TASKID + "=" + taskID);
        }

        public IList<TaskPackageDAL> Select(string filter)
        {
            string sql = string.Format("Select {0} From {1} Where {2}", GetSelectFields(), TABLE_NAME, filter);
            DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sql, true);
            return Translate(dt);
        }

        #endregion

        #region Translate

        private IList<TaskPackageDAL> Translate(DataTable dt)
        {
            string size = string.Empty;
            IList<TaskPackageDAL> lst = new List<TaskPackageDAL>();
            TaskPackageDAL info = null;
            foreach (DataRow row in dt.Rows)
            {
                info = new TaskPackageDAL();
                info.ID = GetSafeDataUtility.ValidateDataRow_N(row, FLD_NAME_F_ID);
                info.Name = GetSafeDataUtility.ValidateDataRow_S(row, FLD_NAME_F_NAME);
                info.TaskID = GetSafeDataUtility.ValidateDataRow_N(row, FLD_NAME_F_TASKID);
                size = GetSafeDataUtility.ValidateDataRow_S(row, FLD_NAME_F_DATASIZE);
                if (string.IsNullOrEmpty(size))
                {
                    info.DataSize = 0;
                }
                else
                {
                    info.DataSize = Convert.ToInt64(size);
                }
                info.State = (EnumExecuteState)GetSafeDataUtility.ValidateDataRow_N(row, FLD_NAME_F_FLAG);
                info.PackagePath = GetSafeDataUtility.ValidateDataRow_S(row, FLD_NAME_F_PATH);

                lst.Add(info);
            }
            return lst;
        }

        #endregion

        #region 内部方法

        public string GetSelectFields()
        {
            return FLD_NAME_F_ID + "," +
                    FLD_NAME_F_NAME + "," +
                    FLD_NAME_F_TASKID + "," +
                    FLD_NAME_F_DATASIZE + "," +
                    FLD_NAME_F_FLAG + "," +
                    FLD_NAME_F_PATH;
        }

        #endregion
    }
}
