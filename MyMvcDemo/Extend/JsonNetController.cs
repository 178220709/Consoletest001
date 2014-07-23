using MyMvcDemo.Models;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyMvcDemo.Extend
{
    public  static  class JsonNetResult 
    {
        public static bool CheckSignature(this CheckModel model)
        {
            var list = new List<string> { model.timestamp, model.nonce, Constants.WeixinConstants.MyToken };
            list = list.OrderBy(t => t).ToList();
            var sha1 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(list[0] + list[1] + list[2], "SHA1");
            return model.signature.Equals(sha1, StringComparison.OrdinalIgnoreCase);
        }
    }
}
