using System.Net;
using System.Web;

namespace MyMvcDemo.Models
{
    public class IndexColumnModel
    {

        public string Name { get; set; }
        public string Url { get; set; }


        public  bool IsCurrentPage()
        {
          return  HttpContext.Current.Request.Url.AbsolutePath.Contains(Url);
        }

    }
}