using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace BaseFeatureDemo.Reflect
{


    [TestClass()]
    public class Test
    {

        [TestMethod()]
        public void Test1()
        {
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

           
         
            writer.WriteStartObject();
            writer.WritePropertyName("11");
            writer.WriteValue("zhangsan");
          
            writer.WritePropertyName("22");
            writer.WriteValue(18);
           
            writer.WritePropertyName("33");
            writer.WriteValue("222");
            writer.WriteEndObject();
           

            writer.Flush();

            string jsonText = sw.GetStringBuilder().ToString();

            var obj = JsonConvert.DeserializeObject(jsonText);
            string jsonStr2 = JsonConvert.SerializeObject(obj);

            Assert.AreEqual(jsonStr2,jsonText);
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
