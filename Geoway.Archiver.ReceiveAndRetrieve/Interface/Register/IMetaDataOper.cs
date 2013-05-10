using System.Collections.Generic;
using Geoway.ADF.MIS.DB.Public;
namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// Ԫ���ݻ�������
    /// </summary>
    public interface IMetaDataOper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int Insert();
        /// <summary>
        /// ��������IDɾ��
        /// </summary>
        /// <param name="isDeleteEntity">�Ƿ�ɾ�����ϴ�ʵ��</param>
        /// <returns></returns>
        bool Delete(bool isDeleteEntity);
        /// <summary>
        /// ����ָ���ֶε�ֵ
        /// </summary>
        /// <returns></returns>
        bool Update(DBFieldItem item);

        /// <summary>
        /// ����ָ���ֶε�ֵ
        /// </summary>
        /// <returns></returns>
        bool Update(IList<DBFieldItem> items);
        /// <summary>
        /// ���ݸ����ֶ�����ȡ�ֶ�ֵ
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        object GetValue(string fieldName);
    }
}