using System;
using System.Collections.Generic;
using JsonSong.SpiderApp.Base;
using JsonSong.SpiderApp.Data;


namespace JsonSong.SpiderApp.Application
{
    public class SpiderManager : BaseManager<SpiderEntity> 
    {
       
        public static bool Add(SpiderEntity en)
        {
            using (var con = new MyDbContext())
            {
                con.Spiders.Add(en);
               return con.SaveChanges()==1;
            }
        }

        public static bool DownYoumin(SpiderEntity en)
        {
            using (var con = new MyDbContext())
            {
                con.Spiders.Add(en);
                return con.SaveChanges() == 1;
            }
        }


    }
}