using System;
using System.Collections.Generic;
using System.IO;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.CatalogDataModel.Public.Catalog;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class DataInstanceHelperEx
    {

        #region private 字段
        
        private Dictionary<string, DataFilePathInfoEx> _dataFiles;
        private CatalogDataScaner _catalogDataScaner;
        private GwDataObject _dataType;
        private string _folderPath;
        private IDBHelper _dbHelper;

        private int _validCount = 0;
        private int _unvalidCount = 0;

        #endregion

        #region 构造函数
        
        public DataInstanceHelperEx(IDBHelper dbHelper, GwDataObject dataType, string srcFolder)
        {
            _dbHelper = dbHelper;
            _dataType = dataType;
            _folderPath = srcFolder;
            _catalogDataScaner = new CatalogDataScaner();
            _dataFiles = new Dictionary<string, DataFilePathInfoEx>();
            _catalogDataScaner.OneCatalogDataScaned += _catalogDataScaner_OneCatalogDataScaned;
        }

        #endregion
 
        #region public 属性

        /// <summary>
        /// 扫描得到的实体
        /// </summary>
        public Dictionary<string, DataFilePathInfoEx> DataEntities
        {
            get
            {
                return _dataFiles;
            }
        }

        /// <summary>
        /// 扫描到的有效数据的数量
        /// </summary>
        public int ValidCount
        {
            get
            {
                return _validCount;
            }
        }

        /// <summary>
        /// 扫描到的无效数据的数量
        /// </summary>
        public int UnvalidCount
        {
            get
            {
                return _unvalidCount;
            }
        }

        #endregion

        #region public 方法
 
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

        #endregion

        #region 事件响应
        
        private void _catalogDataScaner_OneCatalogDataScaned(object sender, CatalogDataScaner.OneCatalogDataScanedEventArgs e)
        {
            if (!e.IsValid)
            {
                // 无效数据增1
                _unvalidCount++;
                return;
            }
                
            // TODO:解析CatalogData数据
            CatalogData currentData = e.CurrentData;
            DataFilePathInfoEx dataFilePathInfo = new DataFilePathInfoEx(_dbHelper);
            dataFilePathInfo.DataEntity = currentData;
            dataFilePathInfo.FolderInfo = new DirectoryInfo(currentData.MainPath);
            
            // 添加到集合
            if (!_dataFiles.ContainsKey(dataFilePathInfo.DataName))
            {
                _dataFiles.Add(dataFilePathInfo.DataName, dataFilePathInfo);
            }

            // 有效数据记录增1
            _validCount++;
        }

        #endregion

        #region private 方法
        
        ///获取完整数据包所在文件夹信息
        /// </summary>
        ///<param name="mainFilePath"></param>
        /// <param name="packagePath"> </param>
        /// <returns></returns>
        private DirectoryInfo GetParentFolderInfo(string mainFilePath, string packagePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(mainFilePath);
                string folderFullPathName = fileInfo.Directory.FullName;

                if (packagePath.Length == 0)
                {
                    return fileInfo.Directory;
                }

                int index = folderFullPathName.LastIndexOf(packagePath);
                if (index == -1)
                {
                    return null;
                }

                string path = folderFullPathName.Substring(0, index - 1);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                return directoryInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        #endregion

        #region public static 方法

        /// <summary>
        /// 获取上传文件时服务器存储路径
        /// </summary>
        /// <param name="svrPrePath"></param>
        /// <param name="taskPrepath"></param>
        /// <param name="pkgPath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string GetServerFilePath(string svrPrePath, string taskPrepath, string pkgPath, string localPath, string rootFolderPath)
        {
            string path;
            int index = pkgPath.LastIndexOf('\\');
            if (index > 0)
            {
                string tmp = pkgPath.Substring(0, index);
                path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" + tmp.Replace('\\', '/') + "/" +
                          Path.GetFileName(localPath).Replace('\\', '/');
            }
            else
            {
                if (string.IsNullOrEmpty(rootFolderPath))
                {
                    path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" +
                        Path.GetFileName(localPath).Replace('\\', '/');
                }
                else
                {
                    path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" +
                          localPath.Replace(rootFolderPath,"").Replace('\\', '/').Trim('/');
                }
            }
            path = path.Replace("//", "/").Replace(":", "_Partion");
            return path;
        }

        #endregion
    }
}
