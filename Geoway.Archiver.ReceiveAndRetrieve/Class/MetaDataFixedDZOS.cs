using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.DB.Public.Interface;
using System.Data;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 元数据表特征属性（固有字段）维护
    /// </summary>
    public class MetaDataFixedDZOS : MetaDataFixedDZInfo, IMetaDataFixedDZ
    {

        public MetaDataFixedDZOS(IDBHelper dbHelper, int dataID)
            : base(dbHelper, dataID)
        {
            if (dataID > 0)
            {
                IMetaDataFixedDZEdit coreMeta;
                if (bExist(out coreMeta)) //判断数据:dataID在表tableName中是否存在,若存在则对属性初始化
                {
                    this._location = coreMeta.Location;
                    this._serverID = coreMeta.ServerId;
                    this._dataTypeName = coreMeta.DataTypeName;
                    this._dataSize = coreMeta.DataSize;
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

            items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_LOCATION, Location, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, ServerId, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE,_dataSize,EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATATYPENAME,_dataTypeName,EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_DATAUNIT,_dataUnit,EnumDBFieldType.FTString));

            this._tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(DBHelper.GlobalDBHelper,(int) this._dataId);
            sqlStatement = SQLStringUtility.GetUpdateSQL(_tableName, items, strFilter, DBHelper.GlobalDBHelper);
            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }

        bool IMetaData.Delete()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetaDataFixedDZEdit Select()
        {
            IList<IMetaDataFixedDZEdit> pList;
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
        private IList<IMetaDataFixedDZEdit> Translate(DataTable dtResult)
        {
            IList<IMetaDataFixedDZEdit> list = new List<IMetaDataFixedDZEdit>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    IMetaDataFixedDZEdit info = new MetaDataFixedDZInfo(_dbHelper);
                    info.Location = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCATION);
                    info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                    info.DataSize = GetSafeDataUtility.ValidateDataRow_F(pRow, FLD_NAME_F_DATASIZE);
                    info.DataTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATATYPENAME);
                    info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
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
        private bool bExist(out IMetaDataFixedDZEdit metaData)
        {
            metaData = this.Select();
            return metaData == null ? false : true;
        }
    }
}
