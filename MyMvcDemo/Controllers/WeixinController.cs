using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using MyProject.MyHtmlAgility.Project.Haha;
using MyProject.WeixinModel.Extend;
using MyProject.WeixinModel.Injection;
using MyProject.WeixinModel.Model;
using Omu.ValueInjecter;
using Suijing.Utils.sysTools;

namespace MyMvcDemo.Controllers
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
                    LogHepler.WriteLog(postStr);
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
            var text = new TextMessage();
            var result = postStr;
            try
            {
                text.InjectFrom<XmlStrInjection>(postStr);
                if (text.Content == "haha")
                {
                    result = string.Join("\n", HahaWebReader.GetRecommand().Select(a => a.Content));
                }
               
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
           
            //var FromUserNameList = postObj.GetElementsByTagName("FromUserName");
            //string FromUserName = string.Empty;
            //for (int i = 0; i < FromUserNameList.Count; i++)
            //{
            //    if (FromUserNameList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
            //    {
            //        FromUserName = FromUserNameList[i].ChildNodes[0].Value;
            //    }
            //}
            //var toUsernameList = postObj.GetElementsByTagName("ToUserName");
            //string ToUserName = string.Empty;
            //for (int i = 0; i < toUsernameList.Count; i++)
            //{
            //    if (toUsernameList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
            //    {
            //        ToUserName = toUsernameList[i].ChildNodes[0].Value;
            //    }
            //}
            //var keywordList = postObj.GetElementsByTagName("Content");
            //string Content = string.Empty;
            //for (int i = 0; i < keywordList.Count; i++)
            //{
            //    if (keywordList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
            //    {
            //        Content = keywordList[i].ChildNodes[0].Value;
            //    }
            //}
          
            var textpl = "<xml><ToUserName><![CDATA[" + text.FromUserName + "]]></ToUserName>" +
                "<FromUserName><![CDATA[" + text.ToUserName + "]]></FromUserName>" +
                "<CreateTime>" + DateTime.Now.ToShortTimeString() + "</CreateTime><MsgType><![CDATA[text]]></MsgType>" +
                "<Content><![CDATA[欢迎来到微信世界---" + result + "]]></Content><FuncFlag>0</FuncFlag></xml> ";

            return textpl;
        }

    }
}
