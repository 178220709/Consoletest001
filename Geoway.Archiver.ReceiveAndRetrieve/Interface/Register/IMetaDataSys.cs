using Geoway.Archiver.ReceiveAndRetrieve.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    /// <summary>
    /// 核心元数据增、删、改
    /// </summary>
    public interface IMetaDataSys:IMetaData
    {
        /// <summary>
        /// 根据数据ID选取
        /// </summary>
        /// <returns></returns>
        MetaDataSysInfo Select();
    }
}
