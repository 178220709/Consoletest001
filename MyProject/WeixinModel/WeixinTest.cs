using System;
using System.IO;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Xml.Serialization;
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
        private string MsgDemo1 = " <xml><ToUserName><![CDATA[gh_a413ed7b46b6]]></ToUserName>" +
                              " <FromUserName><![CDATA[oinzFjmCt9LdPgmnEnvBShE0W5Qk]]></FromUserName>" +
                              " <CreateTime>1424484704</CreateTime>" +
                              " <MsgType><![CDATA[text]]></MsgType>" +
                              " <Content><![CDATA[改成]]></Content>" +
                              " <MsgId>6118115217535609681</MsgId>" +
                              " </xml> ";

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


        [TestMethod]
        public void  ConvertHexToDate()
        {
            //Base64String to Hex
            string   timeStampStr = "AAAAAF+O6G8=";
            byte[] bytes = Convert.FromBase64String(timeStampStr);
            string hexString = BitConverter.ToString(bytes).Replace("-", string.Empty);
            //Hex to DateTime
            long unixTime = long.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
            var dt2 = unixTime.ToString().ToDateTime();
            long lTime = long.Parse(unixTime + "0000000");

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            DateTime dt = epoch.AddSeconds(unixTime);


        }

    }
}
