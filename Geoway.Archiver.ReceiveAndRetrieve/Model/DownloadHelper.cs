using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.ADF.MIS.Utility.DevExpressEx;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.Utility.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.GIS.GeoDB;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public class DownloadHelper
    {
        public delegate void AfterDownloadEventHandler(object sender, DownLoadEventArgs e);
        public delegate void SetDownloadProgressEventhandler(object sender, int pos, int max, string message);
        public event AfterDownloadEventHandler AfterDownload;
        public event SetDownloadProgressEventhandler SetDownloadProgress;
        private IDBHelper _dbHelper;
        private DownLoadEventArgs _eventArgs = new DownLoadEventArgs();
        private object _sender;

        public object Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

            
        public DownloadHelper(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DownLoadEventArgs EventArgs
        {
            get
            {
                return _eventArgs;
            }
        }

        public void OnAfterDownload(DownLoadEventArgs e)
        {
            AfterDownloadEventHandler handler = AfterDownload;
            if (handler != null) handler(_sender, e);
        }


        public bool DownloadData(string localDirectory, IList<RegisterKey> dataIDs)
        {
            bool ignorAll = false;

            LogHelper.Error.Append(string.Format("方法：DownloadData，开始下载{0}条数据。", dataIDs.Count));
            foreach (RegisterKey registerKey in dataIDs)
            {
                IMetaDataOper metaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace,
                                                                          registerKey.DataID);

                double fileSize;
                LogHelper.Error.Append(string.Format("方法：DownloadData，获取数据大小{0}。", registerKey.DataID));
                if(Double.TryParse(metaDataOper.GetValue(FixedFieldName.FLD_NAME_F_DATASIZE).ToString(), out fileSize))
                {
                    _eventArgs.FileSize += fileSize/1024;
                }



                int serverID;
                LogHelper.Error.Append(string.Format("方法：DownloadData，获取{0}数据对应的文件。", registerKey.DataID));
                if (!int.TryParse(metaDataOper.GetValue(FixedFieldName.FLD_NAME_F_SERVERID).ToString(), out serverID)) //未上传数据
                {
                    LogHelper.Error.Append(string.Format("方法：DownloadData，数据{0},不包含可下载文件！", registerKey.DataID));
                    _eventArgs.AlarmList.Add(string.Format("数据{0},不包含可下载文件！", registerKey));
                    continue;
                }

                LogHelper.Error.Append(string.Format("方法：DownloadData，获取{0}数据文件数据源配置信息。", registerKey.DataID));
                StorageServerParam serverParam = CatalogModelEngine.GetStorageNodeByID(_dbHelper,serverID);
                if (serverParam == null)
                {
                    LogHelper.Error.Append(string.Format("方法：DownloadData，数据{0},无法获取服务器信息！。", registerKey.DataID));
                    _eventArgs.ErrorList.Add(string.Format("数据{0},无法获取服务器信息！", registerKey));
                    OnAfterDownload(_eventArgs);
                    return false;
                }

                StorageServer server = CatalogModelEngine.CreateCatalogDataSource(serverParam);
                try
                {

                    if (server != null)
                    {
                        server.BeginGet += server_BeginGet;
                        server.Progress += server_Progress;
                        server.EndGet += server_EndGet;

                        if (!server.Connected())
                        {
                            LogHelper.Error.Append(string.Format("方法：DownloadData，无法连接服务器:“{0}”,数据{1}无法下载！。", registerKey.DataID));
                            _eventArgs.ErrorList.Add(string.Format("无法连接服务器:“{0}”,数据{1}无法下载！", server, dataIDs[0]));
                            continue;
                        }
                    }
                    else
                    {
                        LogHelper.Error.Append(string.Format("方法：DownloadData，数据{0},无法获取服务器信息！", registerKey.DataID));
                        _eventArgs.ErrorList.Add(string.Format("数据{0},无法获取服务器信息！", dataIDs[0].DataID));
                        continue;
                    }

                    DataPathInfo info = new DataPathInfo();
                    info.ObjectID = registerKey.DataID.ToString();
                    IList<DataPathInfo> list = info.SeletByObjectID();
                    _eventArgs.FileCount += list.Count;
                    foreach (DataPathInfo dataPathInfo in list)
                    {
                        LogHelper.Error.Append(string.Format("方法：DownloadData，下载文件{0}！", dataPathInfo.FileLocation));
                        if (dataPathInfo.EnumStorageType == EnumPathStorageType.EnumXML)
                        {
                            DownLoadByXml(server, localDirectory, dataPathInfo, ref ignorAll);

                        }
                        else
                        {
                            if (DownLoadSingleFile(server, localDirectory, dataPathInfo.FileLocation, ref ignorAll))
                            {
                                LogHelper.Error.Append(string.Format("方法：DownloadData，下载文件成功{0}！", dataPathInfo.FileLocation));
                                continue;
                            }
                            //server.ReleaseConnect();
                            LogHelper.Error.Append(string.Format("方法：DownloadData，下载文件失败{0}！", dataPathInfo.FileLocation));
                            return false;

                            #region

                            //bool success = server.GetSingleFile(dataPathInfo.FileLocation,
                            //                                    localDirectory + dataPathInfo.FileLocation);


                            //if (success)
                            //{
                            //    continue;
                            //}
                            //_eventArgs.ErrorList.Add(string.Format("数据文件:“{0}”下载失败", dataPathInfo.FileLocation));
                            //if (!ignorAll)
                            //{
                            //    DialogResult dResult = DevMessageUtil.ShowMsgYesNoCancel(
                            //        string.Format("数据：[{0}]下载失败，忽略全部下载失败数据请选择“是”，忽略本条下载失败数据请选择“否”,停止下载请选择“取消”",
                            //                      dataPathInfo.FileLocation));
                            //    switch (dResult)
                            //    {
                            //        case DialogResult.Yes:
                            //            ignorAll = true;
                            //            continue;
                            //        case DialogResult.No:
                            //            ignorAll = false;
                            //            continue;
                            //        case DialogResult.Cancel:
                            //            _eventArgs.ErrorList.Add("用户停止下载！");
                            //            return false;
                            //    }
                            //}

                            #endregion
                        }

                    }
                    //server.ReleaseConnect();
                }
                catch (System.Exception ex)
                {
                    LogHelper.Error.Append(ex);
                }
                finally
                {
                    server.ReleaseConnect();
                }
            }

            LogHelper.Error.Append(string.Format("方法：DownloadData，结束下载。"));
            OnAfterDownload(_eventArgs);
            return _eventArgs.BSuccess;
        }

        public bool DownloadData(string localDirectory, int dataID)
        {
            try
            {
                if (SysParams.BizWorkspace == null)
                {

                    // 初始化系统参数
                    SysParams.InitSysParams(_dbHelper);
                    SysSpatialParams.InitSysSpatialParams(_dbHelper);

                    //// 构建直连参数
                    //string SDEService = string.Format("sde:{0}", SysParams.Para_OracleType);
                    //string UserPwd = "";
                    string dbServer = string.Empty;
                    if (_dbHelper.DBDriver == DBHelper.enumDBDriver.DDTek)
                    {
                        //UserPwd = string.Format("{0}@{1}/{2}", _dbHelper.DBPwd, _dbHelper.DBServer, _dbHelper.DBSid);
                        dbServer = _dbHelper.DBServer;
                    }
                    else
                    {
                        //UserPwd = string.Format("{0}@{1}", _dbHelper.DBPwd, _dbHelper.DBServiceName);
                        dbServer = _dbHelper.DBServiceName.Substring(0, _dbHelper.DBServiceName.LastIndexOf('/'));
                    }

                    // 获取元数据存储方式
                    if (SysParams.Para_SpatialStorageType == EnumMetaStorageType.enumSDE)
                    {
                        SysSpatialParams.Para_GEOFIELDNAME = "SHAPE";
                        // qfc-此处写死了，后期需要配置修改
                        SdeDataSource sdeDS = new SdeDataSource(dbServer, _dbHelper.DBUser, _dbHelper.DBPwd, "5151", _dbHelper.DBSid, "sde.DEFAULT", "Oracle");
                        string temp = sdeDS.GetConnectionProperties();
                        if (!sdeDS.TestConnection())
                        {
                            DevMessageUtil.ShowMessageDialog(sdeDS.GetConnectionProperties());
                            DevMessageUtil.ShowMessageDialog("业务库空间数据源连接失败！");
                        }
                        else
                        {
                            SysParams.BizWorkspace = sdeDS.Workspace;
                        }
                    }
                    else if (SysParams.Para_SpatialStorageType == EnumMetaStorageType.enumOracleSpatial)
                    {
                        // qfc-此处写死了，后期需要配置修改
                        SdeDataSource sdeDS = new SdeDataSource(dbServer, _dbHelper.DBUser, _dbHelper.DBPwd, "5151", _dbHelper.DBSid, "sde.DEFAULT", "Oracle");
                        if (!sdeDS.TestConnection())
                        {
                            DevMessageUtil.ShowMessageDialog(sdeDS.GetConnectionProperties());
                            DevMessageUtil.ShowMessageDialog("业务库空间数据源连接失败！");
                        }
                        else
                        {
                            SysParams.BizWorkspace = sdeDS.Workspace;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append("方法：DownloadData");
                LogHelper.Error.Append(ex);
                return false;
            }

            StorageServer server = null;
            try
            {
                bool ignorAll = false;

                IMetaDataOper metaDataOper = MetadataFactory.Create(_dbHelper, SysParams.BizWorkspace,
                                                                          dataID);
                double fileSize;
                if (Double.TryParse(metaDataOper.GetValue(FixedFieldName.FLD_NAME_F_DATASIZE).ToString(), out fileSize))
                {
                    _eventArgs.FileSize += fileSize / 1024;
                }

                int serverID;

                if (!int.TryParse(metaDataOper.GetValue(FixedFieldName.FLD_NAME_F_SERVERID).ToString(), out serverID)) //未上传数据
                {
                    _eventArgs.AlarmList.Add(string.Format("数据{0},不包含可下载文件！", dataID));
                    return false;
                }

                StorageServerParam serverParam = CatalogModelEngine.GetStorageNodeByID(_dbHelper, serverID);
                if (serverParam == null)
                {
                    _eventArgs.ErrorList.Add(string.Format("数据{0},无法获取服务器信息！", dataID));
                    OnAfterDownload(_eventArgs);
                    return false;
                }

                server = CatalogModelEngine.CreateCatalogDataSource(serverParam);

                if (server != null)
                {
                    server.BeginGet += server_BeginGet;
                    server.Progress += server_Progress;
                    server.EndGet += server_EndGet;

                    if (!server.Connected())
                    {
                        _eventArgs.ErrorList.Add(string.Format("无法连接服务器:“{0}”,数据{1}无法下载！", server, dataID));
                        return false;
                    }
                }
                else
                {
                    _eventArgs.ErrorList.Add(string.Format("数据{0},无法获取服务器信息！", dataID));
                    return false;
                }

                DataPathInfo info = new DataPathInfo();
                info.ObjectID = dataID.ToString();
                IList<DataPathInfo> list = info.SeletByObjectID(_dbHelper);
                _eventArgs.FileCount += list.Count;
                foreach (DataPathInfo dataPathInfo in list)
                {

                    if (dataPathInfo.EnumStorageType == EnumPathStorageType.EnumXML)
                    {
                        DownLoadByXml(server, localDirectory, dataPathInfo, ref ignorAll);

                    }
                    else
                    {
                        if (DownLoadSingleFile(server, localDirectory, dataPathInfo.FileLocation, ref ignorAll))
                        {
                            return true;
                        }
                        //server.ReleaseConnect();
                        return false;

                        #region

                        //bool success = server.GetSingleFile(dataPathInfo.FileLocation,
                        //                                    localDirectory + dataPathInfo.FileLocation);


                        //if (success)
                        //{
                        //    continue;
                        //}
                        //_eventArgs.ErrorList.Add(string.Format("数据文件:“{0}”下载失败", dataPathInfo.FileLocation));
                        //if (!ignorAll)
                        //{
                        //    DialogResult dResult = DevMessageUtil.ShowMsgYesNoCancel(
                        //        string.Format("数据：[{0}]下载失败，忽略全部下载失败数据请选择“是”，忽略本条下载失败数据请选择“否”,停止下载请选择“取消”",
                        //                      dataPathInfo.FileLocation));
                        //    switch (dResult)
                        //    {
                        //        case DialogResult.Yes:
                        //            ignorAll = true;
                        //            continue;
                        //        case DialogResult.No:
                        //            ignorAll = false;
                        //            continue;
                        //        case DialogResult.Cancel:
                        //            _eventArgs.ErrorList.Add("用户停止下载！");
                        //            return false;
                        //    }
                        //}

                        #endregion
                    }
                    //server.ReleaseConnect();
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append("方法：DownloadData");
                LogHelper.Error.Append(ex);
            }
            finally
            {
                server.ReleaseConnect();
            }

            OnAfterDownload(_eventArgs);
            return _eventArgs.BSuccess;
        }


        private bool DownLoadByXml(StorageServer server,string localDirectroy, DataPathInfo dataPathInfo,ref bool ignorlAll)
        {
            string fileName = Application.StartupPath + "\\temp\\datapath.xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            _dbHelper.ReadBlob2File(fileName,string.Format("{0} = {1}",DataPathDAL.FLD_NAME_F_OBJECTID,dataPathInfo.ObjectID),DataPathDAL.TABLE_NAME,DataPathDAL.FLD_NAME_F_XML);
            if (!File.Exists(fileName))
            {
                return false;
            }

            XmlInfo xmlInfo = new XmlInfo(fileName, false);
            List<string> pathes = xmlInfo.ReadNodes(@"//root/File");
            _eventArgs.FileCount = pathes.Count;
            foreach (string path in pathes)
            {
                if(DownLoadSingleFile(server,localDirectroy,path, ref ignorlAll))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        private bool DownLoadSingleFile(StorageServer server, string localDirectory, string fileLocation, ref bool ignorAll)
        {
            bool success = server.GetSingleFile(fileLocation, localDirectory + fileLocation);

            if (success)
            {
                return true;
            }
            _eventArgs.ErrorList.Add(string.Format("数据文件:“{0}”下载失败", fileLocation));
            if (!ignorAll)
            {
                DialogResult dResult = DevMessageUtil.ShowMsgYesNoCancel(
                    string.Format("数据：[{0}]下载失败，忽略全部下载失败数据请选择“是”，忽略本条下载失败数据请选择“否”,停止下载请选择“取消”",fileLocation));
                switch (dResult)
                {
                    case DialogResult.Yes:
                        ignorAll = true;
                        return true;
                    case DialogResult.No:
                        ignorAll = false;
                        return true;
                    case DialogResult.Cancel:
                        _eventArgs.ErrorList.Add("用户停止下载！");
                        return false;
                }
            }
            return true;
        }
        

        void server_EndGet(object o, ServerFileEventArgs e)
        {
            if (SetDownloadProgress != null)
            {
                if(e.Status==EnumDataExecuteState.Successed)
                {
                    _eventArgs.SucFileCount++;//下载成功数加1
                    _eventArgs.SucFileSize += e.FileSize;//FileNameUtil.GetFileSize(e.FileName);//增加成功下载文件大小
                    SetDownloadProgress(_sender, 100, 100, "下载成功：" + e.FileName);
                }
                else
                {
                    SetDownloadProgress(_sender, 100, 100, "下载失败：" + e.FileName+"原因："+e.Exp.ToString());
                }
            }
        }

        void server_Progress(object o, ServerProgressEventArgs e)
        {
            if(SetDownloadProgress!=null)
            {
                SetDownloadProgress(_sender, (int) e.Position, (int) e.Length, "正在下载：" + e.FileName);
            }
        }

        void server_BeginGet(object o, ServerFileEventArgs e)
        {
            if (SetDownloadProgress != null)
            {
                SetDownloadProgress(_sender, 0, 100, "开始下载：" + e.FileName);
            }
        }


        private void InvokeSetDownloadProgress(int pos, int max, string message)
        {
            if (this.SetDownloadProgress != null)
            {
                this.SetDownloadProgress(_sender, pos, max, message);
            }
        }
    }
    
    public class DownLoadEventArgs:EventArgs
    {
        private List<string> _errorList = new List<string>();
        private List<string> _alarmList = new List<string>();

        private long _fileCount = 0;
        private long _sucFileCount = 0;
        private double _fileSize = 0;
        private double _sucFileSize = 0;



        public List<string> AlarmList
        {
            get { return _alarmList; }
            set { _alarmList = value; }
        } 
        /// <summary>
        /// 下载成功的文件大小
        /// </summary>
        public double SucFileSize
        {
            get { return _sucFileSize; }
            set { _sucFileSize = value; }
        }

        /// <summary>
        /// 下载成功的文件个数
        /// </summary>
        public long SucFileCount
        {
            get { return _sucFileCount; }
            set { _sucFileCount = value; }
        }
        

        /// <summary>
        /// 下载的文件大小
        /// </summary>
        public double FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        /// <summary>
        /// 下载的文件个数
        /// </summary>
        public long FileCount
        {
            get { return _fileCount; }
            set { _fileCount = value; }
        }
        
        /// <summary>
        /// 下载过程中的错误信息
        /// </summary>
        public List<string> ErrorList
        {
            get { return _errorList; }
            set { _errorList = value; }
        }
        
        public bool BSuccess
        {
            get { return _errorList.Count == 0; }
        }
        
    }
}
