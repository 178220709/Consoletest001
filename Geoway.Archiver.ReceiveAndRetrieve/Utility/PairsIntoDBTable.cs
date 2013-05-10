namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using Geoway.ADF.MIS.DB.Public;
    using Geoway.ADF.MIS.Utility.Log;

    /// <summary>
    /// 目的：将键对导入到使用构建平台建立的业务表中
    /// 创建人：秦文汉
    /// 创建日期：2010-3-30
    /// </summary>
    public class PairsIntoDBTable
    {
        private const string FLD_NAME_F_OID = "F_OID";

        #region 字段

        private Dictionary<string, string> _fieldAndValuePairs = null;  //存放字段和字段值的键对哈希表
        private Dictionary<string, string> _dbTableFieldNameAndFileFieldNamePairs = null;//存放数据库内字段名和文件字段名的对应关系哈希表
        private string _tableName = "";//数据库业务表名称
        private List<DBFieldInfo> _lstDbFields = null;//数据库业务表字段信息列表
        private bool[] boolInitMatchHashtable = new bool[] { false, false };

        #endregion

        #region 属性

        public Dictionary<string, string> FieldAndValuePairs
        {
            get { return _fieldAndValuePairs; }
            set
            {
                if (_fieldAndValuePairs == null)
                    _fieldAndValuePairs = new Dictionary<string, string>();
                if (_fieldAndValuePairs != value && value != null)
                {
                    boolInitMatchHashtable[1] = true;
                }
                _fieldAndValuePairs = value;

                initMatchTable();
            }
        }

        public Dictionary<string, string> DBTableFieldNameAndFileFieldNamePairs
        {
            get { return _dbTableFieldNameAndFileFieldNamePairs; }
            set { _dbTableFieldNameAndFileFieldNamePairs = value; }
        }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                //if (!verifyTable(value))
                //    throw new Exception("该表必须是使用构建平台建立的表");
                //else
                //{
                    if (_tableName != value)
                    {
                        initDBFields(value);
                        boolInitMatchHashtable[0] = false;
                    }
                //}
                _tableName = value;
                initMatchTable();
            }
        }

        #endregion

        #region 公开方法

        //将键对导入到使用构建平台建立的业务表中
        public bool Import(out int rowID, out string log)
        {
            if (_tableName.Trim() == "")
            {
                throw new Exception("请设置\"TableName\"属性信息");
            }
            if (_fieldAndValuePairs == null)
            {
                throw new Exception("请设置\"HashtableFieldAndValuePairs\"属性信息");
            }
            if (_dbTableFieldNameAndFileFieldNamePairs == null)
            {
                throw new Exception("请设置\"HashtableDBTableFieldNameAndFileFieldNamePairs\"属性信息");
            }

            log = string.Empty;
            Dictionary<string, string> fieldValues = BuildInsetFieldValues();
            if (!MetaItemCheck.IsValid(fieldValues, _lstDbFields, ref log))
            {
                rowID = -1;
                return false;
            }
            //writeFieldOrderInfo();
            string[] str = BuildInsertSqlCause(fieldValues);
            //string[] str = BuildInsertSqlCause();

            //暂时不考虑数据更新

            //int ID = -1;
            //if (isExist)
            //{
            //    ID = metaID;
            //    DBHelper.GlobalDBHelper.DoSQL("delete from " + _tableName + " where f_oid=" + ID);
            //}
            //else
            //{
            //    rowID = DBHelper.GlobalDBHelper.GetNextValidID(_tableName, "F_OID");
            //}

            rowID = DBHelper.GlobalDBHelper.GetNextValidID(_tableName, "F_OID");
            try
            {
                string id = "'" + Convert.ToString(rowID) + "',";
                string SQL = "insert into " + _tableName + " (F_TASKID,F_ACTIVITYID,F_ISDELETED,F_Hisroot,F_OID," + str[0] + ") values ( '-1','-1','0','0'," + id + str[1] + " ) ";
                DBHelper.GlobalDBHelper.DoSQL(SQL);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error.Append(ex);
                rowID = -1;
                return false;
            }
        }

        /// <summary>
        /// 更新业务表中记录
        /// </summary>
        /// <param name="rowID"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool Update(int rowID, out string log)
        {
            string setClause = BuildUpdateSqlSetClause(_fieldAndValuePairs);
            if (string.IsNullOrEmpty(setClause))
            {
                log = "没有合法的键值对";
                return false;
            }
            else
            {
                string sql = string.Format("update {0} set {1} where {2}={3}",
                                           _tableName,
                                           setClause,
                                           FLD_NAME_F_OID, rowID);
                try
                {
                    log = "";
                    return DBHelper.GlobalDBHelper.DoSQL(sql) > 0;
                }
                catch (Exception exp)
                {
                    LogHelper.Error.Append(exp);
                    log = exp.Message;
                    return false;
                }
            }
        }

        public bool ValidateMetaItems(ref string message)
        {
            return MetaItemCheck.IsValid(_fieldAndValuePairs, _lstDbFields, ref message);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 构建UPDATE语句中的set部分
        /// </summary>
        /// <param name="fieldvalues">键值对</param>
        /// <returns></returns>
        private string BuildUpdateSqlSetClause(Dictionary<string, string> fieldvalues)
        {
            if (fieldvalues.Count > 0)
            {
                string value = "";
                StringBuilder sql = new StringBuilder();
                Dictionary<string, string> tmpValues = new Dictionary<string, string>(fieldvalues);
                foreach (DBFieldInfo field in _lstDbFields)
                {
                    if (tmpValues.ContainsKey(field.FieldDes))
                    {
                        value = GetSqlValueClause(tmpValues[field.FieldDes], field.FieldTypeId);
                        sql.Append(field.FieldName);
                        sql.Append("=");
                        if (string.IsNullOrEmpty(value))
                        {
                            sql.Append(" null ");
                        }
                        else
                        {
                            sql.Append(value);
                        }
                        sql.Append(",");
                        tmpValues.Remove(field.FieldDes);
                    }
                }
                if (sql.Length > 5)
                {
                    return sql.ToString().TrimEnd(',');
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private string GetSqlValueClause(string value, int typeID)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            else
            {
                switch (typeID)
                {
                    case 1:
                    case 8: //VARCHAR
                        return "'" + value + "'";
                    case 2:
                    case 3:
                    case 9://NUMBER
                    case 10:
                    case 4:
                    case 11://float
                        return value;
                    case 12:  //DATE
                        string newdate = ChangeDateStyle(value);
                        return "to_date('" + newdate + "','" + "yyyy-mm-dd" + "')";
                    case 6://datatime,Sql Server字段，李海永添加
                        string newdate1 = ChangeDateStyle(value);
                        return "'" + newdate1 + "'";
                    default:
                        return "'" + value + "'";
                }
            }
        }

        //检验业务表设置是否正确
        private bool verifyTable(string tablename)
        {
            string tblname = tablename.ToUpper().Trim();
            bool valid = false;
            if (tblname.Length >= 5)
            {
                if (tblname.Substring(0, 5) == "TBBIZ")
                {
                    string sql = "select count(f_tableid) from  tbform_tables where upper(f_tablename)='" + tblname + "'";
                    using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
                    {
                        if (reader.Read())
                        {
                            valid = true;
                        }
                    }
                }
            }

            return valid;
        }

        private void initDBFields(string tableName)
        {
            string SQL = "select a.F_FIELDNAME, a.F_FIELDDESC, a.F_FIELDTYPEID,a.F_FIELDLENGTH from TBFORM_FIELDS a, TBFORM_TABLES B where  a.F_TABLEID=b.F_TABLEID and  upper(F_TABLENAME)='" + tableName.ToUpper() + "'";
            using (IDataReader DR = DBHelper.GlobalDBHelper.DoQuery(SQL))
            {
                if (_lstDbFields == null)
                    _lstDbFields = new List<DBFieldInfo>();

                _lstDbFields.Clear();
                while (DR.Read())
                {
                    DBFieldInfo dbfield = new DBFieldInfo();
                    dbfield.FieldDes = DR["F_FIELDDESC"].ToString();
                    dbfield.FieldName = DR["F_FIELDNAME"].ToString();
                    dbfield.FieldTypeId = Int32.Parse(DR["F_FIELDTYPEID"].ToString());
                    dbfield.Length = Convert.ToInt32(DR["F_FIELDLENGTH"]);
                    _lstDbFields.Add(dbfield);
                }
            }
        }

        private void initMatchTable()
        {
            if (boolInitMatchHashtable[0] == true && boolInitMatchHashtable[1] == true)
            {
                if (_dbTableFieldNameAndFileFieldNamePairs == null)
                    _dbTableFieldNameAndFileFieldNamePairs = new Dictionary<string, string>();

                _dbTableFieldNameAndFileFieldNamePairs.Clear();
                foreach (DBFieldInfo info in _lstDbFields)
                {
                    foreach (KeyValuePair<string, string> de in _fieldAndValuePairs)
                    {
                        if (info.FieldDes.Trim().ToUpper() == de.Key.ToString().Trim().ToUpper())
                        {
                            if (!_dbTableFieldNameAndFileFieldNamePairs.ContainsKey(info.FieldDes.Trim().ToUpper()))
                            {
                                _dbTableFieldNameAndFileFieldNamePairs.Add(info.FieldDes.Trim().ToUpper(), de.Key.ToString().Trim().ToUpper());
                                break;
                            }
                        }
                    }
                }

                boolInitMatchHashtable[0] = false;
                boolInitMatchHashtable[1] = false;
            }
        }

        /// <summary>
        /// 构造插入向元数据表插入记录的sql语句
        /// </summary>
        /// <param name="cont"></param>
        /// <returns></returns>
        private string[] BuildInsertSqlCause()
        {
            string[] str ={ "", "" };
            try
            {
                string f_fieldname = "";
                string f_value = "";
                string fvalue = "";
                bool isMactch = false;
                if (_lstDbFields.Count > 0)
                {
                    //按照对应关系来赋值
                    foreach (KeyValuePair<string, string> de in _dbTableFieldNameAndFileFieldNamePairs)
                    {
                        DBFieldInfo dbfield = new DBFieldInfo();
                        isMactch = false;
                        string key = "";
                        if (de.Value.ToString().Trim() != "")
                        {
                            for (int j = 0; j < _lstDbFields.Count; j++)
                            {
                                if (_lstDbFields[j].FieldDes.ToUpper().Trim() == de.Key.ToUpper())
                                {
                                    key = de.Key.ToUpper();
                                    if (_fieldAndValuePairs.ContainsKey(key))
                                    {
                                        if (_fieldAndValuePairs[key] != null && _fieldAndValuePairs[key].Trim() != "")
                                        {
                                            isMactch = true;
                                            fvalue = _fieldAndValuePairs[key].ToString().Trim();
                                            dbfield = _lstDbFields[j];
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (isMactch)
                        {
                            f_fieldname += dbfield.FieldName + ",";

                            switch (dbfield.FieldTypeId)
                            {
                                case 1:
                                case 8: //VARCHAR
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                                case 2:
                                case 3:
                                case 9://NUMBER
                                case 10:
                                    char[] temp = fvalue.ToCharArray();
                                    string tempStr = "";
                                    for (int n = 0; n < temp.Length; n++)
                                    {
                                        if (char.IsDigit(temp[n]))
                                        {
                                            tempStr += temp[n];
                                        }
                                    }

                                    if (tempStr == "")
                                    {
                                        tempStr = "0";
                                    }
                                    f_value += tempStr + ",";
                                    break;
                                case 4:
                                case 11://float
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                                case 12:  //DATE
                                    string value = fvalue;
                                    string newdate = ChangeDateStyle(value);
                                    f_value += "to_date('" + newdate + "','" + "yyyy-mm-dd" + "')" + ",";
                                    break;
                                case 6://datatime,Sql Server字段，李海永添加
                                    string value1 = fvalue;
                                    string newdate1 = ChangeDateStyle(value1);
                                    f_value += "'" + newdate1 + "'" + ",";
                                    break;
                                default:
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                            }
                        }
                    }
                }
                f_fieldname = f_fieldname.Replace("，", ",");
                f_value = f_value.Replace("，", ",");

                str[0] = f_fieldname.TrimEnd(',');
                str[1] = f_value.TrimEnd(',');
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return str;
        }

        private Dictionary<string, string> BuildInsetFieldValues()
        {
            Dictionary<string, string> fieldValues = new Dictionary<string, string>();
            try
            {
                string fvalue = "";
                if (_lstDbFields.Count > 0)
                {
                    //按照对应关系来赋值
                    foreach (KeyValuePair<string, string> de in _dbTableFieldNameAndFileFieldNamePairs)
                    {
                        DBFieldInfo dbfield = null;
                        string key = "";
                        if (de.Value.ToString().Trim() != "")
                        {
                            for (int j = 0; j < _lstDbFields.Count; j++)
                            {
                                if (_lstDbFields[j].FieldDes.ToUpper().Trim() == de.Key.ToUpper())
                                {
                                    key = de.Key.ToUpper();
                                    if (_fieldAndValuePairs.ContainsKey(key))
                                    {
                                        if (_fieldAndValuePairs[key] != null && _fieldAndValuePairs[key].Trim() != "")
                                        {
                                            fvalue = _fieldAndValuePairs[key].ToString().Trim();
                                            dbfield = _lstDbFields[j];

                                            fieldValues.Add(de.Key, fvalue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return fieldValues;


        }

        private string[] BuildInsertSqlCause(Dictionary<string, string> fieldvalues)
        {
            string[] str ={ "", "" };
            try
            {
                string f_fieldname = "";
                string f_value = "";
                string fvalue = "";
                bool isMactch = false;
                if (_lstDbFields.Count > 0)
                {
                    //按照对应关系来赋值
                    foreach (KeyValuePair<string, string> de in fieldvalues)
                    {
                        DBFieldInfo dbfield = null;
                        isMactch = false;
                        string key = "";
                        if (de.Value.ToString().Trim() != "")
                        {
                            for (int j = 0; j < _lstDbFields.Count; j++)
                            {
                                if (_lstDbFields[j].FieldDes.ToUpper().Trim() == de.Key.ToUpper())
                                {
                                    isMactch = true;
                                    fvalue = de.Value.Trim();
                                    dbfield = _lstDbFields[j];
                                    break;
                                }
                            }
                        }

                        if (isMactch)
                        {
                            f_fieldname += dbfield.FieldName + ",";
                            //李海永添加了对Sql库的解析
                            switch (dbfield.FieldTypeId)
                            {
                                case 1:

                                case 8: //VARCHAR
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                                case 2:
                                case 3:
                                case 9://NUMBER
                                case 10:
                                    char[] temp = fvalue.ToCharArray();
                                    string tempStr = "";
                                    for (int n = 0; n < temp.Length; n++)
                                    {
                                        if (char.IsDigit(temp[n]))
                                        {
                                            tempStr += temp[n];
                                        }
                                    }

                                    if (tempStr == "")
                                    {
                                        tempStr = "0";
                                    }
                                    f_value += tempStr + ",";
                                    break;
                                case 4:
                                case 11://float
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                                case 12:  //DATE
                                    string value = fvalue;
                                    string newdate = ChangeDateStyle(value);
                                    f_value += "to_date('" + newdate + "','" + "yyyy-mm-dd hh24:mi:ss" + "')" + ",";
                                    break;
                                case 6://datatime,Sql Server字段，李海永添加
                                    string value1 = fvalue;
                                    string newdate1 = ChangeDateStyle(value1);
                                    f_value += "'" + newdate1 + "'" + ",";
                                    break;
                                default:
                                    f_value += "'" + fvalue + "'" + ",";
                                    break;
                            }
                        }
                    }
                }
                f_fieldname = f_fieldname.Replace("，", ",");
                f_value = f_value.Replace("，", ",");

                str[0] = f_fieldname.TrimEnd(',');
                str[1] = f_value.TrimEnd(',');
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return str;

        }
        /// <summary>
        /// 李海永改造
        /// </summary>
        /// <param name="oldStyle"></param>
        /// <returns></returns>
        private string ChangeDateStyle(string oldStyle)
        {
            string strDate = oldStyle;
            string[] tmp = strDate.Split(":：".ToCharArray());
            if (tmp.Length == 3)
            {
                int h = 0;
                if (int.TryParse(tmp[0], out h))
                {
                    if (h < 24 && h >= 0)
                    {
                        strDate = "1900-01-01 " + strDate;
                    }
                }
            }
            DateTime dt = new DateTime();
            //DateTime.TryParse(strDate, out dt);
            if (!DateTime.TryParse(strDate, out dt))
            {
                string[] strs = strDate.Split(" ".ToCharArray());
                if (strs.Length == 1)
                {
                    if (strDate.Length == 8)
                    {
                        strDate = strDate.Substring(0, 4) + "-" + strDate.Substring(4, 2) + "-" + strDate.Substring(6, 2);
                        if (!DateTime.TryParse(strDate, out dt))
                        {
                            return DateTime.MinValue.ToString();
                        }
                        else
                        {
                            return dt.ToString();
                        }
                    }
                }
                else if (strs.Length > 1)
                {
                    strDate = strs[strs.Length - 1] + " " + strDate.TrimEnd(strs[strs.Length - 1].ToCharArray());
                    //dt = new DateTime();
                    //DateTime.TryParse(strDate, out dt);
                    if (!DateTime.TryParse(strDate, out dt))
                    {
                        return DateTime.MinValue.ToString();
                    }
                    else
                    {
                        return dt.ToString();
                    }
                }
            }
            else
            {
                return dt.ToString();
            }
            return DateTime.MinValue.ToString();

            /*string newdatestyle = "0000-00-00";
            string Temp = "";
            char[] Year = { '0', '0', '0', '0' };
            string year = "";
            char[] Month = { '0', '0' };
            string month = "";
            char[] Date = { '0', '0' };
            string date = "";

            foreach (char s in oldStyle)
            {
                if (char.IsDigit(s) == true)
                {
                    Temp += s;
                }
            }

            if (Temp.Length >= 4)
            {
                Temp.CopyTo(0, Year, 0, 4);
                Temp = Temp.Remove(0, 4);

                for (int yea = 0; yea < Year.Length; yea++)
                {
                    year += Year[yea];
                }

            }
            else
            {
                return newdatestyle;
            }

            if (Temp.Length >= 2)
            {
                Temp.CopyTo(0, Month, 0, 2);
                for (int mon = 0; mon < Month.Length; mon++)
                {
                    month += Month[mon];
                }

                if (Convert.ToInt32(month) > 12)
                {
                    month = month.Remove(1, 1);
                    month = "0" + month;
                    Temp = Temp.Remove(0, 1);
                }
                else
                {
                    Temp = Temp.Remove(0, 2);
                }
            }
            else
            {
                if (Temp.Length == 1)
                {
                    month = "0" + Temp;
                }

                return (year + "-" + month + "-01");
            }


            if (Temp.Length <= 2)
            {
                date = Temp;
                if (date == "")
                {
                    return (year + "-" + month + "-01");
                }
                if (Convert.ToInt32(date) <= 31)
                {
                    if (date.Length == 1)
                    {
                        newdatestyle = year + "-" + month + "-0" + date;
                    }
                    else
                    {
                        newdatestyle = year + "-" + month + "-" + date;
                    }
                }
                else
                {
                    newdatestyle = year + "-" + month + "-" + "01";
                }
            }
            else
            {
                return (year + "-" + month + "-01");
            }

            return newdatestyle;*/
        }

        #region 第一次填写元数据的时候维护元数据字段信息顺序表

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metaTableName"></param>
        /// <returns></returns>
        private bool isExistInFieldOrderTable()
        {
            string sql = "select f_tableid from tbimage_fieldorder where upper(f_tablename)='" + _tableName.ToUpper() + "'";
            bool result = false;
            try
            {
                using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
                {
                    if (reader.Read())
                        result = true;
                }
                return result;
            }
            catch (Exception exp)
            {
                LogHelper.Error.Append(exp);
                return false;
            }
        }

        private void writeFieldOrderInfo()
        {
            if (!isExistInFieldOrderTable())
            {
                string sql = "select f_tableid from tbform_tables where upper(f_tablename)='" + _tableName.ToUpper() + "'";
                int id = -1;
                try
                {
                    using (IDataReader reader = DBHelper.GlobalDBHelper.DoQuery(sql))
                    {
                        if (reader.Read())
                            id = Convert.ToInt32(reader[0].ToString());
                    }
                    if (id != -1)
                    {
                        int index = 0;
                        int oid = -1;
                        foreach (string st in _fieldAndValuePairs.Keys)
                        {
                            oid = DBHelper.GlobalDBHelper.GetNextValidID("tbimage_fieldorder", "f_id");
                            sql = "insert into tbimage_fieldorder (f_id,f_tableid,f_tablename,f_field,f_order) values ";
                            sql += "(" + oid + "," + id + ",'" + _tableName + "','" + st + "'," + index + ")";
                            DBHelper.GlobalDBHelper.DoSQL(sql);
                            index++;
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogHelper.Error.Append(exp);
                }
            }
        }

        #endregion

        #endregion
    }

    public class DBFieldInfo
    {
        public string FieldDes;
        public string FieldName;
        public int FieldTypeId;
        public int Length;
        public DBFieldInfo()
        { }
    }
}
