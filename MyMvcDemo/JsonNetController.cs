using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Mvc;
using Suijing.Utils;

namespace MyMvcDemo
{
    public class JsonNetResult : JsonResult
    {
        private readonly JsonType _type ;
       
        public JsonNetResult(JsonType type)
        {
            this._type = type;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var json = _type.SerializeWithMine(Data);
                response.Output.Write(json);
                response.Output.Flush();
            }
        }
    }

    public enum JsonType
    {
        ServiceStack,
        JsonDotNet
    }

    public static class JsonConverterFactory
    {
        public static string SerializeWithMine(this JsonType type,object obj)
        {
            switch (type)
            {
                case JsonType.JsonDotNet:
                    return    JsonConvert.SerializeObject(obj);
                case JsonType.ServiceStack:
                    return obj.ToJson();
                default :
                    return obj.ToJson();
            }
        }
    }


    public class JsonNetController : Controller
    {
        protected override JsonResult Json(object data, string contentType,
                  Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (behavior == JsonRequestBehavior.DenyGet
                && string.Equals(this.Request.HttpMethod, "GET",
                                 StringComparison.OrdinalIgnoreCase))
                //Call JsonResult to throw the same exception as JsonResult
                return new JsonResult();

            return new JsonNetResult(JsonType.JsonDotNet)
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }
    }
}
