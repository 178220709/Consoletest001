namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{
    using System;
    using Geoway.ADF.MIS.DB.Public.Enum;
    using Geoway.ADF.MIS.DataModel;

    public class MetaFieldOper
    {
        public static EnumDBFieldType MetaTypeToDBType(EnumFieldType enumFieldType)
        {
            switch (enumFieldType)
            {
                case EnumFieldType.DateTime:
                    return EnumDBFieldType.FTDatetime;
                case EnumFieldType.Float:
                    return EnumDBFieldType.FTNumber;
                case EnumFieldType.Int:
                    return EnumDBFieldType.FTNumber;
                case EnumFieldType.Long:
                    return EnumDBFieldType.FTNumber;
                case EnumFieldType.String:
                case EnumFieldType.Unknown:
                    return EnumDBFieldType.FTString;
                default:
                    return EnumDBFieldType.FTString;
            }
        }

        public static object GetDefaultValue(MetaField metaField)
        {
            switch (metaField.Type)
            {
                case EnumFieldType.DateTime:
                    return DateTime.MinValue;
                case EnumFieldType.Float:
                case EnumFieldType.Int:
                case EnumFieldType.Long:
                case EnumFieldType.String:
                case EnumFieldType.Unknown:
                    return "NULL";
                default:
                    return "NULL";
            }
        }
    }
}