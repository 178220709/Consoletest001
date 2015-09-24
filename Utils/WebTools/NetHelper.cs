using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;

namespace Suijing.Utils.WebTools
{
    public class NetHelper
    {
        /// <summary>  
        /// 是否能 Ping 通指定的主机  
        /// </summary>  
        /// <param name="ip">ip 地址或主机名或域名</param>  
        /// <returns>true 通，false 不通</returns>  
        public static bool CheckPing(string ip)
        {
            return GetPing(ip).Status == System.Net.NetworkInformation.IPStatus.Success;
        }

        public static PingReply GetPing(string ip)
        {
            var p = new Ping();
            var options = new PingOptions {DontFragment = true};
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000; // Timeout 时间，单位：毫秒  
            return p.Send(ip, timeout, buffer, options);
        }  
       
    }
}