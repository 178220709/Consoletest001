using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.DB.Public.Interface;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 元数据表特征属性（固有字段）维护
    /// </summary>
    public class MetaDataFixedJZOS : MetaDataFixedJZInfo, IMetaDataFixedJZ
    {

        public MetaDataFixedJZOS(IDBHelper dbHelper, int dataID)
            : base(dbHelper, dataID)
        {
            if (dataID > 0)
            {
                IMetaDataFixedJZEdit meta;
                if (bExist(out meta)) //判断数据:dataID在表tableName中是否存在,若存在则对属性初始化
                {
                    this._barCode = meta.BarCode;
                    this._datumAmount = meta.DatumAmount;
                    this._enumCarrierType = meta.enumCarrierType;
                    this._virtualWarehouseAddress = meta.VirtualWarehouseAddress;
                }
            }
        }

        #region IMetaData 成员
        bool IMetaData.Insert()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        bool IMetaData.Update()
        {
            string sqlStatement;
            string strFilter = FLD_NAME_F_DATAID + " = " + this._dataId;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            //items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber));
            //items.Add(new DBFieldItem(FLD_NAME_F_DATUMNUM, _datumAmount, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_TIAOXINMA, _barCode, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_XULIKUFANG, _virtualWarehouseAddress, EnumDBFieldType.FTString));
            //items.Add(new DBFieldItem(FLD_NAME_F_ZAITITYPE, _enumCarrierType, EnumDBFieldType.FTNumber));

            this._tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, this._dataId);
            sqlStatement = SQLStringUtility.GetUpdateSQL(_tableName, items, strFilter, DBHelper.GlobalDBHelper);
            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }

        bool IMetaData.Delete()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetaDataFixedJZEdit Select()
        {
            IList<IMetaDataFixedJZEdit> pList;
            string strFilter = FLD_NAME_F_DATAID + " = " + _dataId;
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            return pList.Count > 0 ? pList[0] : null;
        }


        #endregion

        #region translate
        /// <summary/>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        private IList<IMetaDataFixedJZEdit> Translate(DataTable dtResult)
        {
            IList<IMetaDataFixedJZEdit> list = new List<IMetaDataFixedJZEdit>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    IMetaDataFixedJZEdit info = new MetaDataFixedJZInfo(_dbHelper);
                    info.BarCode = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TIAOXINMA);
                    info.DatumAmount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATUMNUM);
                    info.VirtualWarehouseAddress = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_XULIKUFANG);
                    info.enumCarrierType = (EnumCarrierType)(GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ZAITITYPE));
                    list.Add(info);
                }
            }
            return list;
        }


        private DataTable DoQuery(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + _tableName + " where " + strFilter + " order by " + FLD_NAME_F_DATAID;
            DataTable dtResult = _dbHelper.DoQueryEx(_tableName, sqlStatement, true);
            return dtResult;
        }


        #endregion

        /// <summary>
        /// 根据数据ID判断该条数据是否在对应数据库表中存在
        /// </summary>
        /// <returns></returns>
        private bool bExist(out IMetaDataFixedJZEdit metaData)
        {
            metaData = this.Select();
            return metaData == null ? false : true;
        }
    }
}
