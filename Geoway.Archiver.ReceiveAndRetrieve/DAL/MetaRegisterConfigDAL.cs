using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 元数据注册字段维护表
    /// eg: table1 F_Scale F_Year...
    /// </summary>
    public class MetaRegisterConfigDAL : DALBase<MetaRegisterConfigDAL>
    {
        #region 字段
        public static string TABLENAME = "TBBIZ_METAREGISTERFIELD";

        public static string F_OID = "F_OID";
        public static string F_TableID = "F_TableID";
        public static string F_Year = "F_Year";
        public static string F_Scale = "F_Scale";
        public static string F_Resulation = "F_Resolution";

        //TODO 继续添加其他字段
        #endregion

        #region 属性

        private int _OID;
        public int OID
        {
            get { return _OID; }
            // set { _OID = value; }
        }

        private int _TableID;
        public int TableID
        {
            get { return _TableID; }
            set { _TableID = value; }
        }

        private string _DataTime;
        public string DataTime
        {
            get { return _DataTime; }
            set { _DataTime = value; }
        }

        private string _Scale;
        public string Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }

        private string _Resulation;
        public string Resulation
        {
            get { return _Resulation; }
            set { _Resulation = value; }
        }

        //TODO 继续添加其他属性

        #endregion

        public static MetaRegisterConfigDAL Singleton = new MetaRegisterConfigDAL();


        [Obsolete("暂时没实现", true)]
        public override IList<MetaRegisterConfigDAL> Select()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool Insert()
        {
            _OID = GetNextID(TABLENAME, F_OID);
            string sql = SQLStringUtility.GetInsertSQL(TABLENAME, this.GetFieldItems(), DBHelper.GlobalDBHelper);
            return DoSQL(sql);
        }

        [Obsolete("暂时没实现", true)]
        public override bool Delete()
        {
            string sqlStatement;
            sqlStatement = "DELETE FROM " + TABLENAME + " WHERE " + F_OID + " = " + "'" + this._OID + "'";

            bool bSuccess = DoSQL(sqlStatement);
            return bSuccess;
        }

        public override bool Update()
        {
            string sql = SQLStringUtility.GetUpdateSQL(TABLENAME, GetFieldItems(), F_OID + "=" + _OID, DBHelper.GlobalDBHelper);
            return DoSQL(sql);
        }

        public MetaRegisterConfigDAL Select(string tableName)
        {
            IList<SystemTableDAL> lstDAL = SystemTableDAL.Singleton.Select(SystemTableDAL.FLD_NAME_F_TABLENAME + " = '" + tableName + "'");
            if (lstDAL.Count == 0)
            {
                return null;
            }
            return Select(lstDAL[0].ID);
        }

        public MetaRegisterConfigDAL Select(int tableID)
        {
            string sql = "SELECT " + GetSelectFields() + " FROM " + TABLENAME + " WHERE " + F_TableID + " =" + tableID;
            DataTable table = DBHelper.GlobalDBHelper.DoQueryEx("tmp", sql, true);
            IList<MetaRegisterConfigDAL> lstDal = Translate(table);
            if (lstDal.Count > 0)
                return lstDal[0];
            return null;
        }


        private string GetSelectFields()
        {
            string fields =
                F_OID + "," +
                F_Resulation + "," +
                F_Scale + "," +
                F_TableID + "," +
                F_Year
                ;
            //TODO 继续增加其他字段
            return fields;
        }

        private IList<MetaRegisterConfigDAL> Translate(DataTable table)
        {
            IList<MetaRegisterConfigDAL> lstDal = new List<MetaRegisterConfigDAL>();
            foreach (DataRow row in table.Rows)
            {
                MetaRegisterConfigDAL dal = new MetaRegisterConfigDAL();
                dal._OID = GetSafeDataUtility.ValidateDataRow_N(row, F_OID);
                dal._Resulation = GetSafeDataUtility.ValidateDataRow_S(row, F_Resulation);
                dal._Scale = GetSafeDataUtility.ValidateDataRow_S(row, F_Scale);
                dal._TableID = GetSafeDataUtility.ValidateDataRow_N(row, F_TableID);
                dal._DataTime = GetSafeDataUtility.ValidateDataRow_S(row, F_Year);

                //TODO 继续添加其他转化

                lstDal.Add(dal);
            }

            return lstDal;
        }

        private IList<DBFieldItem> GetFieldItems()
        {
            IList<DBFieldItem> lstItems = new List<DBFieldItem>();

            lstItems.Add(new DBFieldItem(F_OID, _OID, EnumDBFieldType.FTNumber));
            lstItems.Add(new DBFieldItem(F_Resulation, _Resulation, EnumDBFieldType.FTString));
            lstItems.Add(new DBFieldItem(F_Scale, _Scale, EnumDBFieldType.FTString));
            lstItems.Add(new DBFieldItem(F_TableID, _TableID, EnumDBFieldType.FTNumber));
            lstItems.Add(new DBFieldItem(F_Year, _DataTime, EnumDBFieldType.FTString));

            return lstItems;
        }
    }
}


