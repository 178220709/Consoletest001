using System;
using System.Collections.Generic;
using System.IO;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public.Catalog;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class DataInstanceHelperEx
    {
        private Dictionary<string, DataFilePathInfo> _dataFiles;
        
        private CatalogDataScaner _catalogDataScaner;

        private GwDataObject _dataType;
        
        private string _folderPath;

        private readonly IDBHelper _dbHelper;

        public DataInstanceHelperEx(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
            _catalogDataScaner = new CatalogDataScaner();
            _dataFiles = new Dictionary<string, DataFilePathInfo>();
            _catalogDataScaner.OneCatalogDataScaned += _catalogDataScaner_OneCatalogDataScaned;
        }
        /// <summary>
        /// 扫描所依据的数据类型
        /// </summary>
        public GwDataObject DataType
        {
            set { _dataType = value; }
        }
        /// <summary>
        /// 待扫描的文件夹
        /// </summary>
        public string FolderPath
        {
            set { _folderPath = value; }
        }

        public Dictionary<string, DataFilePathInfo> DataFiles
        {
            get { return _dataFiles; }
        }


        /// <summary>
        /// 执行扫描
        /// </summary>
        /// <returns></returns>
        public bool RunScan()
        {

            try
            {
                _catalogDataScaner.ScanAllCatalogData(_dataType, _folderPath, true);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void _catalogDataScaner_OneCatalogDataScaned(object sender, CatalogDataScaner.OneCatalogDataScanedEventArgs e)
        {
            if(!e.IsValid) return;
            CatalogData currentData = e.CurrentData;
            DataFilePathInfo dataFilePathInfo = new DataFilePathInfo();
            

            #region AllUpLoadFilesInstance
            List<FileInstance> allUpLoadFilesInstance = new List<FileInstance>();
            CatalogFile[] catalogFiles = currentData.GetTempCatalogFiles();
            foreach (CatalogFile catalogFile in catalogFiles)
            {
                if (catalogFile.IsMainFile)
                {
                    dataFilePathInfo.MainFileInstance = new FileInstance(catalogFile.FileLocation,catalogFile.PackagePath);
                }
                allUpLoadFilesInstance.Add(new FileInstance(catalogFile.FileLocation, catalogFile.PackagePath));
            }
            dataFilePathInfo.AllUpLoadFilesInstance = allUpLoadFilesInstance;
            dataFilePathInfo.DataName = currentData.DataName;
            dataFilePathInfo.DataObject = CatalogModelEngine.GetDataObjectByName(_dbHelper, currentData.DataType);
            dataFilePathInfo.DataSize = currentData.DataAmount;
            dataFilePathInfo.FolderInfo = new DirectoryInfo(currentData.MainPath);
            string packagePath="";
            DataInstanceHelper.GetFilesPath(dataFilePathInfo.DataObject, dataFilePathInfo.MainFileInstance,
                                            ref dataFilePathInfo, ref packagePath);
            //dataFilePathInfo.

            #endregion


        }
    }
}
