using System;
using System.Collections.Generic;
using System.Text;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface
{
    /// <summary>
    /// ���ݿ�����ӿڣ������½���ɾ�������µȳ��ò���
    /// </summary>
    interface IRegisterDBOper<T>
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
    }
}
