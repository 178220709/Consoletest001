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
    /// Ԫ���ݱ�����ֶβ�������
    /// </summary>
    public class MetaDataFactory
    {
        /// <summary>
        /// ����Ԫ���ݻ�δ����ʱ����
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumMetaDataType"></param>
        /// <returns></returns>
        public static IMetaData CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType)
        {
            return CreateMetaData(dbHelper, enumMetaDataType, EnumMetaDatumType.enumUnknown, -1);
        }
        /// <summary>
        /// ����DataID����Ԫ���ݲ���ʵ��
        /// ���ڴ���ϵͳά��Ԫ���ݲ�����ʵ��
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
        /// ����DataID����Ԫ���ݲ���ʵ��
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumMetaDataType">Ԫ�������ͣ�ϵͳά��Ԫ���ݡ�����Ԫ���ݡ���չԪ����</param>
        /// <param name="enumMetaDatumType">Ԫ�������ͣ����enumMetaDataTypeʹ��,�ڶ�������ΪEnumMetaDataType.EnumFixedʱ���ã�������ΪEnumMetaDatumType.enumDefault</param>
        /// <param name="dataID">����ID����������Ϊ-1���粻������ֱ�ӵ������������CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType) </param>
        /// <returns></returns>
        public static IMetaData CreateMetaData(IDBHelper dbHelper,EnumMetaDataType enumMetaDataType,EnumMetaDatumType enumMetaDatumType,int dataID)
        {
            IMetaData metaData = null;
            try
            {
                switch (SysParams.Para_SpatialStorageType)//Ԫ���ݴ洢����
                {
                    case EnumMetaStorageType.enumOracleSpatial:
                        switch (enumMetaDataType)//Ԫ��������
                        {
                            case EnumMetaDataType.EnumSystem: //ϵͳά��Ԫ����
                                metaData = new MetaDataSysOS(dbHelper, dataID);
                                break;
                            case EnumMetaDataType.EnumFixed: //����Ԫ����
                                metaData = new MetaDataFixedOS(dbHelper, dataID, enumMetaDatumType);
                                break;
                            case EnumMetaDataType.EnumExtensional: //��չԪ����
                                metaData = new MetaDataExtensionalOS(dbHelper);
                                break;
                        }
                        break;
                    case EnumMetaStorageType.enumSDE:
                        switch (enumMetaDataType)//Ԫ��������
                        {
                            case EnumMetaDataType.EnumSystem://ϵͳά��Ԫ����
                                //metaData = new MetaDataSysSDE();
                                break;
                            case EnumMetaDataType.EnumFixed://����Ԫ����
                                
                                break;
                            case EnumMetaDataType.EnumExtensional://��չԪ����
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