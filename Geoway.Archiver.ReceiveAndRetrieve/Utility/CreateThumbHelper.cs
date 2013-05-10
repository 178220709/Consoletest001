using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class CreateThumbHelper
    {
        public static int size = 64;
        public static double CalThumbSize(string filePath)
        {
            try
            {
                IRasterDataset pRasterDataset = RasterDataOperater.OpenRasterDataset(filePath);
                IRasterProps pRasProps = pRasterDataset.CreateDefaultRaster() as IRasterProps;
                int width = pRasProps.Width / size;
                int height = pRasProps.Height / size;
                double meanSize = width >= height ? width : height;
                meanSize = meanSize * pRasProps.MeanCellSize().X;
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset);
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasProps);
                return meanSize;
            }
            catch (System.Exception ex)
            {
                return 0;
            }

        }

        public static bool CreateGISThumb(string filePath, string targetPath)
        {
            double cellsize = 0;
            cellsize = CalThumbSize(filePath);
            bool success = RasterDataOperater.ResampleRaster(filePath, System.IO.Path.GetDirectoryName(targetPath), System.IO.Path.GetFileName(targetPath), cellsize, ESRI.ArcGIS.GeoAnalyst.esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleBilinear);
            return success;
        }

        public static bool CreateThumb(string filePath, string targetPath)
        {
            double cellsize = 0;
            cellsize = CalThumbSize(filePath);
            bool success = ImageResample.ResampleEx(size, size, filePath, targetPath);
            return success;
        }
    }
}
