namespace Geoway.Archiver.ReceiveAndRetrieve.Interface.Register
{
    public interface IMetaDataFixed : IMetaData
    {
        IMetaDataFixedEdit Select();
        //待扩展
    }
}