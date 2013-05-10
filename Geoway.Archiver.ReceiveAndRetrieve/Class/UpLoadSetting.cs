using Geoway.Archiver.Catalog;
using System;
using System.Data;
using Geoway.Archiver.Catalog.Interface;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Utility;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class UpLoadSetting : IBaseSetting, IMSDZSetting
    {
        private ICatalogNode _catalogNode;
        private string _dataNameFieldName;
        private int _serverID;
        private string _storagePath;
        private bool _bUpLoadDataToServer;
        private DataTable _metaTable;
        private EnumPathStorageType _enumPathStorageType = EnumPathStorageType.EnumDB;
        private bool _bScanMode = false;
        private bool _bAffix = false;
        private List<DBFieldItem> _outerFieldItems;
        private EnumDriveType _enumDriveType = EnumDriveType.enumUNKnown;
        private int _reDataId=-1;
        private string _diskSn;


        public DataTable MetaTable
        {
            get { return _metaTable; }
            set { _metaTable = value; }
        }
        
        public ICatalogNode CatalogNode
        {
            get { return _catalogNode; }
            set { _catalogNode = value; }
        }
        
        ///// <summary>
        ///// 记录数据名称的属性名
        ///// </summary>
        //public string DataNameFieldName
        //{
        //    get { return _dataNameFieldName; }
        //    set { _dataNameFieldName = value; }
        //}
        
        /// <summary>
        /// 存储节点ID
        /// </summary>
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }
        
        /// <summary>
        /// 存储目录
        /// </summary>
        public string StoragePath
        {
            get { return _storagePath; }
            set { _storagePath = value; }
        }

        /// <summary>
        /// 是否为扫描模式
        /// </summary>
        public bool BScanMode
        {
            get { return _bScanMode; }
            set { _bScanMode = value; }
        }

        /// <summary>
        /// 上传的实体数据是否为附件形式
        /// </summary>
        public bool BAffix
        {
            get { return _bAffix; }
            set { _bAffix = value; }
        }

        /// <summary>
        /// 是否上传数据到服务器（数据实体或附件）
        /// </summary>
        public bool BUpLoadDataToServer
        {
            get { return _bUpLoadDataToServer; }
            set { _bUpLoadDataToServer = value; }
        }

        public List<DBFieldItem> OuterFieldItems
        {
            get { return _outerFieldItems; }
            set { _outerFieldItems = value; }
        }
        



        #region 显示实现IMSDZSetting

        /// <summary>
        /// 数据路径的存储方式 
        /// </summary>
        EnumPathStorageType IMSDZSetting.enumPathStorageType
        {
            get { return _enumPathStorageType; }
            set { _enumPathStorageType = value; }
        }
        #endregion

        /// <summary>
        /// 入库数据来源类型
        /// </summary>
        public EnumDriveType DriveType
        {
            get { return _enumDriveType; }
            set { _enumDriveType = value; }
        }
        /// <summary>
        /// 关联数据Id
        /// </summary>
        public int ReDataId
        {
            get { return _reDataId; }
            set { _reDataId = value; }
        }
        /// <summary>
        /// 磁盘序列号
        /// </summary>
        public string DiskSn
        {
            get { return _diskSn; }
            set { _diskSn = value; }
        }
    }

    public interface IBaseSetting
    {
        bool BUpLoadDataToServer { get; set; }
        ICatalogNode CatalogNode { get; set; }
        //string DataNameFieldName { get; set; }
        DataTable MetaTable { get; set; }
        int ServerID { get; set; }
        string StoragePath { get; set; }
        /// <summary>
        /// 是否为扫描模式
        /// </summary>
        bool BScanMode { get; set; }
        /// <summary>
        /// 上传的实体数据是否为附件形式
        /// </summary>
        bool BAffix { get; set; }
    }

    /// <summary>
    /// 主从表电子UI入库设置
    /// </summary>
    public interface IMSDZSetting
    {
        /// <summary>
        /// 数据路径的存储方式 
        /// </summary>
        EnumPathStorageType enumPathStorageType { get; set; }
    }

    /// <summary>
    /// 数据路径的存储方式 
    /// </summary>
    public enum EnumPathStorageType
    {
        UnKnown = -1,

        /// <summary>
        /// 以数据表存储
        /// </summary>
        EnumDB = 0,

        /// <summary>
        /// 以xml方式存储
        /// </summary>
        EnumXML = 1,

        /// <summary>
        /// 以DataTable方式存储
        /// </summary>
        EnumDataTable = 2
    }

}
