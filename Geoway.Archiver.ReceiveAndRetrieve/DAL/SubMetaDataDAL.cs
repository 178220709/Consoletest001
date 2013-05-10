using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.Catalog.Interface;
using Geoway.ADF.MIS.DB.Public.Interface;
using System.Diagnostics;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.Modeling.Model;
using Geoway.Archiver.Query.Model;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    [Serializable]
    public class SubMetaDataDAL
    {
        public const string FLD_NAME_F_OID = "F_OID";

        #region 单实例

        private static SubMetaDataDAL _instance = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static SubMetaDataDAL SingleInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SubMetaDataDAL();
                }
                return _instance;
            }
        }

        #endregion


        private string _subTableName;
        /// <summary>
        /// 从表名称
        /// </summary>
        public string SubTableName
        {
            get { return _subTableName; }
            set { _subTableName = value; }
        }

        private Dictionary<string, object> _dicSubMetaData;

        public Dictionary<string, object> DicSubMetaData
        {
            get { return _dicSubMetaData; }
            set { _dicSubMetaData = value; }
        } 

        private ICatalogNode _catalogNode;

        public ICatalogNode CatalogNode
        {
            get { return _catalogNode; }
            set { _catalogNode = value; }
        }

        private IDBHelper _dbHelper;

        public IDBHelper DbHelper
        {
            get { return _dbHelper; }
            set { _dbHelper = value; }
        }


        public bool Insert()
        {
            try
            {
                string sql = this.GetInsertSQLString(_dicSubMetaData, _dbHelper);
                _dbHelper.DoSQL(sql);
                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }

        }


        /// <summary>
        /// 获取插入字符串
        /// </summary>
        /// <param name="dic"> </param>
        /// <param name="db"></param>
        /// <returns></returns>
        private string GetInsertSQLString(Dictionary<string,object> dic , IDBHelper db)
        {
            Debug.Assert(db != null);
            string insertSQLString = string.Empty;
            int oid = _dbHelper.GetNextValidID(_subTableName, FLD_NAME_F_OID);
            IList<DBFieldItem> insertFieldItems = new List<DBFieldItem>();

            DatumType datumType = _catalogNode.NodeExInfo.DatumTypeObj;
            List<DatumTypeField> lst = datumType.GetDatumFields(EnumModelType.MasterSlaveTable);

            insertFieldItems.Add(new DBFieldItem(FLD_NAME_F_OID, oid, EnumDBFieldType.FTNumber));
            foreach (DatumTypeField datumTypeField in lst)
            {
                insertFieldItems.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, dic[datumTypeField.MetaFieldObj.AliasName], MetaFieldOper.MetaTypeToDBType(datumTypeField.MetaFieldObj.Type)));
                
            }
            
            insertSQLString = SQLStringUtility.GetInsertSQL(_subTableName, insertFieldItems, db);
            Debug.Assert(!string.IsNullOrEmpty(insertSQLString));
            return insertSQLString;
        }
    
        
        private string getFields()
        {
            string fields = string.Empty;

            DatumType datumType = _catalogNode.NodeExInfo.DatumTypeObj;
            List<DatumTypeField> lst = datumType.GetDatumFields(EnumModelType.SingleTable);

            foreach(DatumTypeField datumTypeField in lst)
            {
                fields += datumTypeField.MetaFieldObj.Name + ",";
            }
            return fields.TrimEnd(',');
        }
    }
}
