using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;

namespace JsonSong.BaseDao.MongoDB
{
    internal class DataBaseManager
    {
        private static Dictionary<string, IMongoDatabase> _databases;

        internal static string ConnectionKey = "MongoDB";

        #region Constructors

        static DataBaseManager()
        {
            _databases = new Dictionary<string, IMongoDatabase>();
        }

        #endregion

        internal static IMongoDatabase GetDatabaseByKey(string key)
        {
            if (!_databases.ContainsKey(key))
            {
                _databases.Add(key, GetDatabase(key)); 
            }
            return _databases[key];
        }

        #region Help Methods

        private static IMongoDatabase GetDatabase(string key)
        {
            var connectString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            var mongoUrl = new MongoUrl(connectString);
            var client = new MongoClient(connectString);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        #endregion
    }
}