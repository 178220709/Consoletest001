using Geoway.Archiver.ReceiveAndRetrieve.Class;

namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    public interface IMetaDataFixedDZ:IMetaData
    {
        IMetaDataFixedDZEdit Select();
        //待扩展
    }
}