using System;
using System.Web.Helpers;

namespace JsonSong.Front.Extend
{
    /// <summary>
    /// 标记该controller返回jsonResult时序列化使用哪种库,默认是json.net
    /// </summary>
    public class JsonTypeAttribute : Attribute
    {
        public JsonTypeAttribute(JsonType type)
        {
            JsonType = type;
        }

        public JsonType JsonType { get; set; } 


    }
}