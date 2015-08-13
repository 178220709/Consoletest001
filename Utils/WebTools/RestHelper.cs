using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Suijing.Utils.WebTools
{

    public class RestResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public T Result { get; set; }
    }

    public class RestHelper
    {
        public static string getIp()
        {
            string clientIp = "";
            try
            {
                clientIp = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return clientIp;
        }




        public static RestResult<T> Get<T>(string url)
        {
            var model = new RestResult<T>();
            // Create the web request
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());
                try
                {
                    string str = reader.ReadToEnd();
                    model.Result = JsonConvert.DeserializeObject<T>(str);
                    model.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    model.IsSuccess = false;
                    model.Msg = "发生异常,接口调用成功,但序列化失败" + ex.Message;
                    model.Result = default(T);
                }
                // Console application output
                Console.WriteLine(reader.ReadToEnd());
            }
            return model;
        }

        public static RestResult<T> Post<T>(string url, Dictionary<string, string> paras = null, string name = "", string psw = "")
        {
            var model = new RestResult<T>();
            Uri address = new Uri(url);

            // Create the web request
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // Create the data we want to send
            StringBuilder data = new StringBuilder();
            if (paras != null)
            {
                foreach (var item in paras)
                {
                    data.Append(string.Format("&{0}=", item.Key) + HttpUtility.UrlEncode(item.Value));
                }
            }

            data.Append("&paratest=" + HttpUtility.UrlEncode("para中文test:" + DateTime.Now.ToString("HH:mm:ss")));

            // Create a byte array of the data we want to send
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            // Set the content length in the request headers
            request.ContentLength = byteData.Length;

            // Write data
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());
                try
                {
                    model.Result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    model.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    model.IsSuccess = false;
                    model.Msg = "发生异常,接口调用成功,但序列化失败" + ex.Message;
                    model.Result = default(T);
                }
            }
            return model;
        }


        public void ddd()
        {
            // Create the web request
            HttpWebRequest request
            = WebRequest.Create("https://api.del.icio.us/v1/posts/recent") as HttpWebRequest;

            // Add authentication to request
            request.Credentials = new NetworkCredential("username", "password");

            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output
                Console.WriteLine(reader.ReadToEnd());
            }

        }
    }
}
