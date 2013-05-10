using System;
using System.Collections.Generic;
using System.Text;
using Geoway.ADF.MIS.DB.Public;
using Geoway.Archiver.Modeling.Definition;
using Geoway.Archiver.Modeling.Model;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public sealed class ArchiverCheck
    {
        /// <summary>
        /// 数据有效性核查：检查元数据文件字段是否与资料类型所包含的字段对应
        /// 如果元数据文件所包含字段少于资料类型对应字段，则错误
        /// </summary>
        /// <returns></returns>
        public static string CheckField(DatumType datumType,List<string> fields)
        {
            StringBuilder sbErrorField = new StringBuilder();
            //获取扩展字段集合
            List<DatumTypeField> datumTypeFields = datumType.GetDatumFields(EnumDatumFldAtt.enumIstEmplateFld);
            foreach (DatumTypeField datumTypeField in datumTypeFields)
            {
                if(!fields.Contains(datumTypeField.Name))
                {
                    sbErrorField.Append(string.Format("{0},", datumTypeField.Name));
                }
            }
            string errorFields = sbErrorField.ToString();
            if (errorFields.EndsWith(","))
            {
                errorFields = errorFields.TrimEnd(',');
            }
            return errorFields;
        }
    }
}
