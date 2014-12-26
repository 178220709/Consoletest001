using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace BaseFeatureDemo.image
{
    internal class ImageDemo
    {

        public static void maingsdf()
        {
            GetNewMap("e:\\test.jpg");
            
        }

        /// Resize图片 
        /// 原始Bitmap 
        /// 新的宽度 
        /// 新的高度 
        /// 保留着，暂时未用 
        /// 处理以后的图片 
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }


        public static void GetNewMap(string path)
        {
            Bitmap b1 = new Bitmap(path);
            int width = b1.Width, height = b1.Height;
            //设置好需要的宽高
            Bitmap b = new Bitmap(width*4 + 16, height*2 + 4);
            //取得画刷
            Graphics g = Graphics.FromImage(b);
            //画八张图
            for (int i = 0; i < 4; i++)
            {
                g.DrawImage(b1, (width + 2)*i, 0);
                g.DrawImage(b1, (width + 2)*i, height + 2);
            }

            //把合并后的位图保存到文件流中 
            int tempHigh = b1.Height;
            FileStream fs = new FileStream("e:\\result.jpg", FileMode.Create);

            b.Save(fs, ImageFormat.Jpeg);
            fs.Flush();
            fs.Close();
        }
    }
}
