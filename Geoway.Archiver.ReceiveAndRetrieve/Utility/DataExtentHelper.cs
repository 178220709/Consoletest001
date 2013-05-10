using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using System.IO;
using Geoway.Archiver.Utility.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using ADF.MIS.Utility.Core;
    using Geoway.ADF.MIS.DB.Public;
    using Geoway.ADF.MIS.Utility.Log;

    /// <summary>
    /// 影像数据范围操作方法集合
    /// </summary>
    public class DataExtentHelper
    {
        /// <summary>
        /// 获取影像空间范围
        /// </summary>
        /// <param name="fullPath">影像或者快视图路径</param>
        /// <returns></returns>
        public static IGeometry GetRasterExtent(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || !System.IO.File.Exists(fullPath))
            {
                return null;
            }

            string ext = System.IO.Path.GetExtension(fullPath).TrimStart('.');
            string wfFrt = GetWordFileExtention(ext);
            string wordFileName = System.IO.Path.ChangeExtension(fullPath, wfFrt);

            IRasterDataset rasterDs = RasterDataOperater.OpenRasterDataset(fullPath);
            IRasterBandCollection rasterBandCollection = rasterDs as IRasterBandCollection;
            IRasterProps rasterProps = rasterBandCollection.Item(0) as IRasterProps;

            int width = rasterProps.Width;
            int height = rasterProps.Height;

            IPolygon pPolygon = new PolygonClass();
            IPointCollection pPointCol = pPolygon as IPointCollection;
            System.Object mis = System.Reflection.Missing.Value;
            IPoint pt = null;

            using (StreamReader sr = new StreamReader(wordFileName))
            {
                try
                {
                    double xSize = double.Parse(sr.ReadLine());
                    double offset = double.Parse(sr.ReadLine());
                    double rotation = double.Parse(sr.ReadLine());
                    double ySize = double.Parse(sr.ReadLine());
                    double xLeftTop = double.Parse(sr.ReadLine());
                    double yLeftTop = double.Parse(sr.ReadLine());

                    pt = new ESRI.ArcGIS.Geometry.PointClass();
                    pt.X = xLeftTop;
                    pt.Y = yLeftTop;
                    pPointCol.AddPoint(pt, ref mis, ref mis);

                    pt = new ESRI.ArcGIS.Geometry.PointClass();
                    pt.X = xSize*width + rotation*0 + xLeftTop;
                    pt.Y = offset*width + ySize*0 + yLeftTop;
                    pPointCol.AddPoint(pt, ref mis, ref mis);

                    pt = new ESRI.ArcGIS.Geometry.PointClass();
                    pt.X = xSize*width + rotation*height + xLeftTop;
                    pt.Y = offset*width + ySize*height + yLeftTop;
                    pPointCol.AddPoint(pt, ref mis, ref mis);

                    pt = new ESRI.ArcGIS.Geometry.PointClass();
                    pt.X = xSize*0 + rotation*height + xLeftTop;
                    pt.Y = offset*0 + ySize*height + yLeftTop;
                    pPointCol.AddPoint(pt, ref mis, ref mis);

                    pt = new ESRI.ArcGIS.Geometry.PointClass();
                    pt.X = xLeftTop;
                    pt.Y = yLeftTop;
                    pPointCol.AddPoint(pt, ref mis, ref mis);
                }
                catch
                {
                    throw;
                }
            }

            return pPolygon as IGeometry;


            //IEnvelope env = RasterDataOperater.GetRasterDatasetExtent(fullPath);
            //if (env == null)
            //{
            //    return null;
            //}

            //ISpatialReference tempSR = null;
            //if (env.SpatialReference == null || env.SpatialReference.Name.CompareTo("Unknown") == 0)
            //{
            //    tempSR = SpatialReferenceUtil.getGeographicCoordinateSystem(enumDatum.W84);
            //    env.SpatialReference = tempSR;
            //}
            //if (env.SpatialReference != null)
            //{
            //    //modify by lf 不需要进行空间投影
            //    //if (tempSR == null)
            //    //{
            //    //    tempSR = SpatialReferenceUtil.getGeographicCoordinateSystem(enumDatum.W84);
            //    //}
            //    //env.Project(tempSR);
            //}

            //IPolygon pPolygon = new PolygonClass();
            //IPointCollection pPointCol = pPolygon as IPointCollection;
            //System.Object p1 = System.Reflection.Missing.Value;
            //IPoint pPoint1 = new ESRI.ArcGIS.Geometry.PointClass();
            //pPoint1.PutCoords(env.XMin, env.YMin);
            //pPointCol.AddPoint(pPoint1, ref p1, ref p1);
            //IPoint pPoint2 = new ESRI.ArcGIS.Geometry.PointClass();
            //pPoint2.PutCoords(env.XMin, env.YMax);
            //pPointCol.AddPoint(pPoint2, ref p1, ref p1);
            //IPoint pPoint3 = new ESRI.ArcGIS.Geometry.PointClass();
            //pPoint3.PutCoords(env.XMax, env.YMax);
            //pPointCol.AddPoint(pPoint3, ref p1, ref p1);
            //IPoint pPoint4 = new ESRI.ArcGIS.Geometry.PointClass();
            //pPoint4.PutCoords(env.XMax, env.YMin);
            //pPointCol.AddPoint(pPoint4, ref p1, ref p1);
            //ITopologicalOperator pTopo = pPolygon as ITopologicalOperator;
            //pTopo.Simplify();

            //return pPolygon as IGeometry;
        }

        /// <summary>
        /// 获取WordFile后缀
        /// </summary>
        /// <param name="rasterFormat"></param>
        /// <returns></returns>
        private static string GetWordFileExtention(string rasterFormat)
        {
            switch (rasterFormat.ToUpper())
            {
                case "TIF":
                case "TIFF":
                    return "tfw";
                case "BMP":
                    return "bpw";
                case "JPG":
                case "JPEG":
                    return "jgw";
                case "IMG":
                    return "igw";
                default:
                    throw new Exception("不支持的Raster格式错误");
            }
        }

        /// <summary>
        /// 获取影像空间范围
        /// </summary>
        /// <param name="metaID">元数据记录ID</param>
        /// <param name="metaTableName">元数据表名</param>
        /// <param name="spatialField">范围字段配置</param>
        /// <returns></returns>
        public static IGeometry GetRasterExtent(int metaID, string metaTableName, SpatialFieldConfigDAL spatialField)
        {
            DataTable dt = MetadataDAL.GetExtentMetadata(metaID, metaTableName);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                double lefttopX = 0.0;
                double lefttopY = 0.0;
                double leftbottomX = 0.0;
                double leftbottomY = 0.0;
                double righttopX = 0.0;
                double righttopY = 0.0;
                double rightbottomX = 0.0;
                double rightbottomY = 0.0;

                if (double.TryParse(row["F_XMIN"].ToString(), out lefttopX) &&
                    double.TryParse(row["F_YMAX"].ToString(), out lefttopY) &&
                    double.TryParse(row["F_XMIN"].ToString(), out leftbottomX) &&
                    double.TryParse(row["F_YMIN"].ToString(), out leftbottomY) &&
                    double.TryParse(row["F_XMAX"].ToString(), out rightbottomX) &&
                    double.TryParse(row["F_YMIN"].ToString(), out rightbottomY) &&
                    double.TryParse(row["F_XMAX"].ToString(), out righttopX) &&
                    double.TryParse(row["F_YMAX"].ToString(), out righttopY))
                {
                    IPoint[] pts = new PointClass[5];
                    pts[0] = new PointClass();
                    pts[0].X = lefttopX;
                    pts[0].Y = lefttopY;
                    pts[1] = new PointClass();
                    pts[1].X = righttopX;
                    pts[1].Y = righttopY;
                    pts[2] = new PointClass();
                    pts[2].X = rightbottomX;
                    pts[2].Y = rightbottomY;
                    pts[3] = new PointClass();
                    pts[3].X = leftbottomX;
                    pts[3].Y = leftbottomY;
                    pts[4] = new PointClass();
                    pts[4].X = lefttopX;
                    pts[4].Y = lefttopY;

                    IPointCollection4 pointCollection = new PolylineClass();
                    object miss = Type.Missing;

                    pointCollection.AddPoint(pts[0], ref miss, ref miss);
                    pointCollection.AddPoint(pts[1], ref miss, ref miss);
                    pointCollection.AddPoint(pts[2], ref miss, ref miss);
                    pointCollection.AddPoint(pts[3], ref miss, ref miss);
                    pointCollection.AddPoint(pts[4], ref miss, ref miss);

                    IGeometryCollection pGeoColl = pointCollection as IGeometryCollection;
                    ISegmentCollection pRing = new RingClass();
                    pRing.AddSegmentCollection(pGeoColl as ISegmentCollection);

                    IGeometryCollection pPolygon = new PolygonClass();
                    pPolygon.AddGeometry(pRing as IGeometry, ref miss, ref miss);

                    IGeometry extent = pPolygon as IGeometry;

                    return extent;

                    //IEnvelope env = new EnvelopeClass();
                    //env.XMin = lefttopX < leftbottomX ? lefttopX : leftbottomX;
                    //env.XMax = righttopX > rightbottomX ? righttopX : rightbottomX;
                    //env.YMin = leftbottomY < rightbottomY ? leftbottomY : rightbottomY;
                    //env.YMax = lefttopY > righttopY ? lefttopY : righttopY;

                    //return env;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取影像空间范围
        /// </summary>
        /// <returns></returns>
        public static IGeometry GetRasterExtentFromMetaFile(IList<DBFieldItem> items)
        {
            try
            {
                double lefttopX = 0.0;
                double lefttopY = 0.0;
                double leftbottomX = 0.0;
                double leftbottomY = 0.0;
                double righttopX = 0.0;
                double righttopY = 0.0;
                double rightbottomX = 0.0;
                double rightbottomY = 0.0;
                foreach (DBFieldItem item in items)
                {
                    switch (item.Name)
                    {
                        case "F_XMIN":
                            double.TryParse(item.Value.ToString(), out lefttopX);
                            double.TryParse(item.Value.ToString(), out leftbottomX);
                            break;
                        case "F_YMIN":
                            double.TryParse(item.Value.ToString(), out leftbottomY);
                            double.TryParse(item.Value.ToString(), out rightbottomY);
                            break;
                        case "F_XMAX":
                            double.TryParse(item.Value.ToString(), out righttopX);
                            double.TryParse(item.Value.ToString(), out rightbottomX);
                            break;
                        case "F_YMAX":
                            double.TryParse(item.Value.ToString(), out lefttopY);
                            double.TryParse(item.Value.ToString(), out righttopY);
                            break;
                    }
                }
                PointClass[] pts = new PointClass[5];
                pts[0] = new PointClass();
                pts[0].X = lefttopX;
                pts[0].Y = lefttopY;
                pts[1] = new PointClass();
                pts[1].X = righttopX;
                pts[1].Y = righttopY;
                pts[2] = new PointClass();
                pts[2].X = rightbottomX;
                pts[2].Y = rightbottomY;
                pts[3] = new PointClass();
                pts[3].X = leftbottomX;
                pts[3].Y = leftbottomY;
                pts[4] = new PointClass();
                pts[4].X = lefttopX;
                pts[4].Y = lefttopY;

                IPointCollection4 pointCollection = new PolylineClass();
                object miss = Type.Missing;

                pointCollection.AddPoint(pts[0], ref miss, ref miss);
                pointCollection.AddPoint(pts[1], ref miss, ref miss);
                pointCollection.AddPoint(pts[2], ref miss, ref miss);
                pointCollection.AddPoint(pts[3], ref miss, ref miss);
                pointCollection.AddPoint(pts[4], ref miss, ref miss);

                IGeometryCollection pGeoColl = pointCollection as IGeometryCollection;
                ISegmentCollection pRing = new RingClass();
                pRing.AddSegmentCollection(pGeoColl as ISegmentCollection);

                IGeometryCollection pPolygon = new PolygonClass();
                pPolygon.AddGeometry(pRing as IGeometry, ref miss, ref miss);
                IGeometry extent = pPolygon as IGeometry;
                return extent;
            }
            catch(Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }
    }
}
