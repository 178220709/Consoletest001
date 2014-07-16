using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace MyMvcDemo
{
    public static class Extensions
    {
        public static byte[] GetCaptcha(this Controller controller, string sessionKey)
        {
            using (var ms = new MemoryStream())
            {
                var image = new CaptchaImage(5, 18, false);

                controller.Session[sessionKey] = image;

                image.CaptchaFrame.Save(ms, ImageFormat.Jpeg);

                return ms.GetBuffer();
            }
        }

        public static bool ValidateCaptcha(this Controller controller, string captcha, string key)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(captcha))
            {
                var capt = controller.Session[key] as CaptchaImage;

                if (capt != null && !string.IsNullOrWhiteSpace(capt.CaptchaCode))
                {
                    result = captcha.Equals(capt.CaptchaCode, StringComparison.OrdinalIgnoreCase);
                }
            }

            return result;
        }


      
    }
}