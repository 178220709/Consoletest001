using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.DB.Public.Enum;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.Utility.DAL;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// �ļ�·��_���ݷ��ʲ�
    /// </summary>
    public class DataPathDAL : DALBase<DataPathDAL>
    {
        #region ���ݿ�ṹ

        public const string TABLE_NAME = "TBARC_DATAPATH";
        public const string FLD_NAME_F_ID = "F_ID";
        public const string FLD_NAME_F_OBJECTID = "F_OBJECTID"; //��������

        public const string FLD_NAME_F_TARGETREGISTERLAYERNAME = "F_TARGETREGISTERLAYERNAME";
        public const string FLD_NAME_F_FILELOCATION = "F_FILELOCATION";
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_SOURCETYPE = "F_SOURCETYPE"; //��Դ�����ݰ������ݵ�Ԫ��

        public const string FLD_NAME_F_PACKAGEPATH = "F_PACKAGEPATH"; //���ݰ����ļ��С��ļ� ·��
        public const string FLD_NAME_F_SERVERID = "F_SERVERID";

        public const string FLD_NAME_F_STORAGETYPE = "F_STORAGETYPE"; //����·���Ĵ洢��ʽ

        public const string FLD_NAME_F_XML = "F_XML"; //�洢����·����xml

        #endregion

        #region ����ģʽ

        public static DataPathDAL Singleton = new DataPathDAL();

        #endregion

        #region ����

        private int _id = -1;

        /// <summary>
        /// Ψһ��ʶ
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _objectID = "-1";

        /// <summary>
        /// ����ID������HeadInfo��ʶ
        /// </summary>
        public string ObjectID
        {
            get { return _objectID; }
            set { _objectID = value; }
        }

        private string _registerLayerName;

        /// <summary>
        /// �Ǽ�ͼ����

        /// </summary>
        public string RegisterLayerName
        {
            get { return _registerLayerName; }
            set { _registerLayerName = value; }
        }

        private Int64 _dataSize = -1;

        /// <summary>
        /// �ļ����·��
        /// </summary>
        public Int64 DataSize 
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        private string _fileLocation;

        /// <summary>
        /// �ļ����������·��


        /// </summary>
        public string FileLocation
        {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        private string _packagePath = string.Empty;

        /// <summary>
        /// ���ݰ��ڲ�x·��
        /// </summary>
        public string PackagePath
        {
            get { return _packagePath; }
            set { _packagePath = value; }
        }

        private EnumDataFileSourceType _sourceType = EnumDataFileSourceType.DataUnit;

        /// <summary>
        /// �ļ���Դ
        /// </summary>
        public EnumDataFileSourceType SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        private int _serverID = -1;

        /// <summary>
        /// �洢�ڵ�ID
        /// </summary>
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }

        private EnumPathStorageType _enumPathStorageType = EnumPathStorageType.UnKnown;

        public EnumPathStorageType EnumStorageType
        {
            get { return _enumPathStorageType; }
            set { _enumPathStorageType = value; }
        }


        private string _xmlPath;

        /// <summary>
        /// ��¼·����xml�ļ�
        /// </summary>
        public string XmlPath
        {
            get { return _xmlPath; }
            set { _xmlPath = value; }
        }

        #endregion

        #region ���غ���

        public override bool Insert()
        {
            try
            {
                string sqlStatement = string.Empty;
                IList<DBFieldItem> items = new List<DBFieldItem>();
                _id = GetNextID(TABLE_NAME, FLD_NAME_F_ID);
                items.Add(new DBFieldItem(FLD_NAME_F_ID, _id, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTString));
                items.Add(new DBFieldItem(FLD_NAME_F_STORAGETYPE, (int) _enumPathStorageType, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, _serverID, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_SOURCETYPE, (int) _sourceType, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
                items.Add(new DBFieldItem(FLD_NAME_F_TARGETREGISTERLAYERNAME, _registerLayerName,
                                          EnumDBFieldType.FTString));
                if (_enumPathStorageType == EnumPathStorageType.EnumDB)
                {
                    items.Add(new DBFieldItem(FLD_NAME_F_FILELOCATION, _fileLocation, EnumDBFieldType.FTString));
                    items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEPATH, _packagePath, EnumDBFieldType.FTString));
                }
                sqlStatement = SQLStringUtility.GetInsertSQL(TABLE_NAME, items, DBHelper.GlobalDBHelper);
                bool bSuccess = DoSQL(sqlStatement);
                if (bSuccess && (_enumPathStorageType == EnumPathStorageType.EnumXML ||
                                 _enumPathStorageType == EnumPathStorageType.EnumDataTable))
                {
                    DBHelper.GlobalDBHelper.SaveFile2Blob(_xmlPath, FLD_NAME_F_ID + "=" + _id, TABLE_NAME,
                                                          FLD_NAME_F_XML,
                                                          false);
                }
                return bSuccess;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }

        }

        public override bool Delete()
        {
            string sqlStatement;
            sqlStatement = "DELETE FROM " + TABLE_NAME + " WHERE " + FLD_NAME_F_ID + " = " + "'" + this._id + "'";

            bool bSuccess = DoSQL(sqlStatement);
            return bSuccess;
        }

        public override bool Update()
        {
            string sqlStatement;
            string strFilter = FLD_NAME_F_ID + " = " + "'" + this._id + "'";
            IList<DBFieldItem> items = new List<DBFieldItem>();

            items.Add(new DBFieldItem(FLD_NAME_F_OBJECTID, _objectID, EnumDBFieldType.FTString));
            items.Add(new DBFieldItem(FLD_NAME_F_STORAGETYPE, (int) _enumPathStorageType, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_SERVERID, _serverID, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_SOURCETYPE, (int) _sourceType, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_DATASIZE, _dataSize, EnumDBFieldType.FTNumber));
            items.Add(new DBFieldItem(FLD_NAME_F_TARGETREGISTERLAYERNAME, _registerLayerName,
                                      EnumDBFieldType.FTString));
            if (_enumPathStorageType == EnumPathStorageType.EnumDB)
            {
                items.Add(new DBFieldItem(FLD_NAME_F_FILELOCATION, _fileLocation, EnumDBFieldType.FTString));
                items.Add(new DBFieldItem(FLD_NAME_F_PACKAGEPATH, _packagePath, EnumDBFieldType.FTString));
            }

            sqlStatement = SQLStringUtility.GetUpdateSQL(TABLE_NAME, items, strFilter, DBHelper.GlobalDBHelper);
            bool bSuccess = DoSQL(sqlStatement);

            if (bSuccess && (_enumPathStorageType == EnumPathStorageType.EnumXML ||
                             _enumPathStorageType == EnumPathStorageType.EnumDataTable))
            {
                if (!string.IsNullOrEmpty(_xmlPath))
                {
                    DBHelper.GlobalDBHelper.SaveFile2Blob(_xmlPath, FLD_NAME_F_ID + "=" + _id, TABLE_NAME,
                                                          FLD_NAME_F_XML, false);
                }
            }

            return bSuccess;
        }

        ///<summary>
        /// ѡ��
        /// </summary>
        /// <returns>��������·���б�</returns>
        public override IList<DataPathDAL> Select()
        {
            IList<DataPathDAL> pList = new List<DataPathDAL>();
            DataTable dtResult = DoQuery();
            pList = Translate(dtResult);
            return pList;
        }

        public override string ToString()
        {
            return _fileLocation;
        }

        #endregion

        #region ��չ���Բ�ѯ

        public IList<DataPathDAL> SeletByObjectID(string objID, EnumDataFileSourceType sourceType)
        {
            DataTable dt =
                DoQuery(FLD_NAME_F_OBJECTID + "='" + objID + "' AND " + FLD_NAME_F_SOURCETYPE + "=" + (int) sourceType);
            return Translate(dt);
        }

        public IList<DataPathDAL> SeletByObjectID(IDBHelper db, string objID, EnumDataFileSourceType sourceType)
        {
            DataTable dt =
                DoQuery(db, FLD_NAME_F_OBJECTID + "='" + objID + "' AND " + FLD_NAME_F_SOURCETYPE + "=" + (int)sourceType);
            return Translate(dt);
        }
        
        public DataPathDAL SeletByObjectID(int objID)
        {
            IList<DataPathDAL> dals = SeletByObjectID(objID.ToString(), EnumDataFileSourceType.DataUnit);
            if(dals.Count>0)
            {
                return dals[0];
            }
            return null;
        }

        public IList<DataPathDAL> SeletByObjectID(string layername, int objID, EnumDataFileSourceType sourceType)
        {
            DataTable dt =
                DoQuery(FLD_NAME_F_OBJECTID + "=" + objID.ToString() + " AND " + FLD_NAME_F_SOURCETYPE + "=" +
                        (int) sourceType + " AND " + FLD_NAME_F_TARGETREGISTERLAYERNAME + "='" + layername + "'");
            return Translate(dt);
        }

        public IList<DataPathDAL> Select(string layername, string objectID, string packagePath,
                                         EnumDataFileSourceType sourceType)
        {
            string strFitler = string.Format("{0}='{1}' and {2}='{3}' and {4}={5}",
                                             FLD_NAME_F_OBJECTID, objectID,
                                             FLD_NAME_F_PACKAGEPATH, packagePath,
                                             FLD_NAME_F_SOURCETYPE, (int) sourceType);
            DataTable dt = DoQuery(strFitler);
            return Translate(dt);
        }

        public DataPathDAL Select(int fid)
        {
            IList<DataPathDAL> pList = new List<DataPathDAL>();

            string strFilter = FLD_NAME_F_ID + " = " + fid;
            DataTable dtResult = DoQuery(strFilter);
            pList = Translate(dtResult);
            if (pList.Count > 0)
            {
                return pList[0];
            }
            else
                return null;
        }

        public DataTable DoQuery()
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " order by " + FLD_NAME_F_OBJECTID;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        public DataTable DoQuery(string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " where " + strFilter + " order by " +
                           FLD_NAME_F_OBJECTID;
            DataTable dtResult = DBHelper.GlobalDBHelper.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        public DataTable DoQuery(IDBHelper db, string strFilter)
        {
            string sqlStatement;
            sqlStatement = "SELECT " + getSelectField() + " FROM " + TABLE_NAME + " where " + strFilter + " order by " +
                           FLD_NAME_F_OBJECTID;
            DataTable dtResult = db.DoQueryEx(TABLE_NAME, sqlStatement, true);
            return dtResult;
        }

        /// <summary>
        /// ��ȡ
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="svrPath"></param>
        /// <returns></returns>
        public DataTable GetSameDataPathByServerPath(string svrPath)
        {
            string filter = FLD_NAME_F_FILELOCATION + "='" + svrPath + "'";
            return DoQuery(filter);
        }

        /// <summary>
        /// ��ȡ
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="svrPath"></param>
        /// <returns></returns>
        public IList<DataPathDAL> GetSameDataPathListByServerPath(string svrPath)
        {
            DataTable dt = GetSameDataPathByServerPath(svrPath);
            IList<DataPathDAL> lstDataPathInfoDAL = Translate(dt);
            return lstDataPathInfoDAL;
        }

        /// <summary>
        /// �����ƶ�������·����ȡ���ӦԪ�����б�
        /// </summary>
        /// <param name="dbHelper"> </param>
        /// <param name="svrPath"></param>
        /// <returns></returns>
        public IList<IMetaDataEdit> GetMetaDataByPath(IDBHelper dbHelper, string svrPath)
        {
            IList<IMetaDataEdit> lstMetaData = new List<IMetaDataEdit>();
            try
            {
                IList<DataPathDAL> lstDataPath = GetSameDataPathListByServerPath(svrPath);
                foreach (DataPathDAL dal in lstDataPath)
                {
                    IMetaDataOper metaDataOper = MetadataFactory.Create(dbHelper, SysParams.BizWorkspace,
                                                                        int.Parse(dal.ObjectID));
                    IMetaDataEdit metaDataEdit = metaDataOper as IMetaDataEdit;
                    lstMetaData.Add(metaDataEdit);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return lstMetaData;
        }

        public static bool UpdateBlobField(IDBHelper db, int dataID, object obj)
        {
            try
            {
                byte[] blob = Geoway.Archiver.Utility.Class.SerializationUtility.Serialize(obj);
                string sql = string.Format("{0}={1}", FLD_NAME_F_OBJECTID, dataID);
                db.SaveBytes2Blob(sql, TABLE_NAME, FLD_NAME_F_XML, ref blob);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);

                return false;
            }
        }

        public static object GetBlobFieldValue(IDBHelper db, int dataID)
        {
            try
            {
                byte[] value = db.ReadBlob2Bytes(dataID, FLD_NAME_F_OBJECTID, TABLE_NAME, FLD_NAME_F_XML);
                if (value != null)
                {
                    return Geoway.Archiver.Utility.Class.SerializationUtility.Deserialize(value);
                }

                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        /// <summary>
        /// ���ݷ������洢·����ȡ�����ļ���¼��
        /// </summary>
        /// <param name="svrPath">�������洢·��</param>
        /// <returns></returns>
        public static int GetDataPathCountByServerPath(string svrPath, int serverID)
        {
            string sql = string.Format("select count(*) from {0} where {1}={2} and {3}='{4}'",
                                       TABLE_NAME,
                                       FLD_NAME_F_SERVERID, serverID,
                                       FLD_NAME_F_FILELOCATION, svrPath);
            try
            {
                using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return 0;
            }
        }

        #endregion

        #region translate

        /// <summary>
        /// ����DataTable
        /// <param name="dtResult">Դ����</param>
        /// <returns>ʵ�弯��</returns>
        public IList<DataPathDAL> Translate(DataTable dtResult)
        {
            IList<DataPathDAL> list = new List<DataPathDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                string size = string.Empty;
                DataRow pRow = null;
                string sLayers = string.Empty;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    DataPathDAL info = new DataPathDAL();
                    info.ID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ID);
                    info.ObjectID = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_OBJECTID);
                    info.RegisterLayerName = GetSafeDataUtility.ValidateDataRow_S(pRow,
                                                                                  FLD_NAME_F_TARGETREGISTERLAYERNAME);
                    info.FileLocation = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_FILELOCATION);
                    size = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATASIZE);
                    if (string.IsNullOrEmpty(size))
                    {
                        info.DataSize = 0;
                    }
                    else
                    {
                        info.DataSize = Convert.ToInt64(size);
                    }
                    info.PackagePath = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_PACKAGEPATH);
                    info.SourceType =
                        (EnumDataFileSourceType) GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SOURCETYPE);
                    info.ServerID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                    info.EnumStorageType = (EnumPathStorageType) (Convert.ToInt32(pRow[FLD_NAME_F_STORAGETYPE]));
                    list.Add(info);
                }
            }
            return list;
        }

        #endregion

        #region �ڲ�����

        private string getSelectField()
        {
            string fields = FLD_NAME_F_ID + "," +
                            FLD_NAME_F_TARGETREGISTERLAYERNAME + "," +
                            FLD_NAME_F_OBJECTID + "," +
                            FLD_NAME_F_FILELOCATION + "," +
                            FLD_NAME_F_DATASIZE + "," +
                            FLD_NAME_F_PACKAGEPATH + "," +
                            FLD_NAME_F_SOURCETYPE + "," +
                            FLD_NAME_F_STORAGETYPE + "," +
                            FLD_NAME_F_SERVERID;

            return fields;
        }

        #endregion

        #region ɾ��

        /// <summary>
        /// ɾ��ע����Ϣ�µ������ļ���¼
        /// </summary>
        /// <param name="objId"> </param>
        /// <returns></returns>
        public bool DeleteDataPathByObjID(string objId)
        {
            string sql = string.Format("delete {0} where {1}='{2}'", TABLE_NAME, FLD_NAME_F_OBJECTID, objId);
            return DoSQL(sql);
        }

        public bool DeleteDataPathByHeadInfoID(string layername, int headInfoID)
        {
            string sql = string.Format("delete {0} where {1}={2} and {3}='{4}'", TABLE_NAME, FLD_NAME_F_OBJECTID,
                                       headInfoID, FLD_NAME_F_TARGETREGISTERLAYERNAME, layername);
            return DoSQL(sql);
        }

        #endregion

        /// <summary>
        /// ���ݶ���ID��Ԫ���ݱ���Ϊ����id����ȡ�ϴ��ļ��ڷ������ϵ�����·��
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public IList<string> GetPathesByObjectID(int objectID)
        {
            IList<string> pathes = new List<string>();
            
            DataPathDAL dataPathDAL = SeletByObjectID(objectID);
            //��ȡ�洢����
            if (dataPathDAL != null)
            {
                EnumPathStorageType storageType = dataPathDAL.EnumStorageType;
                switch (storageType)
                {
                    case EnumPathStorageType.EnumXML:
                        pathes = GetPathes_Xml(objectID);
                        break;
                    case EnumPathStorageType.EnumDB:
                        pathes = GetPathes_DB(objectID);
                        break;
                    case EnumPathStorageType.EnumDataTable:
                        break;
                    case EnumPathStorageType.UnKnown:
                        break;
                }
            }
            return pathes;
        }
        /// <summary>
        ///  ��ȡ��DB��ʽ�洢������·��
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        private IList<string> GetPathes_DB(int objectID)
        {
            IList<string> pathes = new List<string>();
            IList<DataPathDAL> dals = SeletByObjectID(objectID.ToString(), EnumDataFileSourceType.DataUnit);
            foreach (DataPathDAL dal in dals)
            {
                pathes.Add(dal.FileLocation);
            }
            return pathes;
        }

        /// <summary>
        /// ��ȡ��XML��ʽ�洢������·��
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        private IList<string> GetPathes_Xml(int objectID)
        {
            IList<string> pathes = new List<string>();
            string fileName = Application.StartupPath + "\\temp\\datapath.xml";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            DBHelper.GlobalDBHelper.ReadBlob2File(fileName,
                                                  string.Format("{0} = {1}", FLD_NAME_F_OBJECTID,
                                                                objectID), TABLE_NAME,
                                                  FLD_NAME_F_XML);
            if (File.Exists(fileName))
            {
                XmlInfo xmlInfo = new XmlInfo(fileName, false);
                pathes = xmlInfo.ReadNodes(@"//root/File");
            }
            return pathes;
        }
    }
}
