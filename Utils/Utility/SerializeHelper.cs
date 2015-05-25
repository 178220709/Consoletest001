using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;

namespace Feature.Zhaogang.SpartanLv.Common.Utility
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// Xml反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xml)
        {
            var t = default(T);
            var serialize = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var xDocument = XDocument.Parse(xml, LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);

            using (var memoryStream = new System.IO.MemoryStream())
            {
                xDocument.Save(memoryStream);
                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                t = (T)serialize.Deserialize(memoryStream);
            }

            return t;
        }

        /// <summary>
        /// Xml序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T t)
        {
            var xml = string.Empty;
            var serialize = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                serialize.Serialize(memoryStream, t);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memoryStream))
                {
                    xml = streamReader.ReadToEnd();
                }
            }
            return xml;
        }

        /// <summary>
        /// Json契约反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonContractDeserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var t = Activator.CreateInstance(typeof(T));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                t = serializer.ReadObject(memoryStream);
            }
            return (T)t;
        }

        /// <summary>
        /// Json契约序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonContractSerialize(dynamic obj)
        {
            var json = string.Empty;
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, obj);
                json = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return json;
        }
    }
}
