using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    public interface IMetaDataFixedJZEdit:IMetaDataEdit
    {
        /// <summary>
        /// ����ⷿλ��
        /// </summary>
        string VirtualWarehouseAddress{ get; set; }
        /// <summary>
        /// ������
        /// </summary>
        string BarCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        int DatumAmount { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        EnumCarrierType enumCarrierType { get; set; }
    }
}