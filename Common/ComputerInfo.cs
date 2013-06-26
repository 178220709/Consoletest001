using System;
using System.Net;
using System.Net.Sockets;

namespace Consoletest001.MyGame
{
    /// <summary>
    ///获取本机信息
    /// </summary>
    public static class ComputerInfo
    {
        public const string TimeFormateString = "yyyy-mm-dd hh:MM:ss";
        /// <summary>
        /// 获取本机hostname
        /// </summary>
        /// <returns></returns>
        public static string HostName
        {
            get { return Dns.GetHostName(); }

        }

        /// <summary>
        /// 获得本机IP地址
        /// </summary>
        /// <returns>IP</returns>
        public static string HostIP
        {
            get
            {
                try
                {
                    IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());

                    for (int i = 0; i < IpEntry.AddressList.Length; i++)
                    {
                        if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            return IpEntry.AddressList[i].ToString();
                        }
                    }
                    return "0.0.0.0";
                }
                catch (Exception ex)
                {
                    return "0.0.0.0";
                }
            }

        }

        /// <summary>
        /// 获取本机OSVersion
        /// </summary>
        /// <returns>OSVersion</returns>
        public static string OSVersion
        {
            get { return System.Environment.OSVersion.VersionString; }

        }

        /// <summary>
        /// 得到当前的时间、计算机名、ip、os组装的信息头
        /// </summary>
        /// <returns></returns>
        public static string ComputerInfoHead
        {
            get
            {
                string str = string.Format("时间：{0}  hostName：{1} ip：{2}, os：{3}", DateTime.Now.ToString(TimeFormateString), HostName, HostIP, OSVersion);
                return str;
            }
        }

    }
}