#region Using namespace

using System;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace JsonSong.Spider.Core
{
    [TestClass]
    public class DownloadImages
    {
        private static readonly WebClient Wc = new WebClient();
        private static readonly char[] InvalidFileNameChars = new[]
                                                                  {
                                                                      '"',
                                                                      '<',
                                                                      '>',
                                                                      '|',
                                                                      '\0',
                                                                      '\u0001',
                                                                      '\u0002',
                                                                      '\u0003',
                                                                      '\u0004',
                                                                      '\u0005',
                                                                      '\u0006',
                                                                      '\a',
                                                                      '\b',
                                                                      '\t',
                                                                      '\n',
                                                                      '\v',
                                                                      '\f',
                                                                      '\r',
                                                                      '\u000e',
                                                                      '\u000f',
                                                                      '\u0010',
                                                                      '\u0011',
                                                                      '\u0012',
                                                                      '\u0013',
                                                                      '\u0014',
                                                                      '\u0015',
                                                                      '\u0016',
                                                                      '\u0017',
                                                                      '\u0018',
                                                                      '\u0019',
                                                                      '\u001a',
                                                                      '\u001b',
                                                                      '\u001c',
                                                                      '\u001d',
                                                                      '\u001e',
                                                                      '\u001f',
                                                                      ':',
                                                                      '*',
                                                                      '?',
                                                                      '\\',
                                                                      '/'
                                                                  };
        public static string CleanInvalidFileName(string fileName)
        {
            fileName = fileName + "";
            fileName = InvalidFileNameChars.Aggregate(fileName, (current, c) => current.Replace(c + "", ""));

            if (fileName.Length > 1)
                if (fileName[0] == '.')
                    fileName = "dot" + fileName.TrimStart('.');

            return fileName;
        }

        [TestMethod]
        public  void Main1()
        {
            Start();
        }

        private static void Start()
        {
            var web = new HtmlWeb();
            var startDate = int.Parse(DateTime.Parse("2010-08-18").ToString("yyyyMMdd"));
            var endDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            const int startPageId = 49430;
            const int endPageId = 124621;
            for (int k = startDate; k <= endDate; k++)
            {
                for (int j = startPageId; j <= endPageId; j++)
                {
                    string cnblogs = "http://tp.58jj.info/toupaizipai/" + k + "/" + j + ".html";
                    HtmlDocument doc = web.Load(cnblogs);
                    var titles = doc.DocumentNode.SelectNodes("//title");
                    var titleName = j.ToString();
                    if (titles != null && titles.Count > 0)
                        titleName = titles[0].InnerText;
                    HtmlNode node = doc.GetElementbyId("ks_xp");
                    if (node == null)
                    {
                        continue;
                    }
                    foreach (HtmlNode child in node.SelectNodes("//img"))
                    {
                        if (child.Attributes["src"] == null)
                            continue;

                        string imgurl = child.Attributes["src"].Value;
                        DownLoadImg(imgurl, k + "", CleanInvalidFileName(titleName));
                        Console.WriteLine("正在下载:" + titleName + " " + imgurl);
                    }
                }
            }
            //善后
            CleanEmptyFolders();
        }

        private static void CleanEmptyFolders()
        {
            var rootFolders = Environment.CurrentDirectory + "\\Images\\";
            var folders = Directory.GetDirectories(rootFolders, "*.*", SearchOption.AllDirectories);
            foreach (var f in folders)
            {
                if (Directory.GetFiles(f, "*.*", SearchOption.AllDirectories).Length == 0)
                    Directory.Delete(f);
            }
        }

        private static void DownLoadImg(string url, string folderName, string subFolderName)
        {
            var fileName = CleanInvalidFileName(url.Substring(url.LastIndexOf("/") + 1));
            var fileFolder = Environment.CurrentDirectory + "\\Images\\" + folderName + "\\" + subFolderName + "\\";
            if (!Directory.Exists(fileFolder))
                Directory.CreateDirectory(fileFolder);
            fileName = fileFolder + fileName;
            try
            {
                Wc.DownloadFile(url, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}