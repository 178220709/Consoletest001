using System;
using System.Collections.Generic;
using JsonSong.BaseDao.LiteDb;
using LiteDB;
using Suijing.Utils.ConfigTools;
using Suijing.Utils.Constants;

namespace JsonSong.Spider.DataAccess.Entity
{
    public class SpiderImgMapEntity : BaseLiteEntity
    {
        public SpiderImgMapEntity()
        {
            MapItems = new List<ImgMapItem>();
        }
      
        [BsonIndex]
        public string Url { get; set; }

        public IList<ImgMapItem> MapItems { get; set; }
     
    }

    public class ImgMapItem
    {
        public string ImgUrl { get; set; }
        public string ImgFileName { get; set; }
    }

    public static class ImgMapItemEx
    {
        public static string GetAbsPath(this ImgMapItem map)
        {
           

            return null;
        }
       
    }
}