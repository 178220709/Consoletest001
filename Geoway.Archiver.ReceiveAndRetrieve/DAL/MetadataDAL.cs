using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Geoway.ADF.MIS.DB.Public;

namespace Geoway.Archiver.ReceiveAndRetrieve.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataDAL
    {
        public const string CONST_META_FLD_NAME_F_EXTENT_XMIN = "F_EXTENT_XMIN";
        public const string CONST_META_FLD_NAME_F_EXTENT_XMAX = "F_EXTENT_XMAX";
        public const string CONST_META_FLD_NAME_F_EXTENT_YMIN = "F_EXTENT_YMIN";
        public const string CONST_META_FLD_NAME_F_EXTENT_YMAX = "F_EXTENT_YMAX";

        private const string CONST_META_FLD_NAME_F_OID = "F_OID";

        public static DataTable GetExtentMetadata(int metaID, string metaTableName)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE {1}={2}",
                                       metaTableName,
                                       CONST_META_FLD_NAME_F_OID, metaID);
            DataTable dt = DBHelper.GlobalDBHelper.DoQueryEx("Metadata", sql, true);
            return dt;
        }
    }
}
