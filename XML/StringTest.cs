using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace Consoletest001.otherThing
{
   public class XMLTest
    {

        public static  void mainsdf()
        {
            string pathstr = @"\\192.98.15.4\Public\source\SC\第一批\Orbit_000929_Product\ZY3\BWD\Level1\ZY3_01a_mynbavp_883174_20120310_104056_0008_SASMAC_CHN_sec_rel_001_1205268307\ZY3_01a_mynbavp_883174_20120310_104056_0008_SASMAC_CHN_sec_rel_001_1205268307.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathstr);
            XmlNode xmlNode = xmlDoc.FirstChild;
          
            string str1 = xmlNode.InnerText;
            XmlNode node2 = xmlDoc.SelectSingleNode(@"sensor_corrected_metadata/productInfo/SatelliteID");
            string s11 = node2.ChildNodes[0].InnerText;
            string s22= node2.InnerText;
            object on = new  object();

            bool  i2 = node2.Equals(node2.ChildNodes[0]);
            string s2 = node2.InnerText;
            XmlNode xmlNode3 = xmlDoc.SelectSingleNode( @"sensor_corrected_metadata/productInfo");
            XmlNode childNodes;
            IDictionary<string, string> dicFileds = new Dictionary<string, string>();
            for (int i = 0; i < xmlNode3.ChildNodes.Count; i++)
            {
                childNodes = xmlNode3.ChildNodes[i];
                if (!string.IsNullOrEmpty(childNodes.InnerText))
                {
                    dicFileds.Add(childNodes.Name, childNodes.InnerText);
                    Console.WriteLine(childNodes.Name+"====>"+childNodes.InnerText);
                }
                
            }

        }

       public XMLTest()
        {
            
        }

    
    }
}