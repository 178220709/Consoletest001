using MongoDB.Bson;

namespace JsonSong.BaseDao.MongoDB
{
    public abstract class BaseMongoEntity
    {
        public virtual ObjectId Id { get; set; }
    }
}