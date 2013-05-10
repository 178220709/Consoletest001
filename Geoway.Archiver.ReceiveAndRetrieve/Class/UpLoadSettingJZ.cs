using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.UpLoad;
using Geoway.Archiver.Catalog;
using Geoway.ADF.MIS.DataModel;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    using Catalog.Interface;

    public class UpLoadSettingJZ : IUpLoadSetting
    {
        private ICatalogNode _catalogNode;

        public ICatalogNode CatalogNode
        {
            get { return _catalogNode; }
            set { _catalogNode = value; }
        }

        private string _dataName;

        public string DataName
        {
            get { return _dataName; }
            set { _dataName = value; }
        }

        private bool _bUpLoadDataToServer;
        /// <summary>
        /// 是否上传数据(或附件)到服务器
        /// </summary>
        public bool BUpLoadDataToServer
        {
            get { return _bUpLoadDataToServer; }
            set { _bUpLoadDataToServer = value; }
        }
        private int _serverID;
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }
        private string _storagePath;
        public string StoragePath
        {
            get { return _storagePath; }
            set { _storagePath = value; }
        }

        private Dictionary<string,object> _dicMetaData;
        
        

        public Dictionary<string, object> DicMetaData
        {
            get { return _dicMetaData; }
            set { _dicMetaData = value; }
        }
    }
}
