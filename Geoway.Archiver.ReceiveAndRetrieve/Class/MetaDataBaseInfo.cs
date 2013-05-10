using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class MetaDataBaseInfo:IMetaDataEdit
    {
        protected const string FLD_NAME_F_DATAID = "F_DATAID";

        protected IDBHelper _dbHelper;
        
        
        public MetaDataBaseInfo(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        
        public MetaDataBaseInfo(IDBHelper dbHelper, int dataID)
        {
            _dbHelper = dbHelper;
            try
            {
                if (dataID > 0)
                {
                    _tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, dataID);
                }
                _dataId = dataID;
            }
            catch(Exception ex)
            {
                
            }
        }
        
        #region IMetaDataEdit 成员
        protected int _dataId;
        public int DataId
        {
            get { return _dataId; }
            set
            {
                _dataId = value;
                try
                {
                    if (_dataId > 0)
                    {
                        _tableName = DataidMetaDAL.SingleInstance.GetTableNamebyDataID(_dbHelper, _dataId);
                    }
                }
                catch(Exception ex)
                {
                    
                }
            }
        }

        protected string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        #endregion
    }
}
