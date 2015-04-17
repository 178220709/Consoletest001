using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using MyProject.MyHtmlAgility.Core;
using Omu.ValueInjecter;

namespace MyProject.MyHtmlAgility.Project.FromNode
{
    public static class HttpRestHelper
    {
        public static string GetPost(string url, IDictionary<string, string> paras)
        {
           // IDictionary<string, string> paras = ObjToParas(obj);
            try
            {
                var res = HttpWebResponseUtility.CreatePostHttpResponse(url, paras, 5000, "", Encoding.UTF8, null);
                var reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static IDictionary<string, string> ObjToParas(object target)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>( );
            var targetPros = target.GetProps();
            foreach (PropertyDescriptor targetPro in targetPros)
            {
                var name = targetPro.Name;
                var o = targetPro.GetValue(target);
                if (o == null) continue;
                var value = o.ToString();
                dic.Add(name, value);
            }
            return dic;
        }

      
    }
}