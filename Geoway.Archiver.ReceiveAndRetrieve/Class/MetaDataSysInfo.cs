using System;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.DataModel;
using Geoway.ADF.MIS.DataModel.Public;
using System.Collections.Generic;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.Archiver.Catalog;
using Geoway.ADF.MIS.Utility.Log;
using System.Text;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Modeling;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.Modeling.Model;
using Geoway.Archiver.Modeling.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    /// <summary>
    /// 元数据表系统维护字段信息基类
    /// </summary>
    public class MetaDataSysInfo : MetaDataBaseInfo,IMetaDataSysEdit
    {
        #region 核心元数据数据库结构
        public const string FLD_NAME_F_DELETETIME = "F_DELETETIME";
        public const string FLD_NAME_F_FLAG = "F_FLAG";
        public const string FLD_NAME_F_IMPORTDATE = "F_IMPORTDATE";
        public const string FLD_NAME_F_IMPORTUSER = "F_IMPORTUSER";
        public const string FLD_NAME_F_DATANAME = "F_DATANAME";
        #endregion

        #region 构造函数
        public MetaDataSysInfo(IDBHelper dbHelper):base(dbHelper){}
        public MetaDataSysInfo(IDBHelper dbHelper, int dataID):base(dbHelper,dataID){}
        #endregion

        

        #region 属性
        protected DateTime _deleteTime;
        protected int _flag;
        protected DateTime _importDate;
        protected string _importUser;
        protected string _name;

        /// <summary>
        /// 入库人
        /// </summary>
        public string ImportUser
        {
            get { return _importUser; }
            set { _importUser = value; }
        }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime ImportDate
        {
            get { return _importDate; }
            set { _importDate = value; }
        }

        /// <summary>
        /// 标识字段
        /// </summary>
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteTime
        {
            get { return _deleteTime; }
            set { _deleteTime = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion


        protected string getSelectField()
        {
            string fields =
                FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_DELETETIME + "," +
                FLD_NAME_F_FLAG + "," +
                FLD_NAME_F_IMPORTDATE + "," +
                FLD_NAME_F_IMPORTUSER + "," +
                FLD_NAME_F_DATANAME;
            return fields;
        }
        
        
        protected string getAllField()
        {
            return getAllField(_dbHelper, _tableName);
        }

        protected string getAllField(IDBHelper dbHelper,string tableName)
        {
            List<string> strFields = new List<string>();
            try
            {
                int catalogID = int.Parse(tableName.Replace(SysParams.ResourceMetaTablePrefix, "").Trim());
                DatumType datumType = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, catalogID).NodeExInfo.DatumTypeObj;
                List<DatumTypeField> datumTypeFields = datumType.GetDatumFields(EnumFldType.enumSystem);
                foreach (DatumTypeField datumTypeField in datumTypeFields)
                {
                    strFields.Add(datumTypeField.MetaFieldObj.Name);
                }
                return string.Join(",", strFields.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return "*";
            }
        }
    }
}
