using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.Modeling.Model;
using Geoway.Archiver.ReceiveAndRetrieve.Class;
using Geoway.ADF.MIS.CatalogDataModel.Private.ModelInstance;
using Geoway.Archiver.Utility.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Utility
{


    public class ListConverter
    {
        public static string ToNameString(DatumTypeField datumTypeField)
        {
            return datumTypeField.MetaFieldObj.Name;
        }


        public static string ToAliadString(DatumTypeField datumTypeField)
        {
            return datumTypeField.MetaFieldObj.AliasName;
        }
        //public static MetadataConfig ToMetadataConfig(DatumTypeField datumTypeField)
        //{
        //    MetadataConfig config = new MetadataConfig();
        //    config.MetaField = datumTypeField.MetaFieldObj;
        //    return config;
        //}

        public static MetadataConfig ToMetadataConfig(DatumTypeField input)
        {
            MetadataConfig config = new MetadataConfig();
            config.MetaField = input.MetaFieldObj;
            return config;
        }
        
        public static FileInstance ToFileInstance(string input)
        {
            return new FileInstance(input,"");
        }
        
        public static string ToString(KeyValue input)
        {
            return input.Value;
        }
    }
}
