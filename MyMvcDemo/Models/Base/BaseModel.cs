using System.Collections.Generic;
using System.Net;
using System.Web;

namespace MyMvcDemo.Models
{
    public class BaseModel
    {
        public IList<IndexColumnModel> Heads { get; set; }
        public string Title { get; set; }

    }
}