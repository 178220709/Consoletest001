using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;
using MyProject.WeixinModel.Extend;
using MyProject.WeixinModel.Injection;
using MyProject.WeixinModel.Model;
using Omu.ValueInjecter;

namespace MyMvcDemo
{
    public class WeixinController : Controller
    {
        public string Index()
        {
            string requestMethod = Request.HttpMethod.ToLower();
            if (requestMethod == "post")
            {
                System.IO.Stream s = Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int) s.Length);
                var postStr = System.Text.Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    return responseMsg(postStr);
                }
            }
            else if (requestMethod == "get")
            {
                var model = new CheckModel();
                model.InjectFrom<RequestInjection>(Request);
                if (model.CheckSignature() && !string.IsNullOrEmpty(model.echostr))
                {
                    return model.echostr;
                }
            }
            return "this is from server";
        }

        public string responseMsg(string postStr)
        {
            System.Xml.XmlDocument postObj = new System.Xml.XmlDocument();
            postObj.LoadXml(postStr);
            
            var FromUserNameList = postObj.GetElementsByTagName("FromUserName");
            string FromUserName = string.Empty;
            for (int i = 0; i < FromUserNameList.Count; i++)
            {
                if (FromUserNameList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
                {
                    FromUserName = FromUserNameList[i].ChildNodes[0].Value;
                }
            }
            var toUsernameList = postObj.GetElementsByTagName("ToUserName");
            string ToUserName = string.Empty;
            for (int i = 0; i < toUsernameList.Count; i++)
            {
                if (toUsernameList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
                {
                    ToUserName = toUsernameList[i].ChildNodes[0].Value;
                }
            }
            var keywordList = postObj.GetElementsByTagName("Content");
            string Content = string.Empty;
            for (int i = 0; i < keywordList.Count; i++)
            {
                if (keywordList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
                {
                    Content = keywordList[i].ChildNodes[0].Value;
                }
            }
            var time = DateTime.Now;
            var textpl = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
                "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
                "<CreateTime>" + DateTime.Now.ToShortTimeString() + "</CreateTime><MsgType><![CDATA[text]]></MsgType>" +
                "<Content><![CDATA[欢迎来到微信世界---" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml> ";

            return textpl;
        }

    }
}
