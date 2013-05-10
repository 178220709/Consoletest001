using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class MetaDataFixedDZInfo:MetaDataBaseInfo,IMetaDataFixedDZEdit
    {
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_DATAUNIT = "F_DATAUNIT";
        public const string FLD_NAME_F_LOCATION = "F_LOCATION";
        public const string FLD_NAME_F_SERVERID = "F_SERVERID";
        public const string FLD_NAME_F_DATATYPENAME = "F_DATATYPENAME";

        #region 构造函数
        public MetaDataFixedDZInfo(IDBHelper dbHelper):base(dbHelper){}
        public MetaDataFixedDZInfo(IDBHelper dbHelper,int dataID):base(dbHelper,dataID)
        {
            
        }
        #endregion
        
        #region IMetaDataFixedEdit 成员
        protected double _dataSize;
        public double DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        protected string _dataTypeName;
        public string DataTypeName
        {
            get { return _dataTypeName; }
            set { _dataTypeName = value; }
        }

        protected string _dataUnit;
        public string DataUnit
        {
            get { return _dataUnit; }
            set { _dataUnit = value; }
        }

        protected long _serverID;
        public long ServerId
        {
            get { return _serverID; }
            set{_serverID = value;}
        }

        protected string _location;
        public string Location
        {
            get{return _location;}
            set{_location = value;}
        }

        protected string getSelectField()
        {
            string fields =
                FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_DATAUNIT + "," +
                FLD_NAME_F_DATATYPENAME + "," +
                FLD_NAME_F_LOCATION + "," +
                FLD_NAME_F_SERVERID;
            return fields;
        }

        #endregion

    }
}