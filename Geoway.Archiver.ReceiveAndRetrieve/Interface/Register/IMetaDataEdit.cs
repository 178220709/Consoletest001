using Geoway.ADF.MIS.DB.Public;
using System.Collections.Generic;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMetaDataEdit
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        int DataId { get; set; }
        /// <summary>
        /// 元数据表名
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// 写元数据表的字段信息集合
        /// </summary>
        List<DBFieldItem> FieldItems { set; }
        /// <summary>
        /// 空间范围的wkt串
        /// </summary>
        string WKT { get; set; }
    }
}