using System;
using System.Collections.Generic;
using System.Text;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface
{
    /// <summary>
    /// 数据库操作接口，包括新建、删除、更新等常用操作
    /// </summary>
    interface IRegisterDBOper<T>
    {
        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        bool Add();
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        bool Delete();

        /// <summary>
        /// 更新
        /// </summary>
        bool Update();
    }
}
