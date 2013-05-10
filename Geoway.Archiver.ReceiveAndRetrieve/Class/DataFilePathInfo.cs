using System.Collections.Generic;
using System.IO;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 数据路径信息类
    /// </summary>
    public class DataFilePathInfo
    {

        #region 基本信息

        private double _dataSize;
        /// <summary>
        /// 获取到的数据包总数据大小（用于使用ADF的CatalogDataScaner后替换GetAllSize方法）
        /// 使用CatalogDataScaner前未使用此属性
        /// </summary>
        public double DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }
        
        private GwDataObject _dataObject;
        /// <summary>
        /// 本数据路径信息依托的数据类型
        /// 可为空
        /// </summary>
        public GwDataObject DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }
        private string _rootFolderPath;
        /// <summary>
        /// 解析数据的根目录
        /// 
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
            get { return _dataName; }
            set { _dataName = value; }
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
        private string _mainFileName;
        /// <summary>
        /// 主数据文件名称
        /// 冗余存储
        /// </summary>
        public string MainFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(_mainFileName))
                {
                    return _mainFileName;
                }
                return Path.GetFileNameWithoutExtension(_mainFileInstance.FullFileName);
            }
            set { _mainFileName = value; }
        }

        private FileInstance _mainFileInstance;
        /// <summary>
        /// 主数据文件路径
        /// </summary>
        public FileInstance MainFileInstance
        {
            get { return _mainFileInstance; }
            set { _mainFileInstance = value; }
        }

        private FileInstance _snapShotFileInstance;
        /// <summary>
        /// 快视图路径
        /// </summary>
        public FileInstance SnapShotFileInstance
        {
            get { return _snapShotFileInstance; }
            set { _snapShotFileInstance = value; }
        }

        private FileInstance _thumbFileInstance;
        /// <summary>
        /// 拇指图路径
        /// </summary>
        public FileInstance ThumbFileInstance
        {
            get { return _thumbFileInstance; }
            set { _thumbFileInstance = value; }
        }

        private FileInstance _metaDataFileInstance;

        /// <summary>
        /// 元数据文件路径
        /// </summary>
        public FileInstance MetaDataFileInstance
        {
            get { return _metaDataFileInstance; }
            set { _metaDataFileInstance = value; }
        }

        private FileInstance _indexFileInstance;
        /// <summary>
        /// 索引文件路径
        /// </summary>
        public FileInstance IndexFileInstance
        {
            get { return _indexFileInstance; }
            set { _indexFileInstance = value; }
        }

        private FileInstance _ckMetadataInstance;
        /// <summary>
        /// 参考元数据文件
        /// </summary>
        public FileInstance CKMetadataInstance
        {
            get { return _ckMetadataInstance; }
            set { _ckMetadataInstance = value; }
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
                
                DataPackage dataPackage = _dataObject as DataPackage;
                GwDataObject dataKeyObject = dataPackage.GetDataKeyObject(); 
                
                
                
                EnumObjectType enumObjectType = dataKeyObject.ObjectType;
                switch (enumObjectType)
                {
                    case EnumObjectType.DataPackage: //主数据为数据包
                        GetFiles_DataPackage();
                        break;
                    case EnumObjectType.DataFolder: //主数据为文件夹

                    case EnumObjectType.DataFile: //主数据为文件
                        GetFiles_DataFile();
                        break;
                    default:
                        return null;
                }

                return _allUpLoadFilesInstance;
            }
            set { _allUpLoadFilesInstance = value; }
            
        }

     

        private void GetFiles_DataFile()
        {
            _allUpLoadFilesInstance = new List<FileInstance>();
            //存在数据包的情况下
            if (_dataObject != null)
            {
                add(_mainFileInstance);
            }

            if (!_bOutIndexFile)
            {
                add(_indexFileInstance);
            }

            if (!_bOutCKMetaData)
            {
                add(_ckMetadataInstance);
            }

            if (!_bOutMetaData)
            {
                add(_metaDataFileInstance);
            }

            if (!_bOutSnopshot)
            {
                add(_snapShotFileInstance);
            }

            if (!_bOutThumb)
            {
                add(_thumbFileInstance);
            }

            _allUpLoadFilesInstance.AddRange(_otherFilesInstance);
        }

        private void GetFiles_DataPackage()
        {
            _allUpLoadFilesInstance = new List<FileInstance>();
            if (Directory.Exists(_mainFileName))
            {
                string[] files = Directory.GetFiles(_mainFileName, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    FileInstance fileInstance = new FileInstance(file, "");
                    _allUpLoadFilesInstance.Add(fileInstance);
                }
            }
        }

        private void add(FileInstance fileInstance)
        {
            if (fileInstance == null)
            {
                return;
            }
            
            FileInstance replaceInstance;

            if (DataInstanceHelper.CanAddToList(_otherFilesInstance, fileInstance))
            {
                _allUpLoadFilesInstance.Add(fileInstance);
            }
            else if (DataInstanceHelper.CanReplace(_otherFilesInstance, fileInstance, out replaceInstance))
            {
                replaceInstance.PackagePath = fileInstance.PackagePath;
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
                    //FileNameUtil.GetFileSize(filePath.FullFileName);
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