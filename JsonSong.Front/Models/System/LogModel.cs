using JsonSong.Front.Extend;

namespace JsonSong.Front.Models
{

    public  class LogModel :BaseModel
    {
        public string   LogContext { get; set; }
        public LogEnum LogEnum { get; set; }
                  
        
    }
}
