using Geoway.Archiver.Catalog.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.DB.Public.Interface;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.Utility.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Factory
{
    public class MetadataFactory
    {
        /// <summary>
        /// 元数据已经存在时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="workspace">确定为OS时，workspace赋null</param>
        /// <param name="dataID"></param>
        /// <returns></returns>
        public static IMetaDataOper Create(IDBHelper dbHelper, IWorkspace workspace, int dataID)
        {
            ICatalogNode catalogNode = DataOper.GetCatalogNodeByDataID(dbHelper, dataID);
            IMetaDataOper metaDataOper;
            if (SysParams.Para_SpatialStorageType == EnumMetaStorageType.enumOracleSpatial
                ||!catalogNode.NodeExInfo.IsSpatialized)
            {
                metaDataOper = new MetadataRegisterOS(dbHelper, dataID);
            }
            else
            {
                metaDataOper = new MetadataRegisterSDE(dbHelper, workspace, dataID);
                
            }
            return metaDataOper;
        }

        /// <summary>
        /// 元数据未存在时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="workspace">确定为OS时，workspace赋null</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static IMetaDataOper Create(IDBHelper dbHelper, IWorkspace workspace, string tableName)
        {
            //如果为从表的话获取其主表的表名
            string masterTableName=StringHelper.TrimEnd(tableName, SysParams.ResourceMetaTableSuffix);

            ICatalogNode catalogNode = DataOper.GetCatalogNodeByTableName(dbHelper, masterTableName);
            IMetaDataOper metaDataOper;
            if (SysParams.Para_SpatialStorageType == EnumMetaStorageType.enumOracleSpatial
                || !catalogNode.NodeExInfo.IsSpatialized)
            {
                metaDataOper = new MetadataRegisterOS(dbHelper, tableName);
               
            }
            else
            {
                metaDataOper = new MetadataRegisterSDE(dbHelper, workspace, tableName);
            }
            return metaDataOper;
        }
    }
}
