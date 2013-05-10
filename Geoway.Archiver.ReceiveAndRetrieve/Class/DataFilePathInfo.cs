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
    /// ����·����Ϣ��
    /// </summary>
    public class DataFilePathInfo
    {

        #region ������Ϣ

        private double _dataSize;
        /// <summary>
        /// ��ȡ�������ݰ������ݴ�С������ʹ��ADF��CatalogDataScaner���滻GetAllSize������
        /// ʹ��CatalogDataScanerǰδʹ�ô�����
        /// </summary>
        public double DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }
        
        private GwDataObject _dataObject;
        /// <summary>
        /// ������·����Ϣ���е���������
        /// ��Ϊ��
        /// </summary>
        public GwDataObject DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }
        private string _rootFolderPath;
        /// <summary>
        /// �������ݵĸ�Ŀ¼
        /// 
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
            get { return _dataName; }
            set { _dataName = value; }
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
        private string _mainFileName;
        /// <summary>
        /// �������ļ�����
        /// ����洢
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
        /// �������ļ�·��
        /// </summary>
        public FileInstance MainFileInstance
        {
            get { return _mainFileInstance; }
            set { _mainFileInstance = value; }
        }

        private FileInstance _snapShotFileInstance;
        /// <summary>
        /// ����ͼ·��
        /// </summary>
        public FileInstance SnapShotFileInstance
        {
            get { return _snapShotFileInstance; }
            set { _snapShotFileInstance = value; }
        }

        private FileInstance _thumbFileInstance;
        /// <summary>
        /// Ĵָͼ·��
        /// </summary>
        public FileInstance ThumbFileInstance
        {
            get { return _thumbFileInstance; }
            set { _thumbFileInstance = value; }
        }

        private FileInstance _metaDataFileInstance;

        /// <summary>
        /// Ԫ�����ļ�·��
        /// </summary>
        public FileInstance MetaDataFileInstance
        {
            get { return _metaDataFileInstance; }
            set { _metaDataFileInstance = value; }
        }

        private FileInstance _indexFileInstance;
        /// <summary>
        /// �����ļ�·��
        /// </summary>
        public FileInstance IndexFileInstance
        {
            get { return _indexFileInstance; }
            set { _indexFileInstance = value; }
        }

        private FileInstance _ckMetadataInstance;
        /// <summary>
        /// �ο�Ԫ�����ļ�
        /// </summary>
        public FileInstance CKMetadataInstance
        {
            get { return _ckMetadataInstance; }
            set { _ckMetadataInstance = value; }
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
                
                DataPackage dataPackage = _dataObject as DataPackage;
                GwDataObject dataKeyObject = dataPackage.GetDataKeyObject(); 
                
                
                
                EnumObjectType enumObjectType = dataKeyObject.ObjectType;
                switch (enumObjectType)
                {
                    case EnumObjectType.DataPackage: //������Ϊ���ݰ�
                        GetFiles_DataPackage();
                        break;
                    case EnumObjectType.DataFolder: //������Ϊ�ļ���

                    case EnumObjectType.DataFile: //������Ϊ�ļ�
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
            //�������ݰ��������
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
        /// ��ȡ�������ݵĴ�С(��λ:B)
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