using System;
using System.Collections.Generic;
using System.IO;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.ADF.MIS.CatalogDataModel.Public.DataModel;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.CatalogDataModel.Public.Catalog;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    /// <summary>
    /// 2012.11.6 修改 GetServerFilePath函数里面的replace方法，把参数顺序弄反了？
    /// 原代码皆注释保留
    /// </summary>
    public class DataInstanceHelper
    {
        private static List<string> GetAllSubFileNames(string dir)
        {
            List<string> files = new List<string>();
            string[] tmpfiles = Directory.GetFiles(dir);
            files.AddRange(tmpfiles);
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string d in dirs)
            {
                files.AddRange(GetAllSubFileNames(d));
            }
            return files;
        }

        /// <summary>
        /// 判断文件是否可以添加到数据中
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="fileName"></param>
        /// <param name="packageDir"></param>
        /// <param name="mainDataPath"></param>
        /// <returns></returns>
        public static bool CanAdd(DataFile dataFile, string fileName, string packageDir, string mainDataPath)
        {
            bool canAdd = false;
            string name = Path.GetFileNameWithoutExtension(fileName).ToUpper();

            string custom = GetCustomFileName(dataFile.CustomNameRuler, packageDir,
                                              Path.GetFileNameWithoutExtension(mainDataPath));

            if (!string.IsNullOrEmpty(custom))
            {
                custom = custom.ToUpper();
            }
            switch (dataFile.FileNameRuler)
            {
                case EnumDataFileNameRuler.SameWithMainFile:

                    #region 与主文件同名

                    {
                        string mainDataName = string.IsNullOrEmpty(mainDataPath)
                                                  ? string.Empty
                                                  : Path.GetFileNameWithoutExtension(mainDataPath);
                        if (!string.IsNullOrEmpty(mainDataName))
                        {
                            canAdd = name.CompareTo(mainDataName.ToUpper()) == 0;
                        }
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.ContainsMainFile:

                    #region 包含主文件名

                    {
                        string mainDataName = string.IsNullOrEmpty(mainDataPath)
                                                  ? string.Empty
                                                  : Path.GetFileNameWithoutExtension(mainDataPath);
                        if (!string.IsNullOrEmpty(mainDataName))
                        {
                            canAdd = name.Contains(mainDataName.ToUpper());
                        }
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.PrefixMainFile:

                    #region 主文件名加前缀

                    {
                        string mainDataName = string.IsNullOrEmpty(mainDataPath)
                                                  ? string.Empty
                                                  : Path.GetFileNameWithoutExtension(mainDataPath);
                        if (!string.IsNullOrEmpty(mainDataName))
                        {
                            canAdd = name.CompareTo((dataFile.FileNameRulerCustomString + mainDataName).ToUpper()) == 0;
                        }
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.SuffixMainFile:

                    #region 主文件名加后缀

                    {
                        string mainDataName = string.IsNullOrEmpty(mainDataPath)
                                                  ? string.Empty
                                                  : Path.GetFileNameWithoutExtension(mainDataPath);
                        if (!string.IsNullOrEmpty(mainDataName))
                        {
                            // canAdd = name.CompareTo((mainDataName + dataFile.FileNameRulerCustomString).ToUpper()) == 0;
                            canAdd = name.CompareTo((mainDataName + custom).ToUpper()) == 0;
                        }
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.SameWithCustom:

                    #region 与用户定义相同

                    {
                        canAdd = name.CompareTo(custom) == 0;
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.ContainsCustom:

                    #region 包含用户定义

                    {
                        canAdd = name.Contains(custom);
                    }

                    #endregion

                    break;
                case EnumDataFileNameRuler.CustomScript:

                    #region 自定义表达式

                    {
                        canAdd = CanAddCurFileByScript(dataFile.CustomNameRuler.Script, name, packageDir,
                                                       Path.GetFileNameWithoutExtension(mainDataPath));
                    }

                    #endregion

                    break;
                default:

                    #region 无限制

                    {
                        canAdd = true;
                    }

                    #endregion

                    break;
            }
            return canAdd;
        }

        /// <summary>
        /// 判断指定文件是否满足要求
        /// </summary>
        /// <param name="script"></param>
        /// <param name="curFileName"></param>
        /// <param name="packageDir"></param>
        /// <param name="mainFileName"></param>
        /// <returns></returns>
        private static bool CanAddCurFileByScript(ScriptMethod script, string curFileName, string packageDir,
                                                  string mainFileName)
        {
            script.SetParameterValue(CustomNameRuler.CONST_PARAM_DATAFOLDER, packageDir);
            script.SetParameterValue(CustomNameRuler.CONST_PARAM_MAINDATAFILENAME, mainFileName);
            script.SetParameterValue(CustomNameRuler.CONST_PARAM_CURRENTFILENAME, curFileName);
            bool result = false;
            try
            {
                result = Convert.ToBoolean(ScriptEngineUtil.ExecuteScript(script));
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
            }
            return result;
        }

        /// <summary>
        /// 获取自定文件名
        /// </summary>
        /// <param name="ruler"></param>
        /// <param name="packageDir"></param>
        /// <returns></returns>
        private static string GetCustomFileName(CustomNameRuler ruler, string packageDir, string mainFileName)
        {
            if (ruler == null)
            {
                return null;
            }
            switch (ruler.Type)
            {
                case EnumCustomNameRulerType.String:
                    return ruler.CustomString;
                case EnumCustomNameRulerType.Script:
                    {
                        ruler.Script.SetParameterValue(CustomNameRuler.CONST_PARAM_DATAFOLDER, packageDir);
                        ruler.Script.SetParameterValue(CustomNameRuler.CONST_PARAM_MAINDATAFILENAME, mainFileName);
                        return ScriptEngineUtil.ExecuteScript(ruler.Script).ToString();
                    }
                case EnumCustomNameRulerType.PlugIn:
                    throw new Exception("No Implement");
                default:
                    throw new Exception("Undefined");
            }
        }

   
        /// <summary>
        /// 获取上传文件时服务器存储路径
        /// </summary>
        /// <param name="svrPrePath"></param>
        /// <param name="taskPrepath"></param>
        /// <param name="pkgPath"></param>
        /// <param name="localPath"></param>
        /// <returns></returns>
        public static string GetServerFilePath(string svrPrePath, string taskPrepath, string pkgPath, string localPath,string rootFolderPath)
        {
            string path;
            int index = pkgPath.LastIndexOf('\\');
            if (index > 0)
            {
                string tmp = pkgPath.Substring(0, index);
                //path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" + localPath.Replace(tmp, "").Replace('\\', '/').TrimStart('/');
        //  原   path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" + tmp.Replace('\\', '/') + "/" +
           //     原           Path.GetFileName(localPath).Replace('\\', '/');
                 path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" + tmp.Replace('/', '\\') + "/" +
                         Path.GetFileName(localPath).Replace('/', '\\');
            }
            else
            {
                //path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" + localPath.Replace('\\', '/').TrimStart('/').Replace('\\', '/');
                if(string.IsNullOrEmpty(rootFolderPath))
                {
            //    原    path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" +
               //    原    Path.GetFileName(localPath).Replace('\\', '/');
                    path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" +
                  Path.GetFileName(localPath).Replace('/', '\\');
                }
                else
                {
               path = svrPrePath + "/" + taskPrepath.TrimEnd('/') + "/" +
               //      原  localPath.Replace(rootFolderPath,"").Replace('\\', '/').Trim('/');
               localPath.Replace(rootFolderPath, "").Replace('/', '\\').Trim('/');
                }
            }
         //原   path = path.Replace("//", "/").Replace(":", "_Partion");
            path = path.Replace("/", "\\").Replace(":", "_Partion");
            return path;
        }
        //同理 做相同修改
        internal static string GetServerFilePath(string svrPrePath, string localPath)
        {
            string path = "";
            localPath = localPath.Substring(localPath.IndexOf(':') + 1);
            path = svrPrePath + "/" + localPath.Replace("/", "\\").TrimStart('/');
            path = path.Replace("/", "\\").Replace(":", "_Partion");
            return path;
        }

        /// <summary>
        /// 获取下载文件时本地存储路径

        /// </summary>
        /// <returns></returns>
        public static string GetLocalFilePath(string localDirectory, string svrPath, string svrPrePath)
        {
            string localPath = svrPath.Replace('/', '\\');
            if (!string.IsNullOrEmpty(svrPrePath.Trim(new char[] { '/', '\\' })))
            {
                localPath = localPath.Replace(svrPrePath.Replace('/', '\\').Trim('\\'), "");
            }
            localPath = localPath.Trim('\\');
            localPath = localDirectory.TrimEnd('\\') + "\\" + localPath;
            return localPath;
        }

        ///// <summary>
        ///// 获取具备某属性的数据文件的具体路径
        ///// </summary>
        ///// <param name="dataPackage"></param>
        ///// <param name="pacPath"></param>
        ///// <param name="enumDataFileProperty"></param>
        ///// <param name="mainDataName"></param>
        ///// <returns></returns>
        //public static void GetDataFileByProperty(GwDataObject dataPackage, string pacPath, EnumDataFileProperty enumDataFileProperty, string mainDataName,ref string filePath)
        //{
        //    if (String.IsNullOrEmpty(pacPath))
        //    {
        //        throw new Exception("请先设置数据包路径");
        //    }

        //    DirectoryInfo de = new DirectoryInfo(pacPath);

        //    List<GwDataObject> dataObjects = dataPackage.ChildObjects;

        //    foreach (GwDataObject dataObject in dataObjects)
        //    {
        //        if (dataObject is DataFile)
        //        {
        //            DataFile dataFile = dataObject as DataFile;
        //            if (dataFile != null)
        //            {
        //                if (dataFile.FileProperty == enumDataFileProperty)
        //                {
        //                    if (filePath == string.Empty)
        //                    {
        //                        filePath = GetDataFileFullName(de, dataFile, mainDataName);
        //                    }
        //                }
        //            }
        //        }
        //        else if (dataObject is DataFolder)
        //        {
        //            DataFolder dataFolder = dataObject as DataFolder;
        //            if (dataFolder != null)
        //            {
        //                DirectoryInfo[] directorys = de.GetDirectories();

        //                foreach (DirectoryInfo di in directorys)
        //                {
        //                    if (di.Name == dataFolder.Name)
        //                    {
        //                        GetDataFileByProperty(dataFolder, di.FullName, enumDataFileProperty, mainDataName, ref filePath);
        //                    }
        //                }
        //            }
        //        }
        //        else if (dataObject is DataPackage)
        //        {
        //            DataPackage package = dataObject as DataPackage;
        //            if (package != null)
        //            {
        //                DirectoryInfo[] directorys = de.GetDirectories();

        //                foreach (DirectoryInfo di in directorys)
        //                {
        //                    GetDataFileByProperty(package, di.FullName, enumDataFileProperty, mainDataName, ref filePath);
        //                }

        //            }
        //        }
        //    }
        //}

        //private static string GetDataFileFullName(DirectoryInfo dirInfo, DataFile dataFile, string mainDataName)
        //{
        //    FileInfo[] files = dirInfo.GetFiles();
        //    foreach (FileInfo file in files)
        //    {
        //        if (file.Extension.ToUpper() == dataFile.FileExtension.ToUpper())
        //        {
        //            if (dataFile.IsSameWithMain)
        //            {
        //                if (!string.IsNullOrEmpty(mainDataName))
        //                {
        //                    if (file.Name.Substring(0, file.Name.LastIndexOf('.')).ToUpper() == mainDataName.ToUpper())
        //                    {
        //                        return file.FullName;
        //                    }
        //                }
        //                else
        //                {
        //                    return file.FullName;
        //                }
        //            }
        //            else
        //            {
        //                return file.FullName;
        //            }
        //        }
        //    }

        //    //DirectoryInfo[] dirs = dirInfo.GetDirectories();
        //    //foreach (DirectoryInfo info in dirs)
        //    //{
        //    //    string tmp = GetDataFileFullName(info, dataFile, mainDataName);
        //    //    if (!string.IsNullOrEmpty(tmp))
        //    //    {
        //    //        return tmp;
        //    //    }
        //    //}
        //    return "";
        //}

        //public static void GetDataFileByProperty(GwDataObject dataPackage, string pacPath, EnumDataFileProperty enumDataFileProperty,ref string filePath)
        //{
        //     GetDataFileByProperty(dataPackage, pacPath, enumDataFileProperty, "", ref filePath);

        //}

        ///// <summary>
        ///// 获取具备某属性的数据文件的具体路径列表
        ///// </summary>
        ///// <param name="dataPackage"></param>
        ///// <param name="pacPath"></param>
        ///// <param name="enumDataFileProperty"></param>
        ///// <param name="mainDataName"></param>
        ///// <returns></returns>
        //public static void GetDataFileByProperty(GwDataObject dataPackage, string pacPath, EnumDataFileProperty enumDataFileProperty, string mainDataPath, ref IList<FileInstance> datafiles)
        //{
        //    if (pacPath == string.Empty)
        //    {
        //        throw new Exception("请先设置数据包路径");
        //    }

        //    DirectoryInfo de = new DirectoryInfo(pacPath);
        //    string tmp = "";

        //    List<GwDataObject> dataObjects = dataPackage.ChildObjects;

        //    foreach (GwDataObject dataObject in dataObjects)
        //    {
        //        if (dataObject is DataFile)
        //        {
        //            DataFile dataFile = dataObject as DataFile;
        //            if (dataFile != null)
        //            {
        //                if (dataFile.FileProperty == enumDataFileProperty)
        //                {
        //                    if (dataFile.IsMainDataFile)
        //                    {
        //                        if (!ContainFileName(mainDataPath, datafiles))
        //                        {
        //                            datafiles.Add(new FileInstance(mainDataPath, dataFile.GetXPath()));
        //                        }
        //                    }
        //                    else
        //                    {
        //                        FileInfo[] files = de.GetFiles();
        //                        foreach (FileInfo file in files)
        //                        {
        //                            if (dataFile.FileExtension == ".*" || file.Extension.ToUpper() == dataFile.FileExtension.ToUpper())
        //                            {
        //                                if (dataFile.IsSameWithMain)
        //                                {
        //                                    string mainDataName = string.IsNullOrEmpty(mainDataPath) ? string.Empty : Path.GetFileNameWithoutExtension(mainDataPath);
        //                                    if (string.IsNullOrEmpty(mainDataName))
        //                                    {
        //                                        if (!ContainFileName(file.FullName, datafiles))
        //                                        {
        //                                            datafiles.Add(new FileInstance(file.FullName, dataFile.GetXPath()));
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        if (System.IO.Path.GetFileNameWithoutExtension(file.Name).ToUpper() == mainDataName.ToUpper()) //.LastIndexOf('.'))
        //                                        {
        //                                            if (!ContainFileName(file.FullName, datafiles))
        //                                            {
        //                                                datafiles.Add(new FileInstance(file.FullName, dataFile.GetXPath()));
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (!ContainFileName(file.FullName, datafiles))
        //                                    {
        //                                        datafiles.Add(new FileInstance(file.FullName, dataFile.GetXPath()));
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else if (dataObject is DataFolder)
        //        {
        //            DataFolder dataFolder = dataObject as DataFolder;
        //            if (dataFolder != null)
        //            {
        //                DirectoryInfo[] directorys = de.GetDirectories();

        //                foreach (DirectoryInfo di in directorys)
        //                {
        //                    if (di.Name == dataFolder.Name)
        //                    {
        //                        GetDataFileByProperty(dataFolder, di.FullName, enumDataFileProperty, mainDataPath, ref datafiles);
        //                    }
        //                }
        //            }
        //        }
        //        else if (dataObject is DataPackage)
        //        {
        //            DataPackage package = dataObject as DataPackage;
        //            if (package != null)
        //            {
        //                DirectoryInfo[] directorys = de.GetDirectories();

        //                foreach (DirectoryInfo di in directorys)
        //                {
        //                    GetDataFileByProperty(package, di.FullName, enumDataFileProperty, mainDataPath, ref datafiles);
        //                }
        //            }

        //        }
        //    }
        //}

        //private static bool ContainFileName(string fileName, IList<FileInstance> fileList)
        //{
        //    foreach (FileInstance file in fileList)
        //    {
        //        if (file.FullFileName.CompareTo(fileName) == 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public static void GetDataFileByProperty(GwDataObject dataPackage, string pacPath, EnumDataFileProperty enumDataFileProperty, ref IList<FileInstance> datafiles)
        //{
        //    GetDataFileByProperty(dataPackage, pacPath, enumDataFileProperty, "", ref datafiles);
        //}


        /// <summary>
        /// 获取主数据文件列表
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <param name="belongPacPath"> </param>
        /// <param name="dataPaths"></param>
        /// <param name="parentPath"> </param>
        /// <returns></returns>
        public static bool GetMainDataFiles(ref GwDataObject dataPackage, string parentPath, string belongPacPath,
                                            ref IList<FileInstance> dataPaths)
        {
            if (parentPath == string.Empty)
            {
                throw new Exception("请先设置数据包路径");
            }
            try
            {
                DirectoryInfo de = new DirectoryInfo(parentPath);
                GwDataObject[] dataObjects = dataPackage.ChildObjects;
                GwDataObject tempObject = null;
                foreach (GwDataObject dataObject in dataObjects)
                {
                    if (dataObject is DataFile)
                    {
                        DataFile dataFile = dataObject as DataFile;
                        if (dataFile != null)
                        {
                            if (dataFile.IsDataKey)
                            {
                                List<string> files = null;
                                if (dataFile.ParentObject.ObjectType ==
                                    EnumObjectType.DataFolder &&
                                    (dataFile.ParentObject as DataFolder).ContainsSubFoldersAndFiles)
                                {
                                    //包含所有子对象
                                    files = GetAllSubFileNames(de.FullName);
                                }
                                else
                                {
                                    files = new List<string>();
                                    files.AddRange(Directory.GetFiles(de.FullName));
                                }
                                foreach (string file in files)
                                {
                                    if (Path.GetExtension(file).ToUpper() == dataFile.FileExtension.ToUpper())
                                    {
                                        if (CanAdd(dataFile, Path.GetFileName(file), "", ""))
                                        {
                                            dataPaths.Add(new FileInstance(file, belongPacPath)); //file.FullName
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (dataObject is DataFolder)
                    {
                        DataFolder dataFolder = dataObject as DataFolder;
                        string customName = string.Empty;
                        if (dataFolder.CustomNameRuler != null)
                        {
                            customName = dataFolder.CustomNameRuler.CustomString.ToUpper();
                        }
                        else
                        {
                            customName = dataFolder.Name.ToUpper();
                        }
                        if (dataFolder != null)
                        {
                            DirectoryInfo[] directorys = de.GetDirectories();
                            foreach (DirectoryInfo di in directorys)
                            {
                                if (di.Name.ToUpper().CompareTo(customName) == 0)
                                {
                                    tempObject = dataFolder;
                                    GetMainDataFiles(ref tempObject, di.FullName, belongPacPath, ref dataPaths);
                                    break;
                                }
                            }
                        }
                    }
                    else if (dataObject is DataPackage)
                    {
                        DataPackage package = dataObject as DataPackage;
                        if (package != null)
                        {
                            dataPackage = package;

                            DirectoryInfo[] directorys = de.GetDirectories();
                            foreach (DirectoryInfo di in directorys)
                            {
                                GetMainDataFiles(ref dataPackage, di.FullName, di.FullName, ref dataPaths);
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取指定目录下的所有主数据文件路径
        /// 依据主数据后句后缀名查找
        /// </summary>
        /// <param name="dataPackage">数据目录</param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public static void GetMainDataFiles(GwDataObject dataPackage, string parentPath, ref FileInfo[] mainFileInfoes)
        {
            try
            {
                DataFile dataFile = GetMainFileDataFile(dataPackage);
                DirectoryInfo directoryInfo = new DirectoryInfo(parentPath);
                string searchPattern = "*.*";
                switch (dataFile.FileNameRuler)
                {
                    case EnumDataFileNameRuler.SameWithCustom: //与用户定义相同
                        searchPattern = string.Format("{0}{1}", dataFile.CustomNameRuler.CustomString,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.ContainsCustom: //包含用户定义
                        searchPattern = string.Format("*{0}*{1}", dataFile.CustomNameRuler.CustomString,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.CustomScript: //自定义表达式
                        //canAdd = CanAddCurFileByScript(dataFile.CustomNameRuler.Script, name, packageDir, Path.GetFileNameWithoutExtension(mainDataPath));
                        break;
                    default: //无限制
                        searchPattern = string.Format("*{0}", dataFile.FileExtension);
                        break;
                }
                mainFileInfoes = directoryInfo.GetFiles(searchPattern,SearchOption.AllDirectories);
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                mainFileInfoes = null;
            }
        }

        /// <summary>
        /// 获取指定数据类型下所有文件的路径
        /// 调用此方法前先获取主数据，调用方法GetMainInstance
        /// 如果数据类型中已定义但依据主数据无法查找到则返回FALSE,并将参数dataFilePath置空
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <param name="mainFilePath"></param>
        /// <param name="dataFilePath"></param>
        /// <param name="packagePath"> </param>
        public static bool GetFilesPath(GwDataObject dataPackage, FileInstance mainFilePath, ref DataFilePathInfo dataFilePath, ref string packagePath)
        {
            //判断主数据是否存在
            if (mainFilePath.FullFileName.Length == 0 || !File.Exists(mainFilePath.FullFileName))
            {
                return false;
            }
            //dataFilePath.DataObject = dataPackage;
            //主文件名
            dataFilePath.MainFileName = Path.GetFileNameWithoutExtension(mainFilePath.FullFileName);
            GwDataObject[] dataObjects = dataPackage.ChildObjects;
            foreach (GwDataObject dataObject in dataObjects)
            {
                
                if (dataObject is DataFile)
                {
                    //获取数据对象在数据类型中的路径
                    packagePath += string.Format(@"\{0}", dataObject.Name);
                    packagePath = packagePath.TrimStart('\\');
                    
                    DataFile dataFile = dataObject as DataFile;
                    if (dataFile.IsDataKey && dataFile.Properties.Count == 0)//主数据
                    {
                        packagePath = packagePath.Replace(dataObject.Name, "").TrimEnd('\\');
                        continue;
                    }
                    if (dataFile.Properties.Count > 1)//索引数据、快视图、拇指图、元数据、参考元数据
                    {
                        string[] tempPathes =
                                   GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                        if (tempPathes != null && tempPathes.Length > 0)
                        {
                            FileInstance fileInstance = new FileInstance(tempPathes.Length > 0 ? tempPathes[0] : "", packagePath);
                            if (dataFile.Properties.Contains(FileAttribute.INDEXFILE))
                            {
                                dataFilePath.IndexFileInstance = fileInstance;
                            }
                            if (dataFile.Properties.Contains(FileAttribute.METADATA))
                            {
                                dataFilePath.MetaDataFileInstance = fileInstance;
                            }
                            if (dataFile.Properties.Contains(FileAttribute.MZTFILE))
                            {
                                dataFilePath.ThumbFileInstance = fileInstance;
                            }
                            if (dataFile.Properties.Contains(FileAttribute.SNAPSHOT))
                            {
                                dataFilePath.SnapShotFileInstance = fileInstance;
                            }
                            if (dataFile.Properties.Contains(FileAttribute.CKMETAFILE))
                            {
                                dataFilePath.CKMetadataInstance = fileInstance;
                            }
                        }
                    }
                    else if (dataFile.Properties.Count == 1)
                    {
                        switch (dataFile.Properties[0])
                        {
                            case FileAttribute.INDEXFILE:
                                string[] indexPathes =
                                    GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                                if (indexPathes == null || indexPathes.Length==0)
                                {
                                    return false;
                                }
                                dataFilePath.IndexFileInstance = new FileInstance(indexPathes.Length > 0 ? indexPathes[0] : "", packagePath);
                                break;
                            case FileAttribute.METADATA:
                                string[] metaDataPathes =
                                    GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                                if (metaDataPathes == null || metaDataPathes.Length==0)
                                {
                                    return false;
                                }
                                dataFilePath.MetaDataFileInstance = new FileInstance(metaDataPathes.Length > 0 ? metaDataPathes[0] : "", packagePath);
                                break;
                            case FileAttribute.MZTFILE:
                                string[] mztPathes =
                                    GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                                if (mztPathes == null || mztPathes.Length==0)
                                {
                                    return false;
                                }
                                dataFilePath.ThumbFileInstance = new FileInstance(mztPathes.Length > 0 ? mztPathes[0] : "", packagePath);
                                break;
                            case FileAttribute.SNAPSHOT:
                                string[] snapshotPathes =
                                    GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                                if (snapshotPathes == null || snapshotPathes.Length==0)
                                {
                                    return false;
                                }
                                dataFilePath.SnapShotFileInstance = new FileInstance(snapshotPathes.Length > 0 ? snapshotPathes[0] : "",packagePath);
                                break;
                            case FileAttribute.CKMETAFILE:
                                string[] ckMetadataFile =
                                    GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                                if (ckMetadataFile == null || ckMetadataFile.Length==0)
                                {
                                    return false;
                                }
                                dataFilePath.CKMetadataInstance = new FileInstance(ckMetadataFile.Length > 0 ? ckMetadataFile[0] : "", packagePath);
                                break;
                            case FileAttribute.FILE_MAIN:

                                break;
                        }
                    }
                    else if (dataFile.Properties.Count == 0)//其它非带属性数据
                    {
                        string[] files = GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                        if (files != null)
                        {
                            if (files.Length == 0)//如果数据类型中已定义但依据主数据无法查找到
                            {
                                return false;
                            }
                            FileInstance replaceInstance;
                            foreach (string file in files)
                            {
                                FileInstance instance = new FileInstance(file, packagePath);
                                if (CanAddToList(dataFilePath.OtherFilesInstance,instance))
                                {
                                    dataFilePath.OtherFilesInstance.Add(instance);
                                }
                                else if (CanReplace(dataFilePath.OtherFilesInstance, instance, out replaceInstance))
                                {
                                    replaceInstance.PackagePath = instance.PackagePath;
                                }
                            }
                        }
                        else//如果数据类型中已定义但依据主数据无法查找到
                        {
                            return false;
                        }
                    }
                    packagePath = packagePath.Replace(dataObject.Name, "").TrimEnd('\\');
                    
                }// end :if (dataObject is DataFile)
                else if (dataObject is DataFolder)
                {
                    DataFolder dataFolder = dataObject as DataFolder;
                    switch (dataFolder.FolderNameRulerType)
                    {
                            case EnumDataFolderNameRuler.FixedName:
                            packagePath += string.Format(@"\{0}", dataFolder.CustomNameRuler.CustomString);
                            
                            break;
                            case EnumDataFolderNameRuler.Others:
                            break;//暂不支持
                    }

                    //获取数据对象在数据类型中的路径
                    
                    packagePath = packagePath.TrimStart('\\');
                    
                    
                    if(!GetFilesPath(dataObject, mainFilePath, ref dataFilePath, ref packagePath))
                    {
                        return false;
                    }
                    packagePath = packagePath.Replace(dataFolder.CustomNameRuler.CustomString, "").TrimEnd('\\');
                }
                
            }

            return true;
        }
      


        public static bool CanAddToList(List<FileInstance> list,FileInstance fileInstance)
        {
            foreach (FileInstance instance in list)
            {
                if(instance.FullFileName==fileInstance.FullFileName)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CanReplace(List<FileInstance> list, FileInstance fileInstance, out FileInstance replaceInstance)
        {
            replaceInstance = null;
            if (CanAddToList(list, fileInstance))
            {

                return false;
            }
            if (fileInstance.PackagePath == "其它文件")
            {
                return false;
            }

            foreach (FileInstance instance in list)
            {
                if (instance.FullFileName == fileInstance.FullFileName)
                {
                    replaceInstance = instance;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取主数据文件
        /// 并依据数据类型和主数据文件路径，判断主数据合法性
        /// 不合法则返回null
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <param name="mainFilePath"></param>
        private static FileInstance GetMainInstance(GwDataObject dataPackage, string mainFilePath, ref string packagePath)
        {
            GwDataObject[] dataObjects = dataPackage.ChildObjects;
            foreach (GwDataObject dataObject in dataObjects)
            {
                packagePath += string.Format(@"\{0}", dataObject.Name);
                packagePath = packagePath.TrimStart('\\');
                if (dataObject is DataFile)
                {
                    DataFile dataFile = dataObject as DataFile;
                    if (dataFile.IsDataKey)
                    {

                        string pre = packagePath.Contains("\\")
                                         ? StringHelper.TrimBehind(packagePath, "\\", true)
                                         : "";
                        if (!Path.GetDirectoryName(mainFilePath).EndsWith(pre))//不合法
                        {
                            return null;
                        }
                        packagePath = dataFile.GetXPath();
                        return new FileInstance(mainFilePath, packagePath); 
                    }
                }
                else if (dataObject is DataFolder)
                {
                    FileInstance main = GetMainInstance(dataObject, mainFilePath, ref packagePath);
                    if (main != null)
                    {
                        return main;
                    }
                }
                packagePath = packagePath.Replace(dataObject.Name, "").TrimEnd('\\');
            }
            return null;

        }

        /// <summary>
        /// 根据数据类型和主数据文件路径获取数据包的名称
        /// </summary>
        /// <param name="dpackage"></param>
        /// <param name="mainFilePath"></param>
        /// <param name="packagePath"> </param>
        /// <returns></returns>
        public static string GetDataPackageName(GwDataObject dpackage, string mainFilePath, string packagePath)
        {
            string dataName;
            string mainfileName = Path.GetFileNameWithoutExtension(mainFilePath);
            string folderName = "";
            if (packagePath.Length != 0)
            {
                int index = Path.GetDirectoryName(mainFilePath).LastIndexOf(packagePath);
                folderName = Path.GetDirectoryName(mainFilePath).Substring(0, index - 1);
                folderName = folderName.Substring(folderName.LastIndexOf("\\") + 1);
            }
            else
            {
                FileInfo fileInfo = new FileInfo(mainFilePath);
                folderName = fileInfo.Directory.Name;
            }
            DataPackage package = dpackage as DataPackage;
            switch (package.DataNameRuler)
            {
                case EnumDataNameRuler.MainDataFile:
                    dataName = mainfileName;
                    break;
                    case EnumDataNameRuler.DataPackageFolder:
                    dataName = folderName;
                    break;
                    case EnumDataNameRuler.DataPackageAndMainDataFile:
                    dataName = folderName +"_"+ mainfileName;
                    break;
                default :
                    dataName = mainfileName;
                    break;
            }
            return dataName;
        }

        /// <summary>
        ///获取完整数据包所在文件夹路径
        /// </summary>
        ///<param name="mainFilePath"></param>
        /// <param name="packagePath"> </param>
        /// <returns></returns>
        public static string GetParentFolderName(string mainFilePath, string packagePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(mainFilePath);
                string folderFullPathName = fileInfo.Directory.FullName;
                
                if(packagePath.Length==0)
                {
                    return fileInfo.Directory.Name;
                }
                
                int index = folderFullPathName.LastIndexOf(packagePath);
                if (index == -1)
                {
                    return "";
                }

                string path = folderFullPathName.Substring(0, index - 1);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                return directoryInfo.Name;

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        ///获取完整数据包所在文件夹信息
        /// </summary>
        ///<param name="mainFilePath"></param>
        /// <param name="packagePath"> </param>
        /// <returns></returns>
        public static DirectoryInfo GetParentFolderInfo(string mainFilePath, string packagePath)
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataPackage">数据类型</param>
        /// <param name="fileAttribute">要查找的特殊文件属性名，定义在FileAttribute类常量</param>
        /// <returns></returns>
        public static DataFile GetDataFile(GwDataObject dataPackage,string fileAttribute)
        {
            DataFile fileObj = null;
            foreach (GwDataObject dataObject in dataPackage.ChildObjects)
            {
                if (dataObject is DataFile)
                {
                    DataFile dataFile = dataObject as DataFile;
                    if (dataFile.IsDataKey)
                    {
                        continue;
                    }
                    if (dataFile.Properties.Count > 0)
                    {
                        if(dataFile.Properties[0]==fileAttribute)
                        {
                            fileObj= dataFile;
                        }
                    }
                }
                else if (dataObject is DataFolder)
                {
                   fileObj= GetDataFile(dataObject, fileAttribute);
                }
            }
            return fileObj;
        }

        /// <summary>
        /// 根据数据类型获取主数据文件DataFile
        /// </summary>
        /// <param name="dataPackage"></param>
        /// <returns></returns>
        public static DataFile GetMainFileDataFile(GwDataObject dataPackage)
        {
            DataFile fileObj = null;
            foreach (GwDataObject dataObject in dataPackage.ChildObjects)
            {
                if (dataObject is DataFile)
                {
                    DataFile dataFile = dataObject as DataFile;
                    if (dataFile.IsDataKey)
                    {
                        return dataFile;
                    }
                }
                else if (dataObject is DataFolder)
                {
                    fileObj = GetMainFileDataFile(dataObject);
                    if(fileObj!=null)
                    {
                        return fileObj;
                    }
                }
            }
            return fileObj;
        }


        /// <summary>
        /// 获取具体文件的文件名（用于非带属性文件）
        /// </summary>
        /// <param name="dataFile">DataFile对象</param>
        /// <param name="mainfileInstance"> </param>
        /// <param name="packagePath">DataFile在数据对象中的路径</param>
        /// <returns></returns>
        private static string[] GetFileFullName(DataFile dataFile, FileInstance mainfileInstance, string packagePath)
        {
            try
            {
                if (mainfileInstance.FullFileName.Length == 0 || !File.Exists(mainfileInstance.FullFileName))
                {
                    return null;
                }
                string mainName = Path.GetFileNameWithoutExtension(mainfileInstance.FullFileName);
                //DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(mainfilePath));

                string searchPattern = "*.*";
                switch (dataFile.FileNameRuler)
                {
                    case EnumDataFileNameRuler.SameWithMainFile: //与主文件同名
                        searchPattern = string.Format("{0}{1}", mainName, dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.ContainsMainFile: //包含主文件名
                        searchPattern = string.Format("*{0}*{1}", mainName, dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.PrefixMainFile: //主文件名加前缀
                        searchPattern = string.Format("{0}{1}{2}", dataFile.CustomNameRuler.CustomString, mainName,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.SuffixMainFile: //主文件名加后缀
                        searchPattern = string.Format("{0}{1}{2}", mainName, dataFile.CustomNameRuler.CustomString,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.SameWithCustom: //与用户定义相同
                        searchPattern = string.Format("{0}{1}", dataFile.CustomNameRuler.CustomString,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.ContainsCustom: //包含用户定义
                        searchPattern = string.Format("*{0}*{1}", dataFile.CustomNameRuler.CustomString,
                                                      dataFile.FileExtension);
                        break;
                    case EnumDataFileNameRuler.CustomScript: //自定义表达式
                        //canAdd = CanAddCurFileByScript(dataFile.CustomNameRuler.Script, name, packageDir, Path.GetFileNameWithoutExtension(mainDataPath));
                        break;
                    default: //无限制
                        searchPattern = string.Format("*{0}", dataFile.FileExtension);
                        break;
                }

                //int index = mainfileInstance.PackagePath.LastIndexOf("\\");
                string root = Path.GetDirectoryName(mainfileInstance.FullFileName).TrimEnd('\\');//获取主数据所在文件夹路径
                #region
                string prePath = StringHelper.TrimBehind(mainfileInstance.PackagePath, "\\", true);//获取主数据在数据类型中的路径
                if (root.EndsWith(prePath))
                {
                    root = StringHelper.TrimEnd(root, prePath);//获取数据包所在文件夹路径

                    // qfc-modify
                    //root = root + "\\" + StringHelper.TrimBehind(packagePath, "\\", true);
                    root = root + StringHelper.TrimBehind(packagePath, "\\", true);

                }
                else
                {
                    return null;
                }
                #endregion
                //if (index == -1)
                //{

                //}
                //else
                //{
                //    string packagePahtPre= mainfileInstance.PackagePath.Substring(0, index);
                //    if (root.LastIndexOf(packagePahtPre) >= 0)
                //    {
                //        root = root.Substring(0, root.LastIndexOf(packagePahtPre));
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}

                //int index2 = packagePath.LastIndexOf("\\");
                //string root2="";
                //if(index2>=0)
                //{
                //    root2 = packagePath.Substring(0, index2);
                //}
                //root = root + root2;
                string[] fileInfoes = Directory.GetFiles(root, searchPattern);
                return fileInfoes;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 根据数据类型获取指定路径下所有的数据信息
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="sourceDataPath">数据路径</param>
        /// <returns></returns>
        public static Dictionary<string, DataFilePathInfo> GetDicDataFilePathes(GwDataObject dataType, string sourceDataPath)
        {
            
            DataPackage dataPackage = dataType as DataPackage;
            GwDataObject dataKeyObject = dataPackage.GetDataKeyObject();
            if(dataKeyObject==null)
            {
                throw new Exception(string.Format("数据:{0},无法获取主数据对象。", dataPackage.Name));
            }
            EnumObjectType enumObjectType = dataKeyObject.ObjectType;
            switch (enumObjectType)
            {
                case EnumObjectType.DataPackage://主数据为数据包
                    return GetPathes_DataPackage(dataType,sourceDataPath);
                case EnumObjectType.DataFolder://主数据为文件夹
                    return null;
                case EnumObjectType.DataFile://主数据为文件

                    return GetPathes_DataFile(dataType, sourceDataPath);
                default:
                    return null;
            }
        }
        /// <summary>
        /// 根据数据类型和主数据路径对应的数据信息
        /// </summary>
        /// <param name="dataType">数据类型</param>
        /// <param name="mainFilePath">主数据路径</param>
        /// <returns></returns>
        public static DataFilePathInfo GetDicDataFilePath(GwDataObject dataType, string mainFilePath)
        {
            DataFilePathInfo dataFilePath = new DataFilePathInfo();
            dataFilePath.DataObject = dataType;
            string packagePath = "";
            FileInstance mainFileInstance = GetMainInstance(dataType, mainFilePath, ref packagePath);
            if (mainFileInstance == null) //不合法主数据
            {
                return null;
            }
            packagePath = "";
            dataFilePath.MainFileInstance = mainFileInstance;
            //根据数据类型和主文件获取非主文件路径
            if (!GetFilesPath(dataType, mainFileInstance, ref dataFilePath, ref packagePath))
            {
                return null;
            }

            string pre = StringHelper.TrimBehind(dataFilePath.MainFileInstance.PackagePath, "\\", true);

            dataFilePath.DataName = GetDataPackageName(dataType, mainFilePath, pre);

            dataFilePath.FolderInfo = GetParentFolderInfo(mainFilePath, pre);

            //dataFilePath.FolderName = GetParentFolderName(mainFilePath, pre);
            return dataFilePath;
        }
        private static Dictionary<string, DataFilePathInfo> GetPathes_DataFile(GwDataObject dataType, string sourceDataPath)
        {
            //解析得到的数据路径信息类，键为数据包名称
            Dictionary<string, DataFilePathInfo> dicDataFilePathes = new Dictionary<string, DataFilePathInfo>();
            //1、获取所有主数据文件路径
            FileInfo[] mainFileInfoes = null;
            GetMainDataFiles(dataType, sourceDataPath, ref mainFileInfoes);
            if (mainFileInfoes == null || mainFileInfoes.Length == 0)
            {
                return new Dictionary<string, DataFilePathInfo>();
            }
            //2、遍历所有主文件获取对应数据类型所有文件路径
            foreach (FileInfo mainFileInfo in mainFileInfoes)
            {
                DataFilePathInfo dataFilePath = new DataFilePathInfo();
                dataFilePath.DataObject = dataType;
                string packagePath = "";
                FileInstance mainFileInstance = GetMainInstance(dataType, mainFileInfo.FullName, ref packagePath);
                if (mainFileInstance == null) //不合法主数据
                {
                    continue;
                }
                packagePath = "";
                dataFilePath.MainFileInstance = mainFileInstance;
                //根据数据类型和主文件获取非主文件路径
                if (!GetFilesPath(dataType, mainFileInstance, ref dataFilePath, ref packagePath))
                {
                    continue;
                }

                string pre = StringHelper.TrimBehind(dataFilePath.MainFileInstance.PackagePath, "\\", true);

                dataFilePath.DataName = GetDataPackageName(dataType, mainFileInfo.FullName, pre);

                dataFilePath.FolderInfo = GetParentFolderInfo(mainFileInfo.FullName, pre);

                //dataFilePath.FolderName = GetParentFolderName(mainFileInfo.FullName, pre);

                if (!dicDataFilePathes.ContainsKey(dataFilePath.DataName))
                {
                    dicDataFilePathes.Add(dataFilePath.DataName, dataFilePath);
                }
            }
            return dicDataFilePathes;
        }


        //当主数据设置为数据包时获取数据路径信息
        private static Dictionary<string, DataFilePathInfo> GetPathes_DataPackage(GwDataObject dataType, string sourceDataPath)
        {
            //解析得到的数据路径信息类，键为数据包名称
            Dictionary<string, DataFilePathInfo> dicDataFilePathes = new Dictionary<string, DataFilePathInfo>();
            string[] directories = Directory.GetDirectories(sourceDataPath);
            foreach (string directory in directories)
            {
                DataFilePathInfo dataFilePathInfo = new DataFilePathInfo();
                dataFilePathInfo.DataObject = dataType;
                dataFilePathInfo.RootFolderPath = sourceDataPath;
                dataFilePathInfo.MainFileName = directory;
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                dataFilePathInfo.DataName = directoryInfo.Name;
                dicDataFilePathes.Add(dataFilePathInfo.DataName, dataFilePathInfo);
            }
            return dicDataFilePathes;
        }

        public static string GetSpecificFileFullName(GwDataObject dataPackage, string fileAttribute, string mainName, string searchFilePath)
        {
            DataFile dataFile = GetDataFile(dataPackage, fileAttribute);
            if (dataFile==null||dataFile.Properties.Count == 0 || searchFilePath.Length == 0 || !Directory.Exists(searchFilePath))
            {
                return "";
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(searchFilePath);

            string searchPattern = "*.*";
            switch (dataFile.FileNameRuler)
            {
                case EnumDataFileNameRuler.SameWithMainFile: //与主文件同名
                    searchPattern = string.Format("{0}{1}", mainName, dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.ContainsMainFile: //包含主文件名
                    searchPattern = string.Format("*{0}*{1}", mainName, dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.PrefixMainFile: //主文件名加前缀
                    searchPattern = string.Format("{0}{1}{2}", dataFile.CustomNameRuler.CustomString, mainName,
                                                  dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.SuffixMainFile: //主文件名加后缀
                    searchPattern = string.Format("{0}{1}{2}", mainName, dataFile.CustomNameRuler.CustomString,
                                                  dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.SameWithCustom: //与用户定义相同
                    searchPattern = string.Format("{0}{1}", dataFile.FileNameRulerCustomString,
                                                  dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.ContainsCustom: //包含用户定义
                    searchPattern = string.Format("*{0}*{1}", dataFile.FileNameRulerCustomString,
                                                  dataFile.FileExtension);
                    break;
                case EnumDataFileNameRuler.CustomScript: //自定义表达式
                    //canAdd = CanAddCurFileByScript(dataFile.CustomNameRuler.Script, name, packageDir, Path.GetFileNameWithoutExtension(mainDataPath));
                    break;
                default: //无限制
                    searchPattern = string.Format("*{0}", dataFile.FileExtension);
                    break;
            }
            FileInfo[] fileInfoes = directoryInfo.GetFiles(searchPattern, SearchOption.AllDirectories);
            return fileInfoes.Length > 0 ? fileInfoes[0].FullName : "";
        }
        
#region 将ADF的CatalogData类解析到资料库自己定义的DataFilePathInfo类，未完成，需要后期修改
        /*
        /// <summary>
        /// 将ADF的CatalogData类解析到资料库自己定义的DataFilePathInfo类
        /// </summary>
        /// <param name="catalogData"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static DataFilePathInfo TransADFToLocal(IDBHelper db,CatalogData catalogData, GwDataObject dataType)
        {
            try
            {
                DataFilePathInfo dataFilePathInfo = new DataFilePathInfo();
                dataFilePathInfo.DataObject = dataType;

                dataFilePathInfo.MainFileName = Path.GetFileNameWithoutExtension(catalogData.MainPath);
                dataFilePathInfo.DataName = catalogData.DataName;
                string packagePath = "";
                dataFilePathInfo.MainFileInstance = GetMainInstance(dataType, catalogData.MainPath, ref packagePath);
                
                string pre = StringHelper.TrimBehind(dataFilePathInfo.MainFileInstance.PackagePath, "\\", true);
                dataFilePathInfo.FolderInfo = GetParentFolderInfo(dataFilePathInfo.MainFileInstance.FullName, pre);
                packagePath = "";
                GwDataObject[] dataObjects = dataType.ChildObjects;
                foreach (GwDataObject dataObject in dataObjects)
                {
                    packagePath = "";

                }
                return dataFilePathInfo;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }
        public static void SetFilesPath(IDBHelper db,GwDataObject dataObject, CatalogData catalogData, ref DataFilePathInfo dataFilePath)
        {
            if (dataObject is DataFile)
            {
                DataFile dataFile = dataObject as DataFile;
                IList<CatalogFile> catalogFileList=null;
                if (dataFile.Properties.Count > 0)//索引数据、快视图、拇指图、元数据、参考元数据
                {
                    switch (dataFile.Properties[0])
                    {
                        case FileAttribute.INDEXFILE:
                            string[] indexPathes =
                                GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                            if (indexPathes == null || indexPathes.Length == 0)
                            {
                                return false;
                            }
                            catalogFileList = catalogData.GetCatalogFiles(db, dataFile.GetXPath());
                            if (indexPathes == null || indexPathes.Length == 0)
                            {
                                return false;
                            }
                            dataFilePath.IndexFileInstance = new FileInstance(indexPathes.Length > 0 ? indexPathes[0] : "", packagePath);
                            break;
                        case FileAttribute.METADATA:
                            string[] metaDataPathes =
                                GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                            if (metaDataPathes == null || metaDataPathes.Length == 0)
                            {
                                return false;
                            }
                            dataFilePath.MetaDataFileInstance = new FileInstance(metaDataPathes.Length > 0 ? metaDataPathes[0] : "", packagePath);
                            break;
                        case FileAttribute.MZTFILE:
                            string[] mztPathes =
                                GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                            if (mztPathes == null || mztPathes.Length == 0)
                            {
                                return false;
                            }
                            dataFilePath.ThumbFileInstance = new FileInstance(mztPathes.Length > 0 ? mztPathes[0] : "", packagePath);
                            break;
                        case FileAttribute.SNAPSHOT:
                            string[] snapshotPathes =
                                GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                            if (snapshotPathes == null || snapshotPathes.Length == 0)
                            {
                                return false;
                            }
                            dataFilePath.SnapShotFileInstance = new FileInstance(snapshotPathes.Length > 0 ? snapshotPathes[0] : "", packagePath);
                            break;
                        case FileAttribute.CKMETAFILE:
                            string[] ckMetadataFile =
                                GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                            if (ckMetadataFile == null || ckMetadataFile.Length == 0)
                            {
                                return false;
                            }
                            dataFilePath.CKMetadataInstance = new FileInstance(ckMetadataFile.Length > 0 ? ckMetadataFile[0] : "", packagePath);
                            break;
                        case FileAttribute.FILE_MAIN:

                            break;
                    }
                }
                else if (dataFile.Properties.Count == 0)//其它非带属性数据
                {
                    string[] files = GetFileFullName(dataFile, dataFilePath.MainFileInstance, packagePath);
                    if (files != null)
                    {
                        if (files.Length == 0)//如果数据类型中已定义但依据主数据无法查找到
                        {
                            return false;
                        }
                        FileInstance replaceInstance;
                        foreach (string file in files)
                        {
                            FileInstance instance = new FileInstance(file, packagePath);
                            if (CanAddToList(dataFilePath.OtherFilesInstance, instance))
                            {
                                dataFilePath.OtherFilesInstance.Add(instance);
                            }
                            else if (CanReplace(dataFilePath.OtherFilesInstance, instance, out replaceInstance))
                            {
                                replaceInstance.PackagePath = instance.PackagePath;
                            }
                        }
                    }
                    else//如果数据类型中已定义但依据主数据无法查找到
                    {
                        return false;
                    }
                }
                packagePath = packagePath.Replace(dataObject.Name, "").TrimEnd('\\');

            }// end :if (dataObject is DataFile)
            else if (dataObject is DataFolder)
            {
                DataFolder dataFolder = dataObject as DataFolder;
                switch (dataFolder.FolderNameRulerType)
                {
                    case EnumDataFolderNameRuler.FixedName:
                        packagePath += string.Format(@"\{0}", dataFolder.CustomNameRuler.CustomString);

                        break;
                    case EnumDataFolderNameRuler.Others:
                        break;//暂不支持
                }

                //获取数据对象在数据类型中的路径

                packagePath = packagePath.TrimStart('\\');


                if (!GetFilesPath(dataObject, mainFilePath, ref dataFilePath, ref packagePath))
                {
                    return false;
                }
                packagePath = packagePath.Replace(dataFolder.CustomNameRuler.CustomString, "").TrimEnd('\\');
            }
        }、
        */
#endregion
    }
}


