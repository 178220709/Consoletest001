using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.DataModel;
using System.Data;
using Geoway.Archiver.Modeling.Model;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Modeling.Definition;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.ReceiveAndRetrieve.Utility;
using ESRI.ArcGIS.Geometry;
using Geoway.Archiver.Catalog.Definition;
using Geoway.Archiver.Utility.Class;
using MAPCOMLib;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{

    public class MetaDataConverter
    {
        public static Dictionary<string,object> ToDictionary(Metadata metadata)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            List<MetaItem> metaItems = metadata.Items;
            foreach (MetaItem metaItem in metaItems)
            {
                if(!dicResult.ContainsKey(metaItem.Field.AliasName))
                {
                    dicResult.Add(metaItem.Field.AliasName,metaItem.Value);
                }
            }

            // 处理新旧图号
            MapNoConvert.MapNoProcess(dicResult);

            return dicResult;
        }
        
        public static DataTable ToDataTable(IDictionary<string,object> dictionary)
        {
            DataTable dataTable = new DataTable();
            foreach (string key in dictionary.Keys)
            {
                if (key=="附件路径")
                {
                    dataTable.Columns.Add(key, typeof(AccessoryPath));
                }
                else
                {
                   dataTable.Columns.Add(key); 
                }
                
            }
            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns>表结构，无数据</returns>
        public static DataTable ToDataTable(IDictionary<string, string> dictionary)
        {
            try
            {
                DataTable dataTable = new DataTable();
                foreach (string key in dictionary.Keys)
                {
                    dataTable.Columns.Add(key);
                }
                return dataTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="datumType"></param>
        /// <param name="eModelType"> </param>
        /// <returns></returns>
        public static DataTable GetTableByDatumType(IDBHelper dbHelper, DatumType datumType, EnumModelType eModelType)
        {
            if(datumType==null)
            {
                return null;
            }

            DataTable dataTable = new DataTable();

            List<DatumTypeField> lstFields = datumType.GetDatumFields(eModelType);
            foreach (DatumTypeField datumTypeField in lstFields)
            {
                dataTable.Columns.Add(datumTypeField.ToString()/*,Type.GetType("System."+datumTypeField.MetaFieldObj.Type.ToString())*/);
            }

            dataTable.TableName = datumType.ToString();

            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="datumType"></param>
        /// <param name="eFldAtt"> </param>
        /// <returns></returns>
        public static DataTable GetTableByDatumType(IDBHelper dbHelper, DatumType datumType, EnumDatumFldAtt eFldAtt)
        {
            if (datumType == null)
            {
                return null;
            }

            DataTable dataTable = new DataTable();

            List<DatumTypeField> lstFields = datumType.GetDatumFields(eFldAtt);
            foreach (DatumTypeField datumTypeField in lstFields)
            {
                dataTable.Columns.Add(datumTypeField.ToString()/*,Type.GetType("System."+datumTypeField.MetaFieldObj.Type.ToString())*/);
            }

            dataTable.TableName = datumType.ToString();

            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="datumType"></param>
        /// <param name="eFldType"> </param>
        /// <returns></returns>
        public static DataTable GetTableByDatumType(IDBHelper dbHelper, DatumType datumType, EnumFldType eFldType)
        {
            if (datumType == null)
            {
                return null;
            }

            DataTable dataTable = new DataTable();

            List<DatumTypeField> lstFields = datumType.GetDatumFields(eFldType);
            foreach (DatumTypeField datumTypeField in lstFields)
            {
                dataTable.Columns.Add(datumTypeField.ToString()/*,Type.GetType("System."+datumTypeField.MetaFieldObj.Type.ToString())*/);
            }

            dataTable.TableName = datumType.ToString();

            return dataTable;
        }

        public static DataRow AddDicToTable(ref DataTable dataTable, IDictionary<string, string> dictionary)
        {
            DataRow dataRow = dataTable.NewRow();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (dictionary.ContainsKey(dataColumn.ColumnName))
                {
                    dataRow[dataColumn.ColumnName] = dictionary[dataColumn.ColumnName];
                }
            }
            dataTable.Rows.Add(dataRow);
            return dataRow;
        }


        public static DataRow AddDicToTable(ref DataTable dataTable, IDictionary<string, object> dictionary)
        {
            DataRow dataRow = dataTable.NewRow();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                try
                {
                    object obj = dictionary[dataColumn.ColumnName];
                    dataRow[dataColumn.ColumnName] = obj;
                }
                catch
                {
                    dataRow[dataColumn.ColumnName] = null;
                }
            }
            
            dataTable.Rows.Add(dataRow);
            return dataRow;
        }
        
        
        public static Dictionary<string,object> ToDictionary(DataRow row)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (DataColumn dataColumn in row.Table.Columns)
            {
                dic.Add(dataColumn.Caption, row[dataColumn]);
            }
            return dic;
        }

        private static string _fieldName;
        public static IList<DBFieldItem> ToList(DataRow row, DatumType datumType)
        {
            IList<DBFieldItem> items = new List<DBFieldItem>();
            List<DatumTypeField> listFields = datumType.Fields;

            foreach (DataColumn dataColumn in row.Table.Columns)
            {
                _fieldName = dataColumn.Caption;
                DatumTypeField datumTypeField = listFields.Find(Exsit);
                if (datumTypeField != null)
                {
                    items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, row[dataColumn],
                                              MetaFieldOper.MetaTypeToDBType(datumTypeField.MetaFieldObj.Type)));
                }
            }
            return items;
        }

        public static IList<DBFieldItem> ToList(DataRow row, DatumType datumType, Predicate<DatumTypeField> match)
        {
            IList<DBFieldItem> items = new List<DBFieldItem>();
            List<DatumTypeField> listFields = datumType.Fields;

            foreach (DataColumn dataColumn in row.Table.Columns)
            {
                _fieldName = dataColumn.Caption;
                DatumTypeField datumTypeField = listFields.Find(match);
                if (datumTypeField != null)
                {
                    items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, row[dataColumn],
                                              MetaFieldOper.MetaTypeToDBType(datumTypeField.MetaFieldObj.Type)));
                }
            }
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="datumType"></param>
        /// <param name="romovedFields"> </param>
        /// <returns></returns>
        public static IList<DBFieldItem> ToList(DataRow row, DatumType datumType, params string[] romovedFields)
        {
            List<string> fields = new List<string>(romovedFields);
            IList<DBFieldItem> items = new List<DBFieldItem>();
            List<DatumTypeField> listFields = datumType.Fields;

            foreach (DataColumn dataColumn in row.Table.Columns)
            {
                _fieldName = dataColumn.Caption;


                if (fields.Contains(_fieldName))
                {
                    continue;
                }

                DatumTypeField datumTypeField = listFields.Find(Exsit);
                if (datumTypeField != null)
                {
                    if (_fieldName == FixedFieldName.ALIAS_NAME_F_HDSTRUCT)
                    {
                        if (row[dataColumn].ToString() == "无")
                        {
                            items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, 0,
                                                  Geoway.ADF.MIS.DB.Public.Enum.EnumDBFieldType.FTNumber));
                        }
                        else if (row[dataColumn].ToString() == "有")
                        {
                            items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, 1,
                                                  Geoway.ADF.MIS.DB.Public.Enum.EnumDBFieldType.FTNumber));
                        }
                    }
                    else
                    {
                        items.Add(new DBFieldItem(datumTypeField.MetaFieldObj.Name, row[dataColumn],
                                                  MetaFieldOper.MetaTypeToDBType(datumTypeField.MetaFieldObj.Type)));
                    }
                }
            }
            return items;
        }
        
        /// <summary>
        /// 获取列名集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> ToList(DataTable dt)
        {
            List<string> strList = new List<string>();
            foreach (DataColumn column in dt.Columns)
            {
                strList.Add(column.Caption);
            }
            return strList;
        }

        /// <summary>
        /// 获取列名集合
        /// </summary>
        /// <returns></returns>
        public static List<string> ToList(Dictionary<string,string> dic)
        {
            List<string> strList = new List<string>();
            foreach (string key in dic.Keys)
            {
                strList.Add(key);
            }
            return strList;
        }
        

        private static bool Exsit(DatumTypeField obj)
        {
            if(obj.MetaFieldObj.AliasName==_fieldName)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 转换为Esri的集合类型
        /// </summary>
        /// <param name="enumGeometryType"></param>
        /// <returns></returns>
        public static esriGeometryType ToEsriType(EnumGeoType enumGeometryType)
        {
            switch (enumGeometryType)
            {
                case EnumGeoType.enumUNKnown:
                    return esriGeometryType.esriGeometryNull;
                case EnumGeoType.enumPoint:
                    return esriGeometryType.esriGeometryPoint;
                case EnumGeoType.enumLine:
                    return esriGeometryType.esriGeometryPolyline;
                case EnumGeoType.enumPolygon:
                    return esriGeometryType.esriGeometryPolygon;
                default:
                    return esriGeometryType.esriGeometryNull;
            }
        }
    }
}
