using System;
using System.Collections.Generic;
using System.Xml;
using BaseFeatureDemo.XML.Utility;

namespace BaseFeatureDemo.XML.DAL
{
    public class RecordInfo
    {
        public string Id { get; private set; }
        public string Project { get; private set; }
        public string Price { get; private set; }
        public string Sex { get; private set; }
        public string Time { get; private set; }
        public string Remark { get; private set; }

        #region 静态公有方法

        public static XmlElement SaveStrToNode(string xmlPath, string id, string project, string price, string sex,
                                               string time,
                                               string remark)
        {

            return null;
        }

      

        public static void GetMaxId(string xmlPath, out int count, out int totalprice)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode root = xmlDoc.SelectSingleNode("records");
            XmlElement xe1 = (XmlElement) root;
            count = Int32.Parse(xe1.GetAttribute("count"));
            totalprice = Int32.Parse(xe1.GetAttribute("totalprice"));
        }

        #endregion

        //        public static int GetMaxId(string xmlPath)
//        {
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.Load(xmlPath);
//            var records = xmlDoc.SelectSingleNode("records");
//            records.
        //        }

        #region 静态私有方法



        #endregion
    }

   
}