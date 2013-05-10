using System;
using System.Collections.Generic;
using Geoway.Archiver.Utility.Class;
using ESRI.ArcGIS.Carto;
using Geoway.Archiver.Utility.Definition;
using ESRI.ArcGIS.Geometry;
using Geoway.Archiver.Query.Factory;
using Geoway.Archiver.Query.Interface;
using Geoway.ADF.MIS.DB.Public;
using Geoway.ADF.MIS.Utility.DevExpressEx;
using System.Data;
using Geoway.ADF.MIS.Utility.Log;
using ESRI.ArcGIS.Geodatabase;
using Geoway.Archiver.Modeling.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class RegisterInfoDisplayHelper
    {
        /// <summary>
        /// 注册图层组合图层名称
        /// </summary>
        public static string RegisterLayerGroupLayerName = "RegisterLayer";
        
        /// <summary>
        /// 叠加空间范围(oraclespatial,叠加元素)
        /// </summary>
        public static void OverlaySpatialExtent(IList<RegisterKey> lstRegisterKeys, IMap map)
        {
            DataExtentDisplay(lstRegisterKeys, map, true,"");
        }
        public static void OverlaySpatialExtent(IList<RegisterKey> lstRegisterKeys, IMap map,string nodePath)
        {
            DataExtentDisplay(lstRegisterKeys, map, true,nodePath);
        }
        public static void DisplaySDELayer(IList<RegisterKey> lstRegisterKeys, IMap map, ref IGeometryCollection geoCollection, bool bAddExtent)
        {
            IGroupLayer groupLayer = GetGroupLayer(map);
            if (groupLayer == null)
            {
                return;
            }
            groupLayer.Clear();
            object obj = Type.Missing;
            string sWhere = string.Empty;
            IDictionary<string, IList<int>> dicDatas = DataOper.PreProcess(lstRegisterKeys);
            foreach (KeyValuePair<string, IList<int>> dicItem in dicDatas)
            {
                sWhere = string.Empty;
                foreach (int dataid in dicItem.Value)
                {
                    sWhere += FixedFieldName.FLD_NAME_F_DATAID + "=" + dataid + " or ";
                }
                if (sWhere != string.Empty)
                {
                    sWhere = sWhere.TrimEnd(" or ".ToCharArray());
                }
                ILayer layer = GetLayer(dicItem.Key, sWhere);
                if (layer != null)
                {
                    groupLayer.Add(layer);
                    geoCollection.AddGeometry(layer.AreaOfInterest as IGeometry, ref obj, ref obj);
                }
            }
        }
        /// <summary>
        /// 得到图层
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private static ILayer GetLayer(string layerName, string filter)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return null;
            }

            try
            {
                IFeatureClass pFeatCls = (SysParams.BizWorkspace as IFeatureWorkspace).OpenFeatureClass(layerName);
                IFeatureLayer pFtLayer = new FeatureLayerClass();
                pFtLayer.FeatureClass = pFeatCls;
                pFtLayer.Name = layerName;

                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = filter;

                ((IFeatureSelection)pFtLayer).SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

                ILayer pLayer = ((IFeatureLayerDefinition)pFtLayer).CreateSelectionLayer(layerName, true, "", "");
                
                return pLayer;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }
        public static void DisplaySpatialExtent(IList<RegisterKey> lstRegisterKeys, IMap map, ref IGeometryCollection geoCollection, bool bAddExtent,string nodePath)
        {
            IGeometry geometry;
            object obj = Type.Missing;
            IGraphicsContainer pGrpCon = (IGraphicsContainer)map;
            //遍历对象
            pGrpCon.Reset();
            IElement pEle = pGrpCon.Next();
            while (pEle != null)
            {
                IElementProperties pElePro = (IElementProperties)pEle;
                if (pElePro.Name == "空间范围")
                {
                    pGrpCon.DeleteElement(pEle);
                }
                pEle = pGrpCon.Next();
            }
            ISpatialQuery spatialQuery = QueryFactory.Create(DBHelper.GlobalDBHelper, SysParams.BizWorkspace);
            foreach (RegisterKey key in lstRegisterKeys)
            {
                try
                {
                    geometry = spatialQuery.QueryGeometry(key.DataID);
                    if (geometry != null)
                    {
                        if (bAddExtent)
                        {
                            //if (string.IsNullOrEmpty(nodePath))
                            //{
                            //    DisplayHelper.AddSpatialPolyonOnMap(map, geometry);
                            //}
                            //else
                            //{
                                PropertiesEle proEle = GetPropertiesEle(key, nodePath);
                                DisplayHelper.AddSpatialPolyonOnMap(map, geometry, proEle);
                            //}
                        }
                        geoCollection.AddGeometry(geometry, ref obj, ref obj);
                    }
                }
                catch (Exception ex)
                {
                    //DevMessageUtil.ShowMessageDialog(string.Format("数据{0}不存在空间范围！", key.DataID));
                    LogHelper.Error.Append(ex);
                    return;
                }
            }
        }
        private static PropertiesEle GetPropertiesEle(RegisterKey key,string nodePath)
        {
            PropertiesEle proEle = new PropertiesEle();
            DatumType datumType = DataOper.GetDatumType(DBHelper.GlobalDBHelper, key.DataID);
            proEle.DataTypeName = datumType.Name;
            string info = "";
            string fields = DataOper.GetBiaoshiFields(DBHelper.GlobalDBHelper, datumType);
            DataTable dt = DataOper.GetTableByDataID(DBHelper.GlobalDBHelper, key.MetaTableName, fields, key.DataID);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0][i].ToString())==false)
                    info += dt.Rows[0][i].ToString() + ",";
                }
            }
            info = info.TrimEnd(',');
            if (string.IsNullOrEmpty(nodePath))
            {
                proEle.Info = string.Format("【{0}】", info);
            }
            else
            {
                proEle.Info = string.Format("【{0}】,{1}", info, nodePath);
            }
            return proEle;

        }
        /// <summary>
        /// 缩放到范围
        /// </summary>
        /// <param name="lstRegisterKeys"></param>
        /// <param name="map"></param>
        public static void ZoomToExtent(IList<RegisterKey> lstRegisterKeys, IMap map)
        {
            DataExtentDisplay(lstRegisterKeys, map, false,"");
        }

        public static void ZoomToExtent(IList<RegisterKey> lstRegisterKeys, IMap map, bool isDisplay)
        {
            DataExtentDisplay(lstRegisterKeys, map, isDisplay, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstRegisterKeys"></param>
        /// <param name="map"></param>
        /// <param name="bAddExtent"></param>
        private static void DataExtentDisplay(IList<RegisterKey> lstRegisterKeys, IMap map, bool bAddExtent,string nodePath)
        {
            IGeometryCollection geoCollection = new GeometryBag() as IGeometryCollection;

            //OracleSpatial查询，查询数据范围以元素形式添加到地图控件
            if (SysParams.Para_MetaQueryMode == EnumMetaQueryType.enumOracleSpatial)
            {
                DisplaySpatialExtent(lstRegisterKeys, map, ref geoCollection, bAddExtent, nodePath);
            }
            else//SDE查询，查询图层，根据DataID条件进行过滤后添加到地图控件
            {
                DisplaySDELayer(lstRegisterKeys, map, ref geoCollection, bAddExtent);
            }
            if (geoCollection.GeometryCount > 0)
            {
                DisplayHelper.ExtentToGeo(map, geoCollection);
            }
            else
            {
                DevMessageUtil.ShowMessageDialog(string.Format("数据不存在空间范围！"));
            }
        }
        /// <summary>
        /// 获取组图层
        /// </summary>
        /// <param name="map">地图控件</param>
        /// <returns></returns>
        public static IGroupLayer GetGroupLayer(IMap map)
        {
            IGroupLayer groupLayer = null; //影像叠加图层
            int layerIndex = -1;
            for (int i = 0; i < map.LayerCount; i++)
            {
                if (map.get_Layer(i).Name.CompareTo(RegisterLayerGroupLayerName) == 0)
                {
                    groupLayer = map.get_Layer(i) as IGroupLayer;
                    layerIndex = i;
                    break;
                }
            }
            if (groupLayer == null)
            {
                //初始化快视图组合图层
                groupLayer = new GroupLayerClass();
                groupLayer.Name = RegisterLayerGroupLayerName;
                map.AddLayer(groupLayer as ILayer);
            }
            else
            {
                if (!groupLayer.Visible)
                {
                    groupLayer.Visible = true;
                }
                if (layerIndex > 0)
                {
                    map.MoveLayer(groupLayer, 0);
                }
            }
            return groupLayer;
        }
        /// <summary>
        /// 查看数据覆盖范围
        /// </summary>
        /// <param name="lstRegisterKeys"></param>
        /// <param name="map"></param>
        public static void ViewDataExtentOverlap(IList<RegisterKey> lstRegisterKeys, IMap map)
        {

            IGeometry geometry;
            IGeometry geoUnion = null;
            object obj = Type.Missing;
            IGeometryCollection geoCollection = new GeometryBag() as IGeometryCollection;
            ITopologicalOperator pTopoOper;
            IGraphicsContainer pGrpCon = (IGraphicsContainer)map;
            //遍历对象
            pGrpCon.Reset();
            IElement pEle = pGrpCon.Next();
            while (pEle != null)
            {
                IElementProperties pElePro = (IElementProperties)pEle;
                if (pElePro.Name == "空间范围")
                {
                    pGrpCon.DeleteElement(pEle);
                }
                pEle = pGrpCon.Next();
            }

            foreach (RegisterKey key in lstRegisterKeys)
            {
                try
                {
                    ISpatialQuery spatialQuery = QueryFactory.Create(DBHelper.GlobalDBHelper, SysParams.BizWorkspace);

                    geometry = spatialQuery.QueryGeometry(key.DataID);
                    if (geometry != null)
                    {
                        if (geoUnion == null)
                        {
                            geoUnion = geometry;
                        }
                        else
                        {
                            pTopoOper = (ITopologicalOperator)geoUnion;
                            geoUnion = pTopoOper.Union(geometry);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DevMessageUtil.ShowMessageDialog(string.Format("数据{0}不存在空间范围！", key.DataID));
                    LogHelper.Error.Append(ex);
                    return;
                }
            }
            if (geoUnion!=null)
            {
                geoCollection.AddGeometry(geoUnion, ref obj, ref obj);
                DisplayHelper.AddSpatialPolyonOnMap(map, geoUnion);
                DisplayHelper.ExtentToGeo(map, geoCollection);
            }
            else
            {
                DevMessageUtil.ShowMessageDialog(string.Format("数据不存在空间范围！"));
            }
        }
    }
}
