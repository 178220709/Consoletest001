namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// Ԫ���ݻ�������
    /// Insert��Ҫ����չ���Բ�������ʵ��
    /// Delete��Ҫ��ϵͳά���ֶβ�������ʵ��
    /// Update��ϵͳά����������Ϣ��������ʵ��
    /// </summary>
    public interface IMetaData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Insert();
        /// <summary>
        /// ��������ID����
        /// </summary>
        /// <returns></returns>
        bool Update();
        /// <summary>
        /// ��������IDɾ��
        /// </summary>
        /// <returns></returns>
        bool Delete();
    }
}