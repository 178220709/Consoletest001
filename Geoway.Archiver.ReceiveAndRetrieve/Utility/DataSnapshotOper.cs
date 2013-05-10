using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Geoway.Archiver.ReceiveAndRetrieve.DAL;
using Geoway.Archiver.ReceiveAndRetrieve.Model;
using Geoway.ADF.MIS.Utility.Log;
using Geoway.ADF.MIS.DB.Public.Interface;
using ESRI.ArcGIS.Geometry;
using Path = System.IO.Path;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    /// <summary>
    /// 2012.11.6 修改ImportSnapshot ，将生成在原数据文件夹的坐标文件（如jgw）修改为在当前运行目录的temp目录下生成并上传
    /// 用完后删除
    /// </summary>
    public class DataSnapshotOper
    {
        /// <summary>
        /// 从文件按导入快视图到数据库
        /// </summary>
        /// <param name="snapshotFileName"> </param>
        /// <param name="spatialReferenceName"> </param>
        /// <returns>入库后ID，失败则为-1</returns>
        public static int ImportSnapshot(string snapshotFileName, string spatialReferenceName)
        {
            DataSnapshotDAL dal = new DataSnapshotDAL();
            dal.SnapShotExtension = System.IO.Path.GetExtension(snapshotFileName).TrimStart('.');
            string wfFrt = GetWordFileExtention(dal.SnapShotExtension);
            string wordFileName = System.IO.Path.ChangeExtension(snapshotFileName, wfFrt);
            if (File.Exists(wordFileName))
            {
                using (StreamReader sr = new StreamReader(wordFileName))
                {
                    try
                    {
                        string auxFileName = snapshotFileName + ".aux";
                        if (File.Exists(auxFileName))
                        {
                            dal.HasAux = true;
                        }
                        else
                        {
                            auxFileName = System.IO.Path.ChangeExtension(snapshotFileName, "aux");
                            if (File.Exists(auxFileName))
                            {
                                dal.HasAux = true;
                            }
                        }

                        dal.XSize = double.Parse(sr.ReadLine());
                        dal.Offset = double.Parse(sr.ReadLine());
                        dal.Rotation = double.Parse(sr.ReadLine());
                        dal.YSize = double.Parse(sr.ReadLine());
                        dal.XLeftTop = double.Parse(sr.ReadLine());
                        dal.YLeftTop = double.Parse(sr.ReadLine());
                        dal.SpatialReferenceName = spatialReferenceName;

                        if (dal.Insert() && dal.SaveSnapShotRasterFile(snapshotFileName))
                        {
                            if (dal.HasAux)
                            {
                                if (dal.SaveAuxFile(auxFileName))
                                {
                                    return dal.ID;
                                }
                            }
                            else
                            {
                                return dal.ID;
                            }
                        }
                        return -1;
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 从文件按导入快视图到数据库
        /// </summary>
        /// <param name="snapshotFileName"> </param>
        /// <param name="thumbFileName"> </param>
        /// <param name="spatialReferenceName"> </param>
        /// <param name="objectID"> </param>
        /// <returns>入库后ID，失败则为-1</returns>
        public static int ImportSnapshot(string snapshotFileName, string thumbFileName,string spatialReferenceName,int objectID)
        {
            DataSnapshotDAL dal = new DataSnapshotDAL();
            dal.SnapShotExtension = System.IO.Path.GetExtension(snapshotFileName).TrimStart('.').ToUpper(); //快视图后缀名
            dal.ThumbExtension = System.IO.Path.GetExtension(thumbFileName).TrimStart('.').ToUpper(); //拇指图后缀名

            string wfFrt = DataSnapshotOper.GetWordFileExtention(dal.SnapShotExtension);
            string wordFileServerName = Path.ChangeExtension(snapshotFileName, wfFrt); //获取wordfile路径
            string wordFileLocalName = string.Format("{0}\\temp\\{1}", Application.StartupPath,
                                                     Path.GetFileName(wordFileServerName));
            string sRstr = "";
            if (File.Exists(wordFileServerName))
            {
                sRstr = wordFileServerName;
            }
            else if (File.Exists(wordFileLocalName))
            {
                sRstr = wordFileLocalName;
            }
            else
            {
                return -1;
            }
            using (StreamReader sr = new StreamReader(sRstr))
            {
                try
                {
                    string auxFileName = snapshotFileName + ".aux";
                    if (File.Exists(auxFileName))
                    {
                        dal.HasAux = true;
                    }
                    else
                    {
                        auxFileName = System.IO.Path.ChangeExtension(snapshotFileName, "aux");
                        if (File.Exists(auxFileName))
                        {
                            dal.HasAux = true;
                        }
                    }

                    dal.HasThumb = File.Exists(thumbFileName);
                    dal.XSize = double.Parse(sr.ReadLine());
                    dal.Offset = double.Parse(sr.ReadLine());
                    dal.Rotation = double.Parse(sr.ReadLine());
                    dal.YSize = double.Parse(sr.ReadLine());
                    dal.XLeftTop = double.Parse(sr.ReadLine());
                    dal.YLeftTop = double.Parse(sr.ReadLine());
                    dal.SpatialReferenceName = spatialReferenceName;
                    dal.ObjectID = objectID;

                    if (dal.Insert() && dal.SaveSnapShotRasterFile(snapshotFileName) &&
                        dal.SaveThumbRasterFile(thumbFileName))
                    {
                        if (dal.HasAux)
                        {
                            if (dal.SaveAuxFile(auxFileName))
                            {
                                return dal.ID;
                            }
                        }
                        else
                        {
                            return dal.ID;
                        }
                    }
                    return -1;
                }
                catch
                {
                    return -1;
                }
            }

            return -1;
        }

        /// <summary>
        /// 从数据库导出快视图到文件
        /// </summary>
        /// <param name="tarDir"> </param>
        /// <param name="filePerfix">快视图命名前缀</param>
        /// <param name="id">快视图标识</param>
        /// <param name="isReplace">是否替换现有文件</param>
        /// <param name="refSpatialReferenceName"> </param>
        /// <returns>出库后文件名，失败则为空</returns>
        public static string ExportSnapshot(string tarDir, string filePerfix, int dataId, bool isReplace, ref string refSpatialReferenceName)
        {
            DataSnapshotDAL dal = DataSnapshotDAL.SelectByDataId(dataId);
            if (dal == null)
            {
                return null;
            }
            else
            {
                refSpatialReferenceName = dal.SpatialReferenceName;
                string fileName = System.IO.Path.Combine(tarDir, filePerfix + dataId + "." + dal.SnapShotExtension);
                if (isReplace || !System.IO.File.Exists(fileName))
                {
                    if (!dal.ReadSnapShotRasterFile(fileName))
                    {
                        return null;
                    }
                }
                string wordFileName = System.IO.Path.ChangeExtension(fileName, GetWordFileExtention(dal.SnapShotExtension));
                if (isReplace || !System.IO.File.Exists(wordFileName))
                {
                    if (!CreateWordFile(dal, wordFileName))
                    {
                        return null;
                    }
                }

                if (dal.HasAux)
                {
                    string auxFileName = System.IO.Path.ChangeExtension(fileName, "aux");
                    if (isReplace || !System.IO.File.Exists(auxFileName))
                    {
                        if (!dal.ReadAuxFile(auxFileName))
                        {
                            return null;
                        }
                    }
                }

                return fileName;
            }
        }

        /// <summary>
        /// 创建Raster文件
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="fileName">Raster文件名</param>
        /// <returns></returns>
        private static string CreateRasterFile(DataSnapshotDAL dal, string fileName)
        {
            if (dal.ReadSnapShotRasterFile(fileName))
            {
                string wfFrt = GetWordFileExtention(dal.SnapShotExtension);
                string wordFileName = System.IO.Path.ChangeExtension(fileName, wfFrt);
                if (CreateWordFile(dal, wordFileName))
                {
                    if (dal.HasAux)
                    {
                        string auxFileName = System.IO.Path.ChangeExtension(fileName, "aux");
                        dal.ReadAuxFile(auxFileName);
                    }
                    return fileName;
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
        /// 创建WordFile
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="wordFileName">WordFile文件名</param>
        /// <returns></returns>
        private static bool CreateWordFile(DataSnapshotDAL dal, string wordFileName)
        {
            using (StreamWriter sw = new StreamWriter(wordFileName))
            {
                try
                {
                    sw.WriteLine(dal.XSize.ToString("#.0000000000"));
                    sw.WriteLine(dal.Offset.ToString("#.0000000000"));
                    sw.WriteLine(dal.Rotation.ToString("#.0000000000"));
                    sw.WriteLine(dal.YSize.ToString("#.0000000000"));
                    sw.WriteLine(dal.XLeftTop.ToString("#.0000000000"));
                    sw.WriteLine(dal.YLeftTop.ToString("#.0000000000"));
                    sw.Close();
                }
                catch
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取WordFile后缀
        /// </summary>
        /// <param name="rasterFormat"></param>
        /// <returns></returns>
        public static string GetWordFileExtention(string rasterFormat)
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

        public static bool IsExsit(IDBHelper db, int dataId)
        {
            return DataSnapshotDAL.IsExist(db, dataId);
        }


        public static int GetRasterIDByKey(RegisterKey key)
        {
            try
            {
                DataSnapshotDAL dataSnapshotDAL = DataSnapshotDAL.SelectByDataId(key.DataID);
                if (dataSnapshotDAL != null)
                {
                    return dataSnapshotDAL.ID;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return -1;
        }

        public static IPolygon CreatGeoByWKT(string strWKT)
        {
            try
            {
                string[] sCoordinates = strWKT.Split(',');

                //构造坐标点
                IPoint[] pointArray = new IPoint[sCoordinates.Length / 2];
                for (int i = 0; i < sCoordinates.Length / 2; i++)
                {
                    double dX = double.Parse(sCoordinates[2 * i]);
                    double dY = double.Parse(sCoordinates[2 * i + 1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(dX, dY);
                    pointArray[i] = pPoint;
                }

                //构造多边形
                IPolygon polygon = new PolygonClass();
                IPointCollection4 pPointCollection = (IPointCollection4)polygon;
                IGeometryBridge geometryBridge = new GeometryEnvironmentClass();
                geometryBridge.AddPoints(pPointCollection, ref pointArray);
                polygon.Close();

                return polygon;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }
        public static bool ExportWordFile(int rowCount, int ColumnCount, IPolygon polygon, string wordFileFullName)
        {
            try
            {
                IPointCollection points = polygon as IPointCollection;
                //if (points.PointCount != 5)
                //{
                //    return false;
                //}
                double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;//x1,y1为左上角点经纬度坐标，顺时针依次赋值

                IPoint point1 = points.get_Point(0);
                IPoint point2 = points.get_Point(1);
                IPoint point3 = points.get_Point(2);
                IPoint point4 = points.get_Point(3);

                double xMid = (point1.X + point2.X + point3.X + point4.X) / 4;
                double yMid = (point1.Y + point2.Y + point3.Y + point4.Y) / 4;
                IPoint point = null;
                for (int i = 0; i < points.PointCount; i++)
                {
                    point = points.get_Point(i);
                    if (point.X < xMid && point.Y > yMid)
                    {
                        x1 = point.X;
                        y1 = point.Y;
                    }
                    if (point.X > xMid && point.Y > yMid)
                    {
                        x2 = point.X;
                        y2 = point.Y;
                    }
                    if (point.X > xMid && point.Y < yMid)
                    {
                        x3 = point.X;
                        y3 = point.Y;
                    }
                    if (point.X < xMid && point.Y < yMid)
                    {
                        x4 = point.X;
                        y4 = point.Y;
                    }
                }


                double line1Value = (x2 - x1) / ColumnCount;
                double line2Value = (y3 - y4) / rowCount;
                double line3Value = (x3 - x2) / ColumnCount;
                double line4Value = (y4 - y1) / rowCount;
                double line5Value = x1;
                double line6Value = y1;

                using (StreamWriter sw = File.CreateText(wordFileFullName))
                {
                    sw.WriteLine(line1Value.ToString("#.000000000000000"));
                    sw.WriteLine(line2Value.ToString("#.000000000000000"));
                    sw.WriteLine(line3Value.ToString("#.000000000000000"));
                    sw.WriteLine(line4Value.ToString("#.000000000000000"));
                    sw.WriteLine(line5Value.ToString("#.000000000000000"));
                    sw.WriteLine(line6Value.ToString("#.000000000000000"));
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return false;
            }
        }

    }
}
