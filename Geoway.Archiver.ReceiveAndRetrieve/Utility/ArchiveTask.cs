namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Data;
    using System.Windows.Forms;
    using ADF.MIS.CatalogDataModel.Public.DataModel;
    using ADF.MIS.DB.Public;
    using ADF.MIS.DataModel;
    using ADF.MIS.DataModel.Public;
    using Archiver.Utility.Definition;
    using Archiver.Utility.UI;
    using Catalog;
    using ESRI.ArcGIS.Geometry;
    using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;

    using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
    using Geoway.ADF.MIS.Core.Public.security;
    using Geoway.ADF.MIS.Utility.Core;
    using Geoway.Archiver.ReceiveAndRetrieve.DAL;
    using Geoway.Archiver.ReceiveAndRetrieve.Definition;
    using Geoway.Archiver.ReceiveAndRetrieve.Model;
    using Geoway.Archiver.Utility.Class;
    using Geoway.Archiver.Utility.DAL;
    using Geoway.ADF.MIS.Utility.Log;

    /// <summary>
    ///目的：重构归档任务创建与验证，增加进度显示
    ///创建人：王金玉
    ///创建日期：2010-12-9
    ///修改描述：
    ///修改人：
    ///修改日期：
    ///备注：
    /// </summary>
    public class ArchiveTask
    {
        /// <summary>
        /// 影像范围最大值，即当影像范围超过该值时认为范围出错
        /// </summary>
        private const int CONST_IMAGE_EXTENT_MAX = 10;

        private const int CONST_SHOW_PROGRESS_MIN_CREATE = 5;
        private const int CONST_SHOW_PROGRESS_MIN_VALIDATE = 2;

        #region 事件定义
        
        public delegate void SetProgressEventHandler(int pos, int max, string message);
        /// <summary>
        /// 显示/隐藏任务进度条
        /// </summary>
        /// <param name="show">是否显示</param>
        public delegate void ShowProgressUIEventHandler(bool show);

        /// <summary>
        /// 显示操作进度
        /// </summary>
        public event SetProgressEventHandler OnSetCreateProgress;

        public event ShowProgressUIEventHandler OnShowCreateProgress;

        public event SetProgressEventHandler OnSetValidateProgress;

        public event ShowProgressUIEventHandler OnShowValidateProgress;

        #endregion

        #region 变量定义

        private string _metaTableName = string.Empty;

        //进度显示
        private int _max = 0;

        private bool _hadShow = false;          //是否已显示出任务进度条


        //private SpatialFieldConfigDAL _spatialFieldConfig = null;   //空间范围字段配置
        //private bool _hasSetSpatialFieldConfig = false;     //是否已经为_spatialFieldConfig赋过值


        //验证进度显示
        private int _maxV = 0;
        private int _positionV = 0;

        private bool _isShowValidateProgress = false;   //是否需要显示任务检查进度条

        private PairsIntoDBTable _pairsIntoDBTable = null;

        private SpatialFieldConfigDAL _spatialFieldConfig = null;   //空间范围字段配置


        private int _newDataCount = 0;
        /// <summary>
        /// 新增数据个数
        /// </summary>
        public int NewDataCount
        {
            get { return _newDataCount; }
        }

        private bool _isRunning = false;
        /// <summary>
        /// 获取或设置是否正在运行


        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        private bool _cancel = false;   //是否取消
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        #endregion

    

        /// <summary>
        /// 获取任务数据序列
        /// </summary>
        /// <param name="task">入库任务</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="isSupplementary"> </param>
        /// <param name="validateOldData"> </param>
        /// <param name="validateNewData"> </param>
        /// <param name="option"> </param>
        public void SetArchiveTaskDatas(UpLoadTask task, GwDataObject dataType, bool isSupplementary, bool validateOldData, bool validateNewData, ValidateOption option)
        {
            _isRunning = true;
            InvokeShowCreateProgress(true);
            try
            {
                InitProgressValue();

                if (!isSupplementary && task.Datas.Count > 0) //读数据库获取任务对应数据
                {
                    foreach (TaskData var in task.Datas)
                    {
                        var.Delete();
                    }
                }

                ReplaceOption replaceOption = new ReplaceOption(); // 任务数据替换选项

                Int64 totalSize = 0; //任务总数据量

                string[] dirs = null; //任务数据所在目录
                int firstLevelStep = 0;

                if (Directory.Exists(task.Directory))
                {


                    if (task.IsParentDir)
                    {
                        dirs = Directory.GetDirectories(task.Directory);
                        firstLevelStep = 1000;
                        _max = firstLevelStep*dirs.Length;
                    }
                    else
                    {
                        dirs = new string[] {task.Directory};
                        firstLevelStep = 10000;
                        _max = 10000;
                    }

                    Dictionary<string, string> dictContent = null;

                    if (dataType.ObjectType == EnumObjectType.DataPackage)
                    {
                        IList<UpLoadTaskData> taskDataList = new List<UpLoadTaskData>();
                        if (dirs.Length > 0)
                        {
                            totalSize = InitTaskData(task, dirs, dataType, 0, "", replaceOption, isSupplementary,
                                                     validateOldData, validateNewData, option, dictContent,
                                                     ref taskDataList, 0, _max);
                        }
                        taskDataList.Clear();

                    }
                }

                else
                {
                    ADF.MIS.Utility.DevExpressEx.DevMessageUtil.ShowMessageDialog("路径“"+task.Directory+"”不存在！");
                }
                if (totalSize >= 0)
                {
                    task.DataAmount = totalSize;
                    task.Update();
                }
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
            }
            finally
                {
                    _isRunning = false;
                    FinishCreate();
                }
            
        }

        /// <summary>
        /// 创建任务数据
        /// </summary>
        /// <param name="task"></param>
        /// <param name="dirs"></param>
        /// <param name="dataType"></param>
        /// <param name="packageID"></param>
        /// <param name="pkgFolderName"></param>
        /// <param name="replaceOption"></param>
        /// <param name="isSupplementary">是否启用追加模式</param>
        /// <param name="validateOldData"> </param>
        /// <param name="validateNewData"> </param>
        /// <param name="option"> </param>
        /// <param name="datas"> </param>
        /// <param name="progStart"> </param>
        /// <param name="progLength"> </param>
        /// <returns>数据量（B）</returns>
        private Int64 InitTaskData(UpLoadTask task, string[] dirs, GwDataObject dataType, int packageID, string pkgFolderName, ReplaceOption replaceOption, bool isSupplementary, bool validateOldData, bool validateNewData, ValidateOption option, Dictionary<string, string> dictContent, ref IList<UpLoadTaskData> datas, int progStart, int progLength)
        {
            bool taskRlt = true;

            string strTmp = string.Empty;
            Int64 totalSize = 0;
            DataTable dt = null;
            if (isSupplementary)
            {
                totalSize = task.DataAmount;
                foreach (UpLoadTaskData var in task.Datas)
                {
                    var.CreateTaskFileListFromDB();
                    if (validateOldData)
                    {
                        ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(InitPara.DBHelper, task.CatalogID);
                        if(catalogNode!=null)
                        {
                            taskRlt = ValidateTaskData(var, option, catalogNode.NodeExInfo.DatumTypeObj.MetaTemplateOjb.ID) && taskRlt;
                        }
                        else
                        {
                            taskRlt = false;
                        }
                    }
                }
            }

            int stepDir = progLength / dirs.Length;
            int indexDir = -1;
            foreach (string dir in dirs)
            {
                indexDir++;

                #region 任务被取消


                if (_cancel)
                {
                    break;
                }
                #endregion

                IList<FileInstance> dataPaths = new List<FileInstance>();
                GwDataObject tempObject = dataType;
                
                //获取主文件路径
                DataInstanceHelper.GetMainDataFiles(ref tempObject, dir, dir, ref dataPaths);

                ////扫描第一个子文件夹
                //if (dataPaths.Count == 0)
                //{
                //    string[] subDirs = Directory.GetDirectories(dir);
                //    if (subDirs.Length > 0)
                //    {
                //        DataInstanceHelper.GetMainDataFiles(ref tempObject, subDirs[0], subDirs[0], ref dataPaths);
                //        foreach (FileInstance var in dataPaths)
                //        {
                //            var.RootPackagePath = dir;
                //        }
                //    }
                //}
                if (dataPaths.Count == 0)
                {
                    continue;
                }
                int stepData = stepDir / dataPaths.Count;
                int indexData = 0;
                int tarPos = 0;
                foreach (FileInstance dataPath in dataPaths)
                {
                    indexData++;
                    tarPos = progStart + stepDir * indexDir + stepData * indexData;

                    #region 任务被取消


                    if (_cancel)
                    {
                        break;
                    }
                    #endregion

                    try
                    {
                        string dataName = GetDataName(dataType as DataPackage, tempObject as DataPackage, GetFolderName(dir), dataPath);
                        if (isSupplementary && task.ContainsData(dataPath.FullFileName))
                        {
                            InvokeSetCreateProgress(tarPos, "任务数据“" + dataName + "”已存在");
                            continue;
                        }

                        InvokeSetCreateProgress(-1, "验证任务数据“" + dataName + "”是否已入库...");

                        #region 验证未执行任务数据重复

                        if (replaceOption.SameTaskData == EnumMessageBoxExResult.Cancel)
                        {
                            break;
                        }
                        else if (replaceOption.SameTaskData != EnumMessageBoxExResult.YesAll)
                        {
                            dt = TaskDataDAL.Singleton.GetTaskDataByMainData(dataPath.FullFileName, LoginControl.userId);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (!UIUtil.ShowMessageBoxEx("任务数据重复", string.Format("数据[{0}]已经在任务[{1}]中存在，是否重复添加任务数据？", dataName, dt.Rows[0][0].ToString()), EnumMessageBoxExResult.No, ref replaceOption.SameTaskData))
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion

                        #region 验证已入库数据重复

                        //if (replaceOption.SameRegisterInfo == EnumMessageBoxExResult.Cancel)
                        //{
                        //    break;
                        //}
                        //else if (replaceOption.SameRegisterInfo != EnumMessageBoxExResult.YesAll)
                        //{
                        //    if (replaceOption.SameRegisterInfo == EnumMessageBoxExResult.NoAll && (replaceOption.SameTaskData == EnumMessageBoxExResult.Yes || replaceOption.SameTaskData == EnumMessageBoxExResult.YesAll))
                        //    {
                        //        replaceOption.SameRegisterInfo = EnumMessageBoxExResult.Undefined;
                        //    }
                          
                        //    strTmp = DataInstanceHelper.GetServerFilePath(task.StorageServer.ServerParameter.FtpPath,
                        //                                                  task.PrePath.TrimEnd('/') + "/" + pkgFolderName,
                        //                                                  dir,
                        //                                                  dataPath.FullFileName);
                            
                        //    DataRegisterInfo sameObj = DataRegisterInfo.GetSameObject(dataName, task.CatalogID, strTmp, (tempObject as DataPackage).GetMainDataFile().GetXPath()) as DataRegisterInfo;
                        //    if (sameObj != null)
                        //    {
                        //        if (!UIUtil.ShowMessageBoxEx("任务数据已入库", string.Format("选中的节点下已经存在名为[{0}]的数据，是否继续添加？", dataName), EnumMessageBoxExResult.No, ref replaceOption.SameRegisterInfo))
                        //        {
                        //            continue;
                        //        }
                        //    }
                        //}
                        #endregion

                        #region 验证存储服务器路径重复

                        if (replaceOption.SameServerFile == EnumMessageBoxExResult.Cancel)
                        {
                            break;
                        }
                        else if (replaceOption.SameServerFile != EnumMessageBoxExResult.YesAll)
                        {
                            if (replaceOption.SameServerFile == EnumMessageBoxExResult.NoAll && (replaceOption.SameServerFile == EnumMessageBoxExResult.Yes || replaceOption.SameServerFile == EnumMessageBoxExResult.YesAll))
                            {
                                replaceOption.SameServerFile = EnumMessageBoxExResult.Undefined;
                            }


                                strTmp = DataInstanceHelper.GetServerFilePath(task.StorageServer.ServerParameter.FtpPath,
                                                                              task.PrePath.TrimEnd('/') + "/" + pkgFolderName,
                                                                              dir,
                                                                              dataPath.FullFileName);
                          
                            
                            
                            
                            dt = DataPathInfoDAL.Singleton.GetSameDataPathByServerPath(strTmp);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                strTmp = string.Format("数据[{0}]中的文件[{1}]\r\n\r\n已经在存储服务器[{2}]上存在[{3}]，\r\n\r\n是否继续添加任务数据[{0}]？",
                                                       dataName,
                                                       dataPath.FullFileName,
                                                       task.StorageServer.ServerParameter.Name,
                                                       strTmp);
                                if (!UIUtil.ShowMessageBoxEx("目标文件已存在", strTmp, EnumMessageBoxExResult.No, ref replaceOption.SameServerFile))
                                {
                                    continue;
                                }
                            }
                        }
                        #endregion

                        InvokeSetCreateProgress(-1, "创建任务数据“" + dataName + "”...");

                        UpLoadTaskData taskData = new UpLoadTaskData();
                        taskData.TaskDataName = dataName;
                        taskData.TaskID = task.ID;
                        //zqq+
                        taskData.MetaTableName =
                            CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, task.CatalogID).MetaTableName;
                        //
                        taskData.PacPath = dir.TrimEnd('\\');
                        taskData.TableName = task.MetaTableName;
                        //taskData.PackageID = packageID;
                        try
                        {
                            taskData.IsUploadSnapshotFile = task.SnapshotSetting.IsUploadFile;
                            
                        }
                        catch
                        {}
                        try
                        {
                          
                            taskData.IsUploadMetadataFile = task.MetadataSetting.IsUploadFile;
                        }
                        catch
                        { }
                        //if (dictContent != null)
                        //{
                        //    taskData.MetaFieldcontent = dictContent;
                        //}
                        if (taskData.Add())
                        {
                            taskData.MainDataPath = dataPath.FullFileName;
                            taskData.RealPackagePath = dataPath.PackagePath;
                            taskData.Package = tempObject as DataPackage;

                            taskData.CreateTaskFileListFromFS(task.SnapshotSetting, task.MetadataSetting);
                            if (taskData.SaveTaskFile())
                            {
                                totalSize += taskData.Size;
                                taskData.Update();
                                task.AddTaskData(taskData);
                                if (validateNewData)
                                {
                                    option.ValidateMetadata = option.ValidateMetadata && task.IsUpLoadMeta;
                                    option.ValidateSnapshot = option.ValidateSnapshot && task.IsUpLoadSnapShot;
                                    option.ValidateDataFile = option.ValidateDataFile && task.IsUpLoadDataPackage;
                                    option.ValidateThumb = option.ValidateSnapshot;

                                    ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(InitPara.DBHelper, task.CatalogID);
                                    if (catalogNode != null)
                                    {
                                        taskRlt = ValidateTaskData(taskData, option, catalogNode.NodeExInfo.DatumTypeObj.MetaTemplateOjb.ID) && taskRlt;
                                    }
                                    else
                                    {
                                        taskRlt = false;
                                    }
                                 
                                }
                                InvokeSetCreateProgress(tarPos, "创建任务数据“" + dataName + "”成功");
                                _newDataCount++;

                                datas.Add(taskData);
                            }
                            else
                            {
                                taskData.Delete();
                                InvokeSetCreateProgress(tarPos, "创建任务数据“" + dataName + "”失败");
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        LogHelper.Error.Append(exp);
                    }
                    finally
                    {
                        InvokeSetCreateProgress(tarPos, "");
                    }
                    #region 任务被取消
                    if (_cancel)
                    {
                        break;
                    }
                    #endregion
                }
                InvokeSetCreateProgress(progStart + stepDir * (indexDir + 1), "");

                if (_cancel)
                {
                    break;
                }
                if (replaceOption.Cancel)
                {
                    break;
                }
            }
            InvokeSetCreateProgress(progStart + progLength, "");

            #region 检查结果
            if (validateOldData || validateNewData)
            {
                switch (task.ValidateResult)
                {
                    case EnumValidateResult.None:
                        task.ValidateResult = taskRlt ? EnumValidateResult.Succeed : EnumValidateResult.Faild;
                        break;
                    case EnumValidateResult.Succeed:
                        if (!taskRlt)
                        {
                            task.ValidateResult = EnumValidateResult.Faild;
                        }
                        break;
                    case EnumValidateResult.Faild:
                        break;
                }
            }
            #endregion

            return totalSize;
        }

        private string GetSubDataPackagePath(DataPackageType type, string pakcagePath)
        {
            List<DataFolder> parentFolders = new List<DataFolder>();
            VirtualDataPackage virtualDataPackage = type.GetVirtualDataPackage();
            GwDataObject tmp = virtualDataPackage.ParentObject;
            while (tmp.ParentObject != null)
            {
                parentFolders.Insert(0, tmp as DataFolder);
                tmp = tmp.ParentObject;
            }
            string tmpDir = pakcagePath;
            string[] tmpSubDirs = null;
            bool success = true;
            foreach (DataFolder var in parentFolders)
            {
                switch (var.FolderNameRulerType)
                {
                    case EnumDataFolderNameRuler.FixedName:
                        tmpDir = System.IO.Path.Combine(tmpDir, var.CustomNameRuler.CustomString);
                        break;
                    case EnumDataFolderNameRuler.Others:
                        tmpSubDirs = Directory.GetDirectories(tmpDir);
                        if (tmpSubDirs.Length == 0)
                        {
                            success = false;
                        }
                        else
                        {
                            tmpDir = tmpSubDirs[0];
                        }
                        break;
                    default:
                        break;
                }
                if (!success)
                {
                    break;
                }
            }
            if (success)
            {
                return tmpDir;
            }
            else
            {
                return null;
            }
        }

        private string GetFolderName(string dir)
        {
            dir = dir.TrimEnd('\\');
            int index = dir.LastIndexOf('\\');
            if (index >= 0)
            {
                return dir.Substring(index + 1, dir.Length - index - 1);
            }
            else
            {
                return dir;
            }
        }

        /// <summary>
        /// 获取数据名称
        /// </summary>
        /// <param name="basePackage">原始数据包</param>
        /// <param name="realPackage">实际数据包(当数据包中存在子数据包时,否则与原始数据包相同)</param>
        /// <param name="folerName">数据文件夹名称</param>
        /// <param name="file">主文件实例</param>
        /// <returns></returns>
        private string GetDataName(DataPackage basePackage, DataPackage realPackage, string folerName, FileInstance file)
        {
            switch (basePackage.DataNameRuler)
            {
                case EnumDataNameRuler.MainDataFile:
                    return System.IO.Path.GetFileNameWithoutExtension(file.FullFileName);
                case EnumDataNameRuler.DataPackageFolder:
                    return folerName + (realPackage.ParentObject == null ? "" : ("_" + GetFolderName(file.PackagePath)));
                case EnumDataNameRuler.DataPackageAndMainDataFile:
                    return folerName + (realPackage.ParentObject == null ? "" : ("_" + GetFolderName(file.PackagePath))) + "_" + System.IO.Path.GetFileNameWithoutExtension(file.FullFileName);
                default:
                    return System.IO.Path.GetFileNameWithoutExtension(file.FullFileName);
            }
        }

        /// <summary>
        /// 初始化进度值

        /// </summary>
        private void InitProgressValue()
        {
            _max = 0;
            _hadShow = false;
            _cancel = false;
            _newDataCount = 0;
        }

 

        #region 验证任务数据

        /// <summary>
        /// 验证任务合法性
        /// </summary>
        /// <param name="task"></param>
        public void ValidateUploadTask(UpLoadTask task, ValidateOption option)
        {
          
            ICatalogNode dal = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, task.CatalogID);
            if (dal != null)
            {
                _metaTableName = dal.MetaTableName;
            }
            bool taskRlt = true;
            if (task.Datas.Count > CONST_SHOW_PROGRESS_MIN_VALIDATE)
            {
                _positionV = 0;
                _maxV = task.Datas.Count;
                _isShowValidateProgress = true;
                InvokeShowValidateProgress(true);
            }
            else
            {
                _isShowValidateProgress = false;
            }
            foreach (UpLoadTaskData var in task.Datas)
            {
                var.CreateThumbFileFullName();
               
                if (_isShowValidateProgress)
                {
                    InvokeSetValidateProgress(1, "检查数据 " + var.TaskDataName);
                }
                var.CreateTaskFileListFromDB();


                option.ValidateMetadata = option.ValidateMetadata && task.IsUpLoadMeta;
                option.ValidateSnapshot = option.ValidateSnapshot && task.IsUpLoadSnapShot;
                option.ValidateThumb = option.ValidateSnapshot;
                ICatalogNode catalogNode = CatalogFactory.GetCatalogNode(InitPara.DBHelper, task.CatalogID);
                if (catalogNode != null)
                {
                    taskRlt = ValidateTaskData(var, option, catalogNode.NodeExInfo.DatumTypeObj.MetaTemplateOjb.ID) && taskRlt;
                }
                else
                {
                    taskRlt = false;
                }
            }
            task.ValidateResult = taskRlt ? EnumValidateResult.Succeed : EnumValidateResult.Faild;
            task.Update();
            if (_isShowValidateProgress)
            {
                InvokeShowValidateProgress(false);
            }
        }

        /// <summary>
        /// 验证一条任务数据 
        /// </summary>
        /// <param name="var"></param>
        /// <param name="option"></param>
        /// <param name="metaTableName"></param>
        /// <returns></returns>
        private bool ValidateTaskData(UpLoadTaskData var, ValidateOption option, int metaTempID)
        {
            bool dataRlt = true;
            bool metaRlt = true;
            bool snapRlt = true;
            bool thumbRlt = true;
            bool fileRlt = true;
            bool extentRlt = true;

            try
            {
                #region 元数据文件

                if (option.ValidateMetadata)
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(var.MetadataFullName) || !File.Exists(var.MetadataFullName))
                    {
                        var.ValidateInfo.MetadataInfo = "元数据文件不存在";
                        metaRlt = false;
                    }
                    else if (!ValidateMetadata(var.MetadataFullName, metaTempID, ref message))
                    {
                        var.ValidateInfo.MetadataInfo = message;
                        metaRlt = false;
                    }
                    else
                    {
                        var.ValidateInfo.MetadataInfo = "可以入库";
                    }
                }
                #endregion

                #region 快视图文件

                if (option.ValidateSnapshot)
                {
                    if (string.IsNullOrEmpty(var.SnapshotFullName) || !File.Exists(var.SnapshotFullName))
                    {
                        var.ValidateInfo.SanpshotInfo = "快视图文件不存在";
                        snapRlt = false;
                    }
                    else
                    {
                        var.ValidateInfo.SanpshotInfo = "可以入库";
                    }
                }
                #endregion

                #region 拇指图

                if (option.ValidateThumb)
                {
                    if (string.IsNullOrEmpty(var.ThumbFullName) || !File.Exists(var.ThumbFullName))
                    {
                        var.ValidateInfo.ThumbInfo = "拇指图文件不存在"; 

                        thumbRlt = false;
                    }
                    else
                    {
                        var.ValidateInfo.ThumbInfo = "可以入库";
                    }
                }
                #endregion

                #region 空间范围
                string msg = "";
                string tmp = "";
                if (option.ValidateExtent)
                {
                    if (string.IsNullOrEmpty(var.MetadataFullName) || !File.Exists(var.MetadataFullName))
                    {
                        var.ValidateInfo.ExtentInfo = "元数据文件不存在,无法获取空间范围!";

                        extentRlt = false;
                    }
                    else
                    {
                        string wkt= XmlOper.ReadNodeValue(var.MetadataFullName, @"//空间范围WKT串");
                        if (wkt.Trim().Length == 0)
                        {
                            var.ValidateInfo.ExtentInfo = "获取空间范围失败!";
                            extentRlt = false;
                        }
                        else
                        {
                            var.ValidateInfo.ExtentInfo = "可以入库";
                            extentRlt = true;
                        }
                    }
                    //if (!string.IsNullOrEmpty(var.SnapshotFullName) && File.Exists(var.SnapshotFullName))
                    //{
                    //    if (ValidateExtent(var.SnapshotFullName, ref tmp))
                    //    {
                    //        var.ValidateInfo.ExtentInfo = "可以读取";
                    //    }
                    //}
                    //else
                    //{
                    //    tmp = "无快视图";
                    //}
                    //msg += tmp + "，";

                    //if (string.IsNullOrEmpty(var.ValidateInfo.ExtentInfo) && !string.IsNullOrEmpty(var.MetadataFullName) && File.Exists(var.MetadataFullName))
                    //{
                    //    if (ValidateExtentofMetadata(var.MetadataFullName, ref tmp))
                    //    {
                    //        var.ValidateInfo.ExtentInfo = "可以读取";
                    //    }
                    //}
                    //else
                    //{
                    //    tmp = "无元数据";
                    //}
                    //msg += tmp + "，";

                    //if (string.IsNullOrEmpty(var.ValidateInfo.ExtentInfo))
                    //{
                    //    var.ValidateInfo.ExtentInfo = msg + "，无法读取空间范围";
                    //    extentRlt = false;
                    //}
                    //else
                    //{
                    //    extentRlt = true;
                    //}
                }

                #endregion

                #region 数据文件
                if (option.ValidateDataFile)
                {
                    string rlt = var.ValidateTaskFile();
                    if (string.IsNullOrEmpty(rlt))
                    {
                        var.ValidateInfo.DataFileInfo = "可以入库";
                    }
                    else
                    {
                        var.ValidateInfo.DataFileInfo = rlt;
                        fileRlt = false;
                    }
                }
                #endregion

                dataRlt = metaRlt && snapRlt && thumbRlt && fileRlt && extentRlt;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                dataRlt = false;
            }
            var.ValidateResult = dataRlt ? EnumValidateResult.Succeed : EnumValidateResult.Faild;
            var.Update();
            return dataRlt;
        }

        /// <summary>
        /// 验证影像范围是否正确
        /// </summary>
        /// <param name="snapFile">影像数据地址</param>
        /// <param name="message">验证信息</param>
        /// <returns>是否正确</returns>
        private bool ValidateExtent(string snapFile, ref string message)
        {
            bool rlt = true;
            IGeometry env = DataExtentHelper.GetRasterExtent(snapFile);
            if (env == null)
            {
                rlt = false;
                message = "无法读取空间范围";
            }
            else
            {
                //TODO:根据注册图层空间参考做范围验证

                rlt = true;
            }
            if (!rlt)
            {
                message = message.TrimEnd('，');
            }
            return rlt;
        }

        /// <summary>
        /// 验证从元数据提取空间范围合法性
        /// </summary>
        /// <param name="metaFile"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateExtentofMetadata(string metaFile, ref string message)
        {
            if (_spatialFieldConfig == null)
            {
                message = "未配置空间范围字段";
                return false;
            }
            else
            {
                message = "";
                return true;
            }
        }

        /// <summary>
        /// 验证元数据项
        /// </summary>
        /// <param name="metaFile">元数据文件</param>
        /// <param name="tableName">元数据表</param>
        /// <param name="message">验证结果信息</param>
        /// <returns></returns>
        private bool ValidateMetadata(string metaFile, int metaTempID, ref string message)
        {
            bool rlt = false;
            try
            {
                //1、元数据文件获取
                MetaDataReader reader = new MetaDataReader();
                reader.MetaFileFullName = metaFile;
                Dictionary<string, string> FieldAndValuePairs = reader.FieldAndValuePairs;
                //2、获取元数据表中的字段信息
                List<MetaField> metaFields = MetaTemplateManager.GetMetaTemplateFields(InitPara.DBHelper, metaTempID);
                //3、核查
                rlt = MetaItemCheck.IsValid(FieldAndValuePairs, metaFields, ref message);
            }
            catch (Exception exp)
            {
                message = exp.Message;
                LogHelper.Error.Append(exp);
                rlt = false;
            }
            return rlt;
        }

        /// <summary>
        /// 验证核心元数据
        /// </summary>
        /// <param name="metafile"></param>
        /// <param name="dataType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateCoreMetadata(string metafile, GwDataObject dataType, ref string message)
        {
            return true;
        }

        #endregion

        #region 内部方法

        private void InvokeSetCreateProgress(int detailStep, string message)
        {
            if (this.OnSetCreateProgress != null)
            {
                this.OnSetCreateProgress(detailStep, _max, "新增数据 " + _newDataCount + " 条，" + message);
            }
        }

        private void InvokeShowCreateProgress(bool show)
        {
            if (this.OnShowCreateProgress != null)
            {
                this.OnShowCreateProgress(show);
            }
        }

        private void FinishCreate()
        {
            if (this.OnSetCreateProgress != null)
            {
                if (!_cancel)
                {
                    this.OnSetCreateProgress(_max, _max, "任务创建完成");
                }
                InvokeShowCreateProgress(false);
            }
        }

        private void InvokeSetValidateProgress(int detailStep, string message)
        {
            if (this.OnSetCreateProgress != null)
            {
                _positionV += detailStep;
                this.OnSetCreateProgress(_positionV, _maxV, message);
            }
        }

        private void InvokeShowValidateProgress(bool show)
        {
            if (this.OnShowValidateProgress != null)
            {
                this.OnShowValidateProgress(show);
            }
        }

        /// <summary>
        /// 初始化空间范围字段配置
        /// </summary>
        /// <param name="metaTableName">元数据表名</param>
        private void InitSpatialExtentFields(string metaTableName)
        {
            if (string.IsNullOrEmpty(_metaTableName))
            {
                _spatialFieldConfig = null;
            }
            else
            {
                int tableID = SystemTableDAL.Singleton.GetTableIDByTableName(_metaTableName);
                if (tableID > 0)
                {
                    _spatialFieldConfig = SpatialFieldConfigDAL.Singleton.Select(tableID);
                }
            }
        }

        #endregion
    }
}
