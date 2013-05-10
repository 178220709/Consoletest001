using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    public class ImageResample
    {
        public static bool Resample(int width, int height, string strImageFile, string fileName)
        {
            Bitmap srcImage = null, destImg = null;
            Graphics g = null;
            try
            {
                srcImage = new Bitmap(strImageFile);
                //ͼƬ�ļ�=strImageFile,ֱ��NEW��ĳͼƬ�ļ�ʱ����ͼƬ̫̫�����OutOfMemory���쳣
                destImg = new Bitmap(width, height);
                Rectangle srcRec = new Rectangle(0, 0, srcImage.Width, srcImage.Width);
                Rectangle destRec = new Rectangle(0, 0, width, height);
                g = Graphics.FromImage(destImg);
                g.DrawImage(srcImage, destRec, srcRec, GraphicsUnit.Pixel);
                destImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);//����ϴ�Jpeg,�Ƚ������ڴ档

                return true;
            }
            catch (Exception ex)
            {
                throw;
                //return false;
            }
            finally
            {
                if (g != null) { g.Dispose(); g = null; }
                if (destImg != null) { destImg.Dispose(); destImg = null; }
                if (srcImage != null) { srcImage.Dispose(); srcImage = null; }
                GC.Collect(); GC.WaitForPendingFinalizers();
            }
        }

        public static bool ResampleEx(int width, int height, string strImageFile, string fileName)
        {
            Image sourceImg = null;
            Image destImg = null;
            try
            {
                sourceImg = new Bitmap(strImageFile);
                if (sourceImg.Height > height && sourceImg.Width > width)
                {
                    destImg = sourceImg.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    destImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    sourceImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                return true;
            }
            catch (Exception ex)
            {
                //throw;
                return false;
            }
            finally
            {
                if (sourceImg != null) { sourceImg.Dispose(); sourceImg = null; }
                if (destImg != null) { destImg.Dispose(); destImg = null; }
                GC.Collect(); GC.WaitForPendingFinalizers();
            }
        }

    }
}
