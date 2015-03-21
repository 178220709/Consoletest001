using System.Collections.Generic;
using System.Net;
using System.Web;
using MyProject.MyHtmlAgility.Core;


namespace MyMvcDemo.Models
{
    public class HahaPagerModel : BasePagerModel<BaseSpiderEntity>
    {
        public int  Flag { get; set; }
        public int  Weight { get; set; }

    }
}