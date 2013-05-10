using System.Data;
using ESRI.ArcGIS.Geometry;
using System.Collections.Generic;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// Ԫ���ݵ���ز�ѯ
    /// </summary>
    public interface IMetaDataQuery
    {
        /// <summary>
        /// ��������ID��ȡWKT
        /// </summary>
        /// <returns></returns>
        string SelectWKT();
        /// <summary>
        /// ��������ID��ȡ���ζ���
        /// </summary>
        /// <returns></returns>
        IGeometry SelectGeometry();
        /// <summary>
        /// ��������ID��ȡ����Ԫ������Ϣ���ֶξ��в�ȷ���ԣ�
        /// ��������ID��Ψһ�ԣ�ֻ���ܷ���һ������
        /// </summary>
        /// <returns></returns>
        DataTable SelectByDataID();
        /// <summary>
        /// ��������״̬��ѯ���ݣ��Ӷ���в�ѯ
        /// </summary>
        /// <param name="dbHelper"> </param>
        /// <param name="enumDataState">����״̬</param>
        /// <returns></returns>
        IList<DataTable> SelectByFlag2List(IDBHelper dbHelper, EnumDataState enumDataState,params string[] columnNames);

        /// <summary>
        /// ��������״̬��ѯ���ݣ��Ӷ���в�ѯ�������ݸ��������ϲ���һ�ű���
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumDataState">����״̬</param>
        /// <param name="columnNames">Ҫչʾ������</param>
        /// <returns></returns>
        DataTable SelectByFlag2Table(IDBHelper dbHelper, EnumDataState enumDataState, params string[] columnNames);
    }
}