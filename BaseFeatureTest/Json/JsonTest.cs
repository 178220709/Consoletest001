using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BaseFeatureTest.Json    
{

    [TestClass()]
    public class JsonTest
    {

        [TestMethod()]
        public void Test1()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartArray();

            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue("zhangsan");
            writer.WriteEndObject();
            writer.WriteEndArray();

            writer.Flush();

            string jsonText = sw.GetStringBuilder().ToString();
        }


        [TestMethod()]
        public void Test2()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartArray();
            writer.WriteValue("JSON!");
            writer.WriteValue(1);
            writer.WriteValue(true);
            writer.WriteStartObject();
            writer.WritePropertyName("property");
            writer.WriteValue("value");
            writer.WriteEndObject();
            writer.WriteEndArray();

            writer.Flush();

            string jsonText = sw.GetStringBuilder().ToString();

            Console.WriteLine(jsonText);
            // ['JSON!',1,true,{property:'value'}]
        }
    }
}
