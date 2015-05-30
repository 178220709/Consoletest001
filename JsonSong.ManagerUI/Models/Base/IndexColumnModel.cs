using System.Net;
using System.Web;

namespace JsonSong.ManagerUI.Models
{
    public class IndexColumnModel
    {

        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }


        public  bool IsCurrentPage()
        {
          return  HttpContext.Current.Request.Url.AbsolutePath.Contains(Url);
        }

    }
}