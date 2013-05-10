using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using System.Windows.Forms;
using Geoway.ADF.MIS.CatalogDataModel.Public.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.GIS.Utility;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve
{

    /// <summary>
    /// 空间范围信息登记表数据库操作类

    /// 提供功能：新增、删除、更新、查询（属性、空间）
    /// </summary>
    [Serializable]
    public class SpatialExtentInfoDAL
    {
        #region 数据库结构

        public string CONST_INDEX_NAME = "DataRegisterInfo";

        public const string FLD_NAME_F_DATAID = "OBJECTID";
        public const string FLD_NAME_F_DATANAME = "F_DATANAME";
        public const string FLD_NAME_F_CATALOGID = "F_CATALOGID";   //待处理

        public const string FLD_NAME_F_CATALOGTYPE = "F_CATALOGTYPE";
        public const string FLD_NAME_F_GEOGRAPHY = "F_GEOGRAPHY";
        public const string FLD_NAME_F_SERVERID = "F_SERVERID";
        public const string FLD_NAME_F_IMPORTUSER = "F_IMPORTUSER";
        public const string FLD_NAME_F_IMPORTDATE = "F_IMPORTDATE";
        public const string FLD_NAME_F_DATASIZE = "F_DATASIZE";
        public const string FLD_NAME_F_DATAUNIT = "F_DATAUNIT";
        public const string FLD_NAME_F_DATADESC = "F_DATADESC";
        public const string FLD_NAME_F_KEYWORD = "F_KEYWORD";
        public const string FLD_NAME_F_FLAG = "F_FLAG";
        public const string FLD_NAME_F_THUMIMGE = "F_THUMIMGE";
        public const string FLD_NAME_F_YEAR = "F_YEAR";
        public const string FLD_NAME_F_SCALE = "F_SCALE";
        public const string FLD_NAME_F_RESOLUTION = "F_RESOLUTION";
        public const string FLD_NAME_F_YMAX = "F_YMAX";
        public const string FLD_NAME_F_YMIN = "F_YMIN";
        public const string FLD_NAME_F_XMAX = "F_XMAX";
        public const string FLD_NAME_F_XMIN = "F_XMIN";
        public const string FLD_NAME_F_METATABLENAME = "F_METATABLENAME";
        public const string FLD_NAME_F_METAID = "F_METAID";
        public const string FLD_NAME_F_HASSNAPSHOT = "F_HASSNAPSHOT";
        public const string FLD_NAME_F_SNAPSHOTLAYER = "F_SNAPSHOTLAYER";
        public const string FLD_NAME_F_DELETETIME = "F_DELETETIME";
        public const string FLD_NAME_F_SDEINDEXLAYER = "F_SDEINDEXLAYER";
        public const string FLD_NAME_F_EXTENT = "F_EXTENT";
        public const string FLD_NAME_F_OBJECTTYPENAME = "F_OBJECTTYPENAME";
        public const string FLD_NAME_F_DATATIME = "F_DATATIME";
        public const string FLD_NAME_F_REALPKGNAME = "F_REALPKGNAME";
        public const string FLD_NAME_F_PACKAGEID = "F_PACKAGEID";
        public const string FLD_NAME_F_ORDERNUM = "F_ORDERNUM";
        public const string FLD_NAME_F_ORDERDW = "F_ORDERDW";  //订购单位  lhy20110617
        public const string FLD_NAME_F_ISDISPLAY = "F_ISDISPLAY"; //标示是否需要显示


        public const int CONST_CONTENT_MAXLENGHT = 4000;
        #endregion

        public SpatialExtentInfoDAL()
        { }

        public SpatialExtentInfoDAL(string layername)
        {
            CONST_INDEX_NAME = layername;
        }

        #region 单例模式
        //public static SpatialExtentInfoDAL Singleton = new SpatialExtentInfoDAL(CONST_INDEX_NAME);
        #endregion

        #region 属性

        static IWorkspace _targetWorkSpace;

        public static IWorkspace TargetWorkSpace
        {
            get { return SpatialExtentInfoDAL._targetWorkSpace; }
            set { SpatialExtentInfoDAL._targetWorkSpace = value; }
        }

        static string sdeUser;

        public static string SDEUser
        {
            get { return SpatialExtentInfoDAL.sdeUser; }
            set { SpatialExtentInfoDAL.sdeUser = value; }
        }

        public string LayerName
        {
            set
            {
                CONST_INDEX_NAME = value;
            }
            get
            {
                return CONST_INDEX_NAME;
            }
        }

        int _dataID = -1;
        /// <summary>
        /// 数据标识
        /// </summary>
        public int DataID
        {
            get { return _dataID; }
            set { _dataID = value; }
        }

        string _dataName = string.Empty;
        /// <summary>
        /// 数据名称
        /// </summary>
        public string DataName
        {
            get { return _dataName; }
            set { _dataName = value; }
        }

        int _catalogID = -1;
        /// <summary>
        /// 树节点ID
        /// </summary>
        public int CatalogID
        {
            get { return _catalogID; }
            set { _catalogID = value; }
        }

        string _geography = string.Empty;

        public string Geography
        {
            get { return _geography; }
            set { _geography = value; }
        }

        int _serverID = -1;
        /// <summary>
        /// 存储节点
        /// </summary>
        public int ServerID
        {
            get { return _serverID; }
            set { _serverID = value; }
        }

        string _indexLayer = string.Empty;
        /// <summary>
        /// 接图表图层

        /// </summary>
        public string IndexLayer
        {
            get { return _indexLayer; }
            set { _indexLayer = value; }
        }

        string _importUser = string.Empty;
        /// <summary>
        /// 入库用户
        /// </summary>
        public string ImportUser
        {
            get { return _importUser; }
            set { _importUser = value; }
        }

        DateTime _importDate = DBHelper.GlobalDBHelper.getServerDate();
        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime ImportDate
        {
            get { return _importDate; }
            set { _importDate = value; }
        }

        private Int64 _dataSize = 0;
        /// <summary>
        /// 数据量

        /// </summary>
        public Int64 DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        string _dataUnit = string.Empty;
        /// <summary>
        /// 数据单位(Byte)
        /// </summary>
        public string DataUnit
        {
            get { return _dataUnit; }
            set { _dataUnit = value; }
        }

        string _dataDesc = string.Empty;
        /// <summary>
        /// 数据描述
        /// </summary>
        public string DataDesc
        {
            get { return _dataDesc; }
            set { _dataDesc = value; }
        }

        string _keyWord = string.Empty;
        /// <summary>
        /// 关键字

        /// </summary>
        public string KeyWord
        {
            get { return _keyWord; }
            set { _keyWord = value; }
        }

        int _flag = 1;
        /// <summary>
        /// 标记（0---正常；1---删除）

        /// </summary>
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        int _year = 0;
        /// <summary>
        /// 年代
        /// </summary>
        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        string _scale = string.Empty;
        /// <summary>
        /// 比例尺

        /// </summary>
        public string Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        string _resolution = string.Empty;
        /// <summary>
        /// 分辨率

        /// </summary>
        public string Resolution
        {
            get { return _resolution; }
            set { _resolution = value; }
        }

        double _yMax = 0;
        /// <summary>
        /// 范围
        /// </summary>
        public double YMax
        {
            get { return _yMax; }
            set { _yMax = value; }
        }

        double _yMin = 0;
        /// <summary>
        /// 范围
        /// </summary>
        public double YMin
        {
            get { return _yMin; }
            set { _yMin = value; }
        }

        double xMax = 0;
        /// <summary>
        /// 范围
        /// </summary>
        public double XMax
        {
            get { return xMax; }
            set { xMax = value; }
        }

        double _xMin = 0;
        /// <summary>
        /// 范围
        /// </summary>
        public double XMin
        {
            get { return _xMin; }
            set { _xMin = value; }
        }

        int _catalogType = 0;
        /// <summary>
        /// 树

        /// </summary>
        public int CatalogType
        {
            get { return _catalogType; }
            set { _catalogType = value; }
        }

        string _metaTableName = string.Empty;
        /// <summary>
        /// 元数据表
        /// </summary>
        public string MetaTableName
        {
            get { return _metaTableName; }
            set { _metaTableName = value; }
        }

        int _metaID = 0;
        /// <summary>
        /// 元数据

        /// </summary>
        public int MetaID
        {
            get { return _metaID; }
            set { _metaID = value; }
        }

        DateTime deleteTime;
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteTime
        {
            get { return deleteTime; }
            set { deleteTime = value; }
        }

        int _hasSnapShot = 0;

        public int HasSnapShot
        {
            get { return _hasSnapShot; }
            set { _hasSnapShot = value; }
        }

        string _snapShotLayer = string.Empty;
        /// <summary>
        /// 快视图目录

        /// </summary>
        public string SnapShotLayer
        {
            get { return _snapShotLayer; }
            set { _snapShotLayer = value; }
        }

        //IEnvelope _extent;
        ///// <summary>
        ///// 四角范围，采用F_XMIN,F_XMAX,F_YMIN,F_YMAX属性字段存储

        ///// </summary>
        //public IEnvelope Extent
        //{
        //    get
        //    {
        //        if (_extent == null)
        //        {
        //            _extent = ConstructExtent();
        //        }
        //        return _extent;
        //    }
        //    set
        //    {
        //        _extent = value;
        //        if (_extent != null)
        //        {
        //            _xMin = _extent.XMin;
        //            _yMax = _extent.YMax;
        //            xMax = _extent.XMax;
        //            _yMin = _extent.YMin;
        //        }
        //    }
        //}

        IGeometry _geometry;
        /// <summary>
        /// 数据真实范围
        /// </summary>
        public IGeometry Geometry
        {
            get { return _geometry; }
            set { _geometry = value; }
        }

        byte[] _thumbImage;  //拇指图

        /// <summary>
        /// 拇指图

        /// </summary>
        public byte[] ThumbImage
        {
            get { return _thumbImage; }
            set { _thumbImage = value; }
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

        int _orderNum = 0;

        public int OrderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
        }
        string _orderDW = "";

        public string OrderDW
        {
            get { return _orderDW; }
            set { _orderDW = value; }
        }
        private int _isdisplay = 0;

        public int IsDisplay
        {
            get { return _isdisplay; }
            set { _isdisplay = value; }
        }

        IFeatureClass _indexFeatureClass;
        public IFeatureClass IndexFeatureClass
        {
            get
            {
                if (_indexFeatureClass == null)
                {
                    _indexFeatureClass = GetIndexFeatureClass();
                }
                return _indexFeatureClass;
            }
        }

        #endregion

        #region 基本操作函数
        public bool Insert()
        {
            if (_indexFeatureClass == null)
            {
                _indexFeatureClass = GetIndexFeatureClass();
            }
            //if (IndexFeatureClass == null)
            //    return false;

            if (BeginLayerEdit(IndexFeatureClass) == false)
                return false;
            try
            {
                IFeature feature = IndexFeatureClass.CreateFeature();
                if (_geometry != null && _geometry.IsEmpty != true)
                {
                    //IEnvelope pEnv = _geometry.Envelope;
                    //IPolygon pPolygon = new PolygonClass();
                    //IPointCollection pPointCol = pPolygon as IPointCollection;
                    //System.Object p1 = System.Reflection.Missing.Value;
                    //IPoint pPoint1 = new ESRI.ArcGIS.Geometry.PointClass();
                    //pPoint1.PutCoords(pEnv.XMin, pEnv.YMin);
                    //pPointCol.AddPoint(pPoint1, ref p1, ref p1);
                    //IPoint pPoint2 = new ESRI.ArcGIS.Geometry.PointClass();
                    //pPoint2.PutCoords(pEnv.XMin, pEnv.YMax);
                    //pPointCol.AddPoint(pPoint2, ref p1, ref p1);
                    //IPoint pPoint3 = new ESRI.ArcGIS.Geometry.PointClass();
                    //pPoint3.PutCoords(pEnv.XMax, pEnv.YMax);
                    //pPointCol.AddPoint(pPoint3, ref p1, ref p1);
                    //IPoint pPoint4 = new ESRI.ArcGIS.Geometry.PointClass();
                    //pPoint4.PutCoords(pEnv.XMax, pEnv.YMin);
                    //pPointCol.AddPoint(pPoint4, ref p1, ref p1);
                    //ITopologicalOperator pTopo = pPolygon as ITopologicalOperator;
                    //pTopo.Simplify();

                    //feature.Shape = pPolygon;

                    feature.Shape = _geometry;
                }

                //处理属性

                SetFieldValue(feature, FLD_NAME_F_DATANAME, _dataName);
                SetFieldValue(feature, FLD_NAME_F_CATALOGID, _catalogID);
                SetFieldValue(feature, FLD_NAME_F_GEOGRAPHY, _geography);
                SetFieldValue(feature, FLD_NAME_F_SERVERID, _serverID);
                SetFieldValue(feature, FLD_NAME_F_IMPORTUSER, _importUser);
                SetFieldValue(feature, FLD_NAME_F_IMPORTDATE, _importDate);
                SetFieldValue(feature, FLD_NAME_F_DATASIZE, _dataSize);
                SetFieldValue(feature, FLD_NAME_F_DATAUNIT, _dataUnit);
                SetFieldValue(feature, FLD_NAME_F_DATADESC, _dataDesc);
                SetFieldValue(feature, FLD_NAME_F_KEYWORD, _keyWord);
                SetFieldValue(feature, FLD_NAME_F_FLAG, _flag);
                SetFieldValue(feature, FLD_NAME_F_YEAR, _year);
                SetFieldValue(feature, FLD_NAME_F_SCALE, _scale);
                SetFieldValue(feature, FLD_NAME_F_RESOLUTION, _resolution);
                SetFieldValue(feature, FLD_NAME_F_CATALOGTYPE, _catalogType);
                SetFieldValue(feature, FLD_NAME_F_METATABLENAME, _metaTableName);
                SetFieldValue(feature, FLD_NAME_F_METAID, _metaID);
                //SetFieldValue(feature, FLD_NAME_F_DELETETIME, );
                SetFieldValue(feature, FLD_NAME_F_HASSNAPSHOT, _hasSnapShot);
                SetFieldValue(feature, FLD_NAME_F_SNAPSHOTLAYER, _snapShotLayer);
                SetFieldValue(feature, FLD_NAME_F_OBJECTTYPENAME, _objectTypeName);
                SetFieldValue(feature, FLD_NAME_F_DATATIME, _dataTime);
                SetFieldValue(feature, FLD_NAME_F_REALPKGNAME, _realPackageName);
                SetFieldValue(feature, FLD_NAME_F_PACKAGEID, _dataPackageID);
                SetFieldValue(feature, FLD_NAME_F_ISDISPLAY, _isdisplay);
                SetFieldValue(feature, FLD_NAME_F_ORDERNUM, _orderNum);
                SetFieldValue(feature, FLD_NAME_F_ORDERDW, _orderDW);
                feature.Store();
                EndLayerEdit(IndexFeatureClass);

                _dataID = feature.OID;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                EndLayerEdit(IndexFeatureClass);
                return false;
            }
        }

        public bool Delete()
        {
            string whereClause = FLD_NAME_F_DATAID + " = " + _dataID;

            ISpatialFilter pFilter = getSpatialFilter(whereClause, null, esriSpatialRelEnum.esriSpatialRelIntersects);
            IFeatureCursor pCorsor = IndexFeatureClass.Search(pFilter, false);

            IFeature pFeature = pCorsor.NextFeature();
            while (pFeature != null)
            {
                pFeature.Delete();
                pFeature = pCorsor.NextFeature();
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCorsor);
            return true;
        }

        public bool Delete(int dataID)
        {
            string whereClause = FLD_NAME_F_DATAID + " = " + dataID;

            ISpatialFilter pFilter = getSpatialFilter(whereClause, null, esriSpatialRelEnum.esriSpatialRelIntersects);
            IFeatureCursor pCorsor = IndexFeatureClass.Search(pFilter, false);

            IFeature pFeature = pCorsor.NextFeature();
            while (pFeature != null)
            {
                pFeature.Delete();
                pFeature = pCorsor.NextFeature();
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCorsor);
            return true;
        }

        public bool Update()
        {
            string whereClause = FLD_NAME_F_DATAID + " = " + _dataID;

            ISpatialFilter pFilter = getSpatialFilter(whereClause, null, esriSpatialRelEnum.esriSpatialRelIntersects);
            IFeatureCursor pCorsor = IndexFeatureClass.Search(pFilter, false);

            BeginLayerEdit(IndexFeatureClass);
            IFeature feature = pCorsor.NextFeature();
            while (feature != null)
            {
                //(feature as IFeatureProject).Project((IndexFeatureClass as IGeoDataset).SpatialReference);
                feature.Shape = null;
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                ISpatialReference f = (IndexFeatureClass as IGeoDataset).SpatialReference;
                (IndexFeatureClass as IGeoDataset).SpatialReference.GetDomain(out x1, out y1, out x2, out y2);
                if (_geometry != null && _geometry.IsEmpty == false)
                {
                    feature.Shape = _geometry;
                }

                //处理属性
                SetFieldValue(feature, FLD_NAME_F_DATANAME, _dataName);
                SetFieldValue(feature, FLD_NAME_F_CATALOGID, _catalogID);
                SetFieldValue(feature, FLD_NAME_F_GEOGRAPHY, _geography);
                SetFieldValue(feature, FLD_NAME_F_SERVERID, _serverID);
                SetFieldValue(feature, FLD_NAME_F_IMPORTUSER, _importUser);
                SetFieldValue(feature, FLD_NAME_F_IMPORTDATE, _importDate);
                SetFieldValue(feature, FLD_NAME_F_DATAUNIT, _dataUnit);
                SetFieldValue(feature, FLD_NAME_F_DATASIZE, _dataSize);
                SetFieldValue(feature, FLD_NAME_F_DATADESC, _dataDesc);
                SetFieldValue(feature, FLD_NAME_F_KEYWORD, _keyWord);
                SetFieldValue(feature, FLD_NAME_F_FLAG, _flag);
                SetFieldValue(feature, FLD_NAME_F_YEAR, _year);
                SetFieldValue(feature, FLD_NAME_F_SCALE, _scale);
                SetFieldValue(feature, FLD_NAME_F_RESOLUTION, _resolution);
                SetFieldValue(feature, FLD_NAME_F_CATALOGTYPE, _catalogType);
                SetFieldValue(feature, FLD_NAME_F_METATABLENAME, _metaTableName);
                SetFieldValue(feature, FLD_NAME_F_METAID, _metaID);
                //SetFieldValue(feature, FLD_NAME_F_DELETETIME, );
                SetFieldValue(feature, FLD_NAME_F_HASSNAPSHOT, _hasSnapShot);
                SetFieldValue(feature, FLD_NAME_F_SNAPSHOTLAYER, _snapShotLayer);
                SetFieldValue(feature, FLD_NAME_F_OBJECTTYPENAME, _objectTypeName);
                SetFieldValue(feature, FLD_NAME_F_DATATIME, _dataTime);
                SetFieldValue(feature, FLD_NAME_F_REALPKGNAME, _realPackageName);
                SetFieldValue(feature, FLD_NAME_F_PACKAGEID, _dataPackageID);
                SetFieldValue(feature, FLD_NAME_F_ISDISPLAY, _isdisplay);
                SetFieldValue(feature, FLD_NAME_F_ORDERNUM, _orderNum);
                SetFieldValue(feature, FLD_NAME_F_ORDERDW, _orderDW);

                feature.Store();
                feature = pCorsor.NextFeature();
            }
            EndLayerEdit(IndexFeatureClass);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCorsor);
            return true;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="dataIDs"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public bool UpdateState(IList<int> dataIDs, EnumObjectState newState)
        {
            string whereClause = SqlUtil.GetORFilterFromList(FLD_NAME_F_DATAID, dataIDs);
            IFeatureCursor cursor = Select(whereClause);
            IFeature feature = cursor.NextFeature();
            while (feature != null)
            {
                SetFieldValue(feature, FLD_NAME_F_FLAG, (int)newState);
                feature.Store();
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);
                feature = cursor.NextFeature();
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cursor);
            return true;
        }

        /// <summary>
        /// 更新存储节点ID
        /// </summary>
        /// <param name="dataID">数据ID</param>
        /// <param name="newServerID">新存储节点ID</param>
        /// <returns></returns>
        public bool UpdateServerID(int dataID, int newServerID)
        {
            string whereClause = FLD_NAME_F_DATAID + "=" + dataID;
            IFeatureCursor cursor = Select(whereClause);
            IFeature feature = cursor.NextFeature();
            while (feature != null)
            {
                SetFieldValue(feature, FLD_NAME_F_SERVERID, newServerID);
                feature.Store();
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);
                feature = cursor.NextFeature();
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cursor);
            return true;
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <returns>所有数据列表</returns>
        public IList<SpatialExtentInfoDAL> Select()
        {
            IList<SpatialExtentInfoDAL> pList = new List<SpatialExtentInfoDAL>();
            IFeatureCursor resultCursor = SelectByCursor();
            pList = Translate(resultCursor);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(resultCursor);
            return pList;
        }

        public bool UpdateDisplay(IList<int> dataIDs, bool isDisplay)
        {
            int display = isDisplay == true ? 1 : 0;
            int undisplay = isDisplay == true ? 0 : 1;

            IFeatureCursor pCorsor = IndexFeatureClass.Search(null, false);

            BeginLayerEdit(IndexFeatureClass);
            int dataID = -1;
            IFeature feature = pCorsor.NextFeature();
            while (feature != null)
            {
                dataID = GetInt32Value(feature, FLD_NAME_F_DATAID);
                if (dataIDs.Contains(dataID))
                {
                    SetFieldValue(feature, FLD_NAME_F_ISDISPLAY, display);
                }
                else
                {
                    SetFieldValue(feature, FLD_NAME_F_ISDISPLAY, undisplay);
                }
                feature.Store();
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);
                feature = pCorsor.NextFeature();
            }
            EndLayerEdit(IndexFeatureClass);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCorsor);
            return true;
        }

        public bool UpdateOrderNum(IList<int> dataIDs, string orderDW)
        {
            string whereClause = SqlUtil.GetORFilterFromList(FLD_NAME_F_DATAID, dataIDs);
            IFeatureCursor cursor = Select(whereClause);
            IFeature feature = cursor.NextFeature();
            int orderNum = 0;
            string tempDW = "";
            while (feature != null)
            {
                orderNum = GetInt32Value(feature, FLD_NAME_F_ORDERNUM);
                orderNum++;
                SetFieldValue(feature, FLD_NAME_F_ORDERNUM, orderNum);
                tempDW = GetStringValue(feature, FLD_NAME_F_ORDERDW);
                if (orderDW != "")
                {
                    if (tempDW == "")
                    {
                        tempDW = orderDW;
                        SetFieldValue(feature, FLD_NAME_F_ORDERDW, tempDW);
                    }
                    else
                    {
                        if (!tempDW.Contains(orderDW))
                        {
                            tempDW += "," + orderDW;
                            SetFieldValue(feature, FLD_NAME_F_ORDERDW, tempDW);
                        }
                    }
                }
                feature.Store();
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);
                feature = cursor.NextFeature();
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cursor);
            return true;
        }
        #endregion

        #region 扩展属性查询

        public SpatialExtentInfoDAL Select(int dataID)
        {
            IList<SpatialExtentInfoDAL> pList = new List<SpatialExtentInfoDAL>();

            string strFilter = FLD_NAME_F_DATAID + " = " + dataID;
            IFeatureCursor resultCursor = Select(strFilter);
            pList = Translate(resultCursor);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(resultCursor);
            if (pList.Count > 0)
            {
                return pList[0];
            }
            else
                return null;

        }

        public IFeature Select2(int dataID)
        {
            IFeature feature = null;
            string strFilter = FLD_NAME_F_DATAID + " = " + dataID;
            IFeatureCursor resultCursor = Select(strFilter);
            feature = resultCursor.NextFeature();
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(resultCursor);
            return feature;
        }

        public IFeatureCursor SelectByCursor()
        {
            IFeatureCursor resultCursor = null;
            resultCursor = IndexFeatureClass.Search(null, false);
            return resultCursor;
        }

        public IFeatureCursor Select(string strFilter)
        {
            IFeatureCursor resultCursor = null;
            ISpatialFilter pFilter = getSpatialFilter(strFilter, null, esriSpatialRelEnum.esriSpatialRelIntersects);
            resultCursor = IndexFeatureClass.Search(pFilter, false);
            return resultCursor;
        }

        public IList<SpatialExtentInfoDAL> Select2(string strFilter)
        {
            IFeatureCursor pFeatureCursor = Select(strFilter);
            IList<SpatialExtentInfoDAL> result = Translate(pFeatureCursor);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);
            return result;
        }

        public byte[] GetThumeImage(int dataID)
        {
            string sql = FLD_NAME_F_DATAID + "=" + dataID.ToString();
            byte[] bytes = null;
            IFeatureCursor pFeatures = Select(sql);
            IFeature feature = pFeatures.NextFeature();
            if (feature != null)
            {
                bytes = GetByteValue(feature, FLD_NAME_F_THUMIMGE);
            }
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatures);
            return bytes;
        }

        public bool SaveThumbImage(int dataID, byte[] stream)
        {
            try
            {
                string sql = FLD_NAME_F_DATAID + "=" + dataID.ToString();

                IFeatureCursor pFeatures = Select(sql);
                IFeature feature = pFeatures.NextFeature();
                if (feature != null)
                {
                    IMemoryBlobStream2 pBlobStream = new MemoryBlobStreamClass();
                    pBlobStream.ImportFromMemory(ref stream[0], (uint)stream.Length);
                    IMemoryBlobStreamVariant pBlobVariant = pBlobStream as IMemoryBlobStreamVariant;
                    SetFieldValue(feature, FLD_NAME_F_THUMIMGE, pBlobVariant);
                    feature.Store();
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatures);
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
            return true;
        }

        public bool SaveThumbImage(int dataID, string streamFile)
        {
            try
            {
                string sql = FLD_NAME_F_DATAID + "=" + dataID.ToString();

                IFeatureCursor pFeatures = Select(sql);
                IFeature feature = pFeatures.NextFeature();
                if (feature != null)
                {
                    IMemoryBlobStream2 pBlobStream = new MemoryBlobStreamClass();
                    pBlobStream.LoadFromFile(streamFile);
                    IMemoryBlobStreamVariant pBlobVariant = pBlobStream as IMemoryBlobStreamVariant;
                    SetFieldValue(feature, FLD_NAME_F_THUMIMGE, pBlobVariant);
                    feature.Store();

                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBlobStream);
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatures);
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
            return true;
        }

        public DataTable DoQuery(string strFilter)
        {

            DataTable dt = CreateQueryView();
            try
            {
                IFeatureCursor pFeatureCursor = Select(strFilter);
                IFeature feature = pFeatureCursor.NextFeature();
                DataRow dr = null;
                while (feature != null)
                {
                    dr = dt.NewRow();
                    CreateQueryRow(feature, ref dr);
                    dt.Rows.Add(dr);

                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);

                    feature = pFeatureCursor.NextFeature();
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);
            }
            catch (System.Exception ex)
            {

            }

            return dt;

        }

        internal DataTable DoQuery(string filter, IGeometry geometry)
        {
            DataTable dt = CreateQueryView();
            try
            {
                IFeatureCursor pFeatureCursor = Select(filter, geometry, esriSpatialRelEnum.esriSpatialRelIntersects);

                IFeature feature = pFeatureCursor.NextFeature();
                DataRow dr = null;
                while (feature != null)
                {
                    dr = dt.NewRow();
                    CreateQueryRow(feature, ref dr);
                    dt.Rows.Add(dr);

                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);
                    feature = pFeatureCursor.NextFeature();
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);
            }
            catch (System.Exception ex)
            {

            }

            return dt;
        }

        public static DataTable CreateQueryView()
        {
            DataTable dt = new DataTable();

            DataColumn pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_DATAID;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            //dt.PrimaryKey = new DataColumn[] { pCol };
            pCol = null;


            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_CATALOGID;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_CATALOGTYPE;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_DATANAME;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_GEOGRAPHY;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;


            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_KEYWORD;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_RESOLUTION;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_SCALE;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_SERVERID;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_YEAR;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_DATASIZE;
            pCol.DataType = Type.GetType("System.Double");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_IMPORTUSER;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_IMPORTDATE;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_OBJECTTYPENAME;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_REALPKGNAME;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_DATATIME;
            pCol.DataType = Type.GetType("System.DateTime");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_PACKAGEID;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_FLAG;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_ORDERNUM;
            pCol.DataType = Type.GetType("System.Int32");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_ORDERDW;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_SNAPSHOTLAYER;
            pCol.DataType = Type.GetType("System.String");
            dt.Columns.Add(pCol);
            pCol = null;

            pCol = new DataColumn();
            pCol.ColumnName = FLD_NAME_F_THUMIMGE;
            pCol.DataType = typeof(byte[]);
            dt.Columns.Add(pCol);
            pCol = null;

            return dt;

        }

        public void CreateQueryRow(IFeature pFeature, ref DataRow dr)
        {
            try
            {
                dr[FLD_NAME_F_DATAID] = GetInt32Value(pFeature, FLD_NAME_F_DATAID);
                dr[FLD_NAME_F_CATALOGID] = GetInt32Value(pFeature, FLD_NAME_F_CATALOGID);
                dr[FLD_NAME_F_CATALOGTYPE] = GetInt32Value(pFeature, FLD_NAME_F_CATALOGTYPE);
                dr[FLD_NAME_F_DATANAME] = GetStringValue(pFeature, FLD_NAME_F_DATANAME);
                dr[FLD_NAME_F_GEOGRAPHY] = GetStringValue(pFeature, FLD_NAME_F_GEOGRAPHY);
                dr[FLD_NAME_F_KEYWORD] = GetStringValue(pFeature, FLD_NAME_F_KEYWORD);
                dr[FLD_NAME_F_RESOLUTION] = GetStringValue(pFeature, FLD_NAME_F_RESOLUTION);
                dr[FLD_NAME_F_SERVERID] = GetStringValue(pFeature, FLD_NAME_F_SERVERID);
                dr[FLD_NAME_F_SCALE] = GetStringValue(pFeature, FLD_NAME_F_SCALE);
                dr[FLD_NAME_F_YEAR] = GetInt32Value(pFeature, FLD_NAME_F_YEAR);
                dr[FLD_NAME_F_DATASIZE] = GetDoubleValue(pFeature, FLD_NAME_F_DATASIZE);
                dr[FLD_NAME_F_IMPORTUSER] = GetStringValue(pFeature, FLD_NAME_F_IMPORTUSER);
                dr[FLD_NAME_F_IMPORTDATE] = GetStringValue(pFeature, FLD_NAME_F_IMPORTDATE);
                dr[FLD_NAME_F_OBJECTTYPENAME] = GetStringValue(pFeature, FLD_NAME_F_OBJECTTYPENAME);
                dr[FLD_NAME_F_REALPKGNAME] = GetStringValue(pFeature, FLD_NAME_F_REALPKGNAME);
                dr[FLD_NAME_F_DATATIME] = GetDateTimeValue(pFeature, FLD_NAME_F_DATATIME);
                dr[FLD_NAME_F_PACKAGEID] = GetInt32Value(pFeature, FLD_NAME_F_PACKAGEID);
                dr[FLD_NAME_F_FLAG] = GetInt32Value(pFeature, FLD_NAME_F_FLAG);
                dr[FLD_NAME_F_ORDERNUM] = GetInt32Value(pFeature, FLD_NAME_F_ORDERNUM);
                dr[FLD_NAME_F_ORDERDW] = GetStringValue(pFeature, FLD_NAME_F_ORDERDW);
                dr[FLD_NAME_F_SNAPSHOTLAYER] = GetInt32Value(pFeature, FLD_NAME_F_SNAPSHOTLAYER);
                dr[FLD_NAME_F_THUMIMGE] = GetBytesValue(pFeature, FLD_NAME_F_THUMIMGE);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }

        }

        public IList<string> GetMetaDataTableName(IList<int> dataids)
        {
            IList<string> strList = new List<string>();
            string strSQL = string.Empty;
            foreach (int dataid in dataids)
            {
                strSQL += SpatialExtentInfoDAL.FLD_NAME_F_DATAID + "=" + dataid + " or ";
            }
            strSQL = strSQL.TrimEnd(" or ".ToCharArray());

            IList<SpatialExtentInfoDAL> spatialExtentInfoDALList = Select2(strSQL);
            foreach (SpatialExtentInfoDAL spatialExtentInfo in spatialExtentInfoDALList)
            {
                if (!string.IsNullOrEmpty(spatialExtentInfo.MetaTableName) && !strList.Contains(spatialExtentInfo.MetaTableName))
                {
                    strList.Add(spatialExtentInfo.MetaTableName);
                }
            }

            return strList;
        }

        #endregion

        #region 扩展空间查询
        public IFeatureCursor Select(string strFilter, IGeometry queryGeometry, esriSpatialRelEnum SpatialRel)
        {
            IFeatureCursor resultCursor = null;
            ISpatialFilter pFilter = getSpatialFilter(strFilter, queryGeometry, SpatialRel);
            resultCursor = IndexFeatureClass.Search(pFilter, false);

            return resultCursor;
        }
        #endregion

        #region 获取空间范围图层

        /// <summary>
        /// 创建注册信息图层
        /// </summary>
        /// <param name="pSR">空间爱你从空间参考</param>
        /// <returns></returns>
        public IFeatureClass CreateRegisterLayer(ISpatialReference pSR)
        {
            if (_targetWorkSpace != null)
            {
                IFeatureClass fcls = CreateFeatureClass(_targetWorkSpace, pSR);
                return fcls;
            }
            else
            {
                throw new Exception("请调用SetWorkspace方法设置工作空间");
            }
        }

        public IFeatureClass GetIndexFeatureClass()
        {
            if (_indexFeatureClass == null)
            {
                try
                {
                    if (_targetWorkSpace != null)
                    {
                        _indexFeatureClass = (_targetWorkSpace as IFeatureWorkspace).OpenFeatureClass(CONST_INDEX_NAME);
                    }
                    else
                    {
                        throw new Exception("请调用SetWorkspace方法设置工作空间");
                    }
                }
                catch
                {
                    ISpatialReference pSR = SpatialReferenceUtil.CreateGCSSpatialReference(enumDatum.W84);
                    _indexFeatureClass = CreateFeatureClass(_targetWorkSpace, pSR);
                    //throw new Exception("暂时不支持创建");
                }
                return _indexFeatureClass;
            }
            else
            {
                return _indexFeatureClass;
            }

        }

        #endregion

        #region translate

        /// <summary>
        /// 解析
        /// <param name="dtResult">源数据</param>
        /// <returns>实体集合</returns>
        public IList<SpatialExtentInfoDAL> Translate(IFeatureCursor featureCursor)
        {
            IList<SpatialExtentInfoDAL> list = new List<SpatialExtentInfoDAL>();
            try
            {
                if (featureCursor != null)
                {
                    SpatialExtentInfoDAL info = null;
                    IFeature feature = featureCursor.NextFeature();
                    while (feature != null)
                    {
                        info = new SpatialExtentInfoDAL();

                        info.Geometry = feature.ShapeCopy;

                        info.DataID = GetInt32Value(feature, FLD_NAME_F_DATAID);
                        info.DataName = GetStringValue(feature, FLD_NAME_F_DATANAME);
                        info.CatalogID = GetInt32Value(feature, FLD_NAME_F_CATALOGID);
                        info.Geography = GetStringValue(feature, FLD_NAME_F_GEOGRAPHY);
                        info.ServerID = GetInt32Value(feature, FLD_NAME_F_SERVERID);
                        info.ImportUser = GetStringValue(feature, FLD_NAME_F_IMPORTUSER);
                        info.ImportDate = GetDateTimeValue(feature, FLD_NAME_F_IMPORTDATE);
                        info.DataSize = GetInt64Value(feature, FLD_NAME_F_DATASIZE);
                        info.DataUnit = GetStringValue(feature, FLD_NAME_F_DATAUNIT);
                        info.DataDesc = GetStringValue(feature, FLD_NAME_F_DATADESC);
                        info.KeyWord = GetStringValue(feature, FLD_NAME_F_KEYWORD);
                        info.Flag = GetInt32Value(feature, FLD_NAME_F_FLAG);
                        info.Year = GetInt32Value(feature, FLD_NAME_F_YEAR);
                        info.Scale = GetStringValue(feature, FLD_NAME_F_SCALE);
                        info.Resolution = GetStringValue(feature, FLD_NAME_F_RESOLUTION);
                        info.CatalogType = GetInt32Value(feature, FLD_NAME_F_CATALOGTYPE);
                        info.MetaTableName = GetStringValue(feature, FLD_NAME_F_METATABLENAME);
                        info.MetaID = GetInt32Value(feature, FLD_NAME_F_METAID);
                        info.DeleteTime = GetDateTimeValue(feature, FLD_NAME_F_DELETETIME);
                        info.SnapShotLayer = GetStringValue(feature, FLD_NAME_F_SNAPSHOTLAYER);
                        info.HasSnapShot = GetInt32Value(feature, FLD_NAME_F_HASSNAPSHOT);
                        info.IndexLayer = GetStringValue(feature, FLD_NAME_F_SDEINDEXLAYER);
                        //info.ThumbImage = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_DATAID + "=" + info.DataID, CONST_INDEX_NAME, FLD_NAME_F_THUMIMGE);
                        info.ObjectTypeName = GetStringValue(feature, FLD_NAME_F_OBJECTTYPENAME);
                        info.DataTime = GetDateTimeValue(feature, FLD_NAME_F_DATATIME);
                        info.RealPackageName = GetStringValue(feature, FLD_NAME_F_REALPKGNAME);
                        info.DataPackageID = GetInt32Value(feature, FLD_NAME_F_PACKAGEID);
                        info.IsDisplay = GetInt32Value(feature, FLD_NAME_F_ISDISPLAY);
                        info.OrderNum = GetInt32Value(feature, FLD_NAME_F_ORDERNUM);
                        info.OrderDW = GetStringValue(feature, FLD_NAME_F_ORDERDW);
                        info.LayerName = this.LayerName;

                        list.Add(info);

                        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature);

                        feature = featureCursor.NextFeature();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            finally
            {

                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(featureCursor);
            }

            return list;
        }

        public IList<SpatialExtentInfoDAL> Translate(DataTable dtResult)
        {
            IList<SpatialExtentInfoDAL> list = new List<SpatialExtentInfoDAL>();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow pRow = null;
                string sLayers = string.Empty;
                SpatialExtentInfoDAL info = null;
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    pRow = dtResult.Rows[i];
                    info = new SpatialExtentInfoDAL();
                    info.DataID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_DATAID);
                    info.DataName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATANAME);
                    info.CatalogID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_CATALOGID);
                    info.Geography = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_GEOGRAPHY);
                    info.ServerID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_SERVERID);
                    info.ImportUser = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_IMPORTUSER);
                    info.ImportDate = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_IMPORTDATE);
                    info.DataSize = Convert.ToInt64(GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATASIZE));
                    info.DataUnit = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATAUNIT);
                    info.DataDesc = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_DATADESC);
                    info.KeyWord = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_KEYWORD);
                    info.Flag = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_FLAG);
                    info.Year = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_YEAR);
                    info.Scale = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SCALE);
                    info.Resolution = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_RESOLUTION);
                    info.CatalogType = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_CATALOGTYPE);
                    info.MetaTableName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_METATABLENAME);
                    info.MetaID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_METAID);
                    info.DeleteTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_DELETETIME);
                    info.SnapShotLayer = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SNAPSHOTLAYER);
                    info.HasSnapShot = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_HASSNAPSHOT);
                    info.IndexLayer = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_SDEINDEXLAYER);
                    info.ThumbImage = DBHelper.GlobalDBHelper.ReadBlob2Bytes(FLD_NAME_F_DATAID + "=" + info.DataID, CONST_INDEX_NAME, FLD_NAME_F_THUMIMGE);
                    info.ObjectTypeName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_OBJECTTYPENAME);
                    info.DataTime = GetSafeDataUtility.ValidateDataRow_T(pRow, FLD_NAME_F_DATATIME);
                    info.RealPackageName = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_REALPKGNAME);
                    info.DataPackageID = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_PACKAGEID);
                    info.IsDisplay = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ISDISPLAY);
                    info.OrderNum = GetSafeDataUtility.ValidateDataRow_N(pRow, FLD_NAME_F_ORDERNUM);
                    info.OrderDW = GetSafeDataUtility.ValidateDataRow_S(pRow, FLD_NAME_F_ORDERDW);
                    list.Add(info);
                }
            }
            return list;
        }
        #endregion

        #region 内部方法

        private IFeatureClass CreateFeatureClass(IWorkspace pWP, ISpatialReference pSR)
        {
            IFeatureClassDescription fcDescription = null;
            IObjectClassDescription ocDescription = null;
            IGeometryDef geometryDef = null;
            IGeometryDefEdit geometryDefEdit = null;
            IFeatureClass pFeatClass = null;

            IFields pFields = null;
            IFieldsEdit pFieldsEdit = null;

            IField field = null;
            IField pField = null;


            IFields pFlds = null;
            IEnumFieldError pError = null;
            IFieldChecker pCheck = null;

            try
            {
                fcDescription = new FeatureClassDescriptionClass();
                ocDescription = (IObjectClassDescription)fcDescription;
                pFields = ocDescription.RequiredFields;
                int shapeFieldIndex = pFields.FindField(fcDescription.ShapeFieldName);
                field = pFields.get_Field(shapeFieldIndex);
                geometryDef = field.GeometryDef;
                geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                geometryDefEdit.SpatialReference_2 = pSR;

                pFieldsEdit = (IFieldsEdit)pFields;

                pField = CreateField("", FLD_NAME_F_DATANAME, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_CATALOGID, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_GEOGRAPHY, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_SERVERID, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_IMPORTUSER, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_IMPORTDATE, esriFieldType.esriFieldTypeDate, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_DATASIZE, esriFieldType.esriFieldTypeDouble, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_DATAUNIT, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_DATADESC, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_KEYWORD, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_FLAG, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_YEAR, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_SCALE, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_RESOLUTION, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_CATALOGTYPE, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_METATABLENAME, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_METAID, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_DELETETIME, esriFieldType.esriFieldTypeDate, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_SNAPSHOTLAYER, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_HASSNAPSHOT, esriFieldType.esriFieldTypeSmallInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_SDEINDEXLAYER, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_OBJECTTYPENAME, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_DATATIME, esriFieldType.esriFieldTypeDate, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_REALPKGNAME, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_PACKAGEID, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_THUMIMGE, esriFieldType.esriFieldTypeBlob, true);
                pFieldsEdit.AddField(pField);


                pField = CreateField("", FLD_NAME_F_ISDISPLAY, esriFieldType.esriFieldTypeSmallInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_ORDERNUM, esriFieldType.esriFieldTypeInteger, true);
                pFieldsEdit.AddField(pField);

                pField = CreateField("", FLD_NAME_F_ORDERDW, esriFieldType.esriFieldTypeString, true);
                pFieldsEdit.AddField(pField);

                pCheck = new FieldCheckerClass();
                pCheck.InputWorkspace = pWP;
                pCheck.ValidateWorkspace = pWP;

                pCheck.Validate(pFields, out pError, out pFlds);

                IFeatureWorkspace pFeatWKS = pWP as IFeatureWorkspace;
                pFeatClass = pFeatWKS.CreateFeatureClass(CONST_INDEX_NAME, pFlds, ocDescription.InstanceCLSID, ocDescription.ClassExtensionCLSID, esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");


            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            finally
            {
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fcDescription);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geometryDef);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFields);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(field);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlds);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCheck);
            }

            return pFeatClass;

        }

        private ISpatialFilter getSpatialFilter(string whereClause, IGeometry searchGeometry, esriSpatialRelEnum spatialRel)
        {
            if (string.IsNullOrEmpty(whereClause) == true && searchGeometry == null)
                return null;

            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            if (string.IsNullOrEmpty(whereClause) == false)
            {
                pSpatialFilter.WhereClause = whereClause;
            }
            if (searchGeometry != null)
            {
                //统一空间参考


                if (searchGeometry.SpatialReference != null && (searchGeometry.SpatialReference is IUnknownCoordinateSystem) == false)
                {
                    IGeoDataset pGeodataset = IndexFeatureClass as IGeoDataset;
                    ISpatialReference classSpatial = pGeodataset.SpatialReference;
                    if (classSpatial != null && (searchGeometry.SpatialReference is IUnknownCoordinateSystem) == false)
                    {
                        searchGeometry.Project(classSpatial);
                    }
                }
                pSpatialFilter.Geometry = searchGeometry;
                pSpatialFilter.SpatialRel = spatialRel;
            }
            return pSpatialFilter;
        }

        private IEnvelope ConstructExtent()
        {
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(XMin, YMin, XMax, YMax);
            return envelope;
        }

        private string getSelectField()
        {
            string fields = FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_DATANAME + "," +
                FLD_NAME_F_CATALOGID + "," +
                FLD_NAME_F_GEOGRAPHY + "," +
                FLD_NAME_F_SERVERID + "," +
                FLD_NAME_F_KEYWORD + "," +
                FLD_NAME_F_FLAG + "," +
                FLD_NAME_F_RESOLUTION + "," +
                FLD_NAME_F_SCALE + "," +
                FLD_NAME_F_YEAR + "," +
                FLD_NAME_F_DATASIZE + "," +
                FLD_NAME_F_DATAUNIT + "," +
                FLD_NAME_F_DATADESC + "," +
                FLD_NAME_F_YMAX + "," +
                FLD_NAME_F_YMIN + "," +
                FLD_NAME_F_XMAX + "," +
                FLD_NAME_F_XMIN + "," +
                FLD_NAME_F_CATALOGTYPE + "," +
                FLD_NAME_F_METATABLENAME + "," +
                FLD_NAME_F_METAID + "," +
                FLD_NAME_F_DELETETIME + "," +
                FLD_NAME_F_IMPORTUSER + "," +
                FLD_NAME_F_IMPORTDATE + "," +
                FLD_NAME_F_HASSNAPSHOT + "," +
                FLD_NAME_F_SDEINDEXLAYER + "," +
                FLD_NAME_F_SNAPSHOTLAYER + "," +
                FLD_NAME_F_THUMIMGE + "," +
                FLD_NAME_F_OBJECTTYPENAME + "," +
                FLD_NAME_F_DATATIME + "," +
                FLD_NAME_F_REALPKGNAME + "," +
                FLD_NAME_F_ISDISPLAY + "," +
                FLD_NAME_F_ORDERNUM + "," +
                FLD_NAME_F_ORDERDW + "," +
                FLD_NAME_F_PACKAGEID;
            return fields;
        }

        private string GetQueryViewField()
        {
            string fields = FLD_NAME_F_DATAID + "," +
                FLD_NAME_F_DATANAME + "," +
                FLD_NAME_F_CATALOGID + "," +
                FLD_NAME_F_GEOGRAPHY + "," +
                //FLD_NAME_F_SERVERID + "," +
                FLD_NAME_F_KEYWORD + "," +
                //FLD_NAME_F_FLAG + "," +
                FLD_NAME_F_RESOLUTION + "," +
                FLD_NAME_F_SCALE + "," +
                FLD_NAME_F_YEAR + "," +
                FLD_NAME_F_DATASIZE + "," +
                //FLD_NAME_F_DATAUNIT + "," +
                //FLD_NAME_F_DATADESC + "," +
                //FLD_NAME_F_YMAX + "," +
                //FLD_NAME_F_YMIN + "," +
                //FLD_NAME_F_XMAX + "," +
                //FLD_NAME_F_XMIN + "," +
                //FLD_NAME_F_CATALOGTYPE + "," +
                //FLD_NAME_F_METATABLEID + "," +
                //FLD_NAME_F_METAID + "," +
                //FLD_NAME_F_DELETETIME + "," +
                FLD_NAME_F_IMPORTUSER + "," +
                FLD_NAME_F_IMPORTDATE + "," +
                //FLD_NAME_F_HASSNAPSHOT + "," +
                //FLD_NAME_F_SDEINDEXLAYER + "," +
                FLD_NAME_F_SNAPSHOTLAYER + "," +
                //FLD_NAME_F_INDEXRELPATH + "," +
                //FLD_NAME_F_DATAPOSITION + "," +
                //FLD_NAME_F_THUMIMGE + "," +
                FLD_NAME_F_OBJECTTYPENAME + "," +
                FLD_NAME_F_DATATIME + "," +
                FLD_NAME_F_REALPKGNAME + "," +
                FLD_NAME_F_ISDISPLAY + "," +
                FLD_NAME_F_ORDERNUM + "," +
                FLD_NAME_F_ORDERDW + "," +
                FLD_NAME_F_PACKAGEID;
            return fields;
        }

        private bool BeginLayerEdit(IFeatureClass pFClass)
        {
            try
            {
                IDataset pDSet = (IDataset)pFClass;
                IWorkspace pWorkspace = pDSet.Workspace;
                IWorkspaceEdit pWSEdit = (IWorkspaceEdit)pWorkspace;
                if (!pWSEdit.IsBeingEdited())
                {
                    pWSEdit.StartEditing(false);
                }
                if (!pWSEdit.IsBeingEdited())
                {
                    return false;
                }
                pWSEdit.StartEditOperation();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

        private void EndLayerEdit(IFeatureClass pFClass)
        {
            try
            {
                IDataset pDSet = (IDataset)pFClass;
                IWorkspace pWorkspace = pDSet.Workspace;
                IWorkspaceEdit pWSEdit = (IWorkspaceEdit)pWorkspace;
                if (pWSEdit.IsBeingEdited())
                {
                    pWSEdit.StopEditOperation();
                    pWSEdit.StopEditing(true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        private IField CreateField(string CName, string EName, esriFieldType fType, bool IsAllowNull)
        {
            IField pField = null;
            IFieldEdit pFieldEdit = null;
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.AliasName_2 = CName;
            pFieldEdit.Name_2 = EName;
            pFieldEdit.IsNullable_2 = IsAllowNull;
            pFieldEdit.Type_2 = fType;
            return pField;
        }

        private void SetFieldValue(IFeature feature, string fldName, object value)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                feature.set_Value(fldIndex, value);
            }
        }

        private int GetInt32Value(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                object obj = feature.get_Value(fldIndex);
                return Convert.IsDBNull(obj) ? -1 : Convert.ToInt32(obj);
            }
            return -1;
        }

        private byte[] GetBytesValue(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                return Geoway.Archiver.Utility.Class.SerializationUtility.ReadEsriBlobToBytes(feature.get_Value(fldIndex));
            }
            else
            {
                return null;
            }
        }

        private Int64 GetInt64Value(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                object obj = feature.get_Value(fldIndex);
                return Convert.IsDBNull(obj) ? -1 : Convert.ToInt64(obj);
            }
            return -1;
        }

        private double GetDoubleValue(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                object obj = feature.get_Value(fldIndex);
                return Convert.IsDBNull(obj) ? 0.0 : Convert.ToDouble(obj);
            }
            return 0.0;
        }

        private string GetStringValue(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                object obj = feature.get_Value(fldIndex);
                return Convert.IsDBNull(obj) ? "" : obj.ToString();
            }
            return "";
        }

        private DateTime GetDateTimeValue(IFeature feature, string fldName)
        {
            DateTime dt = System.DateTime.MinValue;
            DateTime missing = new DateTime(1899, 12, 30, 0, 0, 0);
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                object obj = feature.get_Value(fldIndex);

                return Convert.IsDBNull(obj) || Convert.ToDateTime(obj) == missing ? dt : Convert.ToDateTime(obj);
            }
            return dt;

        }

        private Byte[] GetByteValue(IFeature feature, string fldName)
        {
            int fldIndex = IndexFeatureClass.FindField(fldName);
            if (fldIndex > -1)
            {
                try
                {
                    object obj = feature.get_Value(fldIndex);
                    if (obj == null || obj == DBNull.Value)
                    {
                        return null;
                    }

                    IMemoryBlobStream2 pBlobStream = new MemoryBlobStreamClass();
                    pBlobStream = (IMemoryBlobStream2)obj;
                    int n = (int)pBlobStream.Size;
                    byte[] defaultBytes = new byte[n];
                    object obj2 = null;
                    IMemoryBlobStreamVariant pBlobVariant = pBlobStream as IMemoryBlobStreamVariant;
                    pBlobVariant.ExportToVariant(out obj2);
                    defaultBytes = (byte[])obj2;

                    return defaultBytes;
                }
                catch (Exception ex)
                {
                    LogHelper.Error.Append(ex);
                }
            }
            return null;

        }
        #endregion


        //#region 扩展为业务表查询方式，目前仅用于统计
        ///// <summary>
        ///// 仅用于统计
        ///// </summary>
        //private static DBHelper _dbTmp = null;
        //public static DBHelper DBonSDE
        //{
        //    get
        //    {
        //        if (_dbTmp == null)
        //        {
        //            InitDB();
        //        }
        //        return _dbTmp;
        //    }
        //}

        //private static void InitDB()
        //{
        //    if (_dbTmp != null)
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        IDataReader reader = DBHelper.GlobalDBHelper.DoQuery("select * from TBSYS_SPatialDS a,TBSYS_SUBSYS_SPDS b,TBSYS_SUBSYSINFO c where a.F_SPDSID=b.F_SPDSID and b.F_SUBSYSID=c.F_SUBSYSID and c.F_MARK=6000");
        //        if (!reader.Read()) return;
        //        _dbTmp = new DBHelper();
        //        DsSDE dsg = null;
        //        int sdeType = Int32.Parse(reader["F_TYPE"].ToString());
        //        switch (sdeType)
        //        {
        //            case 1:
        //                dsg = new DsSDE();
        //                break;
        //            case 2:
        //                break;
        //            default:
        //                dsg = new DsSDE();
        //                break;
        //        }
        //        dsg.Recover(reader["F_Attr"].ToString());
        //        _dbTmp.DBName = dsg.Database;
        //        _dbTmp.DBUser = dsg.User;
        //        _dbTmp.DBServer = dsg.Server;
        //        _dbTmp.DBPwd = ConvertUtil.Decode(dsg.Password);
        //        string type2 = string.Empty;
        //        if (dsg.Instance.IndexOf(":") > 0)
        //            type2 = dsg.Instance.Split(':')[1].ToUpper();
        //        else
        //            type2 = "ORACLE";
        //        _dbTmp.DBDriver = DBHelper.enumDBDriver.ADO;
        //        switch (type2)
        //        {
        //            case "SQLSERVER":
        //                _dbTmp.DBType = DBHelper.enumDBType.DB_SQLServer;
        //                break;
        //            case "ORACLE":
        //                _dbTmp.DBSid = "db10g";
        //                _dbTmp.DBType = DBHelper.enumDBType.DB_Oracle;
        //                _dbTmp.DBDriver = DBHelper.enumDBDriver.DDTek;
        //                break;
        //            default:
        //                break;
        //        }
        //        _dbTmp.connect();
        //        if (!_dbTmp.IsConnected())
        //            _dbTmp = null;

        //        //ConvertUtil.Decode(
        //    }
        //    catch (Exception ex)
        //    {
        //        _dbTmp = null;
        //        LogHelper.Error.Append(ex);
        //    }
        //}

        //#endregion


    }

}
