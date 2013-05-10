using System;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMetaDataSysEdit : IMetaDataEdit
    {
        /// <summary>
        /// 数据删除时间
        /// </summary>
        DateTime DeleteTime { get; set; }
        /// <summary>
        /// 数据标识
        /// </summary>
        int Flag { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        DateTime ImportDate { get; set; }
        /// <summary>
        /// 入库人
        /// </summary>
        string ImportUser { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        string Name { get; set; }

    }
}
