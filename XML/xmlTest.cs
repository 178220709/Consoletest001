using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Consoletest001.XML.DAL;
using Consoletest001.XML.Utility;

namespace Consoletest001.XML
{
   public class XmlTest
   {

       private static  string path = "XMLData/everyday/20130119.xml";

        public static  void mainsdf1()
        {
        
            string str5 = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");


            DataSet ds = XmlHelper.GetXml("XMLData/everyday/20130119.xml");

           

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLData/everyday/20130119.xml");
            XmlNode root = xmlDoc.SelectSingleNode("records");//查找<bookstore>
            XmlElement xe1 = xmlDoc.CreateElement("record");//创建一个<book>节点
            xe1.SetAttribute("project", "洗头");//设置该节点项目属性
            xe1.SetAttribute("price", "20");//设置该节点价格属性
            xe1.SetAttribute("sex", "20");//设置该节点顾客性别属性
            xe1.SetAttribute("time", "2013-1-19 22:23:32");//设置该节点时间属性
            XmlElement xesub1 = xmlDoc.CreateElement("remark");
            xesub1.InnerText = "这是备注信息。";//设置文本节点
            xe1.AppendChild(xesub1);//添加到<book>节点中
           
            root.AppendChild(xe1);//添加到<bookstore>节点中
            xmlDoc.Save("XMLData/everyday/20130119.xml");

        }

        public static void mainsdsdf2()
        {
           
          //  RecordDal.SaveStrToNode(path, "5", "剪头", "25", "男","2013-2-1 0:00:18", "dfg");
            string str = "我的是abcd";
           byte [] bs =  Encoding.UTF8.GetBytes(str);
            Console.WriteLine( bs.ToString());
            byte[] bs2 = Encoding.Unicode.GetBytes(str);
            Console.WriteLine(bs.ToString());
            Encoding bs3 = Encoding.GetEncoding("utf-8");
           string strr =  bs3.GetString(bs);
            Console.WriteLine(bs.Length);
            byte[] bs4 = Encoding.UTF8.GetBytes(str);
            Console.WriteLine(bs.Length);
               
        }

       public XmlTest()
        {
            
        }

    
    }
}