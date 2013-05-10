using System;
using System.Collections.Generic;
using System.Xml;
using Consoletest001.XML.Utility;

namespace Consoletest001.XML.DAL
{
    public class RecordDal
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
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            XmlNode root = xmlDoc.SelectSingleNode("records");
            XmlElement xe1 = xmlDoc.CreateElement("record"); //创建一个<book>节点
            xe1.SetAttribute("id", id); //设置该节点项目属性
            xe1.SetAttribute("project", project); //设置该节点项目属性
            xe1.SetAttribute("price", price); //设置该节点价格属性
            xe1.SetAttribute("sex", sex); //设置该节点顾客性别属性
            xe1.SetAttribute("time", time); //设置该节点时间属性2013-1-19 22:23:32
            XmlElement xesub1 = xmlDoc.CreateElement("remark");
            xesub1.InnerText = remark; //设置文本节点
            xe1.AppendChild(xesub1); //添加到<book>节点中
            if (root != null) root.AppendChild(xe1); //添加到<bookstore>节点中
            //添加完之后 记得把records的信息同步更新，这里是xml写入信息的唯一入口，
            //在这里可以保证信息的同步
            XmlElement rootElement = (XmlElement) root;
            int countTemp =Int32.Parse( rootElement.GetAttribute("count"));
            countTemp++;
            rootElement.SetAttribute("count", countTemp.ToString());
            string TotalPriceStrTemp =  rootElement.GetAttribute("totalprice");
            TextManager.AddForStr(ref TotalPriceStrTemp, price);
            rootElement.SetAttribute("totalprice", TotalPriceStrTemp);

            xmlDoc.Save(xmlPath);

            return xe1;
        }

        public static RecordDal GetRecord(string xmlPath, string id)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            RecordDal dal = new RecordDal();
            dal.Id = id;
            var records = xmlDoc.SelectSingleNode("records");
            if (records != null)
            {
                XmlNodeList nodeList = records.ChildNodes; //获取records节点的所有子节点
                foreach (var xn in nodeList) //遍历所有子节点
                {
                    XmlElement xe = (XmlElement) xn; //将子节点类型转换为XmlElement类型
                    if (xe.GetAttribute("id") == id)
                    {
                        dal.Project = xe.GetAttribute("project");
                        dal.Price = xe.GetAttribute("price");
                        dal.Sex = xe.GetAttribute("sex");
                        XmlElement xesub = (XmlElement) xe.FirstChild;
                        dal.Remark = xesub.InnerText;
                        return dal;
                    }
                }
            }
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