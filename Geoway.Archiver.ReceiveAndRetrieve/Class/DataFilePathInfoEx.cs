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
    /// ����·����Ϣ��
    /// </summary>
    public class DataFilePathInfoEx
    {

        #region private �ֶ�

        private IDBHelper _db = null;

        #endregion

        #region ���캯��

        public DataFilePathInfoEx(IDBHelper db)
        {
            _db = db;
        }

        #endregion

        #region ������Ϣ

        private CatalogData _dataEntity = null;
        /// <summary>
        /// ����ʵ��
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
        /// ��ȡ�������ݰ������ݴ�С������ʹ��ADF��CatalogDataScaner���滻GetAllSize������
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
        /// ������·����Ϣ���е���������
        /// ��Ϊ��
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
        /// �������ݵĸ�Ŀ¼
        /// </summary>
        public string RootFolderPath
        {
            get { return _rootFolderPath; }
            set { _rootFolderPath = value; }
        }
        /// <summary>
        /// ���ݰ�DataObject�����ļ�������
        /// ��������Ϊspot��ʱ��Ϊ������
        /// ������Ϊ����洢������ɾ����
        /// </summary>
        public string FolderName
        {
            get
            { return _folderInfo != null ? _folderInfo.Name : ""; }
        }


        private DirectoryInfo _folderInfo;
        //����ȡ���������ݰ������ļ���
        public DirectoryInfo FolderInfo
        {
            get { return _folderInfo; }
            set { _folderInfo = value; }
        }
        
        private string _dataName;
        /// <summary>
        /// ���ݰ�����
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
        
        #region �ж��Ƿ�Ϊ�ⲿ�ļ��������
        private bool _bOutMetaData;
        /// <summary>
        /// �Ƿ�Ϊ�ⲿԪ����
        /// </summary>
        public bool BOutMetaData
        {
            get { return _bOutMetaData; }
            set { _bOutMetaData = value; }
        }

        private bool _bOutCKMetaData;
        /// <summary>
        /// �Ƿ�Ϊ�ⲿ�ο�Ԫ����
        /// </summary>
        public bool BOutCKMetaData
        {
            get { return _bOutCKMetaData; }
            set { _bOutCKMetaData = value; }
        }

        private bool _bOutSnopshot;
        /// <summary>
        /// �Ƿ�Ϊ�ⲿ����ͼ
        /// </summary>
        public bool BOutSnopshot
        {
            get { return _bOutSnopshot; }
            set { _bOutSnopshot = value; }
        }

        private bool _bOutThumb;
        /// <summary>
        /// �Ƿ�Ϊ�ⲿָ��Ĵָͼ
        /// </summary>
        public bool BOutThumb
        {
            get { return _bOutThumb; }
            set { _bOutThumb = value; }
        }

        private bool _bOutIndexFile;
        /// <summary>
        /// �Ƿ�Ϊ�ⲿ�ƶ������ļ�
        /// </summary>
        public bool BOutIndexFile
        {
            get { return _bOutIndexFile; }
            set { _bOutIndexFile = value; }
        }

        #endregion

        #region �ڲ�ָ���ļ�
        //private string _mainFileName;
        ///// <summary>
        ///// �������ļ�����
        ///// ����洢
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
        /// �������ļ�·��
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
        /// ����ͼ·��
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
        /// Ĵָͼ·��
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
        /// Ԫ�����ļ�·��
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
        /// �����ļ�·��
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
        /// �ο�Ԫ�����ļ�
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

        #region �ⲿָ���ļ�
        private FileInstance _outSnapShotFileInstance;
        /// <summary>
        /// ����ͼ·��
        /// </summary>
        public FileInstance OutSnapShotFileInstance
        {
            get { return _outSnapShotFileInstance; }
            set { _outSnapShotFileInstance = value; }
        }

        private FileInstance _outThumbFileInstance;
        /// <summary>
        /// Ĵָͼ·��
        /// </summary>
        public FileInstance OutThumbFileInstance
        {
            get { return _outThumbFileInstance; }
            set { _outThumbFileInstance = value; }
        }

        private FileInstance _OutMetaDataFileInstance;

        /// <summary>
        /// Ԫ�����ļ�·��
        /// </summary>
        public FileInstance OutMetaDataFileInstance
        {
            get { return _OutMetaDataFileInstance; }
            set { _OutMetaDataFileInstance = value; }
        }

        private FileInstance _outIndexFileInstance;
        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public FileInstance OutIndexFileInstance
        {
            get { return _outIndexFileInstance; }
            set { _outIndexFileInstance = value; }
        }

        private FileInstance _outCkMetadataInstance;
        /// <summary>
        /// �ο�Ԫ�����ļ�
        /// </summary>
        public FileInstance OutCKMetadataInstance
        {
            get { return _outCkMetadataInstance; }
            set { _outCkMetadataInstance = value; }
        }
        #endregion

        #region private ����

        private FileInstance GetMainInstanse()
        {
            if (_dataObject == null)
            {
                _dataObject = CatalogModelEngine.GetDataObjectByName(_db, _dataEntity.DataType);
            }

            // ��ȡ�����ݱ�ʶ����
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
                    // ��֧��
                    break;
                case EnumObjectType.DataPackage:
                    fullName = _folderInfo.FullName;
                    packagePath = gwObject.GetXPath();
                    break;
            }
           
            // ����������ʵ��
            FileInstance mainIns = new FileInstance(fullName, packagePath);
            
            return mainIns;
        }

        #endregion

        #region public ����

        /// <summary>
        /// ��ȡָ�����Ե��ļ�
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

        #region ���������

        private List<FileInstance> _otherFilesInstance = new List<FileInstance>();
        /// <summary>
        /// �����ļ���·������
        /// </summary>
        public List<FileInstance> OtherFilesInstance
        {
            get { return _otherFilesInstance; }
            set { _otherFilesInstance = value; }
        }

        private List<FileInstance> _allUpLoadFilesInstance;
        /// <summary>
        /// ����Ҫ�ϴ��������б�
        /// </summary>
        public List<FileInstance> AllUpLoadFilesInstance
        {
            get
            {
                //�����ظ�������ӹ���
                if (_allUpLoadFilesInstance != null && _allUpLoadFilesInstance.Count >= _otherFilesInstance.Count)
                {
                    return _allUpLoadFilesInstance;
                }
                //���������Ͳ���ص����UI����ʱֻ�п��Ƶ����Ϊ�����
                if (_dataObject == null)
                {
                    return _otherFilesInstance;
                }
                
                // TODO:��ȡ�����ļ�
                GetFiles();

                return _allUpLoadFilesInstance;
            }
            set { _allUpLoadFilesInstance = value; }
            
        }
  
        /// <summary>
        /// ��ȡ����ʵ��������ļ�
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
        /// ��ȡ�������ݵĴ�С(��λ:B)
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