namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using ADF.MIS.DB.Public.Interface;
    using Archiver.Utility.Definition;
    using Definition;
    using Interface.Register;
using Geoway.Archiver.Modeling.Definition;

    public class MetaDataFixedInfo : MetaDataBaseInfo, IMetaDataFixedEdit
    {
        public const string FLD_NAME_F_XULIKUFANG = "F_XULIKUFANG";
        public const string FLD_NAME_F_TIAOXINMA = "F_TIAOXINMA";
        public const string FLD_NAME_F_DATUMNUM = "F_DATUMNUM";
        /// <summary>
        /// 介质类型
        /// </summary>
        public const string FLD_NAME_F_MEDIUMTYPE = "F_MEDIUMTYPE";
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_DATAUNIT = "F_DATAUNIT";
        public const string FLD_NAME_F_LOCATION = "F_LOCATION";
        public const string FLD_NAME_F_SERVERID = "F_SERVERID";
        public const string FLD_NAME_F_DATATYPENAME = "F_DATATYPENAME";
        public const string FLD_NAME_F_ISSPATIAL = "F_ISSPATIAL";
        public const string FLD_NAME_F_ISHASAFILE = "F_ISHASAFILE";
        public const string FLD_NAME_F_REDATAID = "F_REDATAID";
        public const string FLD_NAME_F_SRCDATAPATH = "F_SRCDATAPATH";
        

        #region 构造函数
        public MetaDataFixedInfo(IDBHelper dbHelper) : base(dbHelper) { }
        public MetaDataFixedInfo(IDBHelper dbHelper, int dataID)
            : base(dbHelper, dataID)
        {

        }
        #endregion

        #region IMetaDataFixedEdit 成员
        protected double _dataSize;
        protected string _location;
        protected bool _isSpatial;
        protected bool _isHasAFile;
        protected EnumMediumType _enumCarrierType;
        protected int _datumAmount;
        protected string _barCode;
        protected string _virtualWarehouseAddress;
        protected string _dataTypeName;
        protected string _dataUnit;
        protected int _relDataID;
        protected string _scrDataPath;

        /// <summary>
        /// 虚拟库房位置
        /// </summary>
        public string VirtualWarehouseAddress
        {
            get { return _virtualWarehouseAddress; }
            set { _virtualWarehouseAddress = value; }
        }

        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; }
        }

        /// <summary>
        /// 资料数量
        /// </summary>
        public int DatumAmount
        {
            get { return _datumAmount; }
            set { _datumAmount = value; }
        }

        /// <summary>
        /// 载体类型
        /// </summary>
        public EnumMediumType EnumMediumtype
        {
            get { return _enumCarrierType; }
            set { _enumCarrierType = value; }
        }

        public double DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        
        public string DataTypeName
        {
            get { return _dataTypeName; }
            set { _dataTypeName = value; }
        }

       
        public string DataUnit
        {
            get { return _dataUnit; }
            set { _dataUnit = value; }
        }

        protected long _serverID;
        

        public long ServerId
        {
            get { return _serverID; }
            set { _serverID = value; }
        }



        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>
        /// 是否包含附件
        /// </summary>
        public bool IsHasAFile
        {
            get { return _isHasAFile; }
            set { _isHasAFile = value; }
        }

        /// <summary>
        /// 是否存储到空间数据源
        /// </summary>
        public bool IsSpatial
        {
            get { return _isSpatial; }
            set { _isSpatial = value; }
        }

        /// <summary>
        /// 关联字段ID
        /// </summary>
        public int RelDataID
        {
            get { return _relDataID; }
            set { _relDataID = value; }
        }
        /// <summary>
        /// 原文件目录
        /// </summary>
        public string ScrDataPath
        {
            get { return _scrDataPath; }
            set { _scrDataPath = value; }
        }

        protected string getSelectField(EnumMetaDatumType enumMetaDatumType)
        {
            string fields = "";
            switch (enumMetaDatumType)
            {
                case EnumMetaDatumType.enumSTGENMED: //单表普通介质
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_XULIKUFANG + "," +
                        FLD_NAME_F_DATUMNUM + "," +
                        FLD_NAME_F_MEDIUMTYPE;
                    break;
                case EnumMetaDatumType.enumSTMetaTable: //单元数据表
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_DATASIZE + "," +
                        FLD_NAME_F_DATAUNIT + "," +
                        FLD_NAME_F_LOCATION + "," +
                        FLD_NAME_F_ISHASAFILE + "," +
                        FLD_NAME_F_SERVERID;
                    break;
                case EnumMetaDatumType.enumSTELEDatumMED:
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_DATATYPENAME + "," +
                        FLD_NAME_F_DATASIZE + "," +
                        FLD_NAME_F_DATAUNIT + "," +
                        FLD_NAME_F_LOCATION + "," +
                        FLD_NAME_F_ISSPATIAL + "," +
                        FLD_NAME_F_REDATAID;
                    break;
                case EnumMetaDatumType.enumMTELEDatum:
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_SERVERID + "," +
                        FLD_NAME_F_DATATYPENAME + "," +
                        FLD_NAME_F_DATASIZE + "," +
                        FLD_NAME_F_DATAUNIT + "," +
                        FLD_NAME_F_LOCATION + "," +
                        FLD_NAME_F_REDATAID;
                    break;
                case EnumMetaDatumType.enumMTGENMED:
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_XULIKUFANG + "," +
                        FLD_NAME_F_TIAOXINMA + "," +
                        FLD_NAME_F_DATUMNUM + "," +
                        FLD_NAME_F_MEDIUMTYPE + "," +
                        FLD_NAME_F_MEDIUMTYPE;
                    break;
                case EnumMetaDatumType.enumSTELEDatum:
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_DATASIZE + "," +
                        FLD_NAME_F_DATAUNIT + "," +
                        FLD_NAME_F_DATATYPENAME + "," +
                        FLD_NAME_F_LOCATION + "," +
                        FLD_NAME_F_ISSPATIAL + "," +
                        FLD_NAME_F_SERVERID;
                    break;
                case EnumMetaDatumType.enumSTNONUMMED:
                    fields =
                        FLD_NAME_F_DATAID + "," +
                        FLD_NAME_F_XULIKUFANG + "," +
                        FLD_NAME_F_DATUMNUM + "," +
                        FLD_NAME_F_TIAOXINMA + "," +
                        FLD_NAME_F_MEDIUMTYPE;
                    break;
            }
            return fields;
        }
        #endregion
    }
}