using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Consoletest001.WEB
{
    internal class GetWebStr
    {
        /**/

        /// <summary> 
        /// 获取指定远程网页内容 
        /// </summary> 
        /// <param name="strUrl">所要查找的远程网页地址</param> 
        /// <param name="timeout">超时时长设置，一般设置为8000</param> 
        /// <param name="enterType">是否输出换行符，0不输出，1输出文本框换行</param> 
        /// <param name="EnCodeType">编码方式</param> 
        /// <returns></returns> 
        /// 也可考虑 static string 
        public static string GetRequestString(string strUrl, int timeout, int enterType, Encoding EnCodeType)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest) HttpWebRequest.Create(strUrl);
                myReq.Timeout = 8000;
                HttpWebResponse HttpWResp = (HttpWebResponse) myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, EnCodeType);
                StringBuilder strBuilder = new StringBuilder();

                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                    if (enterType == 1)
                    {
                        strBuilder.Append(" ");
                    }
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception err)
            {
                strResult = "请求错误：" + err.Message;
            }
            return strResult;
        }

        public static string[] GetRequestData(string html)
        {
            String[] rS = new String[2];
           
            html = Regex.Replace(html, @"\s{3,}", "");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            string Pat =
                "<td align=\"center\" class=\"24p\"><B>(.*)</B></td></tr><tr>.*(<table width=\"95%\" border=\"0\" cellspacing=\"0\" cellpadding=\"10\">.*</table>)<table width=\"98%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">(.*)<td align=center class='l6h'>";
            Regex Re = new Regex(Pat);
            Match Ma = Re.Match(html);
            if (Ma.Success)
            {
                rS[0] = Ma.Groups[1].ToString();
                rS[1] = Ma.Groups[2].ToString();
                rS[3] = Ma.Groups[3].ToString();
            }
            return rS;
        }


        public static void Maindadsf()
        {
            string str = @"http://www.soaspx.com/dotnet/asp.net/tech/tech_20121119_9823.html";
            string resultStr = GetRequestString(str, 8000, 1, System.Text.Encoding.GetEncoding("gb2312"));

            String[] rs = GetRequestData(resultStr);

            Console.WriteLine(rs);
        }
    }
}