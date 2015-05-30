using JsonSong.Spider.DataAccess.Entity;

namespace JsonSong.ManagerUI.Models.Spider
{
    public class SpiderPagerModel : BasePagerModel<SpiderLiteEntity>
    {
        public string  Flag { get; set; }
        public string Title { get; set; }
        public int  Weight { get; set; }


        


    }

   

}