using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.UpLoad;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.Utility.Class;
using System.Data;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.Core.Public.security;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    public class UpLoadJZData : IMetaDataFixedJZEdit, IWriteMetaData
    {
        private IDBHelper _dbHelper;
        private int _dataId = -1;


        Dictionary<string, object> _dicSoure = null;
        public Dictionary<string, object> DicSoure
        {
            set { _dicSoure = value; }
        }
        
        private int _catalogID;
        public int CatalogID
        {
            set { _catalogID = value; }
        }

        #region IMetaDataFixedJZEdit 成员

        private string _virtualWarehouseAddress;
        /// <summary>
        /// 库房位置
        /// </summary>
        public string VirtualWarehouseAddress
        {
            get { return _virtualWarehouseAddress; }
            set { _virtualWarehouseAddress = value; }
        }

        private string _barCode;
        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode
        {
            get { return _barCode; }
            set { _barCode = value; }
        }

        public int DatumAmount
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public EnumCarrierType enumCarrierType
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IMetaDataEdit 成员

        public int DataId
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string TableName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
        
        
        public UpLoadJZData(IDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        
        /// <summary>
        /// 写元数据表操作
        /// </summary>
        /// <returns></returns>
        public bool WriteMetaData()
        {
            //1、写扩展信息
            if (WriteMetaDataExtensional() != null)
            {
                //2、写固有属性
                if (WriteMetaDataFixed() != null)
                {
                    //3、写系统维护信息
                    if (WriteMetaDataSys() != null)
                    {
                        return true;
                    }
                }
            }

            //4、如果失败则删除相关记录
            IMetaDataSys metaDataSys =
                MetaDataFactory.CreateMetaData(_dbHelper, EnumMetaDataType.EnumSystem, _dataId) as IMetaDataSys;
            if (metaDataSys != null)
            {
                metaDataSys.Delete();
            }

            return false;
        }

        /// <summary>
        /// 写元数据表操作
        /// 元数据通过手动采集获取
        /// </summary>
        /// <returns></returns>
        public bool WriteSingleMetaData()
        {
            //1、写采集到的信息到库表
            if (WriteMetaDataExtensional() != null)
            {
                DataOper.UpdateFlag(_dbHelper, _dataId, 0); 
                
               return true;
        
            }

            //2、如果失败则删除相关记录
            IMetaDataSys metaDataSys =
                MetaDataFactory.CreateMetaData(_dbHelper, EnumMetaDataType.EnumSystem, _dataId) as IMetaDataSys;
            if (metaDataSys != null)
            {
                metaDataSys.Delete();
            }

            return false;
        }


        #region IWriteMetaData 成员
        
        public IMetaDataEdit WriteMetaDataSys()
        {
            bool success = true;
            IMetaDataSysEdit coreMetaDateEdit = null;
            try
            {
                IMetaData coreMetaDate =
                    MetaDataFactory.CreateMetaData(_dbHelper, EnumMetaDataType.EnumSystem, EnumMetaDatumType.enumDefault, _dataId);

                //更新核心元数据信息
                if (coreMetaDate != null)
                {
                    coreMetaDateEdit = coreMetaDate as IMetaDataSysEdit;
                    //coreMetaDateEdit.Name = this.Name;
                    coreMetaDateEdit.ImportDate = DateTime.Now;
                    coreMetaDateEdit.ImportUser = LoginControl.userName;
                    coreMetaDateEdit.Flag = -1;
                    success=coreMetaDate.Update();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                success = false;
            }
            return success? coreMetaDateEdit:null;
        }

  
        public IMetaDataEdit WriteMetaDataFixed()
        {
            bool bSuccess = true;
            IMetaDataFixedJZEdit metaDataFixedjzEdit = null;
            try
            {
                metaDataFixedjzEdit = new MetaDataFixedJZOS(_dbHelper, _dataId);
                metaDataFixedjzEdit.VirtualWarehouseAddress = _virtualWarehouseAddress;
                metaDataFixedjzEdit.BarCode = _barCode;
                IMetaDataFixedJZ metaDataFixedJz = metaDataFixedjzEdit as IMetaDataFixedJZ;
                bSuccess =metaDataFixedJz.Update();
            }
            catch(Exception ex)
            {
                bSuccess = false;
            }
            return bSuccess ? metaDataFixedjzEdit : null;

        }

        public IMetaDataEdit WriteMetaDataExtensional()
        {
            bool success;
            IMetaDataExtensionalEdit metaDataExtensionalEdit = null;
            try
            {
                //IMetaDataExtensionalJZEdit metaDataExtensionalJzEdit;
                IMetaData metaDataDBOper;
                metaDataDBOper = MetaDataFactory.CreateMetaData(_dbHelper, EnumMetaDataType.EnumExtensional);
                metaDataExtensionalEdit = metaDataDBOper as IMetaDataExtensionalEdit;

                metaDataExtensionalEdit.CatalogId = _catalogID;
                metaDataExtensionalEdit.EnumMetaDatumType = EnumMetaDatumType.enumSTNONUMMED;
                metaDataExtensionalEdit.MetaDataSource = _dicSoure;
                success = metaDataDBOper.Insert();
                _dataId = metaDataExtensionalEdit.DataId;
            }
            catch(Exception ex)
            {
                success = false;
            }

            return success ? metaDataExtensionalEdit : null;
        }

        #endregion


    }
}
