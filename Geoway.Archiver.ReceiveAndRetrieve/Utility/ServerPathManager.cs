using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.Catalog.Model.Catalog;
using Geoway.Archiver.ReceiveAndRetrieve.Interface.Register;
using Geoway.Archiver.ReceiveAndRetrieve.Factory;
using Geoway.Archiver.Utility.Class;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Catalog.Model;
using Geoway.Archiver.Catalog.Definition;
using Geoway.Archiver.Modeling.Model;
using Geoway.ADF.MIS.DataModel;
using Geoway.Archiver.Catalog.Interface;
using Geoway.Archiver.Catalog;
using Geoway.ADF.MIS.Utility.Log;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    /// <summary>
    /// ������·��������Ŀ�Ͷ�̬·�����֣�
    /// </summary>
    public class ServerPathManager
    {
        /// <summary>
        /// ��ȡ�������洢·������Ŀ�Ͷ�̬·�����֣�
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataCatalogNode"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public static string GetServerStoragePath(IDBHelper dbHelper, ICatalogNode dataCatalogNode, int dataid)
        {
            string serverPath = "";
            if (dataCatalogNode.NodeExInfo.AutoPathLocation==1)
            {
                serverPath = GetDynamicPath(dbHelper, dataCatalogNode.NodeExInfo, dataid) + "/"
                    + GetCatalogPath(dbHelper, dataCatalogNode);
            }
            else
            {
                serverPath = GetCatalogPath(dbHelper, dataCatalogNode) + "/"
                    + GetDynamicPath(dbHelper, dataCatalogNode.NodeExInfo, dataid);
            }
            serverPath = serverPath.Trim("/".ToCharArray());
            return serverPath;
        }
        /// <summary>
        /// ��ȡ��̬�洢·��
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="nodeExInfo"></param>
        /// <param name="dataid"></param>
        /// <returns></returns>
        public static string GetDynamicPath(IDBHelper dbHelper, CatalogNodeExInfo nodeExInfo, int dataid)
        {
            string storagePath = "";
            if (nodeExInfo.IsHasAutoPath &&
                nodeExInfo.DynamicPathRules != null)
            {
                IMetaDataOper metaDataOper = MetadataFactory.Create(dbHelper, SysParams.BizWorkspace, dataid);
                foreach (DynamicPathRuleInfo info in nodeExInfo.DynamicPathRules)
                {
                    if (info.ElementType == EnumElementType.enumString)
                    {
                        //�̶��ַ�����ʽ
                        storagePath = storagePath + "/" + info.FixedValue;
                    }
                    else
                    {
                        //��Ԫ�����ֶλ�ȡ��ʽ
                        if (nodeExInfo.DatumTypeObj.Fields != null &&
                            nodeExInfo.DatumTypeObj.Fields.Count > 0)
                        {
                            foreach (DatumTypeField field in nodeExInfo.DatumTypeObj.Fields)
                            {
                                if (field.MetaFieldObj.ID == info.MetaFieldId)
                                {
                                    //��ȡ�ֶ�ֵ
                                    object obj = metaDataOper.GetValue(field.MetaFieldObj.Name);
                                    if (obj != null)
                                    {
                                        if (field.MetaFieldObj.Type == EnumFieldType.DateTime)
                                        {
                                            if (string.IsNullOrEmpty(info.MetaFieldRule) == false)
                                            {
                                                DateTime dtTemp;
                                                if (DateTime.TryParse(obj.ToString(), out dtTemp))
                                                {
                                                    string[] strs = info.MetaFieldRule.Split("|".ToCharArray());
                                                    foreach (string strItem in strs)
                                                    {
                                                        storagePath = storagePath + "/" + dtTemp.ToString(strItem);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            storagePath = storagePath + "/" + obj.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                storagePath = storagePath.TrimStart("/".ToCharArray());
            }
            return storagePath;
        }
        /// <summary>
        /// ��ȡ��Ŀ�洢·��
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dataCatalogNode"></param>
        /// <returns></returns>
        public static string GetCatalogPath(IDBHelper dbHelper, ICatalogNode dataCatalogNode)
        {
            string strPath = "";
            if (dataCatalogNode == null)
            {
                return strPath;
            } try
            {
                //��Ŀ�洢·������
                if (dataCatalogNode.NodeExInfo.StorePathType == EnumStorePathType.enumNodeName)
                {
                    strPath = dataCatalogNode.Name;
                }
                else
                {
                    strPath = dataCatalogNode.CatalogCode;
                }

                ICatalogNode pNode = CatalogFactory.GetCatalogNode(dbHelper, dataCatalogNode.ParentID);
                while (pNode != null)
                {
                    if (dataCatalogNode.NodeExInfo.StorePathType == EnumStorePathType.enumNodeName)
                    {
                        strPath = pNode.Name + "/" + strPath;
                    }
                    else
                    {
                        strPath = pNode.CatalogCode + "/" + strPath;
                    }
                    pNode = CatalogFactory.GetCatalogNode(dbHelper, pNode.ParentID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
            }
            //��̬�洢·������
            //if (dataCatalogNode.NodeExInfo.IsHasAutoPath&&dataCatalogNode.NodeExInfo.DynamicPathRules!=null)
            //{
            //    foreach (DynamicPathRuleInfo info in dataCatalogNode.NodeExInfo.DynamicPathRules)
            //    {

            //    }
            //}

            return strPath;
        }
    }
}
