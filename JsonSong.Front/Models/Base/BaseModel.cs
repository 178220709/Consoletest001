using System.Collections.Generic;
using System.Net;
using System.Web;

namespace JsonSong.Front.Models
{
    public class BaseModel
    {
        public IList<IndexColumnModel> Heads { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }

    }
}