using System;
using System.Xml;

namespace Consoletest001.NewFolder1
{
    public class XMLReaderHelper
    {
        /// <summary>
        /// 读取xml节点
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="sXmlPath"></param>
        /// <returns></returns>
        public static XmlNode GetXMLNode(XmlDocument xmlDoc, string sXmlPath)
        {
            try
            {
                return xmlDoc.SelectSingleNode(sXmlPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 读取xml节点值
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="sXmlPath"></param>
        /// <returns></returns>
        public static string GetXMLNodeValue(XmlDocument xmlDoc, string sXmlPath)
        {
            try
            {
                return xmlDoc.SelectSingleNode(sXmlPath).InnerText.Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 读取xml节点值
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="sXmlPath"></param>
        /// <returns></returns>
        public static string GetXMLNodeValue(XmlNode xmlNode, string sXmlPath)
        {
            try
            {
                return xmlNode.SelectSingleNode(sXmlPath).InnerText.Trim();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
