using System;
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProject.WeixinModel.Extend;
using MyProject.WeixinModel.Injection;
using MyProject.WeixinModel.Model;
using Omu.ValueInjecter;

namespace MyProject.WeixinModel
{
    [TestClass]
    public class WeixinTest
    {
        public const string MsgDemo1 = "<xml><ToUserName><![CDATA[gh_a413ed7b46b6]]></ToUserName>" +
                                       "<FromUserName><![CDATA[oinzFjmCt9LdPgmnEnvBShE0W5Qk]]></FromUserName>" +
                                       "<CreateTime>1406179796</CreateTime>" +
                                       "<MsgType><![CDATA[text]]></MsgType>" +
                                       "<Content><![CDATA[this is client msg]]></Content>" +
                                       "<MsgId>6039496236316578696</MsgId>" +
                                       "</xml>";
        [TestMethod]
        public void XmlStrInjectionTest()
        {
            var msg = new TextMessage();
            msg.InjectFrom<XmlStrInjection>(MsgDemo1);
            Assert.AreEqual(msg.FromUserName, "oinzFjmCt9LdPgmnEnvBShE0W5Qk");
            Assert.AreEqual(msg.MsgId, "6039496236316578696");
            Assert.AreEqual(msg.CreateTime, "1406179796");

            var time = msg.CreateTime.ToDateTime();
            var str = time.ToTimeInt();
            Assert.AreEqual(msg.CreateTime, str);
        }  
        
        
        [TestMethod]
        public void ModelToXmlTest()
        {
            var msg = new TextMessage();
            msg.InjectFrom<XmlStrInjection>(MsgDemo1);
           
            

            var time = msg.CreateTime.ToDateTime();
            var str = time.ToTimeInt();
            Assert.AreEqual(msg.CreateTime, str);
        }

        public string responseMsg(string postStr)
        {
          
            //var time = DateTime.Now;
            //var textpl = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
            //    "<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
            //    "<CreateTime>" + DateTime.Now.ToShortTimeString() + "</CreateTime><MsgType><![CDATA[text]]></MsgType>" +
            //    "<Content><![CDATA[欢迎来到微信世界---" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml> ";

            return "";
        }

     


    }
}
