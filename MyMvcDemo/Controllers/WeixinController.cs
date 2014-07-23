using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMvcDemo.Extend;
using MyMvcDemo.Models;

namespace MyMvcDemo
{
    public class WeixinController : Controller
    {
        //  public void ProcessRequest(WeixinMsgModel  model)
        //{
        //    if (context.Request.RequestType == "GET")
        //    {
        //        if (CheckSignature(context))
        //        {
        //            context.Response.Write(context.Request.QueryString["echostr"]);
        //        }
        //        return;
        //    }
        //    var messageBase=MessageBase.Parse(context.Request.InputStream, context.Request.ContentEncoding);
        //    Tencent.WeiXin.TextReplyMessage text = new Tencent.WeiXin.TextReplyMessage();
        //    text.Content = "我收到了";
        //    switch (messageBase.Type)
        //    {
        //        case MessageType.Text:
        //            text.Content += string.Format("文本消息“{0}”",((TextMessage)messageBase).Content);break;
        //        case MessageType.Image:
        //            text.Content += string.Format("图片消息“{0}”", ((ImageMessage)messageBase).PicUrl); break;
        //        case MessageType.Link:
        //            text.Content += string.Format("链接消息“{0}”", ((LinkMessage)messageBase).Url); break;
        //        case MessageType.Location:
        //            text.Content += string.Format("地图消息“X:{0}Y:{1}C{2}L{3}”", ((LocationMessage)messageBase).X,((LocationMessage)messageBase).Y,((LocationMessage)messageBase).Scale,((LocationMessage)messageBase).Label); break;
        //        case MessageType.Event:
        //            text.Content += string.Format("时间推送消息“{0}”", ((EventMessage)messageBase).EventType); break;
        //    }
        //    text.Init(messageBase);
        //    text.Write(context.Response.Output);
        //    //context.Response.Write(mess.ToString());
        //    context.Response.ContentType = "text/xml";
        //    context.Response.Flush();
        //}

        public JsonResult CheckSignature(CheckModel model)
        {
            return Json(model.CheckSignature());
        }

    

    public class WeixinMsgModel
    {

    }
   

 


    public ActionResult Index()
        {
            return View();
        }

        public ActionResult JStest()
        {
            return View();
        }
    }
}
