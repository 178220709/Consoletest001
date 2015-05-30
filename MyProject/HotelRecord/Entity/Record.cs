using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSong.BaseDao.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace MyProject.HotelRecord.Entity
{


  
    public class Record : BaseMongoEntity
    {

       public int OldId { get; set; }
       public string Name  { get; set; }
       public string CtfTp  { get; set; }
       public string Code { get; set; }

       public bool Gender { get; set; }

       public int BirthDay { get; set; }
       public string Address { get; set; }
       public string Mobile { get; set; }
       public string Tel { get; set; }
       public string Fax { get; set; }
       public string Email { get; set; }
        public DateTime Date { get; set; }

        [BsonIgnore]
        public string OldBirthDay { get; set; }
        [BsonIgnore]
        public string OldDate { get; set; }
    }



    public class tempRecord : BaseMongoEntity
    {
        public int OldId { get; set; }
        public string Name { get; set; }
        public string CtfTp { get; set; }
        public string Code { get; set; }

        public bool Gender { get; set; }

        public int BirthDay { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string OldBirthDay { get; set; }
        public string OldDate { get; set; }
    }

}
