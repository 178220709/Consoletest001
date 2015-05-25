using MongoDB.Bson;

namespace JsonSong.BaseDao.MongoDB
{
    public abstract class BaseEntity
    {
        public virtual ObjectId Id { get; set; }
    }
}