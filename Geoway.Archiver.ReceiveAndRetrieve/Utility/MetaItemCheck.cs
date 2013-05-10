using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using Geoway.Archiver.Utility.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using Geoway.ADF.MIS.DataModel;

    class MetaItemCheck
    {
        public static bool IsValid(Dictionary<string,string> htTable, List<DBFieldInfo> curFields, ref string loginfo)
        {
            bool matchResult = true;


            bool isformat = false;

            string fieldvalue = string.Empty;

            for (int i = 0; i < curFields.Count; i++)
            {
                fieldvalue = getFieldValue(htTable, curFields[i]);
                if (fieldvalue != string.Empty)
                {
                    switch ((EnumDBDataType)curFields[i].FieldTypeId)
                    {
                        case EnumDBDataType.SQLDatetime: //日期
                        case EnumDBDataType.ORADate:
                            isformat = IsDate(fieldvalue);
                            break;
                        case EnumDBDataType.SQLFloat://浮点
                        case EnumDBDataType.ORAFloat:
                            isformat = IsFolat(fieldvalue);
                            break;
                        case EnumDBDataType.SQLInt: //整型
                        case EnumDBDataType.ORANumber:
                            isformat = IsNumber(fieldvalue);
                            break;

                        case EnumDBDataType.SQLVarchar: //字符
                        case EnumDBDataType.ORAVarchar2:
                            isformat = IsVarchar(fieldvalue, curFields[i].Length);
                            break;

                        case EnumDBDataType.SQLImage: //blob
                        case EnumDBDataType.ORABlob:
                            isformat = IsBlob(fieldvalue);
                            break;
                        default:
                            isformat = true;
                            break;
                    }
                    if (isformat == false)
                    {
                        loginfo += curFields[i].FieldDes + "的值不符合规范; ";
                    }
                    matchResult = matchResult && isformat;
                }
            }

            return matchResult;
        }


        public static bool IsValid(Hashtable htTable, IList<FieldInfo> curFields, ref string loginfo)
        {
            bool matchResult = true;    

      
            bool isformat=false;

            for(int i=0;i<curFields.Count;i++)
            {
                switch(curFields[i].Type)
                {
                    case 12: //日期
                        isformat = IsDate(getFieldValue(htTable, curFields[i]));
                        break;
                    case 11://浮点
                        isformat = IsFolat(getFieldValue(htTable, curFields[i]));
                        break;
                    case 9: //整型
                    case 10:
                        isformat = IsNumber(getFieldValue(htTable, curFields[i]));
                        break;

                    case 8: //字符
                        isformat = IsVarchar(getFieldValue(htTable, curFields[i]), curFields[i].Length);
                        break;

                    case 13: //blob
                        isformat = IsBlob(getFieldValue(htTable, curFields[i]));
                        break;
                    default:
                        isformat=true;
                        break;
                }
                if (isformat == false)
                {
                    loginfo += curFields[i].Name + "的值不符合规范 ";
                }
                matchResult = matchResult && isformat;
            }

            return matchResult;         
        }


        public static bool IsValid(Dictionary<string, string> htTable, IList<MetaField> curFields, ref string loginfo)
        {
            bool matchResult = true;
            bool isformat = false;

            //for (int i = 0; i < curFields.Count; i++)
            //{
            //    switch (curFields[i].Type)
            //    {
            //        case EnumFieldType.DateTime: //日期
            //            isformat = IsDate(getFieldValue(htTable, curFields[i]));
            //            break;
            //        case EnumFieldType.Float://浮点
            //            isformat = IsFolat(getFieldValue(htTable, curFields[i]));
            //            break;
            //        case EnumFieldType.Int: //整型
            //        case EnumFieldType.Long:
            //            isformat = IsNumber(getFieldValue(htTable, curFields[i]));
            //            break;

            //        case EnumFieldType.String: //字符
            //            isformat = IsVarchar(getFieldValue(htTable, curFields[i]), curFields[i].Length);
            //            break;
            //        default:
            //            isformat = true;
            //            break;
            //    }
            //    if (isformat == false)
            //    {
            //        loginfo += curFields[i].Name + "的值不符合规范 ";
            //    }
            //    matchResult = matchResult && isformat;
            //}

            return matchResult;
        }

        public static string getFieldValue(Dictionary<string, string> table, DBFieldInfo fieldInfo)
        {
            foreach (KeyValuePair<string,string> de in table)
            {
                if (de.Key.Trim() == fieldInfo.FieldDes.Trim())
                {
                    if (de.Value != null)
                    {
                        return de.Value;
                    }
                }
            }
            return "";
        }

        public static string getFieldValue(Hashtable table, FieldInfo fieldInfo)
        {
            foreach (DictionaryEntry de in table)
            {
                if (de.Key.ToString() == fieldInfo.Name)
                {
                    if (de.Value != null)
                    {
                        return de.Value.ToString();
                    }
                }
            }
            return "";
        }

        public static string getFieldValue(Dictionary<string, string> table, MetaField fieldInfo)
        {
            foreach (KeyValuePair<string,string> de in table)
            {
                if (de.Key == fieldInfo.Name)
                {
                    if (de.Value != null)
                    {
                        return de.Value;
                    }
                }
            }
            return "";
        }

        public static int GetByteLength(string str)
        {
            byte[] bytestr = System.Text.Encoding.Unicode.GetBytes(str);
            int len = 0;
            for (int i = 0; i < bytestr.GetLength(0); i++)
            {
                if (i % 2 == 0)
                {
                    len++;
                }
                else
                {
                    if (bytestr[i] > 0)
                    {
                        len++;
                    }
                }
            }
            return len;
        }


        /// <summary>
        /// 获取指定字节长度的中英文混合字符串

        /// </summary>
        public static string GetString(string strValue, int maxlen)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(strValue);// 单字节字符长度

            int charLen = strValue.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > maxlen)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(strValue.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > maxlen)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == maxlen)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = strValue.Substring(0, pos);
            }
            else
                result = strValue;

            return result;
        }

        #region 数据类型一致性判断

        private static bool IsDate(string fieldValue)
        {
            DateTime dateTime;

            if (DateTime.TryParse(fieldValue, out  dateTime))
            {
                return true;
            }
            else
            {
                string[] strs = fieldValue.Split(" ".ToCharArray());
                string strDate = fieldValue;
                if (strs.Length == 1)
                {
                    strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);
                    if (DateTime.TryParse(strDate, out dateTime))
                    {
                        return true;
                    }
                }
                else if (strs.Length > 1)
                {
                    strDate = strs[strs.Length - 1] + " " + strDate.TrimEnd(strs[strs.Length - 1].ToCharArray());
                    if (DateTime.TryParse(strDate, out  dateTime))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        
        private static bool IsNumber(string fieldValue)
        {
            double num;

            if (double.TryParse(fieldValue, out  num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsFolat(string fieldValue)
        {
            float num;

            if (float.TryParse(fieldValue, out  num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsInt(string fieldValue)
        {
            Int32 num;

            if (Int32.TryParse(fieldValue, out  num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsBigInt(string fieldValue)
        {
            Int64 num;

            if (Int64.TryParse(fieldValue, out  num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsDecimal(string fieldValue)
        {
            double num;

            if (double.TryParse(fieldValue, out  num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //判断方法待定
        private static bool IsBlob(string fieldValue)
        {
            return true;
        }

        //判断方法待定
        private static bool IsImage(string fieldValue)
        {
            return true;
        }

        private static bool IsVarchar(string fieldValue,int length)
        {
            if (fieldValue.Length > length)
            {
                return false;
            }
            return true;
        }

        private static bool IsVarchar2(string fieldValue)
        {
            return true;
        }

        #endregion

    }

    class FieldInfo
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        int length;

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public FieldInfo(string name, int type, int length)
        {
            this.name = name;
            this.type = type;
            this.length = length;
        }

    }
}
