namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 元数据基本操作
    /// Insert主要在扩展属性操作类中实现
    /// Delete主要在系统维护字段操作类中实现
    /// Update在系统维护、固有信息操作类中实现
    /// </summary>
    public interface IMetaData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Insert();
        /// <summary>
        /// 根据数据ID更新
        /// </summary>
        /// <returns></returns>
        bool Update();
        /// <summary>
        /// 根据数据ID删除
        /// </summary>
        /// <returns></returns>
        bool Delete();
    }
}