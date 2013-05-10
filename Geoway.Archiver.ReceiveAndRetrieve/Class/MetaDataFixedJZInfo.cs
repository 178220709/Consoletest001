using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using Interface.Register;
    using Geoway.Archiver.ReceiveAndRetrieve.Definition;


    public class MetaDataFixedJZInfo :MetaDataBaseInfo, IMetaDataFixedJZEdit
    {
        public const string FLD_NAME_F_XULIKUFANG = "F_XULIKUFANG";//数据标识，元数据的选择、删除、更新皆基于此
        public const string FLD_NAME_F_TIAOXINMA = "F_TIAOXINMA";
        public const string FLD_NAME_F_DATUMNUM = "F_DATUMNUM";
        public const string FLD_NAME_F_ZAITITYPE = "F_ZAITITYPE";

        public MetaDataFixedJZInfo(IDBHelper dbHelper) : base(dbHelper) { }

        public MetaDataFixedJZInfo(IDBHelper dbHelper, int dataID):base(dbHelper,dataID)
        {
            
        }


        #region IMetaDataFixedJZEdit 成员

        protected string _virtualWarehouseAddress;
        public string VirtualWarehouseAddress
        {
            get { return _virtualWarehouseAddress; }
            set { _virtualWarehouseAddress = value; }
        }

        protected string _barCode;
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; }
        }

        protected int _datumAmount;
        public int DatumAmount
        {
            get { return _datumAmount; }
            set { _datumAmount = value; }
        }

        protected EnumCarrierType _enumCarrierType;
        public EnumCarrierType enumCarrierType
        {
            get { return _enumCarrierType; }
            set { _enumCarrierType = value; }
        }

        #endregion

        protected string getSelectField()
        {
            string fields =
                FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_XULIKUFANG + "," +
                FLD_NAME_F_TIAOXINMA + "," +
                FLD_NAME_F_DATUMNUM + "," +
                FLD_NAME_F_ZAITITYPE;
            return fields;
        }
    }
}