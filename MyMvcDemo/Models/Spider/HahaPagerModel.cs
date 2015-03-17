using System.Collections.Generic;
using System.Net;
using System.Web;
using MyProject.MyHtmlAgility.Project.Haha;

namespace MyMvcDemo.Models
{
    public class HahaPagerModel : BasePagerModel<JokeEntity>
    {
        public int  Flag { get; set; }
        public int  Weight { get; set; }

    }
}