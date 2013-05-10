namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    using Definition;

    public interface IMetaDataFixedEdit : IMetaDataEdit
    {
        /// <summary>
        /// ����ⷿλ��
        /// </summary>
        string VirtualWarehouseAddress { get; set; }
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
        EnumMediumType EnumMediumtype { get; set; }
        /// <summary>
        /// ���ݴ�С
        /// </summary>
        double DataSize { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        string DataTypeName { get; set; }
        /// <summary>
        /// ���ݴ�С��λ
        /// </summary>
        string DataUnit { get; set; }
        /// <summary>
        /// �洢�ڵ�ID
        /// </summary>
        long ServerId { get; set; }
        /// <summary>
        /// ���ݴ洢·��
        /// </summary>
        string Location { get; set; }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        bool IsHasAFile { get; set; }
        /// <summary>
        /// �Ƿ�洢���ռ�����Դ
        /// </summary>
        bool IsSpatial { get; set; }
        /// <summary>
        /// �����ֶ�ID
        /// </summary>
        int RelDataID { get; set; }
        
    }
}