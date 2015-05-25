
using LiteDB;
using System;


namespace JsonSong.BaseDao.LiteDb
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            AddedTime = DateTime.Now;
        }

        [BsonId(true)]
        public  int Id { get; set; }


        [BsonIndex]
        public  DateTime AddedTime { get; set; }
    }
}