
using LiteDB;
using System;


namespace JsonSong.BaseDao.LiteDb
{
    public abstract class BaseLiteEntity
    {
        protected BaseLiteEntity()
        {
            AddedTime = DateTime.Now;
        }

        [BsonId(true)]
        public  int Id { get; set; }


        [BsonIndex]
        public  DateTime AddedTime { get; set; }
    }
}