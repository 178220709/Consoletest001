using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;

namespace JsonSong.Front.Extend
{
   
    public class ResponseJsonModel
    {

        public bool success { get; set; }
        public string msg { get; set; }
        public string code { get; set; }
        public object rows { get; set; }

        public object result { get; set; }

    }
}