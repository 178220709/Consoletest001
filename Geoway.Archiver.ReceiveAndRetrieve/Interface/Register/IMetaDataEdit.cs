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
        /// ����ID
        /// </summary>
        int DataId { get; set; }
        /// <summary>
        /// Ԫ���ݱ���
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// дԪ���ݱ���ֶ���Ϣ����
        /// </summary>
        List<DBFieldItem> FieldItems { set; }
        /// <summary>
        /// �ռ䷶Χ��wkt��
        /// </summary>
        string WKT { get; set; }
    }
}