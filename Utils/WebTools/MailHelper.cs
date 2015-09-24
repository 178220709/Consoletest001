using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Suijing.Utils.WebTools
{
    public class MailHelper
    {
        private readonly string _name;
        private readonly string _psw;
        private readonly string _host;
        private readonly int _port = 25;


        public MailHelper()
        {
            _name = "cugsuijing@126.com";
            _psw = "xzosmirnhtimzowi";
            _host = "smtp.126.com";
        }

        public MailHelper(string name, string psw,string host , int port = 25)
        {
            _name = name;
            _psw = psw;
            _host = host;
            _port = port;
        }
      
        public void SendMessage(string title, string content, IList<string> toAddrList)
        {
            if (content == "" || toAddrList == null || !toAddrList.Any())
            {
                return;
            }

            #region 邮件信息
            MailMessage myMail = new MailMessage();
            myMail.From = new MailAddress(_name);
            toAddrList.ToList().ForEach(a => myMail.To.Add(new MailAddress(a)));
            myMail.Subject = title;
            myMail.SubjectEncoding = Encoding.UTF8;
            myMail.Body = content;
            myMail.BodyEncoding = Encoding.UTF8;
            myMail.IsBodyHtml = true;
            #endregion

            SmtpClient smtp = new SmtpClient();
            smtp.Host =_host;
            smtp.Port = _port;
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(_name, _psw);
            smtp.EnableSsl = true;  //
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //Gmail的发送方式是通过网络的方式，需要指定

            try
            {
                smtp.Send(myMail);
            }
            catch (Exception ex)
            {

            }
        }
    }
}