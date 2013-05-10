using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    class DeleteHelper
    {
        /// <summary>
        /// 删除实体数据
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="workspace"> </param>
        /// <param name="reDataID"></param>
        /// <returns></returns>
        public static bool DeleteData(IDBHelper dbHelper, IWorkspace workspace, int reDataID, out string errInfo)
        {
            errInfo = "";
            IMetaDataOper metaDataOper = MetadataFactory.Create(dbHelper, workspace,
                                                                reDataID);

            int serverID = -1;
            if (!Int32.TryParse(metaDataOper.GetValue(FixedFieldName.FLD_NAME_F_SERVERID).ToString(), out serverID))
                //未上传数据
            {

                return true;
            }

            if (serverID < 0)
            {
                return true;
            }

            StorageServerParam serverParam = CatalogModelEngine.GetStorageNodeByID(dbHelper, serverID);
            if (serverParam == null) //无法获取服务器信息
            {
                errInfo = string.Format("无法获取存储数据源信息，请确认文件数据源中存储数据源[{0}]的配置信息。", serverID);
                return false;
            }

            StorageServer server = CatalogModelEngine.CreateCatalogDataSource(serverParam);
            try
            {
                if (server != null)
                {
                    if (!server.Connected()) //无法连接服务器
                    {
                        //server.ReleaseConnect();
                        errInfo = string.Format("无法连接存储数据源，请确认存储数据源[{0}]连接是否正常。", serverParam.Name);
                        return false;
                    }

                }
                else //无法获取服务器信息
                {
                    return false;
                }
                DataPathInfo info = new DataPathInfo();
                info.ObjectID = reDataID.ToString();
                IList<DataPathInfo> list = info.SeletByObjectID();
                foreach (DataPathInfo dataPathInfo in list)
                {
                    if (dataPathInfo.EnumStorageType == EnumPathStorageType.EnumXML)
                    {
                        bool isSuccess = DeleteDataByXml(dbHelper, server, dataPathInfo, ref errInfo);
                        //server.ReleaseConnect();
                        return isSuccess;
                    }
                    if (!DeleteDataSingle(server, dataPathInfo, ref errInfo))
                    {
                        //server.ReleaseConnect();
                        return false;
                    }
                }
                //server.ReleaseConnect();
                return true;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
            finally
            {
                server.ReleaseConnect();
            }
        }

        private static bool DeleteDataSingle(StorageServer server, DataPathInfo dataPathInfo, ref string errInfo)
        {
            if (server.FileExist(dataPathInfo.FileLocation)) //存在则执行删除，不存在则默认删除成功
            {
                if (!server.DeleteFile(dataPathInfo.FileLocation))
                {
                    errInfo = String.Format("文件【{0}】删除失败。", dataPathInfo.FileLocation);

                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="dataPathInfo"></param>
        /// <param name="errInfo"></param>
        /// <returns></returns>
        private static bool DeleteDataByXml(IDBHelper dbHelper,StorageServer server, DataPathInfo dataPathInfo, ref string errInfo)
        {
            string fileName = Application.StartupPath + "\\temp\\datapath.xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            dbHelper.ReadBlob2File(fileName,
                                    String.Format("{0} = {1}", DataPathDAL.FLD_NAME_F_OBJECTID,
                                                  dataPathInfo.ObjectID), DataPathDAL.TABLE_NAME,
                                    DataPathDAL.FLD_NAME_F_XML);
            if (!File.Exists(fileName))
            {
                return false;
            }

            XmlInfo xmlInfo = new XmlInfo(fileName, false);
            List<string> pathes = xmlInfo.ReadNodes(@"//root/File");
            foreach (string path in pathes)
            {
                if (!server.DeleteFile(path))
                {
                    errInfo = String.Format("文件【{0}】删除失败。", path);
                    return false;
                }
            }
            return true;
        }
    }
}
