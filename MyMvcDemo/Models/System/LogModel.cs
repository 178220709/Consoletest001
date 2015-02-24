using MyMvcDemo.Extend;

namespace MyMvcDemo.Models
{

    public  class LogModel :BaseModel
    {
        public string   LogContext { get; set; }
        public LogEnum LogEnum { get; set; }
                  
        
    }
}
