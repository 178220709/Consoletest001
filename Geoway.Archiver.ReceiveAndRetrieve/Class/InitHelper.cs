using System;
using System.Collections.Generic;
using Geoway.ADF.MIS.CatalogDataModel.Public;
using Geoway.ADF.MIS.DB.Public.Interface;
using Geoway.Archiver.Catalog;
using Geoway.Archiver.Catalog.Definition;
using Geoway.Archiver.Catalog.Interface;

namespace Geoway.Archiver.ReceiveAndRetrieve.Class
{
    public class InitHelper
    {
        public static List<StorageServerParam> GetStorageServers(IDBHelper dbHelper, ICatalogNode catalogNode)
        {
            try
            {
                string[] storageIDs = null;

                if (catalogNode.NodeExInfo.IsHasAFile)
                {
                    storageIDs =
                        catalogNode.NodeExInfo.AFileStoreServers.Split(SplitChar.STORAGESERVERS_SPLITCHAE.ToCharArray());
                }
                else if (!string.IsNullOrEmpty(catalogNode.NodeExInfo.StorageServers))
                {

                    storageIDs =
                        catalogNode.NodeExInfo.StorageServers.Split(SplitChar.STORAGESERVERS_SPLITCHAE.ToCharArray());

                }

                List<StorageServerParam> lstStorages = new List<StorageServerParam>();
                if (storageIDs == null)
                {
                    return lstStorages;
                }
            
                foreach (string storageID in storageIDs)
                {
                    int id;
                    int.TryParse(storageID, out id);
                    lstStorages.Add(CatalogModelEngine.GetStorageNodeByID(dbHelper, id));
                }

                return lstStorages;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public static string GetStoragePrePath(IDBHelper dbHelper, ICatalogNode catalogNode)
        {
            string prePath="";
            while (catalogNode != null)
            {
                prePath = catalogNode.Name + "/" + prePath;
                catalogNode = CatalogFactory.GetCatalogNode(dbHelper, catalogNode.ParentID);
            }
            return prePath.Trim('/');
        }
    }
}