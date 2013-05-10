using System;
using System.Collections.Generic;
using System.Text;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface
{
    /// <summary>
    /// ���ݿ�����ӿڣ������½���ɾ�������µȳ��ò���
    /// </summary>
    interface IDBOper<T>
    {
        /// <summary>
        /// �½�
        /// </summary>
        /// <returns></returns>
        bool Add();
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <returns></returns>
        bool Delete();

        /// <summary>
        /// ����
        /// </summary>
        bool Update();

        /// <summary>
        /// �õ�ΨһID
        /// </summary>
        /// <returns></returns>
        int GetNextID();

        /// <summary>
        /// ѡ��
        /// </summary>
        /// <returns></returns>
        IList<T> Select();

        /// <summary>
        /// ѡ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Select(int id);
    }
}
