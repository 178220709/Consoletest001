using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public;
namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 元数据基本操作
    /// </summary>
    public interface IMetaDataOper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Insert();
        /// <summary>
        /// 根据数据ID删除
        /// </summary>
        /// <param name="isDeleteEntity">是否删除已上传实体</param>
        /// <returns></returns>
        bool Delete(bool isDeleteEntity);
        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        bool Update(DBFieldItem item);

        /// <summary>
        /// 更新指定字段的值
        /// </summary>
        /// <returns></returns>
        bool Update(IList<DBFieldItem> items);
        /// <summary>
        /// 根据给定字段名获取字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        object GetValue(string fieldName);
    }
}