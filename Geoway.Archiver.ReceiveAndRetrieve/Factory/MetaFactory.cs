using Geoway.Archiver.Utility.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using System;
using Geoway.Archiver.Modeling.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Factory
{
    /// <summary>
    /// 元数据表核心字段操作工厂
    /// </summary>
    public class MetaDataFactory
    {
        /// <summary>
        /// 该条元数据还未存在时调用
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumMetaDataType"></param>
        /// <returns></returns>
        public static IMetaData CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType)
        {
            return CreateMetaData(dbHelper, enumMetaDataType, EnumMetaDatumType.enumUnknown, -1);
        }
        /// <summary>
        /// 根据DataID创建元数据操作实例
        /// 用于创建系统维护元数据操作类实例
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumMetaDataType"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public static IMetaData CreateMetaData(IDBHelper dbHelper, EnumMetaDataType enumMetaDataType,int dataId)
        {
            return CreateMetaData(dbHelper, enumMetaDataType, EnumMetaDatumType.enumUnknown, dataId);
        }
        /// <summary>
        /// 根据DataID创建元数据操作实例
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumMetaDataType">元数据类型：系统维护元数据、固有元数据、扩展元数据</param>
        /// <param name="enumMetaDatumType">元资料类型，配合enumMetaDataType使用,第二个参数为EnumMetaDataType.EnumFixed时设置，否则设为EnumMetaDatumType.enumDefault</param>
        /// <param name="dataID">数据ID，不存在则为-1，如不存在则直接调用替代方法：CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType) </param>
        /// <returns></returns>
        public static IMetaData CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType,EnumMetaDatumType enumMetaDatumType,int dataID)
        {
            IMetaData metaData = null;
            try
            {
                switch (SysParams.Para_SpatialStorageType)//元数据存储类型
                {
                    case EnumMetaStorageType.enumOracleSpatial:
                        switch (enumMetaDataType)//元数据类型
                        {
                            case EnumMetaDataType.EnumSystem: //系统维护元数据
                                metaData = new MetaDataSysOS(dbHelper, dataID);
                                break;
                            case EnumMetaDataType.EnumFixed: //固有元数据
                                metaData = new MetaDataFixedOS(dbHelper, dataID, enumMetaDatumType);
                                break;
                            case EnumMetaDataType.EnumExtensional: //扩展元数据
                                metaData = new MetaDataExtensionalOS(dbHelper);
                                break;
                        }
                        break;
                    case EnumMetaStorageType.enumSDE:
                        switch (enumMetaDataType)//元数据类型
                        {
                            case EnumMetaDataType.EnumSystem://系统维护元数据
                                //metaData = new MetaDataSysSDE();
                                break;
                            case EnumMetaDataType.EnumFixed://固有元数据
                                
                                break;
                            case EnumMetaDataType.EnumExtensional://扩展元数据
                                metaData = new MetaDataExtensionalSDE();
                                break;
                        }
                        break;
                }
               
                return metaData;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        
        
    }
    
    
}