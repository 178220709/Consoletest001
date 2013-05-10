using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.GeoAnalyst;
using System.Collections;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.Archiver.Utility.Definition;
using Geoway.ADF.MIS.Utility.Core;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    /// <summary>
    /// ����Ŀ�ģ�����դ�����ݲ���
    /// ����ʱ�䣺2007��05��09
    /// �����ߣ�icefire
    /// ʵ���ߣ����
    /// ����޸�ʱ�䣺
    /// ��ע��
    /// </summary>
    public static class RasterDataOperater
    {
        #region դ�����ݴ�

        /// <summary>
        /// ���ļ���դ��,����IRasterDataset
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IRasterDataset OpenRasterDataset(string path, string name)
        {
            try
            {
                IWorkspaceFactory wsFactory = new RasterWorkspaceFactoryClass();
                IRasterWorkspace ws = (IRasterWorkspace)wsFactory.OpenFromFile(path, 0);
                IRasterDataset rasterDataset = ws.OpenRasterDataset(name);
                return rasterDataset;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// ���ļ���դ��,����IRasterDataset
        /// </summary>
        /// <param name="filepath">ȫ·���ļ���</param>
        /// <returns></returns>
        public static IRasterDataset OpenRasterDataset(string filepath)
        {
            try
            {
                string path = System.IO.Path.GetDirectoryName(filepath);
                string name = System.IO.Path.GetFileName(filepath);
                IWorkspaceFactory wsFactory = new RasterWorkspaceFactoryClass();
                IRasterWorkspace ws = (IRasterWorkspace)wsFactory.OpenFromFile(path, 0);
                IRasterDataset rasterDataset = ws.OpenRasterDataset(name);
                return rasterDataset;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// ���SDE�е�RasterDataset,Ҳ���������ж�SDE���Ƿ���ڸ����ݼ�
        /// </summary>
        /// <param name="workSapce"></param>
        /// <param name="rasterName"></param>
        /// <returns></returns>
        public static IRasterDataset OpenRasterDataset(IWorkspace worksapce, string rasterName)
        {
            try
            {
                if (worksapce == null) return null;
                IRasterWorkspaceEx rasterWorkspaceEx = worksapce as IRasterWorkspaceEx;
                return rasterWorkspaceEx.OpenRasterDataset(rasterName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// ��Raster���RasterDataset
        /// </summary>
        /// <param name="raster"></param>
        /// <returns></returns>
        public static IRasterDataset GetRasterDataset(IRaster raster)
        {
            try
            {
                IRasterBandCollection bandCol = (IRasterBandCollection)raster;
                IRasterBand rasterBand = bandCol.Item(0);
                IRasterDataset rasterDataset = rasterBand.RasterDataset;
                return rasterDataset;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        /// <summary>
        ///  ��RasterDataset�����ɫ��
        /// </summary>
        /// <param name="rasterDataset"></param>
        /// <returns></returns>
        public static IRasterColormap GetRasterColormap(IRasterDataset rasterDataset)
        {
            IRasterBandCollection bandCol = (IRasterBandCollection)rasterDataset;
            IRasterBand rasterBand = bandCol.Item(0);
            IRasterColormap rasterColormap = rasterBand.Colormap;

            IRaster2 raster = (IRaster2)rasterDataset.CreateDefaultRaster();
            rasterColormap = raster.Colormap;
            return rasterColormap;
        }

        #endregion

        #region ��������ͳ�ƣ�����
        /// <summary>
        /// ����Ӱ�������
        /// </summary>
        /// <param name="rasterDataset">դ������</param>
        public static void BuildPyramid(IRasterDataset rasterDataset)
        {
            IRasterPyramid rasterPyramids = rasterDataset as IRasterPyramid;
            if (rasterPyramids == null) return;

            if (rasterPyramids.Present == false)
            {
                try
                {
                    rasterPyramids.Create();
                }
                catch { }
            }
        }

        /// <summary>
        /// Builds the pyramid.
        /// </summary>
        /// <param name="rasterDataset">The raster dataset.</param>
        /// <param name="PyramidLevel">The pyramid level.</param>
        /// <param name="ResamplingType">Type of the resampling.</param>
        public static void BuildPyramid(IRasterDataset rasterDataset, int PyramidLevel, rstResamplingTypes ResamplingType)
        {
            IRasterPyramid2 rasterPyramids = rasterDataset as IRasterPyramid2;
            if (rasterPyramids == null) return;
            if (rasterPyramids.Present == false) //�Ƿ���Ҫ�ȴ�������������
            {
                try
                {
                    rasterPyramids.Create();
                }
                catch { }
            }
            try
            {
                rasterPyramids.BuildPyramid(PyramidLevel, ResamplingType);
            }
            catch { }
        }
        /// <summary>
        /// ����ͳ����Ϣ
        /// </summary>
        /// <param name="rasterDataset">դ������</param>
        public static void ComputeStatsAndHist(IRasterDataset rasterDataset)
        {
            try
            {
                IRasterBandCollection pBandCol = (IRasterBandCollection)rasterDataset;
                IRasterBand pBand = null;
                bool bHasStatistic;
                for (int i = 0; i < pBandCol.Count; i++)
                {
                    pBand = pBandCol.Item(i);
                    pBand.HasStatistics(out bHasStatistic);
                    if (bHasStatistic == false)
                    {
                        pBand.ComputeStatsAndHist();
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// ����SDE������ͳ�ƣ���֧��ArcSDE�е����ݼ�,��֧���ļ��ͻ����GeoDatabase
        /// </summary>
        /// <param name="sdeRasterDataset">դ������</param>
        public static void AnalyzeDataset(IRasterDataset sdeRasterDataset, esriTableComponents Type)
        {
            IDatasetAnalyze analyze = null;
            analyze = sdeRasterDataset as IDatasetAnalyze;
            try
            {
                analyze.Analyze((int)Type);
            }
            catch { }
        }

        public static IEnvelope GetRasterDatasetExtent(string fullPath)
        {
            IEnvelope env = null;
            string path = System.IO.Path.GetDirectoryName(fullPath);
            string name = System.IO.Path.GetFileName(fullPath);
            IWorkspaceFactory wsFactory = null;
            IRasterWorkspace ws = null;
            IRasterDataset rasterDataset = null;
            try
            {
                wsFactory = new RasterWorkspaceFactoryClass();
                ws = (IRasterWorkspace)wsFactory.OpenFromFile(path, 0);
                rasterDataset = ws.OpenRasterDataset(name);
                if (rasterDataset == null)
                {
                    env = null;
                }
                env = GetRasterDatasetExtent(rasterDataset);
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
            }
            finally
            {
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterDataset);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(wsFactory);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(ws);
            }
            return env;
        }

        /// <summary>
        /// �õ�դ�����ݼ��ķ�Χ
        /// </summary>
        /// <param name="pRasDataset"></param>
        /// <returns></returns>
        public static IEnvelope GetRasterDatasetExtent(IRasterDataset pRasDataset)
        {
            IGeoDataset pGeoDataset = pRasDataset as IGeoDataset;
            if (pGeoDataset == null)
                return null;
            return pGeoDataset.Extent;
        }
        #endregion

        #region ����դ�����ݼ�

        /// <summary>
        /// ���ݸ�����IRasterDataset������RasterDataset����ָ���������ͣ�����ASCII��DEM���
        /// </summary>
        /// <param name="pRasterDataset">The raster dataset.</param>
        /// <param name="name">The name.</param>
        /// <param name="pWorkspace">The workspace.</param>
        /// <param name="pRasterStorageDef">�洢����</param>
        /// <param name="pSpatialReference">The spatial reference.</param>
        /// <param name="pPixelType">��������</param>
        /// <returns></returns>
        public static IRasterDataset CreateRasterDataset(ref IRasterDataset pRasterDataset, string name,
            ref IWorkspace pWorkspace, IRasterStorageDef pRasterStorageDef, ISpatialReference pSpatialReference, rstPixelType pPixelType)
        {
            IRasterWorkspaceEx pRasterWorkspaceEx = (IRasterWorkspaceEx)pWorkspace;
            IRasterDataset newRasterDataset = null;
            try
            {
                newRasterDataset = pRasterWorkspaceEx.OpenRasterDataset(name);
                if (newRasterDataset != null)
                {
                    return newRasterDataset;
                }
            }
            catch (Exception)
            { }

            IRaster pRaster = pRasterDataset.CreateDefaultRaster();
            IRasterBandCollection pRasterBandCollection = (IRasterBandCollection)pRaster;
            int numbands = pRasterBandCollection.Count;

            IRasterProps pRasterProps = (IRasterProps)pRaster;

            if (pRasterStorageDef == null)
            {
                pRasterStorageDef = GetDefaultRasterStorageDef();
            }

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.SpatialReference = pSpatialReference;
            pRasterDef.IsRasterDataset = true;
            pRasterDef.Description = "rasterdataset";

            try
            {
                newRasterDataset = pRasterWorkspaceEx.CreateRasterDataset(name, numbands, pPixelType, pRasterStorageDef, "", pRasterDef, null);
                return newRasterDataset;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        /// <summary>
        /// ���ݸ�����IRasterDataset������RasterDataset
        /// </summary>
        /// <param name="pRasterDataset">The raster dataset.</param>
        /// <param name="name">The name.</param>
        /// <param name="pWorkspace">The workspace.</param>
        /// <param name="pSpatialReference">The spatial reference.</param>
        /// <returns></returns>
        public static IRasterDataset CreateRasterDataset(ref IRasterDataset pRasterDataset, string name,
            ref IWorkspace pWorkspace, IRasterStorageDef pRasterStorageDef, ISpatialReference pSpatialReference)
        {
            IRasterWorkspaceEx pRasterWorkspaceEx = (IRasterWorkspaceEx)pWorkspace;
            IRasterDataset newRasterDataset = null;
            try
            {
                newRasterDataset = pRasterWorkspaceEx.OpenRasterDataset(name);
                if (newRasterDataset != null)
                {
                    return newRasterDataset;
                }
            }
            catch (Exception)
            { }

            IRaster pRaster = pRasterDataset.CreateDefaultRaster();
            IRasterBandCollection pRasterBandCollection = (IRasterBandCollection)pRaster;
            int numbands = pRasterBandCollection.Count;

            IRasterProps pRasterProps = (IRasterProps)pRaster;
            rstPixelType pPixelType = pRasterProps.PixelType;
            if (pRasterStorageDef == null)
            {
                pRasterStorageDef = GetDefaultRasterStorageDef();
            }

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.SpatialReference = pSpatialReference;
            pRasterDef.IsRasterDataset = true;
            pRasterDef.Description = "rasterdataset";

            try
            {
                newRasterDataset = pRasterWorkspaceEx.CreateRasterDataset(name, numbands, pPixelType, pRasterStorageDef, "", pRasterDef, null);
                return newRasterDataset;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        #endregion

        #region �����ռ�

        /// <summary>
        /// ��SDE���ӣ�����IWorkspace
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="Instance"></param>
        /// <param name="User"></param>
        /// <param name="Psw"></param>
        /// <returns></returns>
        public static IRasterWorkspace OpenArcSDEWorkspace(string Server, string Instance, string User, string Psw)
        {
            try
            {
                IPropertySet sdeProperty = new PropertySet();
                sdeProperty.SetProperty("Server", Server);
                sdeProperty.SetProperty("Instance", Instance);
                sdeProperty.SetProperty("user", User);
                sdeProperty.SetProperty("password", Psw);
                sdeProperty.SetProperty("version", "sde.DEFAULT");

                IWorkspaceFactory mWork = new SdeWorkspaceFactoryClass();
                IWorkspace ws = mWork.Open(sdeProperty, 0);
                IRasterWorkspace pRasterWorkspace = ws as IRasterWorkspace;
                return pRasterWorkspace;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Open an Access geodatabase
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static IRasterWorkspace OpenPersonalGDBWorkspace(string sPath)
        {
            try
            {
                IWorkspaceFactory2 mWork = new AccessWorkspaceFactoryClass();
                IWorkspace ws = mWork.OpenFromFile(sPath, 0);
                IRasterWorkspace pRasterWorkspace = ws as IRasterWorkspace;
                return pRasterWorkspace;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IRasterWorkspace OpenFileGDBWorkspace(string filePath)
        {
            IWorkspaceFactory wsFactory = new FileGDBWorkspaceFactoryClass();
            IWorkspace ws = wsFactory.OpenFromFile(filePath, 0);
            IRasterWorkspace pRasterWorkspace = ws as IRasterWorkspace;
            return pRasterWorkspace;
        }

        public static IRasterWorkspace OpenFileWorkspace(string filePath)
        {
            IWorkspaceFactory wsFactory = new RasterWorkspaceFactoryClass();
            IWorkspace ws = wsFactory.OpenFromFile(filePath, 0);
            IRasterWorkspace pRasterWorkspace = ws as IRasterWorkspace;
            return pRasterWorkspace;
        }

        public static IRasterWorkspaceEx GetRasterWorkspaceEx(IRasterWorkspace pWorkspace)
        {
            return (IRasterWorkspaceEx)pWorkspace;
        }

        #region ��ʱ�����ռ�

        /// <summary>
        /// ����Ascess��ʱ�����ռ䣨PGDB��
        /// </summary>
        /// <returns></returns>
        public static IWorkspace CreateAcessScratchWorkspace()
        {
            IScratchWorkspaceFactory workspaceFactory = new ScratchWorkspaceFactoryClass();
            IWorkspace scratchWorkspace = workspaceFactory.CreateNewScratchWorkspace();
            return scratchWorkspace;
        }
        /// <summary>
        /// �õ���ǰAscess��ʱ�����ռ䣨PGDB��
        /// </summary>
        /// <returns></returns>
        public static IWorkspace GetCurrentAcessScratchWorkspace()
        {
            IScratchWorkspaceFactory workspaceFactory = new ScratchWorkspaceFactoryClass();
            IScratchWorkspaceFactory2 workspaceFactory2 = workspaceFactory as IScratchWorkspaceFactory2;
            IWorkspace scratchWorkspace = workspaceFactory2.CurrentScratchWorkspace;
            return scratchWorkspace;
        }
        /// <summary>
        /// �����ļ�����ʱ�����ռ䣨FileGDB��
        /// </summary>
        /// <returns></returns>
        public static IWorkspace CreateFileGDBScratchWorkspace()
        {
            IScratchWorkspaceFactory workspaceFactory = new FileGDBScratchWorkspaceFactoryClass();
            IWorkspace scratchWorkspace = workspaceFactory.CreateNewScratchWorkspace();
            return scratchWorkspace;
        }
        /// <summary>
        /// �õ���ǰ�ļ�����ʱ�����ռ䣨FileGDB��
        /// </summary>
        /// <returns></returns>
        public static IWorkspace GetCurrentFileGDBScratchWorkspace()
        {
            IScratchWorkspaceFactory workspaceFactory = new FileGDBScratchWorkspaceFactoryClass();
            IScratchWorkspaceFactory2 workspaceFactory2 = workspaceFactory as IScratchWorkspaceFactory2;
            IWorkspace scratchWorkspace = workspaceFactory2.CurrentScratchWorkspace;
            return scratchWorkspace;
        }

        #endregion
        #endregion

        #region դ�������ز���
        /// <summary>
        /// դ�������ز���
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="name">�ļ���</param>
        /// <param name="newCellsize">Ŀ��������(�ֱ���)</param>
        /// <param name="pResampleType">�ز�������</param>
        /// <returns>������դ�����ݼ�</returns>
        public static IRasterDataset Resample(string path, string name, double newCellsize,
            esriGeoAnalysisResampleEnum pResampleType)
        {
            IRasterDataset pOriRsDs = null;
            IRasterDataset pDesRsDs = null;
            try
            {
                pOriRsDs = OpenRasterDataset(path, name);
                pDesRsDs = Resample(pOriRsDs, newCellsize, pResampleType);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return pDesRsDs;
        }

        /// <summary>
        /// դ�������ز���
        /// </summary>
        /// <param name="pOriRsDs">ԭʼդ�����ݼ�</param>
        /// <param name="newCellsize">Ŀ��������(�ֱ���)</param>
        /// <param name="pResampleType">�ز�������</param>
        /// <returns>������դ�����ݼ�</returns>
        public static IRasterDataset Resample(IRasterDataset sourceRasterDataset, double newCellsize,
            esriGeoAnalysisResampleEnum pResampleType)
        {
            IGeoDataset pOriGeodataset = null;
            IGeoDataset pDesGeoDataset = null;
            IRasterDataset pDesRsDs = null;
            ITransformationOp pTransformationOp = new RasterTransformationOpClass();
            try
            {
                pOriGeodataset = (IGeoDataset)sourceRasterDataset;
                pDesGeoDataset = pTransformationOp.Resample(pOriGeodataset, newCellsize, pResampleType);
                pDesRsDs = (IRasterDataset)pDesGeoDataset;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return pDesRsDs;
        }

        /// <summary>
        /// դ�������ز��������浽����
        /// </summary>
        /// <param name="sourceRasterDataset">ԭʼդ�����ݼ�</param>
        /// <param name="name">�ز����������ļ���</param>
        /// <param name="path">�ز���֮������·��</param>
        /// <param name="Format">�ز���֮���դ���������</param>
        /// <param name="newCellSize">Ŀ��������(�ֱ���)</param>
        /// <param name="pResampleType">�ز�������</param>
        public static void Resample(IRasterDataset sourceRasterDataset, string name, string path,
            enumRasterFileFormat pFormatType, double newCellSize, esriGeoAnalysisResampleEnum pResampleType)
        {
            try
            {
                if (sourceRasterDataset == null)
                    return;
                IRasterDataset pOutputRasterDataset = null;

                pOutputRasterDataset = Resample(sourceRasterDataset, newCellSize, pResampleType);
                SaveRaster(pOutputRasterDataset, name, path, pFormatType);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }


        /// <summary>
        /// դ�������ز��������浽����
        /// </summary>
        /// <param name="sourceRasterDataset">ԭʼդ�����ݼ�</param>
        /// <param name="name">�ز����������ļ���</param>
        /// <param name="path">�ز���֮������·��</param>
        /// <param name="Format">�ز���֮���դ���������</param>
        /// <param name="newCellSize">Ŀ��������(�ֱ���)</param>
        /// <param name="pResampleType">�ز�������</param>
        public static void Resample(string fullpath, string name, string path,
            enumRasterFileFormat pFormatType, double newCellSize, esriGeoAnalysisResampleEnum pResampleType)
        {
            try
            {
                IRasterDataset sourceRasterDataset = null;
                sourceRasterDataset = OpenRasterDataset(fullpath);
                if (sourceRasterDataset == null)
                    return;
                IRasterDataset pOutputRasterDataset = null;

                pOutputRasterDataset = Resample(sourceRasterDataset, newCellSize, pResampleType);
                SaveRaster(pOutputRasterDataset, name, path, pFormatType);

                GC.Collect();

            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        public static bool ResampleRaster(string fullpath, string OutRasterPath, string OutRasterName,
            double newCellsize, esriGeoAnalysisResampleEnum pResampleType)
        {
            IRasterDataset pOriRsDs = null;
            pOriRsDs = OpenRasterDataset(System.IO.Path.GetDirectoryName(fullpath), System.IO.Path.GetFileName(fullpath));
            bool rlt = ResampleRaster(pOriRsDs, OutRasterPath, OutRasterName, newCellsize, pResampleType);
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOriRsDs);
            return rlt;
        }

        /// <summary>
        /// դ�������ز���
        /// </summary>
        /// <param name="pOriRsDs">ԭʼդ�����ݼ�</param>
        /// <param name="newCellsize">Ŀ��������(�ֱ���)</param>
        /// <param name="pResampleType">�ز�������</param>
        /// <returns>������դ�����ݼ�</returns>
        public static bool ResampleRaster(IRasterDataset pOriRsDs, string OutRasterPath, string OutRasterName,
            double newCellsize, esriGeoAnalysisResampleEnum pResampleType)
        {
            IGeoDataset pOriGeodataset = null;
            IGeoDataset pDesGeoDataset = null;
            IRasterDataset pDesRsDs = null;

            IRaster pResampleRaster = null;
            IWorkspace pOutWorkspace = null;

            ITransformationOp m_TransformationOp = new RasterTransformationOpClass();

            try
            {
                pOriGeodataset = (IGeoDataset)pOriRsDs;
                pDesGeoDataset = m_TransformationOp.Resample(pOriGeodataset, newCellsize, pResampleType);
                pDesRsDs = (IRasterDataset)pDesGeoDataset;

                ComputeStatsAndHist(pDesRsDs);

                #region ����Bitmap�ı��淽ʽ

                string fileName = OutRasterPath.TrimEnd('\\') + "\\" + OutRasterName;
                string frtStr = System.IO.Path.GetExtension(fileName).TrimStart('.');
                ImageFormat frt;
                switch (frtStr.ToUpper())
                {
                    case "JPG":
                    case "JPEG":
                        frt = ImageFormat.Jpeg;
                        break;
                    case "BMP":
                        frt = ImageFormat.Bmp;
                        break;
                    case "PNG":
                        frt = ImageFormat.Png;
                        break;
                    default:
                        frt = ImageFormat.Bmp;
                        break;
                }
                Raster2LocalPicture(pDesRsDs, fileName, frt);

                #endregion

                #region ����AE�ı��淽ʽ

                //IWorkspaceFactory pWorkspaceFactory = new RasterWorkspaceFactoryClass();

                //pResampleRaster = pDesRsDs.CreateDefaultRaster();
                //ISaveAs pSaveAs = (ISaveAs)pResampleRaster;
                //pOutWorkspace = pWorkspaceFactory.OpenFromFile(OutRasterPath, 0);

                //string Format = GetFormat(OutRasterName);


                //if (pSaveAs.CanSaveAs(Format))
                //{
                //    if (System.IO.File.Exists(OutRasterPath + @"\" + OutRasterName))
                //    {
                //        try
                //        {
                //            System.IO.File.Delete(OutRasterPath + "\\" + OutRasterName);
                //        }
                //        catch (Exception exp)
                //        {
                //            LogHelper.Error.Append(exp);
                //        }
                //    }
                //    pSaveAs.SaveAs(OutRasterName, pOutWorkspace, Format);
                //}

                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResampleRaster);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDesGeoDataset);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutWorkspace);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return true;
        }

        public static string GetFormat(string OutRasterName)
        {
            string strFormat = "TIFF";
            if (System.IO.Path.GetExtension(OutRasterName).ToLower() == ".tif" || System.IO.Path.GetExtension(OutRasterName).ToLower() == ".tiff")
            {
                strFormat = "TIFF";
            }
            else if (System.IO.Path.GetExtension(OutRasterName).ToLower() == ".img")
            {
                strFormat = "IMAGINE Image";
            }
            else if (System.IO.Path.GetExtension(OutRasterName).ToLower() == ".jpg")
            {
                strFormat = "JPG";
            }
            else if (System.IO.Path.GetExtension(OutRasterName).ToLower() == ".bmp")
            {
                strFormat = "BMP";
            }
            else if (System.IO.Path.GetExtension(OutRasterName).ToLower() == ".jp2")
            {
                strFormat = "JP2";
            }
            return strFormat;
        }
        #endregion

        #region ����դ�񵽱����ļ�
        /// <summary>
        /// ����դ�����ݼ�������
        /// </summary>
        /// <param name="pInputRasterDataset">�������դ�����ݼ�</param>
        /// <param name="name">�����ļ���</param>
        /// <param name="path">����·��</param>
        /// <param name="pFormatType">����դ���ļ�����</param>
        public static void SaveRaster(IRasterDataset pInputRasterDataset, string name, string path,
            enumRasterFileFormat pFormatType)
        {
            try
            {
                ISaveAs pSaveAs = (ISaveAs)pInputRasterDataset;
                IWorkspaceFactory wsFactory = new RasterWorkspaceFactoryClass();
                IWorkspace pWorkspace = wsFactory.OpenFromFile(path, 0);
                pSaveAs.SaveAs(name, pWorkspace, GetFormatString(pFormatType));
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRasterDataset);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSaveAs);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(wsFactory);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// ����դ��������ͻ�ùؼ���
        /// </summary>
        /// <param name="pFromat"></param>
        /// <returns></returns>
        public static string GetFormatString(enumRasterFileFormat pFromat)
        {
            switch (pFromat)
            {
                case enumRasterFileFormat.enumRasterFileFormatGeoTIFF:
                    return "TIFF";
                case enumRasterFileFormat.enumRasterFileFormatGRID:
                    return "GRID";
                case enumRasterFileFormat.enumRasterFileFormatIMAGE:
                    return "IMAGINE Image";
            }
            return "GRID";
        }

        /// <summary>
        /// ����RasterDataset������Windows��ʶ��ͼ���ʽ(����.NET Bitmap��ʵ��)
        /// </summary>
        /// <param name="rasterDs">RasterDataset</param>
        /// <param name="tarFullName">Ŀ��ͼ����Ե�ַ</param>
        /// <param name="format">Ŀ��ͼ���ʽ</param>
        /// <returns></returns>//Added by wjy 2011-01-17
        public static bool Raster2LocalPicture(IRasterDataset rasterDs, string tarFullName, ImageFormat format)
        {
            if (rasterDs == null)
            {
                return false;
            }
            IRaster raster = null;
            IRasterBandCollection rasterBandCollection = null;
            IRasterProps rasterProps = null;
            IPixelBlock pixelBlock = null;
            Bitmap bmp = null;

            try
            {
                raster = rasterDs.CreateDefaultRaster();
                rasterBandCollection = rasterDs as IRasterBandCollection;
                rasterProps = rasterBandCollection.Item(0) as IRasterProps;

                int width = rasterProps.Width;
                int height = rasterProps.Height;

                object band1 = null;
                object band2 = null;
                object band3 = null;

                #region ��ȡRasterDatasetÿ�����ε�����ֵ

                IPnt point = new PntClass();
                point.X = width;
                point.Y = height;

                pixelBlock = raster.CreatePixelBlock(point); //Raster2PixeBlock(pRaster, 1, 1);
                point.X = 0;
                point.Y = 0;
                raster.Read(point, pixelBlock);

                if (pixelBlock.Planes <= 0)
                {
                    throw new Exception("No Bands on RasterDataset");
                }

                switch (pixelBlock.Planes)
                {
                    case 1:
                        {
                            band1 = pixelBlock.get_SafeArray(0);
                            band2 = band1;
                            band3 = band1;
                        }
                        break;
                    case 2:
                        {
                            band1 = pixelBlock.get_SafeArray(0);
                            band2 = pixelBlock.get_SafeArray(1);
                            band3 = band1;
                        }
                        break;
                    default:
                        {
                            band1 = pixelBlock.get_SafeArray(0);
                            band2 = pixelBlock.get_SafeArray(1);
                            band3 = pixelBlock.get_SafeArray(2);
                        }
                        break;
                }

                #endregion

                bmp = new Bitmap(width, height);

                #region д��Bitmap

                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                                                  ImageLockMode.ReadWrite,
                                                  bmp.PixelFormat);
                byte[] rgbValues = new byte[width * height * 4];
                //int[] pixelValue = new int[width * height];

                int index = 0;

                switch (pixelBlock.BytesPerPixel)
                {
                    case 1:
                        #region 8 bit
                        {
                            byte[,] tmpBand1 = (byte[,])band1;
                            byte[,] tmpBand2 = (byte[,])band2;
                            byte[,] tmpBand3 = (byte[,])band3;

                            for (int i = 0; i < height; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    rgbValues[index++] = (byte)tmpBand3[j, i];
                                    rgbValues[index++] = (byte)tmpBand2[j, i];
                                    rgbValues[index++] = (byte)tmpBand1[j, i];
                                    index++;

                                    //pixelValue[index++] = GetIntValueFromBytes(new byte[] { (byte)tmpBand3[j, i], (byte)tmpBand2[j, i], (byte)tmpBand1[j, i] });
                                }
                            }
                        }
                        #endregion
                        break;
                    case 2:
                        #region 16 bit
                        {
                            UInt16[,] tmpBand1 = (UInt16[,])band1;
                            UInt16[,] tmpBand2 = (UInt16[,])band2;
                            UInt16[,] tmpBand3 = (UInt16[,])band3;
                            for (int i = 0; i < height; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    rgbValues[index++] = (byte)tmpBand3[j, i];
                                    rgbValues[index++] = (byte)tmpBand2[j, i];
                                    rgbValues[index++] = (byte)tmpBand1[j, i];
                                    index++;
                                    //pixelValue[index++] = GetIntValueFromBytes(new byte[] { (byte)tmpBand3[j, i], (byte)tmpBand2[j, i], (byte)tmpBand1[j, i] });
                                }
                            }
                        }
                        #endregion
                        break;
                    case 4:
                        #region 32 bit
                        {
                            UInt32[,] tmpBand1 = (UInt32[,])band1;
                            UInt32[,] tmpBand2 = (UInt32[,])band2;
                            UInt32[,] tmpBand3 = (UInt32[,])band3;
                            for (int i = 0; i < height; i++)
                            {
                                for (int j = 0; j < width; j++)
                                {
                                    rgbValues[index++] = (byte)tmpBand3[j, i];
                                    rgbValues[index++] = (byte)tmpBand2[j, i];
                                    rgbValues[index++] = (byte)tmpBand1[j, i];
                                    index++;
                                    //pixelValue[index++] = GetIntValueFromBytes(new byte[] { (byte)tmpBand3[j, i], (byte)tmpBand2[j, i], (byte)tmpBand1[j, i] });
                                }
                            }
                        }
                        #endregion
                        break;
                    default:
                        break;
                }

                Marshal.Copy(rgbValues, 0, bmpData.Scan0, rgbValues.Length);
                //Marshal.Copy(pixelValue, 0, bmpData.Scan0, pixelValue.Length);

                bmp.UnlockBits(bmpData);

                #endregion

                bmp.Save(tarFullName, format);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pixelBlock);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterProps);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterBandCollection);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(raster);
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }

            return true;
        }

        /// <summary>
        /// ���ֽ�����intֵ
        /// </summary>
        /// <param name="bytes">byte���飬�Ӹ�λ����λ</param>
        /// <returns></returns>
        public static int GetIntValueFromBytes(byte[] bytes)
        {
            if (bytes.Length >= sizeof(int))
            {
                throw new Exception("������byte���鳤�ȳ���int�������ܴ�ŵ��ֽ���");
            }
            int value = 0;
            foreach (byte var in bytes)
            {
                value = value << 8;
                value += var;
            }
            return value;
        }

        #endregion

        #region RasterCatalog����ģ��

        /// <summary>
        /// ����դ���Ŀ��֧�� PGDB, FGDB, and ArcSDE��
        /// </summary>
        /// <param name="pWs">�����ռ����</param>
        /// <param name="sCatalogName">դ���Ŀ������</param>
        /// <param name="sName">դ���Ŀ���Ա�������ֶ���</param>
        /// <param name="sRasterFldName">դ���Ŀ���Ա��դ�������ֶ���</param>
        /// <param name="sShapeFldName">դ���Ŀ���Ա�ļ���ͼ���ֶ��ֶ���</param>
        /// <param name="pShpSpatialRef">դ���Ŀ����ͼ�εĿռ�ο�</param>
        /// <param name="pRasterSpatialRef">դ���Ŀդ�����ݵ�Ĭ�Ͽռ�ο�</param>
        /// <param name="isManaged">�Ƿ�Ϊ�й�ģʽ,����PGDB����Ч</param>
        /// <param name="pFields">���Ա��ֶμ��ϣ���Null�Ļ�������Ĭ�ϵ����Ա��ֶ�</param>
        /// <param name="sKeyword">դ���Ŀ�Ĺؼ���</param>
        /// <returns>���ش����ɹ���դ���Ŀ����</returns>
        public static IRasterCatalog CreateRasterCatalog(IRasterWorkspaceEx pWs, string sCatalogName, string sNameFldName,
            string sRasterFldName, string sShapeFldName, ISpatialReference pShpSpatialRef,
            ISpatialReference pRasterSpatialRef, bool isManaged, IFields pFields, string sKeyword)
        {
            IRasterCatalog pRasterCatalog = null;
            try
            {
                if (string.IsNullOrEmpty(sShapeFldName)) sShapeFldName = CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE;
                if (string.IsNullOrEmpty(sNameFldName)) sNameFldName = CONST_RASTERCATALOG.CONST_FLDNAME_NAME;
                if (string.IsNullOrEmpty(sKeyword)) sKeyword = CONST_RASTERCATALOG.CONST_CONFIG_KEYWORD;

                if (pFields == null)
                {
                    pFields = CreatDefaultFields(sNameFldName, sRasterFldName, sShapeFldName, isManaged, pShpSpatialRef, pRasterSpatialRef);
                }

                pRasterCatalog = pWs.CreateRasterCatalog(sCatalogName, pFields, sShapeFldName, sRasterFldName, sKeyword);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            return pRasterCatalog;
        }

        private static IFields CreatDefaultFields(string sName, string sRasterFld, string sShapeFld, bool bIsManaged,
            ISpatialReference pShpSpatialRef, ISpatialReference pRasterSpatialRef)
        {
            IFieldsEdit pFieldsEdit;
            IFields pFields;
            pFields = new FieldsClass();
            pFieldsEdit = (IFieldsEdit)pFields;

            IField pField = null;
            IFieldEdit pFieldEdit = null;

            //oid
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = CONST_RASTERCATALOG.CONST_FLDNAME_OBJECTID;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            pFieldsEdit.AddField(pField);

            //name
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = sName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);

            //Raster
            IField2 pRField = null;
            IFieldEdit2 pRFieldEdit = null;
            if (string.IsNullOrEmpty(sRasterFld)) sRasterFld = CONST_RASTERCATALOG.CONST_FLDNAME_RASTER;
            pRField = new FieldClass();
            pRFieldEdit = (IFieldEdit2)pRField;
            pRFieldEdit.Name_2 = sRasterFld;
            pRFieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "raster catalog";
            pRasterDef.IsManaged = bIsManaged; //only for PGDB
            if (pRasterSpatialRef == null)
                pRasterSpatialRef = new UnknownCoordinateSystemClass();
            pRasterDef.SpatialReference = pRasterSpatialRef;
            pRFieldEdit.RasterDef = pRasterDef;
            pFieldsEdit.AddField(pRField);

            //shape
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            if (string.IsNullOrEmpty(sShapeFld)) sShapeFld = CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE;
            pFieldEdit.Name_2 = sShapeFld;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoEdit = (IGeometryDefEdit)pGeoDef;
            pGeoEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoEdit.AvgNumPoints_2 = 4;
            pGeoEdit.GridCount_2 = 1;
            pGeoEdit.set_GridSize(0, 1000);
            if (pShpSpatialRef == null)
                pShpSpatialRef = new UnknownCoordinateSystemClass();
            pGeoEdit.SpatialReference_2 = pShpSpatialRef;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField);

            return pFields;
        }

        /// <summary>
        /// ����դ���Ŀ��ں���
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static IRasterCatalog CreateRasterCatalog(string sCatalogName, IWorkspace pWorkspace, ISpatialReference ShpSpatialReference, ISpatialReference RasterSpatialReference)
        {
            try
            {
                IFields pFields = null;
                pFields = CreateFields(ShpSpatialReference, RasterSpatialReference);
                IRasterWorkspaceEx pRasterWorkspaceEx = (IRasterWorkspaceEx)pWorkspace;

                IRasterCatalog pRasterCatalog = pRasterWorkspaceEx.CreateRasterCatalog(sCatalogName, pFields, CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, CONST_RASTERCATALOG.CONST_CONFIG_KEYWORD);
                return pRasterCatalog;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }

        public static IRasterCatalog CreateSnapShotRasterCatalog(string sCatalogName, IWorkspace pWorkspace, ISpatialReference ShpSpatialReference, ISpatialReference RasterSpatialReference)
        {
            try
            {
                IFields pFields = null;
                pFields = CreateSnapShotFields(ShpSpatialReference, RasterSpatialReference);
                IRasterWorkspaceEx pRasterWorkspaceEx = (IRasterWorkspaceEx)pWorkspace;

                IRasterCatalog pRasterCatalog = pRasterWorkspaceEx.CreateRasterCatalog(sCatalogName, pFields, CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, CONST_RASTERCATALOG.CONST_CONFIG_KEYWORD);
                return pRasterCatalog;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                return null;
            }
        }


        /// <summary>
        /// ����դ��Ŀ¼��𴴽���ͬ��դ���Ŀ�����ֶμ���
        /// </summary>
        /// <param name="dType"></param>
        /// <returns></returns>
        private static IFields CreateFields(ISpatialReference ShpSpatialReference, ISpatialReference RasterSpatialReference)
        {
            IFields pFields = null;
            IFieldsEdit pFieldsEdit = null;

            pFields = new FieldsClass();
            pFieldsEdit = (IFieldsEdit)pFields;
            IField pField = null;

            #region ����
            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_OBJECTID, CONST_RASTERCATALOG.CONST_FLDNAME_OBJECTID, esriFieldType.esriFieldTypeOID, false);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_NAME, CONST_RASTERCATALOG.CONST_FLDNAME_NAME, esriFieldType.esriFieldTypeString, false);
            pFieldsEdit.AddField(pField);

            pField = CreateShapeField(CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, esriFieldType.esriFieldTypeGeometry, esriGeometryType.esriGeometryPolygon, ShpSpatialReference);
            pFieldsEdit.AddField(pField);

            pField = CreateRasterField(CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, esriFieldType.esriFieldTypeRaster, RasterSpatialReference);
            pFieldsEdit.AddField(pField);
            #endregion

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDALIAS_FRAME, CONST_RASTERCATALOG.CONST_FLDNAME_FRAME, esriFieldType.esriFieldTypeString, true);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_IMPORTDATE, CONST_RASTERCATALOG.CONST_FLDNAME_IMPORTDATE, esriFieldType.esriFieldTypeDate, true);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_REMARK, CONST_RASTERCATALOG.CONST_FLDNAME_REMARK, esriFieldType.esriFieldTypeString, true);
            pFieldsEdit.AddField(pField);

            return pFields;
        }

        private static IFields CreateSnapShotFields(ISpatialReference ShpSpatialReference, ISpatialReference RasterSpatialReference)
        {
            IFields pFields = null;
            IFieldsEdit pFieldsEdit = null;

            pFields = new FieldsClass();
            pFieldsEdit = (IFieldsEdit)pFields;
            IField pField = null;

            #region ����
            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_OBJECTID, CONST_RASTERCATALOG.CONST_FLDNAME_OBJECTID, esriFieldType.esriFieldTypeOID, false);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_NAME, CONST_RASTERCATALOG.CONST_FLDNAME_NAME, esriFieldType.esriFieldTypeString, false);
            pFieldsEdit.AddField(pField);

            pField = CreateShapeField(CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, CONST_RASTERCATALOG.CONST_FLDNAME_SHAPE, esriFieldType.esriFieldTypeGeometry, esriGeometryType.esriGeometryPolygon, ShpSpatialReference);
            pFieldsEdit.AddField(pField);

            pField = CreateRasterField(CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, CONST_RASTERCATALOG.CONST_FLDNAME_RASTER, esriFieldType.esriFieldTypeRaster, RasterSpatialReference);
            pFieldsEdit.AddField(pField);
            #endregion

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDALIAS_FRAME, CONST_RASTERCATALOG.CONST_FLDNAME_FRAME, esriFieldType.esriFieldTypeString, true);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_IMPORTDATE, CONST_RASTERCATALOG.CONST_FLDNAME_IMPORTDATE, esriFieldType.esriFieldTypeDate, true);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_RASTERCATALOG.CONST_FLDNAME_REMARK, CONST_RASTERCATALOG.CONST_FLDNAME_REMARK, esriFieldType.esriFieldTypeString, true);
            pFieldsEdit.AddField(pField);

            pField = CreateField(CONST_SNAPSHOT.CONST_ALIASCTALOGID, CONST_SNAPSHOT.CONST_CATALOGID, esriFieldType.esriFieldTypeInteger, true);
            pFieldsEdit.AddField(pField);


            pField = CreateField(CONST_SNAPSHOT.CONST_ALIASOBJECTID, CONST_SNAPSHOT.CONST_OBJECTID, esriFieldType.esriFieldTypeInteger, true);
            pFieldsEdit.AddField(pField);


            pField = CreateField(CONST_SNAPSHOT.CONST_ALIASDATAID, CONST_SNAPSHOT.CONST_DATAID, esriFieldType.esriFieldTypeInteger, false);
            pFieldsEdit.AddField(pField);
            return pFields;

        }

        private static IField CreateField(string CName, string EName, esriFieldType fType, bool IsAllowNull)
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

        private static IField CreateField(string CName, string EName, esriFieldType fType, bool IsAllowNull, int length)
        {
            IField pField = null;
            IFieldEdit pFieldEdit = null;
            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.AliasName_2 = CName;
            pFieldEdit.Name_2 = EName;
            pFieldEdit.IsNullable_2 = IsAllowNull;
            pFieldEdit.Type_2 = fType;
            pFieldEdit.Length_2 = length;

            return pField;
        }

        /// <summary>
        /// ���������ֶ�
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="EName"></param>
        /// <param name="fType"></param>
        /// <param name="geoMetrytype"></param>
        /// <param name="SpatialRef"></param>
        /// <returns></returns>
        private static IField CreateShapeField(string CName, string EName, esriFieldType fType, esriGeometryType geoMetrytype, ISpatialReference SpatialRef)
        {
            IField pField = null;
            IFieldEdit pFieldEdit = null;

            pField = new FieldClass();
            pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = EName;
            pFieldEdit.AliasName_2 = CName;
            pFieldEdit.Type_2 = fType;

            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoEdit = (IGeometryDefEdit)pGeoDef;
            pGeoEdit.GeometryType_2 = geoMetrytype;
            pGeoEdit.AvgNumPoints_2 = 4;
            IUnknownCoordinateSystem pUnknown = null;

            if (SpatialRef == null || SpatialRef is IUnknownCoordinateSystem)
            {
                pUnknown = new UnknownCoordinateSystemClass();
                pGeoEdit.SpatialReference_2 = new UnknownCoordinateSystemClass();
            }
            else
            {
                IControlPrecision2 pCP = SpatialRef as IControlPrecision2;
                if (!pCP.IsHighPrecision)
                {
                    pCP.IsHighPrecision = true;
                }
                pGeoEdit.SpatialReference_2 = SpatialRef;
            }
            pFieldEdit.GeometryDef_2 = pGeoDef;
            return pField;
        }

        /// <summary>
        /// ����դ���ֶ�
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="EName"></param>
        /// <param name="fType"></param>
        /// <param name="pSpatialRef"></param>
        /// <returns></returns>
        private static IField2 CreateRasterField(string CName, string EName, esriFieldType fType, ISpatialReference pSpatialRef)
        {
            IField2 pRField = null;
            IFieldEdit2 pRFieldEdit = null;
            pRField = new FieldClass();
            pRFieldEdit = (IFieldEdit2)pRField;
            pRFieldEdit.Name_2 = EName;
            pRFieldEdit.AliasName_2 = CName;
            pRFieldEdit.Type_2 = fType;

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "raster catalog";

            IUnknownCoordinateSystem pUnknown = null;
            if (pSpatialRef == null || pSpatialRef is IUnknownCoordinateSystem)
            {
                pUnknown = new UnknownCoordinateSystemClass();
                pRasterDef.SpatialReference = new UnknownCoordinateSystemClass();
            }
            else
            {
                IControlPrecision2 pCP = pSpatialRef as IControlPrecision2;
                if (!pCP.IsHighPrecision)
                {
                    pCP.IsHighPrecision = true;
                }
                pRasterDef.SpatialReference = pSpatialRef;
            }
            pRFieldEdit.RasterDef = pRasterDef;
            return pRField;
        }

        /// <summary>
        /// ��դ��Ŀ¼ ֧��  PGDB, FGDB, and ArcSDE
        /// </summary>
        /// <param name="rasterWorkspaceEx">�����ռ�</param>
        /// <param name="catalogName">դ���Ŀ����</param>
        /// <returns>����դ���Ŀ����</returns>
        public static IRasterCatalog OpenRasterCatalog(IRasterWorkspaceEx rasterWorkspaceEx, string catalogName)
        {
            try
            {
                return rasterWorkspaceEx.OpenRasterCatalog(catalogName);

            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// �ж�դ��Ŀ¼�Ƿ����
        /// </summary>
        /// <param name="pWorkspace">�����ռ�</param>
        /// <param name="catalogName">դ���Ŀ����</param>
        /// <returns></returns>
        public static bool IsExistRasterCatalog(IWorkspace pWorkspace, string catalogName)
        {
            try
            {
                IRasterWorkspaceEx rasterWorkspaceEx = pWorkspace as IRasterWorkspaceEx;
                IRasterCatalog rasterCatalog = rasterWorkspaceEx.OpenRasterCatalog(catalogName);
                if (rasterCatalog == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the default raster storage def.
        /// </summary>
        /// <returns></returns>
        public static IRasterStorageDef GetDefaultRasterStorageDef()
        {
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = esriRasterCompressionType.esriRasterCompressionUncompressed;
            pRasterStorageDef.PyramidLevel = 0;
            pRasterStorageDef.PyramidResampleType = rstResamplingTypes.RSP_NearestNeighbor;
            pRasterStorageDef.TileHeight = 128;
            pRasterStorageDef.TileWidth = 128;
            return pRasterStorageDef;
        }

        /// <summary>
        /// ���һ���Զ����դ��洢����
        /// </summary>
        /// <param name="CompressionType">ѹ������.</param>
        /// <param name="PyramidLevel">0-��������-1���Զ�����������ֵ����������������.</param>
        /// <param name="PyramidResampleType">Type of the pyramid resample.</param>
        /// <param name="TileHeight">Height of the tile.</param>
        /// <param name="TileWidth">Width of the tile.</param>
        /// <returns></returns>
        public static IRasterStorageDef GetCustomRasterStorageDef(esriRasterCompressionType CompressionType, int PyramidLevel,
            rstResamplingTypes PyramidResampleType, int TileHeight, int TileWidth)
        {
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = CompressionType;
            pRasterStorageDef.PyramidLevel = PyramidLevel;
            pRasterStorageDef.PyramidResampleType = PyramidResampleType;
            pRasterStorageDef.TileHeight = TileHeight;
            pRasterStorageDef.TileWidth = TileWidth;
            return pRasterStorageDef;
        }

        /// <summary>
        /// ���һ���Զ����դ��洢����
        /// </summary>
        /// <param name="CompressionType">ѹ������.</param>
        /// <param name="CompressionQuality">ѹ��������ֻ��JPEGʱ��Ч.</param>
        /// <param name="PyramidLevel">0-��������-1���Զ�����������ֵ����������������</param>
        /// <param name="PyramidResampleType">Type of the pyramid resample.</param>
        /// <param name="TileHeight">Height of the tile.</param>
        /// <param name="TileWidth">Width of the tile.</param>
        /// <returns></returns>
        public static IRasterStorageDef GetCustomRasterStorageDef(esriRasterCompressionType CompressionType, int CompressionQuality, int PyramidLevel,
    rstResamplingTypes PyramidResampleType, int TileHeight, int TileWidth)
        {
            IRasterStorageDef pRasterStorageDef = new RasterStorageDefClass();
            pRasterStorageDef.CompressionType = CompressionType;
            pRasterStorageDef.CompressionQuality = CompressionQuality;
            pRasterStorageDef.PyramidLevel = PyramidLevel;
            pRasterStorageDef.PyramidResampleType = PyramidResampleType;
            pRasterStorageDef.TileHeight = TileHeight;
            pRasterStorageDef.TileWidth = TileWidth;
            return pRasterStorageDef;
        }
        /// <summary>
        /// �����㼣 footprint (bounding box) 
        /// </summary>
        /// <param name="pRasterCatalog">�����µ�դ���Ŀ</param>
        public static void UpdataFootprint(IRasterCatalog pRasterCatalog)
        {
            try
            {
                IRasterCatalogHelper pHelper = new RasterCatalogHelperClass();
                pHelper.UpdateFootprint(pRasterCatalog);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        /// <summary>
        /// �����㼣(֧�ֲ��ָ���,�����������) footprint (bounding box) 
        /// </summary>
        /// <param name="pRasterCatalog">�����µ�դ���Ŀ</param>
        /// <param name="where">��ѯ������䣬Ϊ�յĻ�������ȫ��</param>
        /// <param name="onlyIfEmpty"></param>
        public static void UpdataFootprintEx(IRasterCatalog pRasterCatalog, string where, bool onlyIfEmpty)
        {
            try
            {
                IRasterCatalogHelper pHelper = new RasterCatalogHelperClass();
                IRasterCatalogHelper2 pHelperEx = (IRasterCatalogHelper2)pHelper;
                pHelperEx.UpdateFootprintEx(pRasterCatalog, where, onlyIfEmpty, null);
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
        }

        public static void InitDatasets(ComboBoxEdit cboDatasets, IWorkspace workspace, esriDatasetType DatasetType)
        {
            if (cboDatasets == null) return;
            cboDatasets.Properties.Items.Clear();
            //cboDatasets.Text = "";
            if (workspace == null) return;

            IEnumDatasetName pEnumDatasetName = null;
            IDatasetName pDatasetName = null;
            try
            {
                pEnumDatasetName = workspace.get_DatasetNames(DatasetType);
                pEnumDatasetName.Reset();
                pDatasetName = pEnumDatasetName.Next();
                IPropertySet pPropset = workspace.ConnectionProperties;
                string user = pPropset.GetProperty("USER").ToString().ToUpper();
                string datasetName = "";
                while (pDatasetName != null)
                {
                    datasetName = pDatasetName.Name;
                    if (user.CompareTo(GetUsernameOfDatasetName(datasetName).ToUpper()) == 0)
                    {
                        int index = datasetName.LastIndexOf(".");
                        if (index > 0)
                        {
                            datasetName = datasetName.Substring(index + 1, datasetName.Length - index - 1);
                        }
                        cboDatasets.Properties.Items.Add(datasetName);
                    }
                    pDatasetName = pEnumDatasetName.Next();
                }
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
            }
        }
        private static string GetUsernameOfDatasetName(string datasetName)
        {
            int first = datasetName.IndexOf('.');
            if (first < 0)
            {
                return "";
            }
            int second = datasetName.IndexOf('.', first + 1);
            if (second < 0)
            {
                return datasetName.Substring(0, first);
            }
            else
            {
                return datasetName.Substring(first + 1, second - first - 1);
            }
        }

        public static IList<string> InitDatasets(IWorkspace workspace, esriDatasetType DatasetType)
        {
            if (workspace == null) return null;

            IEnumDatasetName pEnumDatasetName = null;
            IDatasetName pDatasetName = null;
            IList<string> strList = new List<string>();
            try
            {
                pEnumDatasetName = workspace.get_DatasetNames(DatasetType);
                pEnumDatasetName.Reset();
                pDatasetName = pEnumDatasetName.Next();
                IPropertySet pPropset = workspace.ConnectionProperties;
                string UserName = pPropset.GetProperty("USER").ToString();
                while (pDatasetName != null)
                {
                    string DatasetName = pDatasetName.Name;
                    int Index = DatasetName.IndexOf(".");
                    if (Index > 0)
                    {
                        string NameWithoutUser = DatasetName.Substring(Index + 1, DatasetName.Length - Index - 1);
                        if (DatasetName.Substring(0, Index).ToUpper() == UserName.ToUpper())
                        {
                            strList.Add(NameWithoutUser);
                        }
                    }
                    pDatasetName = pEnumDatasetName.Next();
                }
                return strList;
            }
            catch
            {
                return null;
            }
        }


        public static ILayer ConvertRasterCatalogToLayer(IRasterCatalog pCatalog, string layername)
        {
            try
            {
                IGdbRasterCatalogLayer gdbRasterCatalogLayer = new GdbRasterCatalogLayerClass();
                if (gdbRasterCatalogLayer.Setup((ITable)pCatalog) == false)
                {
                    return null;
                }
                IRasterCatalogDisplayProps pRasterDisplay = gdbRasterCatalogLayer as IRasterCatalogDisplayProps;
                pRasterDisplay.UseScale = false;
                pRasterDisplay.DrawRastersOnly = true;
                pRasterDisplay.DisplayRasters = 100;

                ILayer esriGdbRasterCatalogLayer = (ILayer)gdbRasterCatalogLayer;
                esriGdbRasterCatalogLayer.Name = layername;
                return esriGdbRasterCatalogLayer;
            }
            catch
            {
                return null;
            }
        }

        public static ILayer FilterRasterCatalog(IRasterCatalog pCatalog, string catalogName, string rasterName)
        {
            ILayer pLayer = ConvertRasterCatalogToLayer(pCatalog, rasterName);
            IFeatureLayer pFeatlayer = pLayer as IFeatureLayer;
            IFeatureLayerDefinition pFeatDefinition = pFeatlayer as IFeatureLayerDefinition;
            int index = pCatalog.NameFieldIndex;
            string name = pFeatlayer.FeatureClass.Fields.get_Field(index).Name;
            string filter = name + "=" + "'" + rasterName + "'";
            pFeatDefinition.DefinitionExpression = filter;
            return pFeatlayer as ILayer;
        }

        public static ILayer FilterRasterCatalog(IRasterCatalog pCatalog, string catalogName, int dataid)
        {
            ILayer pLayer = ConvertRasterCatalogToLayer(pCatalog, catalogName + dataid.ToString());
            IFeatureLayer pFeatlayer = pLayer as IFeatureLayer;
            IFeatureLayerDefinition pFeatDefinition = pFeatlayer as IFeatureLayerDefinition;
            string filter = CONST_SNAPSHOT.CONST_DATAID + "=" + dataid;
            pFeatDefinition.DefinitionExpression = filter;
            return pFeatlayer as ILayer;
        }

        public static ILayer GetRasterLayer(IRasterCatalog pCatalog, string rasterName)
        {
            IRasterLayer pRasLayer = null;
            IFeatureClass pFeatureClass = null;
            pFeatureClass = pCatalog as IFeatureClass;
            IFeatureCursor pFeatureCursor = null;
            IQueryFilter pQueryFilter = null;
            IRasterCatalogItem pRasterCatalogItem = null;
            IRasterDataset pRasDataset = null;

            pQueryFilter = new QueryFilterClass();

            int index = pCatalog.NameFieldIndex;
            string name = pFeatureClass.Fields.get_Field(index).Name;
            string filter = name + "=" + "'" + rasterName + "'";
            pQueryFilter.WhereClause = filter;

            pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
            pRasterCatalogItem = pFeatureCursor.NextFeature() as IRasterCatalogItem;

            if (pRasterCatalogItem == null)
                return null;

            pRasDataset = pRasterCatalogItem.RasterDataset;


            pRasLayer = new RasterLayerClass();
            pRasLayer.CreateFromDataset(pRasDataset);
            pRasLayer.Name = rasterName;
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);

            return pRasLayer as ILayer;
        }


        public static ILayer GetRasterLayer(IRasterCatalog pCatalog, int dataID)
        {
            IRasterLayer pRasLayer = null;
            IFeatureClass pFeatureClass = null;
            pFeatureClass = pCatalog as IFeatureClass;
            IFeatureCursor pFeatureCursor = null;
            IQueryFilter pQueryFilter = null;
            IRasterCatalogItem pRasterCatalogItem = null;
            IRasterDataset pRasDataset = null;

            pQueryFilter = new QueryFilterClass();

            string filter = CONST_SNAPSHOT.CONST_DATAID + "=" + dataID;
            pQueryFilter.WhereClause = filter;

            pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            string rasterName = pFeature.get_Value(pCatalog.NameFieldIndex).ToString();
            pRasterCatalogItem = pFeature as IRasterCatalogItem;

            if (pRasterCatalogItem == null)
                return null;

            pRasDataset = pRasterCatalogItem.RasterDataset;


            pRasLayer = new RasterLayerClass();
            pRasLayer.CreateFromDataset(pRasDataset);
            pRasLayer.Name = rasterName;
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureCursor);

            return pRasLayer as ILayer;
        }

        public static bool DeleteRaster(IWorkspace workspace, string catalogName, int dataID)
        {
            try
            {
                IRasterCatalog rasterCatalog = OpenRasterCatalog(workspace as IRasterWorkspaceEx, catalogName);
                IFeatureClass fCls = rasterCatalog as IFeatureClass;
                IFeatureCursor fCursor = null;
                //IRasterCatalogItem rasterCatalogItem = null;
                //IRasterDataset rasterDataset = null;
                IQueryFilter queryFilter = new QueryFilter();

                queryFilter.WhereClause = CONST_SNAPSHOT.CONST_DATAID + "=" + dataID;
                fCursor = fCls.Search(queryFilter, true);
                IFeature feature = fCursor.NextFeature();
                if (feature != null)
                {
                    feature.Delete();
                }
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fCursor);

                return true;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// ��֤RasterCatalog���������֡���ĸ�����ֺ��»��ߣ��Ҳ��������ֿ�ͷ��
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidateRasterCatalogName(string name)
        {
            Regex regex = new Regex("^[a-zA-Z_\u4e00-\u9fa5][a-zA-z_0-9\u4e00-\u9fa5]*$");
            Match match = regex.Match(name);
            return match.Success;
        }
    }
}
