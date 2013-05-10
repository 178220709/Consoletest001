using System;
using System.Data;
using System.IO;
using System.Xml;

namespace Consoletest001.XML
{

    /**//// <summary>
    /// XML 操作基类
    /// </summary>
    public class XmlHelper : IDisposable
    {
        //以下为单一功能的静态类

        #region 读取XML到DataSet
        /**//// <summary>
        /// 功能:读取XML到DataSet中
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <returns>DataSet</returns>
        public static DataSet GetXml(string XmlPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(@XmlPath);
            return ds;
        }
        #endregion

        #region 读取xml文档并返回一个节点
        /**//// <summary>
        /// 读取xml文档并返回一个节点:适用于一级节点
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="NodeName">节点</param>
        /// <returns></returns>
        public static string ReadXmlReturnNode(string XmlPath, string Node)
        {
            XmlDocument docXml = new XmlDocument();
            docXml.Load(@XmlPath);
            XmlNodeList xn = docXml.GetElementsByTagName(Node);
            return xn.Item(0).InnerText.ToString();
        }
        #endregion

        #region 查找数据,返回一个DataSet
        /**//// <summary>
        /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
        /// <returns></returns>
        public static DataSet GetXmlData(string xmlPath, string XmlPathNode)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds;
        }


        #endregion

        #region 更新Xml节点内容
        /**//// <summary>
        /// 更新Xml节点内容
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="Node">要更换内容的节点:节点路径 根节点/父节点/当前节点</param>
        /// <param name="Content">新的内容</param>
        public static void XmlNodeReplace(string xmlPath, string Node, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            objXmlDoc.SelectSingleNode(Node).InnerText = Content;
            objXmlDoc.Save(xmlPath);

        }
        #endregion

