using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Consoletest001.NewFolder1
{
    public class MetaPluginZ3
    {
        public static Dictionary<string, string> lis = new Dictionary<string, string>();
        public static Dictionary<string, string> lisRight = new Dictionary<string, string>();
        public static Dictionary<string, string> lisErrorC = new Dictionary<string, string>();
        public static Dictionary<string, string> lisErrorE = new Dictionary<string, string>();
        public static int noin = 0;

        public static void MainSdfsd()
        {
            GetXmlDatas();
        }

        public static void getFileds()
        {
            string configFile = System.Windows.Forms.Application.StartupPath + "\\metaFieldConfig.txt"; //配置文件路径
            //_fileKey_DisPlayName.Add("", new string[] { "productOrderId" });
            string line = string.Empty;
            string strValue = string.Empty;
            if (!File.Exists(configFile)) return;

            using (StreamReader read = new StreamReader(configFile, Encoding.GetEncoding("gb2312")))
            {
                while ((line = read.ReadLine()) != null)
                {
                    if (line.Trim() == "") //跳过空行
                    {
                        continue;
                    }
                    // 将字符间的多个空格换为一个空格
                    //    line = Regex.Replace(line.Trim(), @"\s+", " ");

                    line = Regex.Replace(line.Trim(), @"\s{1,}", " ");
                    //解析行
                    string[] strFildsTxt = line.Split(" ".ToCharArray());
                    if (strFildsTxt.Length < 2)
                    {
                        lisRight.Add(strFildsTxt[0], "");
                        noin++;
                        continue;
                    }

                    for (int i = 1; i < strFildsTxt.Length; i++)
                    {
                        strValue = strFildsTxt[i] + ",";
                    }
                    strValue = strValue.TrimEnd(",".ToCharArray());
                    /*   if (!lis.ContainsKey(strFildsTxt[0]))
                        {
                       
                            lis.Add(strFildsTxt[0], strValue);
                        }
                        else
                        {
                            lisErrorC.Add(strFildsTxt[0], strValue);
                        }*/

                    if (!lis.ContainsValue(strValue))
                    {
                        lis.Add(strFildsTxt[0], strValue);
                    }
                    else
                    {
                        lisErrorE.Add(strFildsTxt[0], strValue);
                    }
                }
            }
        }


        public static IDictionary<string, object> GetXmlDatas()
        {
            string metadatafileName = @"C:\Documents and Settings\Administrator\桌面\用于SC的元数据翻译字典\MYN-ZY3-FWD-20120127-000282-0000001021.XML";
            //    Dictionary<string, string> dicFilds = new Dictionary<string, string>();
            try
            {
                IDictionary<string, object> dicFields = new Dictionary<string, object>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(metadatafileName);
                string sDataName = Path.GetFileNameWithoutExtension(metadatafileName);
                string sTemp = "";
                dicFields.Add("satelliteID", XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/SatelliteID"));
                dicFields.Add("receiveStationID",
                              XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/ReceiveStationID"));
                dicFields.Add("sensorID", XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/SensorID"));
                dicFields.Add("orbitID",
                              ConvertEx.ToInt32(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/OrbitID")));
                dicFields.Add("productName", sDataName);
                string str = sDataName.Substring(sDataName.Length - 10);
                dicFields.Add("productID", ConvertEx.ToInt32(sDataName.Substring(sDataName.Length - 10)));
                dicFields.Add("sceneCount",
                              ConvertEx.ToInt32(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/SceneCount")));
                dicFields.Add("dataUpperLeftLat",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/UpperLeftLat")));
                dicFields.Add("dataUpperLeftLong",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/UpperLeftLong")));
                dicFields.Add("dataUpperRightLat",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/UpperRightLat")));
                dicFields.Add("dataUpperRightLong",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/UpperRightLong")));
                dicFields.Add("dataLowerRightLat",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/LowerRightLat")));
                dicFields.Add("dataLowerRightLong",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/LowerRightLong")));
                dicFields.Add("dataLowerLeftLat",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/LowerLeftLat")));
                dicFields.Add("dataLowerLeftLong",
                              ConvertEx.ToDouble(XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/LowerLeftLong")));
                //景开始/结束时间
                sTemp = XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/ImagingStartTime");
                DateTime dtTemp = DateTime.MinValue;
                if (String.IsNullOrEmpty(sTemp) == false)
                {
                    dicFields.Add("imagingStartTime", ConvertEx.ToDateTime(sTemp));
                }

                sTemp = XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/ImagingStopTime");
                if (String.IsNullOrEmpty(sTemp) == false)
                {
                    dicFields.Add("imagingStopTime", ConvertEx.ToDateTime(sTemp));
                }

                //采集时间SceneDate
                sTemp = XMLReaderHelper.GetXMLNodeValue(xmlDoc, @"/l0DataMeta/SceneDate");
                if (String.IsNullOrEmpty(sTemp) == false)
                {
                    dicFields.Add("sceneDate", ConvertEx.ToDateTime(sTemp));
                }
                else
                {
                    if (dtTemp != DateTime.MinValue)
                    {
                        dicFields.Add("sceneDate", dtTemp);
                    }
                }
                return dicFields;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}