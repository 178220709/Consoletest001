using System;
using System.Collections.Generic;
using System.Text;
using Geoway.Archiver.ReceiveAndRetrieve.Definition;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    using Modeling.Definition;

    /// <summary>
    /// 注册元数据表
    /// </summary>
    interface IMetaDataExtEdit : IMetaDataEdit
    {
        /// <summary>
        /// 入库节点ID
        /// </summary>
        int CatalogId { get;set; }
        /// <summary>
        /// 元数据信息集合
        /// </summary>
        Dictionary<string, object> MetaDataSource { get;set; }
    }
}