        #region 更改节点的属性值
        /**//// <summary>
        /// 更改节点的属性值
        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径</param>
        /// <param name="NodeAttribute1">要更改的节点属性的名称</param>
        /// <param name="NodeAttributeText">更改的属性值</param>
        public static void XmlAttributeEide(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);

        }
        public static void XmlAttributeEide(string xmlPath, string strNode, string strAttribute1, string context1, string strAttribute2, string context2, string strAttribute3, string context3, string strAttribute4, string context4, string strAttribute5, string context5)
        {
           
            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                XmlNodeList childlist = xe.ChildNodes;
                foreach (XmlNode x in childlist)
                {
                    XmlElement e1 = (XmlElement)x;
                    XmlNodeList childlist1 = e1.ChildNodes;
                    foreach (XmlNode x1 in childlist1)
                    {
                        XmlElement e = (XmlElement)x1;

                        if (e.Attributes[strAttribute1].Value == context1)
                        {
                            e.ParentNode.Attributes[strAttribute2].Value = context2;
                            e.ParentNode.Attributes[strAttribute3].Value = context3;
                            e.Attributes[strAttribute4].Value = context4;
                            e.ParentNode.ParentNode.Attributes[strAttribute5].Value = context5;
                            docxml.Save(xmlPath);
                            return;
                        }
                    }
                }

            }
            
        }
        public static void XmlAttributeEide(string xmlPath, string strNode, string strAttribute1, string context1,  string strAttribute3, string context3, string strAttribute4, string context4, string strAttribute5, string context5)
        {

            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                XmlNodeList childlist = xe.ChildNodes;
                foreach (XmlNode x in childlist)
                {
                    XmlElement e1 = (XmlElement)x;
                    XmlNodeList childlist1 = e1.ChildNodes;
                    foreach (XmlNode x1 in childlist1)
                    {
                        XmlElement e = (XmlElement)x1;

                        if (e.Attributes[strAttribute1].Value == context1)
                        {
                          //  e.ParentNode.Attributes[strAttribute2].Value = context2;
                            e.ParentNode.Attributes[strAttribute3].Value = context3;
                            e.Attributes[strAttribute4].Value = context4;
                            e.ParentNode.ParentNode.Attributes[strAttribute5].Value = context5;
                            docxml.Save(xmlPath);
                            return;
                        }
                    }
                }

            }

        }
        #endregion

        #region 删除XML节点和此节点下的子节点
        /**//// <summary>
        /// 删除XML节点和此节点下的子节点
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="Node">节点路径</param>
        public static void XmlNodeDelete(string xmlPath, string Node)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
            objXmlDoc.Save(xmlPath);
        }

        #endregion

        #region 删除一个节点的属性
        /**//// <summary>
        /// 删除一个节点的属性
        /// </summary>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="NodePath">节点路径（xpath）</param>
        /// <param name="NodeAttribute">属性名称</param>
        public static void xmlnNodeAttributeDel(string xmlPath, string NodePath, string NodeAttribute)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlElement xe = (XmlElement)nodePath;
            xe.RemoveAttribute(NodeAttribute);
            objXmlDoc.Save(xmlPath);

        }
        #endregion

        #region 插入一个节点和此节点的子节点
        /**//// <summary>
        /// 插入一个节点和此节点的字节点
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="MailNode">当前节点路径</param>
        /// <param name="ChildNode">新插入节点</param>
        /// <param name="Element">插入节点的子节点</param>
        /// <param name="Content">子节点的内容</param>
        public static void XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }

        #endregion

        #region 向一个节点添加属性
       /**//// <summary>
       /// 向一个节点添加属性
       /// </summary>
       /// <param name="xmlPath">xml文件路径</param>
       /// <param name="NodePath">节点路径</param>
       /// <param name="NodeAttribute1">要添加的节点属性的名称</param>
       /// <param name="NodeAttributeText">要添加属性的值</param>
        public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(NodeAttribute1);
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            nodePath.Attributes.Append(nodeAttribute);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText);
            objXmlDoc.Save(xmlPath);
        }
        public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText1, string NodeAttribute2, string NodeAttributeText2, string NodeAttribute3, string NodeAttributeText3, string NodeAttribute4, string NodeAttributeText4, string NodeAttribute5, string NodeAttributeText5)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath); 
            XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
            XmlAttribute nodeAttribute1 = objXmlDoc.CreateAttribute(NodeAttribute1);
            XmlAttribute nodeAttribute2 = objXmlDoc.CreateAttribute(NodeAttribute2);
            XmlAttribute nodeAttribute3 = objXmlDoc.CreateAttribute(NodeAttribute3);
            XmlAttribute nodeAttribute4 = objXmlDoc.CreateAttribute(NodeAttribute4);
            XmlAttribute nodeAttribute5= objXmlDoc.CreateAttribute(NodeAttribute5);
            nodePath.Attributes.Append(nodeAttribute1);
            nodePath.Attributes.Append(nodeAttribute2);
            nodePath.Attributes.Append(nodeAttribute3);
            nodePath.Attributes.Append(nodeAttribute4);
            nodePath.Attributes.Append(nodeAttribute5);
            XmlElement xe = (XmlElement)nodePath;
            xe.SetAttribute(NodeAttribute1, NodeAttributeText1);
            xe.SetAttribute(NodeAttribute2, NodeAttributeText2);
            xe.SetAttribute(NodeAttribute3, NodeAttributeText3);
            xe.SetAttribute(NodeAttribute4, NodeAttributeText4);
            xe.SetAttribute(NodeAttribute5, NodeAttributeText5);
            objXmlDoc.Save(xmlPath);
        }
        #endregion

        #region 插入一节点,带一属性
        /**//// <summary>
        /// 插入一节点,带一属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribContent">属性值</param>
        /// <param name="Content">新节点值</param>
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
           
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        #endregion
      
        
        #region 插入一节点不带属性

        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element)
        {
            try
            {
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(xmlPath);
                XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
                XmlElement objElement = objXmlDoc.CreateElement(Element);

                objNode.AppendChild(objElement);
                objXmlDoc.Save(xmlPath);
            }
            catch
            { }
        }
        #endregion
        #region 插入一节点带2属性
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content,string Attrib1, string AttribContent1,  string Attrib2, string AttribContent2)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib1, AttribContent1);
            objElement.SetAttribute(Attrib2, AttribContent2);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        #endregion
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element,  string Attrib1, string AttribContent1, string Attrib2, string AttribContent2)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib1, AttribContent1);
            objElement.SetAttribute(Attrib2, AttribContent2);
           
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib1, string AttribContent1, string Attrib2, string AttribContent2, string Attrib3, string AttribContent3, string Attrib4, string AttribContent4)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib1, AttribContent1);
            objElement.SetAttribute(Attrib2, AttribContent2);
            objElement.SetAttribute(Attrib3, AttribContent3);
            objElement.SetAttribute(Attrib4, AttribContent4);
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        #region 在根节点下添加父节点
        /**/
        /// <summary>
        /// 在根节点下添加父节点
        /// </summary>
        public static void AddParentNode(string xmlPath,string parentNode)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            // 创建一个新的menber节点并将它添加到根节点下
            XmlElement Node = xdoc.CreateElement(parentNode);
            xdoc.DocumentElement.PrependChild(Node);
            xdoc.Save(xmlPath);
        }
        #endregion
     
        //必须创建对象才能使用的类

        private bool _alreadyDispose = false;
        private XmlDocument xmlDoc = new XmlDocument();
      

        private XmlNode xmlNode;
        private XmlElement xmlElem;

        #region 构造与释构
        public XmlHelper()
        {

        }
        ~XmlHelper()
        {
            Dispose();
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            if (isDisposing)
            {
                //
            }
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 创建xml文档


        #region 创建一个只有声明和根节点的XML文档
        /**//// <summary>
        /// 创建一个只有声明和根节点的XML文档
        /// </summary>
        /// <param name="root"></param>
        public void CreateXmlRoot(string root)
        {
            //加入XML的声明段落
           // xmlNode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "GB2312", null);
            xmlDoc.InsertBefore(xmlDecl, xmlDoc.DocumentElement);
           // xmlDoc.AppendChild(xmlNode);
            //加入一个根元素
            XmlNode RootNode = xmlDoc.CreateNode(XmlNodeType.Element, root, "");
           
            xmlDoc.AppendChild(RootNode);

        }
        #endregion

        #region 在当前节点下插入一个空节点节点
        /**//// <summary>
        /// 在当前节点下插入一个空节点节点
        /// </summary>
        /// <param name="mainNode">当前节点路径</param>
        /// <param name="node">节点名称</param>
        public void CreatXmlNode(string mainNode, string node)
        {
            XmlNode MainNode = xmlDoc.SelectSingleNode(mainNode);
            XmlElement objElem = xmlDoc.CreateElement(node);
            MainNode.AppendChild(objElem);
        }
        #endregion

        #region 在当前节点插入一个仅带值的节点
        /**//// <summary>
        ///  在当前节点插入一个仅带值的节点
        /// </summary>
        /// <param name="mainNode">当前节点</param>
        /// <param name="node">新节点</param>
        /// <param name="content">新节点值</param>
        public void CreatXmlNode(string mainNode, string node, string content)
        {
            XmlNode MainNode = xmlDoc.SelectSingleNode(mainNode);
            XmlElement objElem = xmlDoc.CreateElement(node);
            objElem.InnerText = content;
            MainNode.AppendChild(objElem);
        }
        #endregion

        #region 在当前节点的插入一个仅带属性值的节点
        /**//// <summary>
        /// 在当前节点的插入一个仅带属性值的节点
        /// </summary>
        /// <param name="MainNode">当前节点或路径</param>
        /// <param name="Node">新节点</param>
        /// <param name="Attrib">新节点属性名称</param>
        /// <param name="AttribValue">新节点属性值</param>
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue)
        {
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode);
            XmlElement objElem = xmlDoc.CreateElement(Node);
            objElem.SetAttribute(Attrib, AttribValue);
            mainNode.AppendChild(objElem);
        }
        #endregion

        #region 创建一个带属性值的节点值的节点
        /**//// <summary>
        /// 创建一个带属性值的节点值的节点
        /// </summary>
        /// <param name="MainNode">当前节点或路径</param>
        /// <param name="Node">节点名称</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribValue">属性值</param>
        /// <param name="Content">节点传情</param>
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue, string Content)
        {
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode);
            XmlElement objElem = xmlDoc.CreateElement(Node);
            objElem.SetAttribute(Attrib, AttribValue);
            objElem.InnerText = Content;
            mainNode.AppendChild(objElem);
        }
        #endregion

        #region 在当前节点的插入一个仅带2个属性值的节点
        /**//// <summary>
        ///  在当前节点的插入一个仅带2个属性值的节点
        /// </summary>
        /// <param name="MainNode">当前节点或路径</param>
        /// <param name="Node">节点名称</param>
        /// <param name="Attrib">属性名称一</param>
        /// <param name="AttribValue">属性值一</param>
        /// <param name="Attrib2">属性名称二</param>
        /// <param name="AttribValue2">属性值二</param>
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue, string Attrib2, string AttribValue2)
        {
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode);
            XmlElement objElem = xmlDoc.CreateElement(Node);
            objElem.SetAttribute(Attrib, AttribValue);
            objElem.SetAttribute(Attrib2, AttribValue2);
            mainNode.AppendChild(objElem);
        }
        #endregion

        #region 在当前节点插入带两个属性的节点
        /**//// <summary>
        ///  在当前节点插入带两个属性的节点
        /// </summary>
        /// <param name="MainNode">当前节点或路径</param>
        /// <param name="Node">节点名称</param>
        /// <param name="Attrib">属性名称一</param>
        /// <param name="AttribValue">属性值一</param>
        /// <param name="Attrib2">属性名称二</param>
        /// <param name="AttribValue2">属性值二</param>
        /// <param name="Content">节点值</param>
        public void CreatXmlNode(string MainNode, string Node, string Attrib, string AttribValue, string Attrib2, string AttribValue2, string Content)
        {
            XmlNode mainNode = xmlDoc.SelectSingleNode(MainNode);
            XmlElement objElem = xmlDoc.CreateElement(Node);
            objElem.SetAttribute(Attrib, AttribValue);
            objElem.SetAttribute(Attrib2, AttribValue2);
            objElem.InnerText = Content;
            mainNode.AppendChild(objElem);
        }
        #endregion

        #region 保存Xml
        /**//// <summary>
        /// 保存Xml
        /// </summary>
        /// <param name="path">保存的当前路径</param>
        public void XmlSave(string path)
        {
            xmlDoc.Save(path);
        }

        #endregion

        #endregion

        #region 读取指定节点的指定属性值
        /**//// <summary>
        /// 功能:
        /// 读取指定节点的指定属性值
        /// </summary>
        /// <param name="strNode">节点路径</param>
        /// <param name="strAttribute">此节点的属性</param>
        /// <returns></returns>
        public string GetXmlNodeAttribute(string xmlPath, string strNode, string strAttribute)
        {
            string strReturn = "";
            xmlDoc.Load(xmlPath);
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == strAttribute)
                        strReturn = xmlAttr.Item(i).Value;
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }
        #endregion
        #region //根据InnerText读取属性值
        public static  string  GetXmlNodeAttribute(string xmlPath, string strNode, string strAttribute, string context)
        {
            string  s = "";
            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                if (xe.InnerText == context)
                {
                  s=  xe.GetAttribute(strAttribute);
                  return s;
                }
               

            } 
            return s;
        }
        #region //根据属性读取属性
        public static string GetXmlNodeAttribute(string xmlPath, string strNode, string strAttribute1, string context1, string strAttribute2, string strAttribute3, string strAttribute4, string strAttribute5)
        {
            string s = "";
            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型
                XmlNodeList childlist = xe.ChildNodes;
                foreach (XmlNode x in childlist)
                {
                    XmlElement e1 = (XmlElement)x;
                    XmlNodeList childlist1 = e1.ChildNodes;
                    foreach (XmlNode x1 in childlist1)
                    {
                        XmlElement e = (XmlElement)x1;

                        if (e.Attributes[strAttribute1].Value == context1)
                            return e.ParentNode.Attributes[strAttribute2].Value + "," + e.ParentNode.Attributes[strAttribute3].Value +","+ e.Attributes[strAttribute4].Value+","+e.ParentNode.ParentNode.Attributes[strAttribute5].Value;
                    }
                }
               
            }
            return s;
        }
        public static string GetXmlNodeAttribute1(string xmlPath, string strNode, string strAttribute)
        {
            string s = "";
            string value = "";
            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                s = xn.Attributes[strAttribute].Value;
                value = value +s + ",";

            }
          value= value.Trim(',');
            return value;
        }
        public static string GetXmlNodeAttribute1(string xmlPath, string strNode)
        {
            string s = "";
            string value = "";
            XmlDocument docxml = new XmlDocument();
            docxml.Load(xmlPath);
            XmlNodeList nodeList = docxml.SelectSingleNode(strNode).ChildNodes;//
            foreach (XmlNode xn in nodeList)//遍历所有子节点
            {
                s = xn.InnerText;
               value= value +s+ ",";


            }
            value = value.Substring(0, value.Length - 1);
            return value;
        }
        #endregion
        #endregion
        #region 读取指定节点的值
        /**/
        /// <summary>
        /// 功能:
        /// 读取指定节点的值
        /// </summary>
        /// <param name="strNode">节点名称（xpath）</param>
        /// <returns></returns>
        public string GetXmlNodeValue(string xmlPath,string strNode)
        {
            string strReturn = String.Empty;
            xmlDoc.Load(xmlPath);
            try
            {
                //根据路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                strReturn = xmlNode.InnerText;
            }
            catch (XmlException xmle)
            {
                System.Console.WriteLine(xmle.Message);
            }
            return strReturn;
        }
        #endregion

        #region 根据节点属性读取子节点值(较省资源模式)
        /**//// <summary>
        /// 根据节点属性读取子节点值(较省资源模式)
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="FatherElement">父节点值</param>
        /// <param name="AttributeName">属性名称</param>
        /// <param name="AttributeValue">属性值</param>
        /// <param name="ArrayLength">返回的数组长度</param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength)
        {
            System.Collections.ArrayList al = new System.Collections.ArrayList();
            XmlDocument docXml = new XmlDocument();
            docXml.Load(@XmlPath);
            XmlNodeList xn;
            xn = docXml.DocumentElement.SelectNodes("//" + FatherElement + "[" + @AttributeName + "='" + AttributeValue + "']");
            XmlNodeList xx = xn.Item(0).ChildNodes;
            for (int i = 0; i < ArrayLength & i < xx.Count; i++)
            {

                al.Add(xx.Item(i).InnerText);
            }
            return al;

        }
        #endregion
        #region 插入一节点,带四属性
        /**/
        /// <summary>
        /// 插入一节点,带5属性
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="MainNode">当前节点路径</param>
        /// <param name="Element">新节点</param>
        /// <param name="Attrib">属性名称</param>
        /// <param name="AttribContent">属性值</param>
        /// <param name="Content">新节点值</param>
        public  void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib1, string AttribContent1, string Attrib2, string AttribContent2, string Attrib3, string AttribContent3, string Attrib4, string AttribContent4, string Content)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib1, AttribContent1);
            objElement.SetAttribute(Attrib2, AttribContent2);
            objElement.SetAttribute(Attrib3, AttribContent3);
            objElement.SetAttribute(Attrib4, AttribContent4);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
            objXmlDoc.Save(xmlPath);
        }
        
         #endregion
        #region 通过节点名称找到指定的节点
         //<summary>
         //通过节点名称找到指定的节点
         //</summary>
         //<param name="xnl"></param>
         //<param name="strName"></param>
         //<returns></returns>
        protected XmlNode FindXnByName(XmlNodeList xnl, string strName)
        {
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl.Item(i).LocalName == strName) return xnl.Item(i);
            }
            return null;
        }
     
      #endregion


        #region 找到指定名称属性的值
        /// <summary>
        /// 找到指定名称属性的值
        /// </summary>
        /// <param name="xac"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        protected string GetXaValue(XmlAttributeCollection xac, string strName)
        {
            for (int i = 0; i < xac.Count; i++)
            {
                if (xac.Item(i).LocalName == strName) return xac.Item(i).Value;
            }
            return null;
        }
        #endregion

        #region 找到指定名称属性的值
        /// <summary>
        /// 找到指定名称属性的值
        /// </summary>
        /// <param name="xnl"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetXnValue(XmlNodeList xnl, string strName,string context)
        {
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl.Item(i).LocalName == strName) return xnl.Item(i).InnerText;
            }
            return null;
        }
        public bool ExistXnValue(string xmlPath, string MainNode,  string strName, string context)
        {
            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(xmlPath);
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlNodeList xnl = objNode.ChildNodes;
            for (int i = 0; i < xnl.Count; i++)
            {
                    if (xnl.Item(i).Attributes[strName].Value == context)
                        return true;
            }
            return false;
        }
        #endregion

        #region 为一个节点指定值
        /// <summary>
        /// 为一个节点指定值
        /// </summary>
        /// <param name="xnl"></param>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        protected void SetXnValue(XmlNodeList xnl, string strName, string strValue)
        {
            for (int i = 0; i < xnl.Count; i++)
            {
                if (xnl.Item(i).LocalName == strName)
                {
                    xnl.Item(i).InnerText = strValue;
                    return;
                }
            }
            return;
        }
        #endregion

        #region 为一个属性指定值
        /// <summary>
        /// 为一个属性指定值
        /// </summary>
        /// <param name="xac"></param>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        protected void SetXaValue(XmlAttributeCollection xac, string strName, string strValue)
        {
            for (int i = 0; i < xac.Count; i++)
            {
                if (xac.Item(i).LocalName == strName)
                {
                    xac.Item(i).Value = strValue;
                    return;
                }
            }
            return;
        }
        #endregion

        #region 寻找具有指定名称和属性/值组合的节点
        /// <summary>
        /// 寻找具有指定名称和属性/值组合的节点
        /// </summary>
        /// <param name="xnl"></param>
        /// <param name="strXaName"></param>
        /// <param name="strXaValue"></param>
        /// <returns></returns>
        protected XmlNode FindXnByXa(XmlNodeList xnl, string strXaName, string strXaValue)
        {
            string xa;
            for (int i = 0; i < xnl.Count; i++)
            {
                xa = GetXaValue(xnl.Item(i).Attributes, strXaName);
                if (xa != null)
                {
                    if (xa == strXaValue) return xnl.Item(i);
                }
            }
            return null;
        }
        #endregion
        #region 取得名称为name的结点的值
        /// <summary>
        /// 取得名称为name的结点的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValue(string name)
        {
            XmlNode xn = FindXnByName( xmlElem.ChildNodes, name);
            if (xn == null) return null;
            return xn.InnerText;
        }
         #endregion

        #region 创建一个包含version和指定根节点的XmlDocument
        /// <summary>
        /// 创建一个包含version和指定根节点的XmlDocument
        /// </summary>
        /// <param name="rootName"></param>
        public void CreateRoot(string rootName)
        {
            XmlElement xe = xmlDoc.CreateElement(rootName);
            xmlDoc.AppendChild(xe);
            xmlElem = xe;
        }
        #endregion

        #region 增加一个子结点
        /// <summary>
        /// 增加一个子结点
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="_value">值</param>
        /// <returns></returns>
        public XmlElement AppendChild(string name, string _value)
        {
            return AddChild((XmlElement)xmlElem, name, _value);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return xmlDoc.OuterXml;
        }
        #endregion

        #region 为一个XmlElement添加子节点，并返回添加的子节点引用
        /// <summary>
        /// 为一个XmlElement添加子节点，并返回添加的子节点引用
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="sField"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public XmlElement AddChild(XmlElement xe, string sField, string sValue)
        {
            XmlElement xeTemp = xmlDoc.CreateElement(sField);
            xeTemp.InnerText = sValue;
            xe.AppendChild(xeTemp);
            return xeTemp;
        }
        #endregion

        #region 为一个XmlElement添加子节点，并返回添加的子节点引用
        /// <summary>
        /// 为一个XmlElement添加子节点，并返回添加的子节点引用
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="xd"></param>
        /// <param name="sField"></param>
        /// <returns></returns>
        protected XmlElement AddChild(XmlElement xe, XmlDocument xd, string sField)
        {
            XmlElement xeTemp = xd.CreateElement(sField);
            xe.AppendChild(xeTemp);
            return xeTemp;
        }
        #endregion

        #region 为一个节点添加属性
        /// <summary>
        /// 为一个节点添加属性
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public void AddAttribute(XmlElement xe, string strName, string strValue)
        {
            //判断属性是否存在
            string s = GetXaValue(xe.Attributes, strName);
            //属性已经存在
            if (s != null)
            {
                throw new System.Exception("attribute exists");
            }
            XmlAttribute xa = xmlDoc.CreateAttribute(strName);
            xa.Value = strValue;
            xe.Attributes.Append(xa);
        }
        #endregion

        #region 为一个节点添加属性，不是系统表
        /// <summary>
        /// 为一个节点添加属性，不是系统表
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="xe"></param>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public void AddAttribute(XmlDocument xdoc, XmlElement xe, string strName, string strValue)
        {
            //判断属性是否存在
            string s = GetXaValue(xe.Attributes, strName);
            //属性已经存在
            if (s != null)
            {
                throw new Exception("Error:The attribute '" + strName + "' has been existed!");
            }
            XmlAttribute xa = xdoc.CreateAttribute(strName);
            xa.Value = strValue;
            xe.Attributes.Append(xa);
        }
        #endregion

    }

}