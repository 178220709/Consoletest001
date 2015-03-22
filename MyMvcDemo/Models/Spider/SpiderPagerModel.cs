using System.Collections.Generic;
using System.Net;
using System.Web;
using MyProject.MongoDBDal;
using MyProject.MyHtmlAgility.Core;
using MyProject.MyHtmlAgility.Project.SpiderBase;


namespace MyMvcDemo.Models
{
    public class SpiderPagerModel : BasePagerModel<BaseSpiderEntity>
    {
        public string  Flag { get; set; }
        public string Title { get; set; }
        public int  Weight { get; set; }


        


    }

   

}