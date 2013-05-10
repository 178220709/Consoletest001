using System.Data;
using ESRI.ArcGIS.Geometry;
using System.Collections.Generic;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;
using Geoway.ADF.MIS.DB.Public.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 元数据的相关查询
    /// </summary>
    public interface IMetaDataQuery
    {
        /// <summary>
        /// 根据数据ID获取WKT
        /// </summary>
        /// <returns></returns>
        string SelectWKT();
        /// <summary>
        /// 根据数据ID获取几何对象
        /// </summary>
        /// <returns></returns>
        IGeometry SelectGeometry();
        /// <summary>
        /// 根据数据ID获取所有元数据信息（字段具有不确定性）
        /// 由于数据ID的唯一性，只可能返回一条数据
        /// </summary>
        /// <returns></returns>
        DataTable SelectByDataID();
        /// <summary>
        /// 根据数据状态查询数据，从多表中查询
        /// </summary>
        /// <param name="dbHelper"> </param>
        /// <param name="enumDataState">数据状态</param>
        /// <returns></returns>
        IList<DataTable> SelectByFlag2List(IDBHelper dbHelper, EnumDataState enumDataState,params string[] columnNames);

        /// <summary>
        /// 根据数据状态查询数据，从多表中查询，并根据给定列名合并到一张表中
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="enumDataState">数据状态</param>
        /// <param name="columnNames">要展示的列名</param>
        /// <returns></returns>
        DataTable SelectByFlag2Table(IDBHelper dbHelper, EnumDataState enumDataState, params string[] columnNames);
    }
}