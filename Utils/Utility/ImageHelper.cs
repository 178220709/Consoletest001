using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Suijing.Utils.Utility
{
    /// <summary>
    /// 图片处理帮助类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>  
        /// 图像转换为Base64编码  
        /// </summary>  
        /// <param name="image">图像</param>
        /// <returns>转换成功返回其Base64编码；失败返回空串</returns>  
        public static string ImageToBase64(Image image)
        {
            return ImageToBase64(image, ImageFormat.Png);
        }

        /// <summary>  
        /// 图像转换为Base64编码  
        /// </summary>  
        /// <param name="image">图像</param>  
        /// <param name="format">图像格式</param>  
        /// <returns>转换成功返回其Base64编码；失败返回空串</returns>  
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            string base64String = string.Empty;

            if (image == null)
            {
                return base64String;
            }

            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                base64String = Convert.ToBase64String(imageBytes);
            }
            return base64String;
        }

        /// <summary>
        /// 转换制定大小的图片 然后转换为Base64编码  
        /// </summary>
        /// <param name="imageBase64">图像base64编码</param>
        /// <param name="thumbWidth">请求的缩略图的宽度（以像素为单位）</param>
        /// <param name="thumbHeight">请求的缩略图的高度（以像素为单位）</param>
        /// <returns></returns>
        public static string ThumbnailToBase64(string imageBase64, int thumbWidth = 95, int thumbHeight = 90)
        {
            if (string.IsNullOrEmpty(imageBase64))
            {
                return string.Empty;
            }
            var image = Base64ToImage(imageBase64);

            return ThumbnailToBase64(image, thumbWidth, thumbHeight);
        }

        /// <summary>
        /// 转换制定大小的图片 然后转换为Base64编码  
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="thumbWidth">请求的缩略图的宽度（以像素为单位）</param>
        /// <param name="thumbHeight">请求的缩略图的高度（以像素为单位）</param>
        /// <returns></returns>
        public static string ThumbnailToBase64(Image image, int thumbWidth = 95, int thumbHeight = 90)
        {
            if (image == null)
            {
                return string.Empty;
            }

            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(() => false);
            var thumbImage = image.GetThumbnailImage(thumbWidth, thumbHeight, callback, IntPtr.Zero);
            return ImageToBase64(thumbImage);
        }

        /// <summary>
        /// 固定宽度定制图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fixWidth"></param>
        /// <returns></returns>
        public static string FixWidthThumbnailToBase64(Image image, int fixWidth = 800)
        {
            if (image == null)
            {
                return string.Empty;
            }
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(() => false);
            //缩放比例
            var ratio = Math.Round((decimal)fixWidth / (decimal)image.Width, 2);
            //新的高度
            var newHeight = (int)Math.Ceiling(image.Height * ratio);

            var thumbImage = image.GetThumbnailImage(fixWidth, newHeight, callback, IntPtr.Zero);

            return ImageToBase64(thumbImage);
        }



        /// <summary>  
        /// Base64编码转换为图像  
        /// </summary>  
        /// <param name="base64String">Base64字符串</param>  
        /// <returns>转换成功返回图像；失败返回null</returns>  
        public static Image Base64ToImage(string base64String)
        {
            Image image = null;

            if (string.IsNullOrEmpty(base64String))
            {
                return image;
            }

            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
            }

            return image;
        }
    }
}
