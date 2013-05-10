using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.Archiver.Utility.Class;
using Geoway.Archiver.Catalog;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.Model
{
    /// <summary>
    /// 实体类DataRegisterInfo 
    /// </summary>
    [Serializable]
    public class DataRegisterInfo : RegisterInfo
    {
        public static  string STATIC_F_DATAID
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATAID;
            }
        }

        #region 登记信息字段名
        public static string STATIC_F_DATANAME
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATANAME;
            }
        }
        public static string STATIC_F_CATALOGID 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_CATALOGID;
            }
        }
        public static string STATIC_F_GEOGRAPHY
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_GEOGRAPHY;
            }
        }
        public static string STATIC_F_SERVERID 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_SERVERID;
            }
        }
        public static string STATIC_F_IMPORTUSER 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_IMPORTUSER;
            }
        }
        public static string STATIC_F_IMPORTDATE 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_IMPORTDATE;
            }
        }
        public static string STATIC_F_DATASIZE
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATASIZE;
            }
        }
        public static string STATIC_F_DATAUNIT
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATAUNIT;
            }
        }
        public static string STATIC_F_DATADESC
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATADESC;
            }
        }
        public static string STATIC_F_KEYWORD
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_KEYWORD;
            }
        }
        public static string STATIC_F_FLAG 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_FLAG;
            }
        }
        public static string STATIC_F_THUMIMGE 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_THUMIMGE; 
            }
        }
        public static string STATIC_F_YEAR
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_YEAR;
            }
        }
        public static string STATIC_F_SCALE
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_SCALE;
            }
        }
        public static string STATIC_F_RESOLUTION 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_RESOLUTION;
            }
        }
        public static string STATIC_F_YMAX
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_YMAX;
            }
        }
        public static string STATIC_F_YMIN
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_YMIN;
            }
        }
        public static string STATIC_F_XMAX 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_XMAX;
            }
        }
        public static string STATIC_F_XMIN
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_XMIN;
            }
}
        public static string STATIC_F_METATABLENAME 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_METATABLENAME;
            }
        }
        public static string STATIC_F_METAID
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_METAID;
            }
        }
        public static string STATIC_F_CATALOGTYPE
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_CATALOGTYPE;
            }
        }
        public static string STATIC_F_HASSNAPSHOT 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_HASSNAPSHOT; 
            }
        }
        public static string STATIC_F_SNAPSHOTLAYER 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_SNAPSHOTLAYER;
            }
        }
        public static string STATIC_F_DELETETIME 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DELETETIME;
            }
        }
        public static string STATIC_F_SDEINDEXLAYER
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_SDEINDEXLAYER;
            }
        }
        public static string STATIC_F_EXTENT 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_EXTENT;
            }
        }
        public static string STATIC_F_OBJECTTYPENAME 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_OBJECTTYPENAME;
            }
        }
        public static string STATIC_F_DATATIME 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_DATATIME;
            }
        }
        public static string STATIC_F_REALPKGNAME 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_REALPKGNAME;
            }
        }
        public static string STATIC_F_PACKAGEID 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_PACKAGEID;
            }
        }
        public static string STATIC_F_ORDERNUM 
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_ORDERNUM;
            }
        }
        public static string STATIC_F_ORDERDW
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_ORDERDW;
            }
        }
        public static string STATIC_F_ISDISPLAY
        {
            get
            {
                return SpatialExtentInfoDAL.FLD_NAME_F_ISDISPLAY;
            }
        }
        #endregion

        private SpatialExtentInfoDAL Singleton = null;

        public DataRegisterInfo()
        {
        }

        public DataRegisterInfo(string layername)
        {
            layerName = layername;

            Singleton = new SpatialExtentInfoDAL(layerName);
        }

        #region 属性


        public static IWorkspace TargetWorkspace
        {
            get { return SpatialExtentInfoDAL.TargetWorkSpace; }
            set { SpatialExtentInfoDAL.TargetWorkSpace = value; }
        }

        public static string SDEUser
        {
            get { return SpatialExtentInfoDAL.SDEUser; }
            set { SpatialExtentInfoDAL.SDEUser = value; }

        }

        private string _dataname;
        private int _catalogid = -1;
        private int _catalogType = -1;
        private int _serverid = -1;
        private string _datapath;
        private string _serverPath;
        private string _indexLayer;
        private string metaRelPath;
        string snapRelPath;
        string snapLayer;
        private string _importuser = string.Empty;
        private DateTime _importdate;
        private string _dataunit = string.Empty;
        private string _datadesc = string.Empty;
        private string _keyword = string.Empty;
        private int _isfile = -1;
        private int _hassdelayer = -1;
        private string _relatedfiles;
        private byte[] _thumimge;
        private string _f_scale;
        private string _resolution;
        private int _datatype;
        private string _sdelayer;
        private int _fileordirectory = -1;
        private int _orderNum = 0;
        private string _orderDW;

        /// <summary>
        /// 数据名
        /// </summary>
        public string DataName
        {
            set { _dataname = value; }
            get { return _dataname; }
        }
        /// <summary>
        /// 目录节点ID
        /// </summary>
        public int CatalogID
        {
            set { _catalogid = value; }
            get { return this._catalogid; }
        }

        public int CatalogType
        {
            get { return _catalogType; }
            set { _catalogType = value; }
        }

        /// <summary>
        /// ftp服务标识
        /// </summary>
        public int ServerID
        {
            set { _serverid = value; }
            get { return this._serverid; }
        }
        /// <summary>
        /// 远程数据路径
        /// </summary>
        public string RemotePath
        {
            set { _datapath = value; }
            get { return _datapath; }
        }

        public string ServerPath
        {
            get { return _serverPath; }
            set { _serverPath = value; }
        }

        /// <summary>
        /// SDE对应接图表图层名
        /// </summary>
        public string IndexLayer
        {
            get 
            {
                return _indexLayer;
            }
            set { _indexLayer = value; }
        }

        public string SnapLayer
        {
            get { return snapLayer; }
            set { snapLayer = value; }
        }

        /// <summary>
        /// 入库
        /// </summary>
        public string ImportUser
        {
            set { _importuser = value; }
            get { return _importuser; }
        }
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime ImportDate
        {
            set { _importdate = value; }
            get { return _importdate; }
        }

        /// <summary>
        /// 数据单位
        /// </summary>
        public string DataSizeUnit
        {
            set { _dataunit = value; }
            get { return _dataunit; }
        }
        /// <summary>
        /// 数据描述
        /// </summary>
        public string DataDescription
        {
            set { _datadesc = value; }
            get { return _datadesc; }
        }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords
        {
            set { _keyword = value; }
            get { return _keyword; }
        }
        /// <summary>
        /// 是否有关联文件
        /// </summary>
        public int HasRelativeFiles
        {
            set { _isfile = value; }
            get { return this._isfile; }
        }

        /// <summary>
        /// 是否有sde图层
        /// </summary>
        public int HasSDELayer
        {
            set { _hassdelayer = value; }
            get { return this._hassdelayer; }
        }
        /// <summary>
        /// 关联文件列表
        /// </summary>
        public string RelativeFiles
        {
            set { _relatedfiles = value; }
            get { return _relatedfiles; }
        }
        /// <summary>
        /// 拇指图
        /// </summary>
        public byte[] ThumbImage
        {
            set { _thumimge = value; }
            get { return _thumimge; }
        }

        /// <summary>
        /// 比例尺
        /// </summary>
        public string Scale
        {
            set { _f_scale = value; }
            get { return _f_scale; }
        }
        /// <summary>
        /// 分辨率
        /// </summary>
        public string Resolution
        {
            set { _resolution = value; }
            get { return _resolution; }
        }
        /// <summary>
        /// 数据类型级别
        /// </summary>
        public int DataGrade
        {
            set { _datatype = value; }
            get { return _datatype; }
        }

        public string SnapRelPath
        {
            get { return snapRelPath; }
            set { snapRelPath = value; }
        }

        /// <summary>
        /// sde库对应图层物理名称
        /// </summary>
        public string SdeLayerName
        {
            set { _sdelayer = value; }
            get { return _sdelayer; }
        }

        /// <summary>
        /// 文件或文件夹类型
        /// </summary>
        public int FileOrDirectory
        {
            set { _fileordirectory = value; }
            get { return this._fileordirectory; }
        }

        public string MetaRelPath
        {
            get { return metaRelPath; }
            set { metaRelPath = value; }
        }

        private string _objectTypeName = string.Empty;
        /// <summary>
        /// 对象类型名称
        /// </summary>
        public string ObjectTypeName
        {
            get { return _objectTypeName; }
            set { _objectTypeName = value; }
        }

        private DateTime _dataTime;
        /// <summary>
        /// 数据时间
        /// </summary>
        public DateTime DataTime
        {
            get { return _dataTime; }
            set { _dataTime = value; }
        }

        private string _realPackageName = string.Empty;
        /// <summary>
        /// 数据类型实际文件夹名（多层使用“/”分隔）
        /// </summary>
        public string RealPackageName
        {
            get { return _realPackageName; }
            set { _realPackageName = value; }
        }

        private int _dataPackageID = -1;

        public int DataPackageID
        {
            get { return _dataPackageID; }
            set { _dataPackageID = value; }
        }

        public int OrderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
        }

        public string OrderDW
        {
            get { return _orderDW; }
            set { _orderDW = value; }
        }

        string layerName = string.Empty;

        public string LayerName
        {
            get { return layerName; }
            set 
            {
                layerName = value;
                if (Singleton == null)
                {
                    Singleton = new SpatialExtentInfoDAL(value); 
                }
            }
        }

        public IFeatureClass RegisterFeatureClass
        {
            get
            {
                if (Singleton != null)
                {
                    return Singleton.IndexFeatureClass;
                }
                return null;
            }
        }

        #endregion 

        #region 重载函数
        public override bool Add()
        {
            bool result = true;

            SpatialExtentInfoDAL headInfoDAL = Translate(this);
            result = result && headInfoDAL.Insert();

            if (result)
            {
                this.ID = headInfoDAL.DataID;
            }

            return result;
        }

        public override bool Delete()
        {
            bool result = true;

            SpatialExtentInfoDAL headInfoDAL = Translate(this);
            result = result && headInfoDAL.Delete();

            return result;
        }

        public override bool Update()
        {
            bool result = true;

            SpatialExtentInfoDAL headInfoDAL = Translate(this);
            result = result && headInfoDAL.Update();
            
            return result;
        }
        #endregion

        #region 公共函数

        public  IList<RegisterInfo> Select()
        {
            IList<RegisterInfo> result = new List<RegisterInfo>();
            IList<SpatialExtentInfoDAL> dataRegisterInfoDALs = Singleton.Select();

            foreach (SpatialExtentInfoDAL dataRegisterInfoDAl in dataRegisterInfoDALs)
            {
                result.Add(Translate(dataRegisterInfoDAl));
            }
            return result;
        }

        public  RegisterInfo Select(int id)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = Singleton.Select(id);
            if (dataRegisterInfoDAL == null)
                return null;
            return Translate(dataRegisterInfoDAL);
        }

        public static RegisterInfo Select(Geoway.Archiver.Utility.Class.RegisterKey rk)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = Translate(rk);
            if (dataRegisterInfoDAL == null)
                return null;
            return Translate(dataRegisterInfoDAL);
        }

        public static RegisterInfo Select(string layername, int id)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layername);
            dataRegisterInfoDAL = dataRegisterInfoDAL.Select(id);
            if (dataRegisterInfoDAL == null)
                return null;
            return Translate(dataRegisterInfoDAL);
        }

        public static IFeature Select2(string layername, int id)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layername);
            return dataRegisterInfoDAL.Select2(id);
        }

        public IFeatureCursor Select(string filter)
        {
            return Singleton.Select(filter);
        }

        public IFeatureCursor Select(string _filter, ESRI.ArcGIS.Geometry.IGeometry geometry)
        {
            return Singleton.Select(_filter, geometry, esriSpatialRelEnum.esriSpatialRelIntersects);
        }

        public static IFeatureCursor Select(string layername, IList<int> ids)
        {

            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layername);
            string filter = string.Empty;
            for (int i = 0; i < ids.Count; i++)
            {
                filter += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + ids[i] + " or ";
 
            }
            if (filter != string.Empty)
            {
                filter = filter.TrimEnd(" or ".ToCharArray());
            }
            return dataRegisterInfoDAL.Select(filter);
        }

        public DataTable DoQuery(string filter)
        {
            return Singleton.DoQuery(filter);
        }

        public static DataTable DoQuery(string layername,string filter)
        {
            SpatialExtentInfoDAL se = new SpatialExtentInfoDAL(layername);
            return se.DoQuery(filter);
        }

        public  DataTable DoQuery(string _filter, ESRI.ArcGIS.Geometry.IGeometry geometry)
        {
            return Singleton.DoQuery(_filter, geometry);
        }

        public void CreateQueryRow(IFeature pFeature, ref DataRow dr)
        {
            Singleton.CreateQueryRow(pFeature, ref dr);
        }

        public static DataTable CreateQueryView()
        {
            return SpatialExtentInfoDAL.CreateQueryView();
        }

        public static IList<string> CreateQueryField()
        {
            IList<string> fields = new List<string>();
            DataTable dt = SpatialExtentInfoDAL.CreateQueryView();
            foreach (DataColumn dc in dt.Columns)
            {
                fields.Add(dc.ColumnName);
            }
            return fields;
        }

        public static IList<int> GetSnapIDs(IList<Geoway.Archiver.Utility.Class.RegisterKey> rks)        
        {
            IList<int> ids = new List<int>();
            IDictionary<string, IList<int>> dataids = PreProcess(rks);
            IFeatureCursor pFeatureCursor = null;
            IFeature pFeature = null;
            int fldIndex = 0;
            int rasterID = 0;
            string tmp = "";
            foreach (KeyValuePair<string, IList<int>> vk in dataids)
            {
                pFeatureCursor = Select(vk.Key, vk.Value);
                pFeature = pFeatureCursor.NextFeature();
                if (pFeature != null)
                {
                    fldIndex = pFeature.Fields.FindField(SpatialExtentInfoDAL.FLD_NAME_F_SNAPSHOTLAYER);
                    tmp = pFeature.get_Value(fldIndex).ToString();
                    if (int.TryParse(tmp, out rasterID))
                    {
                        ids.Add(rasterID);
                    }
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);
            }
            return ids;
        }

        /// <summary>
        /// 获取相同数据对象
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <param name="catalogID">所属节点ID</param>
        /// <param name="svrPath">服务器存储路径</param>
        /// <returns></returns>
        public static RegisterInfo GetSameObject(string name, int catalogID, string svrPath, string packagePath)
        {
            string sql = "select " + DataPathDAL.FLD_NAME_F_OBJECTID 
                       + " from " + DataPathDAL.TABLE_NAME 
                       + " where " + DataPathDAL.FLD_NAME_F_FILELOCATION + "=" + "'" + svrPath + "'"
                       + " and " + DataPathDAL.FLD_NAME_F_PACKAGEPATH + "='" + packagePath + "'";
            DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx(DataPathDAL.TABLE_NAME, sql, true);
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + Convert.ToInt32(dt.Rows[i][DataPathDAL.FLD_NAME_F_OBJECTID]) + " or ";
                }

                sql = sql.TrimEnd(" or ".ToCharArray());

                sql = "( " + sql + " )"
                     + " and " + SpatialExtentInfoDAL.FLD_NAME_F_CATALOGID + "=" + catalogID
                     + " and " + SpatialExtentInfoDAL.FLD_NAME_F_DATANAME + "='" + name + "'";


                string layername = CatalogFactory.GetCatalogNode(DBHelper.GlobalDBHelper, catalogID).MetaTableName;
                SpatialExtentInfoDAL se = new SpatialExtentInfoDAL(layername);
                IList<SpatialExtentInfoDAL> lst = se.Select2(sql);

                if (lst.Count > 0)
                {
                    return Translate(lst[0]);
                }
            }
            return null;

        }

        public  bool SaveThumbImage(int registerID, byte[] dBytes)
        {
            return Singleton.SaveThumbImage(registerID, dBytes);
        }

        public  bool SaveThumbImage(int registerID, string streamFile)
        {
            return Singleton.SaveThumbImage(registerID, streamFile);
        }

        public  byte[] GetThumeImage(int dataID)
        {
            return Singleton.GetThumeImage(dataID);
        }

        public static byte[] GetThumeImage(Geoway.Archiver.Utility.Class.RegisterKey rk)
        {
            SpatialExtentInfoDAL se = Translate(rk);
            if (se != null)
            {
                return se.GetThumeImage(rk.DataID);
            }
            return null;
        }

        public  bool UpdateDisplay(IList<int> dataIDs, bool isDisplay)
        {
            return Singleton.UpdateDisplay(dataIDs, isDisplay);
        }

        public bool UpdateOrderNum(IList<int> dataIDs)
        {
            return Singleton.UpdateOrderNum(dataIDs,"");
        }

        public static bool UpdateOrderNum(string layername, IList<int> dataIDs, string orderDW)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layername);
            return dataRegisterInfoDAL.UpdateOrderNum(dataIDs,orderDW);
        }

        public static bool UpdateOrderNum(IList<Geoway.Archiver.Utility.Class.RegisterKey> dataIDs,string orderDW)
        {
            bool succssed = true;
            IDictionary<string, IList<int>> endIDs = PreProcess(dataIDs);
            foreach (KeyValuePair<string, IList<int>> vp in endIDs)
            {
                succssed = UpdateOrderNum(vp.Key, vp.Value, orderDW) && succssed;
 
            }
            return succssed;
            //return Singleton.UpdateOrderNum(dataIDs);
        }

        public static bool UpdateState(IList<Geoway.Archiver.Utility.Class.RegisterKey> dataIDs, EnumObjectState newState)
        {
            bool succssed = true;
            IDictionary<string, IList<int>> endIDs = PreProcess(dataIDs);
            foreach (KeyValuePair<string, IList<int>> vp in endIDs)
            {
                succssed = UpdateState(vp.Key, vp.Value,newState) && succssed;

            }
            return succssed;
        }

        public static bool UpdateState(string layername, IList<int> dataIDs, EnumObjectState newState)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layername);
            return dataRegisterInfoDAL.UpdateState(dataIDs, newState);
        }

        /// <summary>
        /// 更新存储节点ID
        /// </summary>
        /// <param name="layerName">注册信息图层名称</param>
        /// <param name="dataID">数据ID</param>
        /// <param name="newServerID">新存储节点ID</param>
        /// <returns></returns>
        public static bool UpdateServerID(string layerName, int dataID, int newServerID)
        {
            SpatialExtentInfoDAL dataRegisterInfoDAL = new SpatialExtentInfoDAL(layerName);
            return dataRegisterInfoDAL.UpdateServerID(dataID, newServerID);
        }

        public void RefreshQueryExent(IMapControl4 mapControl, ILayer pLayer, IList<int> dataids, bool isZoom)
        {
            // qfc
            //string sql = string.Empty;
            //foreach (int dataid in dataids)
            //{
            //    sql += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + dataid + " or ";
            //}
            //sql = sql.TrimEnd(" or ".ToCharArray());
            //IFeatureCursor featureCursor = Singleton.Select(sql);
            //IList<SpatialExtentInfoDAL> spatialExtentInfoDALs = Singleton.Select2(sql);

            //ISymbol pSymbol = null;
            //IActiveView pActiveView = mapControl.ActiveView;

            //ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            //envelope.SetEmpty();

            //ESRI.ArcGIS.Geometry.IGeometry geometry = null;
            //ESRI.ArcGIS.Geometry.IEnvelope featureExtent = null;

            //IMap map = mapControl.Map;

            //try
            //{
            //    IFeature feature = featureCursor.NextFeature();
            //    int i = 0;
            //    while (feature != null)
            //    {
            //        geometry = feature.ShapeCopy;
            //        featureExtent = geometry.Envelope;
            //        envelope.Union(featureExtent);

            //        pSymbol = OrderSysmbolConfig.getOrderSymbol(spatialExtentInfoDALs[i++].ObjectTypeName);

            //        if (pSymbol == null)
            //        {
            //            pSymbol = LocateHelper.GetFillSymbol() as ISymbol;
            //        }

            //        LocateHelper.AddCreateElement(geometry, ref pActiveView, pSymbol);
            //        map.SelectFeature(pLayer, feature);
            //        feature = featureCursor.NextFeature();
            //    }
            //    if (isZoom)
            //    {
            //        envelope.Expand(1.5, 1.5, true);
            //        mapControl.Extent = envelope;
            //    }
            //    else
            //    {
            //        ESRI.ArcGIS.Geometry.IPoint pPoint = new ESRI.ArcGIS.Geometry.PointClass();
            //        pPoint.PutCoords((envelope.XMax + envelope.XMin) / 2, (envelope.YMax + envelope.YMin) / 2);
            //        mapControl.CenterAt(pPoint);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error.Append(ex);
            //}
            //throw new Exception("The method or operation is not implemented.");
        }

        public  void RefreshOrderExent(IMapControl4 mapControl, ILayer pLayer, IList<int> dataids, bool isZoom)
        {
            //UpdateDisplay(dataids, true); //更新显示设置

            //string sql = string.Empty;
            //foreach (int dataid in dataids)
            //{
            //    sql += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + dataid + " or ";
            //}
            //sql = sql.TrimEnd(" or ".ToCharArray());

            //IFeatureLayerDefinition pFeatureLayerDefinition = pLayer as IFeatureLayerDefinition;
            //pFeatureLayerDefinition.DefinitionExpression = sql;

            //IList<SpatialExtentInfoDAL> spatialExtentInfoDALs = Singleton.Select2(sql);

            //IActiveView pActiveView = mapControl.ActiveView;

            //ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            //envelope.SetEmpty();

            //ESRI.ArcGIS.Geometry.IGeometry geometry = null;
            //ESRI.ArcGIS.Geometry.IEnvelope featureExtent = null;
            //ISymbol pSymbol = null;
            //try
            //{
            //    foreach (SpatialExtentInfoDAL spatialExtentInfoDAL in spatialExtentInfoDALs)
            //    {
            //        geometry = spatialExtentInfoDAL.Geometry;
            //        featureExtent = geometry.Envelope;
            //        envelope.Union(featureExtent);

            //        pSymbol = OrderSysmbolConfig.getOrderSymbol(spatialExtentInfoDAL.ObjectTypeName);

            //        if (pSymbol == null)
            //        {
            //            pSymbol = LocateHelper.GetFillSymbol() as ISymbol;
            //        }

            //        LocateHelper.AddCreateElement(geometry, ref pActiveView, pSymbol);
            //    }
            //    if (isZoom)
            //    {
            //        envelope.Expand(1.5, 1.5, true);
            //        mapControl.Extent = envelope;
            //    }
            //    else
            //    {
            //        ESRI.ArcGIS.Geometry.IPoint pPoint = new ESRI.ArcGIS.Geometry.PointClass();
            //        pPoint.PutCoords((envelope.XMax + envelope.XMin) / 2.0, (envelope.YMax + envelope.YMin) / 2.0);
            //        mapControl.CenterAt(pPoint);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //throw new Exception("The method or operation is not implemented.");
        }

        public  IList<IFeature> GetFeaturesByIDs(IList<int> dataids)
        {
            string sql = string.Empty;
            foreach (int dataid in dataids)
            {
                sql += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + dataid + " or ";
            }
            sql = sql.TrimEnd(" or ".ToCharArray());

            IFeatureCursor pFeatureCursor = Singleton.Select(sql);

            IList<IFeature> features = new List<IFeature>();
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                features.Add(pFeature);
                pFeature = pFeatureCursor.NextFeature();
            }
            return features;
        }

        public  IList<IFeature> GetFeaturesByIDs(IList<int> dataids, string metaTableName)
        {
            string sql = SpatialExtentInfoDAL.FLD_NAME_F_METATABLENAME + "='" + metaTableName + "' and ";
            foreach (int dataid in dataids)
            {
                sql += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + dataid + " or ";
            }
            sql = sql.TrimEnd(" or ".ToCharArray());

            IFeatureCursor pFeatureCursor = Singleton.Select(sql);

            IList<IFeature> features = new List<IFeature>();
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                features.Add(pFeature);
                pFeature = pFeatureCursor.NextFeature();
            }
            return features;
        }

        public  IList<string> GetMetaDataTableName(IList<int> dataids)
        {
            return Singleton.GetMetaDataTableName(dataids);
        }

        public  ISpatialReference GetRegisterSpatialRF()
        {
            if (Singleton.IndexFeatureClass != null)
            {
                return (Singleton.IndexFeatureClass as IGeoDataset).SpatialReference;
            }
            return null;
        }

        public bool UpdateSpatialReference(ISpatialReference sr)
        {
            if (Singleton.IndexFeatureClass != null)
            {
                IGeoDataset pGeoDataset = Singleton.IndexFeatureClass as IGeoDataset;
                ISchemaLock schemaLock = (ISchemaLock)pGeoDataset;
                if (pGeoDataset.SpatialReference != sr)
                {
                    //ISpatialReference p = sr;
                    //double x1, y1, x2, y2;
                    //p.GetDomain(out x1, out y1, out x2, out y2);

                    //QI到IGeoDatasetSchemaEdit
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                        IGeoDatasetSchemaEdit2 pGeoDatasetSchemaEdit = pGeoDataset as IGeoDatasetSchemaEdit2;
                        if (pGeoDatasetSchemaEdit.CanAlterSpatialReference == true)
                        {
                            pGeoDatasetSchemaEdit.AlterSpatialReference(sr);
                            //pGeoDataset.SpatialReference.SetDomain(x1, y1, x2, y2);
                            pGeoDataset.SpatialReference.Changed();

                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error.Append(ex);
                        return false;
                    }
                    finally
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }

                    //p = pGeoDataset.SpatialReference;
                    //p.GetDomain(out x1, out y1, out x2, out y2);
                }
            }
            return true;
        }

        public  IList<IField> GetCustomFields()
        {
            IList<IField> fields = new List<IField>();
            IField pField = null;

            int index = -1;
            if (Singleton.IndexFeatureClass != null)
            {
                index = Singleton.IndexFeatureClass.FindField(STATIC_F_DATANAME);
                if (index > 0)
                {
                    fields.Add(Singleton.IndexFeatureClass.Fields.get_Field(index));
                }

                index = Singleton.IndexFeatureClass.FindField(STATIC_F_OBJECTTYPENAME);
                if (index > 0)
                {
                    fields.Add(Singleton.IndexFeatureClass.Fields.get_Field(index));
                }

                index = Singleton.IndexFeatureClass.FindField(STATIC_F_RESOLUTION);
                if (index > 0)
                {
                    fields.Add(Singleton.IndexFeatureClass.Fields.get_Field(index));
                }

                index = Singleton.IndexFeatureClass.FindField(STATIC_F_DATATIME);
                if (index > 0)
                {
                    fields.Add(Singleton.IndexFeatureClass.Fields.get_Field(index));
                }
            }
            return fields;
        }

        public  IList<IField> GetCustomFields(string MetaTableName)
        {
            IList<IField> fields = new List<IField>();
            IField pField = null;
            //
            int index = -1;
            if (Singleton.IndexFeatureClass != null)
            {
                index = Singleton.IndexFeatureClass.FindField(STATIC_F_DATANAME);
                if (index > 0)
                {
                    pField = VectorDataOperater.CreateField("", STATIC_F_DATANAME.TrimStart("F_".ToCharArray()), esriFieldType.esriFieldTypeString, true);
                    fields.Add(pField);//Singleton.IndexFeatureClass.Fields.get_Field(index)
                }

                IFieldChecker pFieldChecker = new FieldCheckerClass();
                

                index = Singleton.IndexFeatureClass.FindField(STATIC_F_OBJECTTYPENAME);
                if (index > 0)
                {
                    pField = VectorDataOperater.CreateField("", STATIC_F_OBJECTTYPENAME.TrimStart("F_".ToCharArray()), esriFieldType.esriFieldTypeString, true);
                    fields.Add(pField);//Singleton.IndexFeatureClass.Fields.get_Field(index)
                }
            }
            IList<SystemTableDAL> pList = SystemTableDAL.Singleton.Select(SystemTableDAL.FLD_NAME_F_TABLENAME + "='" + MetaTableName + "'");
            if (pList.Count > 0)
            {
                IList<SystemFieldsDAL> pFieldsList = SystemFieldsDAL.Singleton.Select(pList[0].ID);
                foreach (SystemFieldsDAL fieldsDAL in pFieldsList)
                {
                    if (fieldsDAL.FieldName.ToUpper() != STATIC_F_DATANAME && fieldsDAL.FieldName.ToUpper() != STATIC_F_OBJECTTYPENAME)
                    {
                        pField = VectorDataOperater.CreateField("", fieldsDAL.FieldName.TrimStart("F_".ToCharArray()), SystemFieldsDAL.GetEsriFieldType(fieldsDAL.FieldTypeId), true);
                        if (!fields.Contains(pField))
                        {
                            fields.Add(pField);
                        }
                    }
                    
                }
            }

            return fields;
        }
        public IList<IField> GetCustomFields(IList<SystemFieldsDAL> pFieldsList)
        {
            IList<IField> fields = new List<IField>();
            IField pField = null;
            //
            int index = -1;
            if (Singleton.IndexFeatureClass != null)
            {
                index = Singleton.IndexFeatureClass.FindField(STATIC_F_DATANAME);
                if (index > 0)
                {
                    pField = VectorDataOperater.CreateField("", STATIC_F_DATANAME, esriFieldType.esriFieldTypeString, true);
                    fields.Add(pField);//Singleton.IndexFeatureClass.Fields.get_Field(index)
                }

                IFieldChecker pFieldChecker = new FieldCheckerClass();


                index = Singleton.IndexFeatureClass.FindField(STATIC_F_OBJECTTYPENAME);
                if (index > 0)
                {
                    pField = VectorDataOperater.CreateField("", STATIC_F_OBJECTTYPENAME, esriFieldType.esriFieldTypeString, true);
                    fields.Add(pField);//Singleton.IndexFeatureClass.Fields.get_Field(index)
                }
            }
            //IList<SystemTableDAL> pList = SystemTableDAL.Singleton.Select(SystemTableDAL.FLD_NAME_F_TABLENAME + "='" + MetaTableName + "'");
            //if (pList.Count > 0)
            //{
            //    IList<SystemFieldsDAL> pFieldsList = SystemFieldsDAL.Singleton.Select(pList[0].ID);
                foreach (SystemFieldsDAL fieldsDAL in pFieldsList)
                {
                    if (fieldsDAL.FieldName.ToUpper() != STATIC_F_DATANAME && fieldsDAL.FieldName.ToUpper() != STATIC_F_OBJECTTYPENAME)
                    {
                        pField = VectorDataOperater.CreateField("", fieldsDAL.FieldName, SystemFieldsDAL.GetEsriFieldType(fieldsDAL.FieldTypeId), true);
                        fields.Add(pField);
                    }

                }
            //}

            return fields;
        }
        public static RegisterInfo Translate(SpatialExtentInfoDAL spatialExtentInfoDAL)
        {

            DataRegisterInfo dataRegisterInfo = new DataRegisterInfo();
            dataRegisterInfo.ID = spatialExtentInfoDAL.DataID;
            dataRegisterInfo.DataName = spatialExtentInfoDAL.DataName;
            dataRegisterInfo.CatalogID = spatialExtentInfoDAL.CatalogID;
            dataRegisterInfo.DataDescription = spatialExtentInfoDAL.DataDesc;
            dataRegisterInfo.CatalogType = spatialExtentInfoDAL.CatalogType;
            dataRegisterInfo.DataSize = spatialExtentInfoDAL.DataSize;
            dataRegisterInfo.DataSizeUnit = spatialExtentInfoDAL.DataUnit;
            dataRegisterInfo.Flag = (EnumObjectState)spatialExtentInfoDAL.Flag;
            dataRegisterInfo.Geography = spatialExtentInfoDAL.Geography;

            dataRegisterInfo.ImportDate = spatialExtentInfoDAL.ImportDate;
            dataRegisterInfo.ImportUser = spatialExtentInfoDAL.ImportUser;
            dataRegisterInfo.Keywords = spatialExtentInfoDAL.KeyWord;
            dataRegisterInfo.MetaID = spatialExtentInfoDAL.MetaID;
            dataRegisterInfo.MetaTableName = spatialExtentInfoDAL.MetaTableName;
            //dataRegisterInfo.MinX = spatialExtentInfoDAL.XMin;
            //dataRegisterInfo.MinY = spatialExtentInfoDAL.YMin;
            //dataRegisterInfo.MaxX = spatialExtentInfoDAL.XMax;
            //dataRegisterInfo.MaxY = spatialExtentInfoDAL.YMax;
            dataRegisterInfo.SnapLayer = spatialExtentInfoDAL.SnapShotLayer;
            dataRegisterInfo.IndexLayer = spatialExtentInfoDAL.IndexLayer;
            dataRegisterInfo.Resolution = spatialExtentInfoDAL.Resolution;
            dataRegisterInfo.Scale = spatialExtentInfoDAL.Scale;
            dataRegisterInfo.SdeLayerName = spatialExtentInfoDAL.SnapShotLayer;
            dataRegisterInfo.ServerID = spatialExtentInfoDAL.ServerID;
            dataRegisterInfo.Year = spatialExtentInfoDAL.Year;
            dataRegisterInfo.Geometry = spatialExtentInfoDAL.Geometry;
            dataRegisterInfo.ThumbImage = spatialExtentInfoDAL.ThumbImage;
            dataRegisterInfo.DataTime = spatialExtentInfoDAL.DataTime;
            dataRegisterInfo.ObjectTypeName = spatialExtentInfoDAL.ObjectTypeName;
            dataRegisterInfo.RealPackageName = spatialExtentInfoDAL.RealPackageName;
            dataRegisterInfo.DataPackageID = spatialExtentInfoDAL.DataPackageID;
            dataRegisterInfo.OrderNum = spatialExtentInfoDAL.OrderNum;
            dataRegisterInfo.OrderDW = spatialExtentInfoDAL.OrderDW;
            dataRegisterInfo.LayerName = spatialExtentInfoDAL.LayerName;
            return dataRegisterInfo;
        }

        public static SpatialExtentInfoDAL Translate(RegisterInfo registerInfo)
        {
            DataRegisterInfo dataRegisterInfo = (DataRegisterInfo)registerInfo;
            SpatialExtentInfoDAL spatialExtentInfoDAL = new SpatialExtentInfoDAL(dataRegisterInfo.layerName);
            spatialExtentInfoDAL.DataID = dataRegisterInfo.ID;
            spatialExtentInfoDAL.CatalogID = dataRegisterInfo.CatalogID;
            spatialExtentInfoDAL.DataDesc = dataRegisterInfo.DataDescription;
            spatialExtentInfoDAL.DataName = dataRegisterInfo.DataName;
            spatialExtentInfoDAL.CatalogType = dataRegisterInfo.CatalogType;
            spatialExtentInfoDAL.DataSize = dataRegisterInfo.DataSize;
            spatialExtentInfoDAL.DataUnit = dataRegisterInfo.DataSizeUnit;
            spatialExtentInfoDAL.Flag = (int)dataRegisterInfo.Flag;
            spatialExtentInfoDAL.Geography = dataRegisterInfo.Geography;
            spatialExtentInfoDAL.ImportDate = dataRegisterInfo.ImportDate;
            spatialExtentInfoDAL.ImportUser = dataRegisterInfo.ImportUser;
            spatialExtentInfoDAL.KeyWord = dataRegisterInfo.Keywords;
            spatialExtentInfoDAL.MetaID = dataRegisterInfo.MetaID;
            spatialExtentInfoDAL.MetaTableName = dataRegisterInfo.MetaTableName;
            //spatialExtentInfoDAL.XMin = dataRegisterInfo.MinX;
            //spatialExtentInfoDAL.YMin = dataRegisterInfo.MinY;
            //spatialExtentInfoDAL.XMax = dataRegisterInfo.MaxX;
            //spatialExtentInfoDAL.YMax = dataRegisterInfo.MaxY;
            spatialExtentInfoDAL.IndexLayer = dataRegisterInfo.IndexLayer;
            spatialExtentInfoDAL.SnapShotLayer = dataRegisterInfo.SnapLayer;
            spatialExtentInfoDAL.Resolution = dataRegisterInfo.Resolution;
            spatialExtentInfoDAL.Scale = dataRegisterInfo.Scale;
            spatialExtentInfoDAL.ServerID = dataRegisterInfo.ServerID;
            spatialExtentInfoDAL.Year = dataRegisterInfo.Year;
            spatialExtentInfoDAL.Geometry = dataRegisterInfo.Geometry;
            spatialExtentInfoDAL.ThumbImage = dataRegisterInfo.ThumbImage;
            spatialExtentInfoDAL.DataTime = dataRegisterInfo.DataTime;
            spatialExtentInfoDAL.ObjectTypeName = dataRegisterInfo.ObjectTypeName;
            spatialExtentInfoDAL.RealPackageName = dataRegisterInfo.RealPackageName;
            spatialExtentInfoDAL.DataPackageID = dataRegisterInfo.DataPackageID;
            spatialExtentInfoDAL.OrderNum = dataRegisterInfo.OrderNum;
            spatialExtentInfoDAL.OrderDW = dataRegisterInfo.OrderDW;
            return spatialExtentInfoDAL;
        }

        /// <summary>
        /// 创建注册信息图层
        /// </summary>
        /// <param name="pSR">空间参考</param>
        /// <returns></returns>
        public static IFeatureClass CreateRegisterLayer(string layerName, ISpatialReference pSR)
        {
            SpatialExtentInfoDAL dal = new SpatialExtentInfoDAL();
            dal.CONST_INDEX_NAME = layerName;
            return dal.CreateRegisterLayer(pSR);
        }

        /// <summary>
        /// 获取数据包内所有可见数据
        /// </summary>
        /// <param name="layerName">注册图层名称</param>
        /// <param name="packageID">数据包ID</param>
        /// <returns></returns>
        public static DataTable GetDataPackageContent(string layerName, int packageID)
        {
            string filter = string.Format("{0}={1} AND {2}={3}",
                                          DataRegisterInfo.STATIC_F_PACKAGEID, packageID,
                                          DataRegisterInfo.STATIC_F_FLAG, (int)EnumObjectState.Normal);
            return DataRegisterInfo.DoQuery(layerName, filter);
        }

        /// <summary>
        /// 获取数据包内所有数据
        /// </summary>
        /// <param name="layerName">注册图层名称</param>
        /// <param name="packageID">数据包ID</param>
        /// <returns></returns>
        public static DataTable GetDataPackageContent(string layerName, int packageID, bool isAll)
        {
            string filter = "";
            if (isAll)
            {
                filter = string.Format("{0}={1}",
                                   DataRegisterInfo.STATIC_F_PACKAGEID, packageID);
            }
            else
            {
                filter = string.Format("{0}={1} AND {2}={3}",
                                      DataRegisterInfo.STATIC_F_PACKAGEID, packageID,
                                      DataRegisterInfo.STATIC_F_FLAG, (int)EnumObjectState.Normal);
            }
            return DataRegisterInfo.DoQuery(layerName, filter);
        }

        public static bool IsRegisterLayer(string layername)
        {
            
            
            //string sql = CatalogNodeDAL.FLD_NAME_F_TARGETREGISTERLAYERNAME + "=" + "'" + layername + "'";
            //DataTable temp = CatalogNodeDAL.Singleton.DoQuery(sql);
            //return temp != null && temp.Rows.Count > 0;
            return true;
        }
        #endregion

        #region 私有函数
        private static SpatialExtentInfoDAL Translate(Geoway.Archiver.Utility.Class.RegisterKey rk)
        {
            SpatialExtentInfoDAL se = new SpatialExtentInfoDAL(rk.MetaTableName);
            return se.Select(rk.DataID);
        }

        public static IDictionary<string,IList<int>> PreProcess(IList<Geoway.Archiver.Utility.Class.RegisterKey> dataIDs)
        {
            IDictionary<string, IList<int>> endIDs = new Dictionary<string, IList<int>>();

            bool isExist = false;
            foreach (Geoway.Archiver.Utility.Class.RegisterKey rk in dataIDs)
            {
                isExist = false;
                foreach(KeyValuePair<string,IList<int>> vp in endIDs)
                {
                    if (rk.MetaTableName == vp.Key)
                    {
                        vp.Value.Add(rk.DataID);
                        isExist = true;
                    } 
                }
                if (!isExist)
                {
                    IList<int> ids = new List<int>();
                    ids.Add(rk.DataID);
                    endIDs.Add(rk.MetaTableName, ids);
                }
            }
            return endIDs;

        }
        #endregion

    }

    //public class RegisterKey:IComparable<RegisterKey>
    //{
    //    int dataID;
    //    public int DataID
    //    {
    //        get { return dataID; }
    //        set { dataID = value; }
    //    }

    //    string layerName;
    //    public string LayerName
    //    {
    //        get { return layerName; }
    //        set { layerName = value; }
    //    }
    //    public RegisterKey(int dataid, string layername)
    //    {
    //        this.dataID = dataid;
    //        this.layerName = layername;
    //    }


    //    #region IComparable<RegisterKey> 成员

    //    public int CompareTo(RegisterKey other)
    //    {
    //        if (this.dataID == other.DataID && this.layerName == other.LayerName)
    //        {
    //            return 0;
    //        }
    //        return -1;
    //    }

    //    #endregion

    //    public override bool Equals(object obj)
    //    {
    //        RegisterKey tmp = obj as RegisterKey;
    //        if (tmp == null)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return this.dataID == tmp.DataID && this.layerName == tmp.LayerName;
    //        }
    //    }
    //}
}
