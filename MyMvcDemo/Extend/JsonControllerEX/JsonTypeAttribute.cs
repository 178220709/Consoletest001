using System;
using System.Web.Helpers;

namespace MyMvcDemo.Extend
{
    /// <summary>
    /// 标记该controller和里面的action方法是否用于首页菜单
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