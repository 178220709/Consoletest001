using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public class DataPathInfo
    {
        #region 属性

        private int _id;

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _objectID;

        /// <summary>
        /// 对象ID，关联HeadInfo标识
        /// </summary>
        public string ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        private string _registerLayerName;

        /// <summary>
        /// 登记图层名

        /// </summary>
        public string RegisterLayerName
        {
            get { return _registerLayerName; }
            set { _registerLayerName = value; }
        }

        private Int64 _dataSize;

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public Int64 DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        private string _fileLocation;

        /// <summary>
        /// 文件服务器相对路径


        /// </summary>
        public string FileLocation
        {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        private string _packagePath = string.Empty;

        /// <summary>
        /// 数据包内部x路径
        /// </summary>
        public string PackagePath
        {
            get { return _packagePath; }
            set { _packagePath = value; }
        }

        private EnumDataFileSourceType _sourceType = EnumDataFileSourceType.DataUnit;

        /// <summary>
        /// 文件来源
        /// </summary>
        public EnumDataFileSourceType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        private int _serverID = 0;

        /// <summary>
        /// 存储节点ID
        /// </summary>
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }

        private EnumPathStorageType _enumPathStorageType;

        public EnumPathStorageType EnumStorageType
        {
            get { return _enumPathStorageType; }
            set { _enumPathStorageType = value; }
        }


        private string _xmlPath;

        /// <summary>
        /// 记录路径的xml文件
        /// </summary>
        public string XmlPath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }

        #endregion

        public DataPathDAL ToDAL()
        {
            DataPathDAL dal = new DataPathDAL();
            dal.DataSize = this._dataSize;
            dal.EnumStorageType = this._enumPathStorageType;
            dal.ID = this.ID;
            dal.FileLocation = this._fileLocation;
            dal.ObjectID = this._objectID;
            dal.PackagePath = this._packagePath;
            dal.RegisterLayerName = this._registerLayerName;
            dal.ServerID = this._serverID;
            dal.SourceType = this._sourceType;
            dal.XmlPath = this._xmlPath;
            return dal;
        }

        public static DataPathInfo ToInfo(DataPathDAL dal)
        {
            DataPathInfo info = new DataPathInfo();
            info._dataSize = dal.DataSize;
            info._enumPathStorageType = dal.EnumStorageType;
            info.ID = dal.ID;
            info._fileLocation = dal.FileLocation;
            info._objectID = dal.ObjectID;
            info._packagePath = dal.PackagePath;
            info._registerLayerName = dal.RegisterLayerName;
            info._serverID = dal.ServerID;
            info._sourceType = dal.SourceType;
            info._xmlPath = dal.XmlPath;
            return info;
        }

        public bool Insert()
        {
            return this.ToDAL().Insert();
        }

        public IList<DataPathInfo> SeletByObjectID()
        {
            IList<DataPathDAL> dals = DataPathDAL.Singleton.SeletByObjectID(_objectID, EnumDataFileSourceType.DataUnit);
            List<DataPathDAL> listDAL = dals as List<DataPathDAL>;
            List<DataPathInfo> listInfo = listDAL.ConvertAll(new Converter<DataPathDAL, DataPathInfo>(ToInfo));
            return listInfo;
        }

        public IList<DataPathInfo> SeletByObjectID(IDBHelper db)
        {
            IList<DataPathDAL> dals = DataPathDAL.Singleton.SeletByObjectID(db, _objectID, EnumDataFileSourceType.DataUnit);
            List<DataPathDAL> listDAL = dals as List<DataPathDAL>;
            List<DataPathInfo> listInfo = listDAL.ConvertAll(new Converter<DataPathDAL, DataPathInfo>(ToInfo));
            return listInfo;
        }

        public static IList<string> GetPathesByObjectID(int objectID)
        {
            return DataPathDAL.Singleton.GetPathesByObjectID(objectID);
        }
    }
}