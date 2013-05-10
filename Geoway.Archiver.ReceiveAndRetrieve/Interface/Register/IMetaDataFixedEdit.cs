namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    using Definition;

    public interface IMetaDataFixedEdit : IMetaDataEdit
    {
        /// <summary>
        /// 虚拟库房位置
        /// </summary>
        string VirtualWarehouseAddress { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        string BarCode { get; set; }
        /// <summary>
        /// 资料数量
        /// </summary>
        int DatumAmount { get; set; }
        /// <summary>
        /// 载体类型
        /// </summary>
        EnumMediumType EnumMediumtype { get; set; }
        /// <summary>
        /// 数据大小
        /// </summary>
        double DataSize { get; set; }
        /// <summary>
        /// 数据类型名称
        /// </summary>
        string DataTypeName { get; set; }
        /// <summary>
        /// 数据大小单位
        /// </summary>
        string DataUnit { get; set; }
        /// <summary>
        /// 存储节点ID
        /// </summary>
        long ServerId { get; set; }
        /// <summary>
        /// 数据存储路径
        /// </summary>
        string Location { get; set; }
        /// <summary>
        /// 是否包含附件
        /// </summary>
        bool IsHasAFile { get; set; }
        /// <summary>
        /// 是否存储到空间数据源
        /// </summary>
        bool IsSpatial { get; set; }
        /// <summary>
        /// 关联字段ID
        /// </summary>
        int RelDataID { get; set; }
        
    }
}