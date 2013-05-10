using System.Collections.Generic;
using System.IO;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.CatalogDataModel.Public.Catalog;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 数据路径信息类
    /// </summary>
    public class DataFilePathInfoEx
    {

        #region private 字段

        private IDBHelper _db = null;

        #endregion

        #region 构造函数

        public DataFilePathInfoEx(IDBHelper db)
        {
            _db = db;
        }

        #endregion

        #region 基本信息

        private CatalogData _dataEntity = null;
        /// <summary>
        /// 数据实体
        /// </summary>
        public CatalogData DataEntity
        {
            get
            {
                return _dataEntity;
            }
            set
            {
                _dataEntity = value;
            }
        }

        private double _dataSize;
        /// <summary>
        /// 获取到的数据包总数据大小（用于使用ADF的CatalogDataScaner后替换GetAllSize方法）
        /// </summary>
        public double DataSize
        {
            get 
            {
                _dataSize = GetAllSize();
                return _dataSize;
            }
        }
        
        private GwDataObject _dataObject = null;
        /// <summary>
        /// 本数据路径信息依托的数据类型
        /// 可为空
        /// </summary>
        public GwDataObject DataObject
        {
            get 
            {
                if (_dataEntity != null)
                {
                    if (_dataObject == null)
                    {
                        _dataObject = CatalogModelEngine.GetDataObjectByName(_db, _dataEntity.DataType);
                    }
                }
                else
                {
                    _dataObject = null;
                }
                return _dataObject;
            }
        }
        private string _rootFolderPath = string.Empty;
        /// <summary>
        /// 解析数据的根目录
        /// </summary>
        public string RootFolderPath
        {
            get { return _rootFolderPath; }
            set { _rootFolderPath = value; }
        }
        /// <summary>
        /// 数据包DataObject所在文件夹名称
        /// 数据类型为spot等时即为数据名
        /// 此属性为冗余存储（后期删除）
        /// </summary>
        public string FolderName
        {
            get
            { return _folderInfo != null ? _folderInfo.Name : ""; }
        }


        private DirectoryInfo _folderInfo;
        //所获取的整个数据包所在文件夹
        public DirectoryInfo FolderInfo
        {
            get { return _folderInfo; }
            set { _folderInfo = value; }
        }
        
        private string _dataName;
        /// <summary>
        /// 数据包名称
        /// </summary>
        public string DataName
        {
            get
            {
                if (_dataEntity != null)
                {
                    _dataName = _dataEntity.DataName;
                }
                else
                {
                    _dataName = string.Empty;
                }
                return _dataName; 
            }
        }
        #endregion
        
        #region 判断是否为外部文件相关属性
        private bool _bOutMetaData;
        /// <summary>
        /// 是否为外部元数据
        /// </summary>
        public bool BOutMetaData
        {
            get { return _bOutMetaData; }
            set { _bOutMetaData = value; }
        }

        private bool _bOutCKMetaData;
        /// <summary>
        /// 是否为外部参考元数据
        /// </summary>
        public bool BOutCKMetaData
        {
            get { return _bOutCKMetaData; }
            set { _bOutCKMetaData = value; }
        }

        private bool _bOutSnopshot;
        /// <summary>
        /// 是否为外部快视图
        /// </summary>
        public bool BOutSnopshot
        {
            get { return _bOutSnopshot; }
            set { _bOutSnopshot = value; }
        }

        private bool _bOutThumb;
        /// <summary>
        /// 是否为外部指定拇指图
        /// </summary>
        public bool BOutThumb
        {
            get { return _bOutThumb; }
            set { _bOutThumb = value; }
        }

        private bool _bOutIndexFile;
        /// <summary>
        /// 是否为外部制定索引文件
        /// </summary>
        public bool BOutIndexFile
        {
            get { return _bOutIndexFile; }
            set { _bOutIndexFile = value; }
        }

        #endregion

        #region 内部指定文件
        //private string _mainFileName;
        ///// <summary>
        ///// 主数据文件名称
        ///// 冗余存储
        ///// </summary>
        //public string MainFileName
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(_mainFileName))
        //        {
        //            return _mainFileName;
        //        }
        //        return Path.GetFileNameWithoutExtension(_mainFileInstance.FullFileName);
        //    }
        //    set { _mainFileName = value; }
        //}

        private FileInstance _mainInstance;
        /// <summary>
        /// 主数据文件路径
        /// </summary>
        public FileInstance MainInstance
        {
            get 
            { 
                if (_dataEntity != null)
                {
                    if (_mainInstance == null)
                    {
                        _mainInstance = GetMainInstanse();
                    }
                }
                return _mainInstance; 
            }
        }

        private FileInstance _snapShotFileInstance = null;
        /// <summary>
        /// 快视图路径
        /// </summary>
        public FileInstance SnapShotFileInstance
        {
            get 
            {
                if (_dataEntity != null)
                {
                    if (_snapShotFileInstance == null)
                    {
                        _snapShotFileInstance = GetSpecificFile(FileAttribute.SNAPSHOT);
                    }
                }
                return _snapShotFileInstance; 
            }
        }

        private FileInstance _thumbFileInstance = null;
        /// <summary>
        /// 拇指图路径
        /// </summary>
        public FileInstance ThumbFileInstance
        {
            get
            {
                if (_dataEntity != null)
                {
                    if (_thumbFileInstance == null)
                    {
                        _thumbFileInstance = GetSpecificFile(FileAttribute.MZTFILE);
                    }
                }
                return _thumbFileInstance;
            }
        }

        private FileInstance _metaDataFileInstance = null;

        /// <summary>
        /// 元数据文件路径
        /// </summary>
        public FileInstance MetaDataFileInstance
        {
            get 
            {
                if (_dataEntity != null)
                {
                    if (_metaDataFileInstance == null)
                    {
                        _metaDataFileInstance = GetSpecificFile(FileAttribute.METADATA);
                    }
                }
                return _metaDataFileInstance;
            }
        }

        private FileInstance _indexFileInstance = null;
        /// <summary>
        /// 索引文件路径
        /// </summary>
        public FileInstance IndexFileInstance
        {
            get 
            {
                if (_dataEntity != null)
                {
                    if (_indexFileInstance == null)
                    {
                        _indexFileInstance = GetSpecificFile(FileAttribute.INDEXFILE);
                    }
                }
                return _indexFileInstance; 
            }
        }

        private FileInstance _ckMetadataInstance = null;
        /// <summary>
        /// 参考元数据文件
        /// </summary>
        public FileInstance CKMetadataInstance
        {
            get 
            {
                if (_dataEntity != null)
                {
                    if (_ckMetadataInstance == null)
                    {
                        _ckMetadataInstance = GetSpecificFile(FileAttribute.CKMETAFILE);
                    }
                }
                return _ckMetadataInstance; 
            }
        }
        #endregion

        #region 外部指定文件
        private FileInstance _outSnapShotFileInstance;
        /// <summary>
        /// 快视图路径
        /// </summary>
        public FileInstance OutSnapShotFileInstance
        {
            get { return _outSnapShotFileInstance; }
            set { _outSnapShotFileInstance = value; }
        }

        private FileInstance _outThumbFileInstance;
        /// <summary>
        /// 拇指图路径
        /// </summary>
        public FileInstance OutThumbFileInstance
        {
            get { return _outThumbFileInstance; }
            set { _outThumbFileInstance = value; }
        }

        private FileInstance _OutMetaDataFileInstance;

        /// <summary>
        /// 元数据文件路径
        /// </summary>
        public FileInstance OutMetaDataFileInstance
        {
            get { return _OutMetaDataFileInstance; }
            set { _OutMetaDataFileInstance = value; }
        }

        private FileInstance _outIndexFileInstance;
        /// <summary>
        /// 索引文件路径
        /// </summary>
        public FileInstance OutIndexFileInstance
        {
            get { return _outIndexFileInstance; }
            set { _outIndexFileInstance = value; }
        }

        private FileInstance _outCkMetadataInstance;
        /// <summary>
        /// 参考元数据文件
        /// </summary>
        public FileInstance OutCKMetadataInstance
        {
            get { return _outCkMetadataInstance; }
            set { _outCkMetadataInstance = value; }
        }
        #endregion

        #region private 方法

        private FileInstance GetMainInstanse()
        {
            if (_dataObject == null)
            {
                _dataObject = CatalogModelEngine.GetDataObjectByName(_db, _dataEntity.DataType);
            }

            // 获取主数据标识类型
            GwDataObject gwObject = (_dataObject as DataPackage).GetDataKeyObject();
            List<CatalogFile> cFiles = new List<CatalogFile>();
            string fullName = string.Empty;
            string packagePath = string.Empty;
            switch(gwObject.ObjectType)
            {
                case EnumObjectType.DataFile:
                    cFiles = _dataEntity.GetTempCatalogFiles(gwObject.GetXPath());
                    if (cFiles.Count > 0)
                    {
                        fullName = cFiles[0].FileLocation;
                        packagePath = cFiles[0].PackagePath;
                    }
                    break;
                case EnumObjectType.DataFolder:
                    // 不支持
                    break;
                case EnumObjectType.DataPackage:
                    fullName = _folderInfo.FullName;
                    packagePath = gwObject.GetXPath();
                    break;
            }
           
            // 构建主数据实例
            FileInstance mainIns = new FileInstance(fullName, packagePath);
            
            return mainIns;
        }

        #endregion

        #region public 方法

        /// <summary>
        /// 获取指定属性的文件
        /// </summary>
        /// <param name="fileAttribute"></param>
        /// <returns></returns>
        public FileInstance GetSpecificFile(string fileAttribute)
        {
            List<FileInstance> specificFiles = new List<FileInstance>();
            CatalogFile[] files = _dataEntity.GetTempCatalogFiles();
            FileInstance tmpFileIns = null;
            if (files == null || files.Length == 0)
            {
                return null;
            }
            foreach (CatalogFile iFile in files)
            {
                if (iFile.Properties.Contains(fileAttribute))
                {
                    tmpFileIns = new FileInstance(iFile.FileLocation, iFile.PackagePath);
                    specificFiles.Add(tmpFileIns);
                }
            }

            if (specificFiles.Count == 0)
            {
                return null;
            }
            return specificFiles[0];
        }

        #endregion

        #region 结果类属性

        private List<FileInstance> _otherFilesInstance = new List<FileInstance>();
        /// <summary>
        /// 其它文件的路径集合
        /// </summary>
        public List<FileInstance> OtherFilesInstance
        {
            get { return _otherFilesInstance; }
            set { _otherFilesInstance = value; }
        }

        private List<FileInstance> _allUpLoadFilesInstance;
        /// <summary>
        /// 所有要上传的数据列表
        /// </summary>
        public List<FileInstance> AllUpLoadFilesInstance
        {
            get
            {
                //避免重复调用添加过程
                if (_allUpLoadFilesInstance != null && _allUpLoadFilesInstance.Count >= _otherFilesInstance.Count)
                {
                    return _allUpLoadFilesInstance;
                }
                //与数据类型不相关的入库UI，暂时只有控制点入库为此情况
                if (_dataObject == null)
                {
                    return _otherFilesInstance;
                }
                
                // TODO:获取所有文件
                GetFiles();

                return _allUpLoadFilesInstance;
            }
            set { _allUpLoadFilesInstance = value; }
            
        }
  
        /// <summary>
        /// 获取数据实体的所有文件
        /// </summary>
        private void GetFiles()
        {
            _allUpLoadFilesInstance = new List<FileInstance>();
            CatalogFile[] files = _dataEntity.GetTempCatalogFiles();
            if (files != null && files.Length != 0)
            {
                FileInstance fileInstance = null;
                foreach (CatalogFile file in files)
                {
                    fileInstance = new FileInstance(file.FileLocation, file.PackagePath); //file.LocalLocation;
                    _allUpLoadFilesInstance.Add(fileInstance);
                }
            }
        }

        /// <summary>
        /// 获取所有数据的大小(单位:B)
        /// </summary>
        /// <returns></returns>
        public double GetAllSize()
        {
            double totalSize = 0.0d;
            foreach (FileInstance filePath in AllUpLoadFilesInstance)
            {
                double singleSize = (new FileInfo(filePath.FullFileName)).Length;
                if (singleSize > 0)
                {
                    totalSize += singleSize;
                }
            }
            return totalSize;
        }
 
        #endregion
    }
}