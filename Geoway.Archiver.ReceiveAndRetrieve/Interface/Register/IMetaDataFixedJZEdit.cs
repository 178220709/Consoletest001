using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    public interface IMetaDataFixedJZEdit:IMetaDataEdit
    {
        /// <summary>
        /// 虚拟库房位置
        /// </summary>
        string VirtualWarehouseAddress{ get; set; }
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
        EnumCarrierType enumCarrierType { get; set; }
    }
}