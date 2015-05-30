using JsonSong.ManagerUI.Extend;

namespace JsonSong.ManagerUI.Models
{

    public  class LogModel :BaseModel
    {
        public string   LogContext { get; set; }
        public LogEnum LogEnum { get; set; }
                  
        
    }
}
