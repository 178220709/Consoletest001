using System;
using System.ComponentModel.DataAnnotations.Schema;
using JsonSong.SpiderApp.Base;

namespace JsonSong.SpiderApp.Data
{
    [Table("SpiderLog")]
    public class SpiderLogEntity : BaseEntity
    {
        
        public int TypeId { get; set; }
        public string Flag { get; set; }
      
        public DateTime AddedTime { get; set; }
        public string Remark { get; set; }
    }
}