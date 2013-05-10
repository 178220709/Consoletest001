namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    public interface IMetaDataFixedDZEdit:IMetaDataEdit
    {
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
    }
}