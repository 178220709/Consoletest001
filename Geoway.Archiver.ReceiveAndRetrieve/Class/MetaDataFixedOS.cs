namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ADF.MIS.DB.Public;
    using ADF.MIS.DB.Public.Enum;
    using ADF.MIS.DB.Public.Interface;
    using Archiver.Utility.DAL;
    using Interface.Register;
    using Modeling.Definition;
    using Geoway.Archiver.ReceiveAndRetrieve.Definition;

    public class MetaDataFixedOS : MetaDataFixedInfo, IMetaDataFixed
    {
        private EnumMetaDatumType _enumMetaDatumType;
        public MetaDataFixedOS(IDBHelper dbHelper, int dataID, EnumMetaDatumType enumMetaDatumType)
            : base(dbHelper, dataID)
        {
            _enumMetaDatumType = enumMetaDatumType;
            if (dataID > 0)
            {
                IMetaDataFixedEdit coreMeta;
                if (bExist(out coreMeta)) //判断数据:dataID在表tableName中是否存在,若存在则对属性初始化
                {
                    switch (_enumMetaDatumType)
                    {
                        case EnumMetaDatumType.enumSTELEDatum://单表电子资料
                            this._dataSize = coreMeta.DataSize;
                            this._dataTypeName = coreMeta.DataTypeName;
                            this._dataUnit = coreMeta.DataUnit;
                            this._isSpatial = coreMeta.IsSpatial;
                            this._location = coreMeta.Location;
                            this._serverID = coreMeta.ServerId;
                            break;
                        case EnumMetaDatumType.enumSTMetaTable://单元数据表
                            this._dataSize = coreMeta.DataSize;
                            this._dataUnit = coreMeta.DataUnit;
                            this._isHasAFile = coreMeta.IsHasAFile;
                            this._location = coreMeta.Location;
                            this._serverID = coreMeta.ServerId;
                            break;
                        case EnumMetaDatumType.enumSTGENMED://单表普通介质
                            this._virtualWarehouseAddress = coreMeta.VirtualWarehouseAddress;
                            this._datumAmount = coreMeta.DatumAmount;
                            this._enumCarrierType = coreMeta.EnumMediumtype;
                            break;
                            case EnumMetaDatumType.enumSTNONUMMED://单表条形码介质
                            this._datumAmount = coreMeta.DatumAmount;
                            this._isHasAFile = coreMeta.IsHasAFile;
                            this._isSpatial = coreMeta.IsSpatial;
                            this._enumCarrierType = coreMeta.EnumMediumtype;
                            this._barCode = coreMeta.BarCode;
                            this._virtualWarehouseAddress = coreMeta.VirtualWarehouseAddress;
                            break;
                            case EnumMetaDatumType.enumSTELEDatumMED://单表电子资料介质关联
                            this._dataSize = coreMeta.DataSize;
                            this._dataTypeName = coreMeta.DataTypeName;
                            this._dataUnit = coreMeta.DataUnit;
                            this._isSpatial = coreMeta.IsSpatial;
                            this._location = coreMeta.Location;
                            //this._relDataID=???
                            this._serverID = coreMeta.ServerId;
                            //this.=coreMeta.
                            break;
                        case EnumMetaDatumType.enumMTELEDatum:// 主从表电子资料
                            this._dataSize = coreMeta.DataSize;
                            this._dataTypeName = coreMeta.DataTypeName;
                            this._location = coreMeta.Location;
                            this._serverID = coreMeta.ServerId;
                            break;
                        case EnumMetaDatumType.enumMTGENMED://主从表介质资料
                            this._datumAmount = coreMeta.DatumAmount;
                            this._enumCarrierType = coreMeta.EnumMediumtype;
                            this._barCode = coreMeta.BarCode;
                            this._virtualWarehouseAddress = coreMeta.VirtualWarehouseAddress;
                            break;

                    }

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
            string strFilter = FLD_NAME_F_DATAID + " = " + this._dataId;
            IList<DBFieldItem> items = new List<DBFieldItem>();
            switch (_enumMetaDatumType)
            {
                case EnumMetaDatumType.enumSTELEDatum://单表电子资料
                    items = new List<DBFieldItem>();
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_LOCATION, _location, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, _serverID, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATATYPENAME, _dataTypeName, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAUNIT, _dataUnit, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_ISSPATIAL, _isSpatial?0:1, EnumDBFieldType.FTNumber));
                    break;
                case EnumMetaDatumType.enumSTGENMED://单表普通介质
                    items = new List<DBFieldItem>();
                    items.Add(new DBFieldItem(FLD_NAME_F_XULIKUFANG, _virtualWarehouseAddress, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATUMNUM, _datumAmount, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_MEDIUMTYPE, (int)_enumCarrierType, EnumDBFieldType.FTNumber));
                    break;
                case EnumMetaDatumType.enumSTMetaTable://单元数据表
                    items = new List<DBFieldItem>();
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAID, _dataId, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_LOCATION, Location, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, _serverID, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAUNIT, _dataUnit, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_ISHASAFILE, _isHasAFile ? 0 : 1, EnumDBFieldType.FTNumber));
                    break;
                case EnumMetaDatumType.enumSTELEDatumMED://单表电子资料介质关联
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAID,_dataId,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_SERVERID,_serverID,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATATYPENAME,_dataTypeName,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE,_dataSize,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAUNIT,_dataUnit,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_LOCATION,_location,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_ISSPATIAL,_isSpatial?0:1,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_SRCDATAPATH,_scrDataPath,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_REDATAID,_relDataID,EnumDBFieldType.FTNumber));
                    break;
                    case EnumMetaDatumType.enumMTELEDatum://主从表电子资料
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAID,_dataId,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_SERVERID,_serverID,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATATYPENAME,_dataTypeName,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE,_dataSize,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAUNIT,_dataUnit,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_LOCATION,_location,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_REDATAID,_relDataID,EnumDBFieldType.FTNumber));
                    break;
                    case EnumMetaDatumType.enumMTGENMED://主从表介质资料
                    items.Add(new DBFieldItem(FLD_NAME_F_DATAID,_dataId,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_XULIKUFANG,_virtualWarehouseAddress,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_TIAOXINMA,_barCode,EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATUMNUM,_datumAmount,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_MEDIUMTYPE,(int)_enumMetaDatumType,EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_REDATAID,_relDataID,EnumDBFieldType.FTNumber));
                    break;
                case EnumMetaDatumType.enumSTNONUMMED://单表条形码介质
                    items = new List<DBFieldItem>();
                    items.Add(new DBFieldItem(FLD_NAME_F_XULIKUFANG, _virtualWarehouseAddress, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_TIAOXINMA, _barCode, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_DATUMNUM, _datumAmount, EnumDBFieldType.FTNumber));
                    items.Add(new DBFieldItem(FLD_NAME_F_MEDIUMTYPE, (int) _enumCarrierType, EnumDBFieldType.FTNumber));
                    break;
            }


            this._tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(DBHelper.GlobalDBHelper, this._dataId);
            string sqlStatement = SQLStringUtility.GetUpdateSQL(_tableName, items, strFilter, DBHelper.GlobalDBHelper);
            bool bSuccess = DBHelper.GlobalDBHelper.DoSQL(sqlStatement) > 0;
            return bSuccess;
        }

        bool IMetaData.Delete()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMetaDataFixedEdit Select()
        {
            string strFilter = FLD_NAME_F_DATAID + " = " + _dataId;
            DataTable dtResult = DoQuery(strFilter);
            IList<IMetaDataFixedEdit> pList = Translate(dtResult);
            return pList.Count > 0 ? pList[0] : null;
        }


        #endregion

        #region translate
        /// <summary/>
        /// 解析DataTable
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        private IList<IMetaDataFixedEdit> Translate(DataTable dtResult)
        {
            IList<IMetaDataFixedEdit> list = new List<IMetaDataFixedEdit>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    DataRow pRow = dtResult.Rows[i];
                    IMetaDataFixedEdit info = new MetaDataFixedInfo(_dbHelper);
                    switch (_enumMetaDatumType)
                    {
                        case EnumMetaDatumType.enumSTGENMED: //单表普通介质
                            info.VirtualWarehouseAddress = GetSafeDataUtility.ValidateDataRow_S(pRow,
                                                                                                FLD_NAME_F_XULIKUFANG);
                            info.DatumAmount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATUMNUM);
                            info.EnumMediumtype =
                                (EnumMediumType) (GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_MEDIUMTYPE));
                            list.Add(info);
                            break;
                        case EnumMetaDatumType.enumSTMetaTable: //单元数据表
                            info.Location = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCATION);
                            info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                            info.DataSize = GetSafeDataUtility.ValidateDataRow_F(pRow, FLD_NAME_F_DATASIZE);
                            info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                            info.IsHasAFile = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISHASAFILE) == 0;
                            list.Add(info);
                            break;
                        case EnumMetaDatumType.enumSTELEDatumMED: //单表电子资料介质关联
                            info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                            info.DataTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATATYPENAME);
                            info.DataSize = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATASIZE);
                            info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                            info.Location = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCATION);
                            info.IsSpatial = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISSPATIAL) == 0;
                            list.Add(info);
                            break;
                        case EnumMetaDatumType.enumMTELEDatum: //主从表电子资料
                            info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                            info.DataTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATATYPENAME);
                            info.DataSize = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATASIZE);
                            info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                            info.Location = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCATION);
                            info.RelDataID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_REDATAID);
                            list.Add(info);
                            break;
                        case EnumMetaDatumType.enumMTGENMED: //主从表介质资料
                            info.VirtualWarehouseAddress = GetSafeDataUtility.ValidateDataRow_S(pRow,
                                                                                                FLD_NAME_F_XULIKUFANG);
                            info.BarCode = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TIAOXINMA);
                            info.DatumAmount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATUMNUM);
                            info.EnumMediumtype =
                                (EnumMediumType) (GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_MEDIUMTYPE));
                            list.Add(info);
                            break;

                        case EnumMetaDatumType.enumSTELEDatum: //单表电子资料
                            info.Location = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_LOCATION);
                            info.ServerId = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                            info.DataSize = GetSafeDataUtility.ValidateDataRow_F(pRow, FLD_NAME_F_DATASIZE);
                            info.DataTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATATYPENAME);
                            info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                            info.IsSpatial = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISSPATIAL) == 0;
                            list.Add(info);
                            break;
                        case EnumMetaDatumType.enumSTNONUMMED:
                            info.VirtualWarehouseAddress = GetSafeDataUtility.ValidateDataRow_S(pRow,
                                                                                                FLD_NAME_F_XULIKUFANG);
                            info.DatumAmount = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATUMNUM);
                            info.BarCode = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_TIAOXINMA);
                            info.EnumMediumtype =
                                (EnumMediumType) (GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_MEDIUMTYPE));
                            list.Add(info);
                            break;
                    }

                }
            }
            return list;
        }


        private DataTable DoQuery(string strFilter)
        {
            string sqlStatement = "SELECT " + getSelectField(_enumMetaDatumType) + " FROM " + _tableName + " where " + strFilter + " order by " + FLD_NAME_F_DATAID;
            DataTable dtResult = _dbHelper.DoQueryEx(_tableName, sqlStatement, true);
            return dtResult;
        }


        #endregion

        /// <summary>
        /// 根据数据ID判断该条数据是否在对应数据库表中存在
        /// </summary>
        /// <returns></returns>
        private bool bExist(out IMetaDataFixedEdit metaData)
        {
            metaData = this.Select();
            return metaData != null;
        }
    }
}