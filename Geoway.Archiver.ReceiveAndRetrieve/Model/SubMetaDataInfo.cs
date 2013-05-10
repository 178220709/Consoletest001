using System;
using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Catalog.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    [Serializable]
    public class SubMetaDataInfo
    {
        private Dictionary<string, object> _dicSubMetaData;

        public Dictionary<string, object> DicSubMetaData
        {
            get { return _dicSubMetaData; }
            set { _dicSubMetaData = value; }
        }

        private ICatalogNode _catalogNode;

        public ICatalogNode CatalogNode
        {
            get { return _catalogNode; }
            set { _catalogNode = value; }
        }

        private IDBHelper _dbHelper;

        public IDBHelper DbHelper
        {
            get { return _dbHelper; }
            set { _dbHelper = value; }
        }


        public bool Insert()
        {
            return this.ToDAL().Insert();
        }
        
        public SubMetaDataDAL ToDAL()
        {
            SubMetaDataDAL dal = new SubMetaDataDAL();
            dal.CatalogNode = this._catalogNode;
            dal.DbHelper = this._dbHelper;
            dal.DicSubMetaData = this._dicSubMetaData;
            dal.SubTableName = this._catalogNode.SubMetaTableName;
            return dal;
        }

    }
}