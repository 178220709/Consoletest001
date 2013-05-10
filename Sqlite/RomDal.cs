using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Consoletest001.Test
{
    public class RomDal
    {
        public static void Mainasfasdf()
        {
            string pathStr = @"F:\Mania\bmg_bak\狮子の星霊 [初级].bms";
            IList<RomDal> roms = new List<RomDal>();
            string[] filesName = GetFilesName(Path.GetDirectoryName(pathStr));
            foreach (string path in filesName)
            {
                RomDal tempDal = new RomDal();
                if (!SetRomInfo(ref tempDal, path))
                {
                    Console.WriteLine(tempDal.Filename + "发生错误");
                }
                roms.Add(tempDal);
            }
            RomDal dal = new RomDal();
            SetRomInfo(ref dal, pathStr);
            Console.WriteLine(dal.SongName);
        }

         public static void Mainasfas222F()
         {
             ;

         }

        private static string[] GetFilesName(string dirPath)
        {
            return Directory.GetFiles(dirPath, "*.bms", SearchOption.AllDirectories);
        }

        public static readonly string FN_FILENAME = "filename";
        public static readonly string FN_SONENAME = "songname";
        public static readonly string FN_TITLE = "title";
        public static readonly string FN_LEVEL = "level";

        #region 参数声明

        public string Filename { get; set; }

        public string SongName { get; set; }

        public string Title { get; set; }

        public string Level { get; set; }

        public string ImagePath { get; set; }

        #endregion

        /// <summary>
        /// 带4参数的构造函数
        /// </summary>
        /// <param name="filename">文件名字</param>
        /// <param name="songname">歌曲名字（同时是路径名）</param>
        /// <param name="title">歌曲标题，显示与游戏中</param>
        /// <param name="level">歌曲级别</param>
        public RomDal(string filename, string songname, string title, string level)
        {
            Filename = filename;
            SongName = songname;
            Title = title;
            Level = level;
        }

        public RomDal()
        {
        }

        /// <summary>
        /// 根据bms文件里面的信息，设置好一个RomDal的属性
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="bmsPath"></param>
        /// <returns></returns>
        public static bool SetRomInfo(ref RomDal dal, string bmsPath)
        {
            FileInfo fileInfo = new FileInfo(bmsPath);
            if (fileInfo.Directory == null)
            {
                return false;
            }
            dal.Filename = Path.GetFileName(fileInfo.FullName);
            
            StreamReader objReader = new StreamReader(bmsPath, Encoding.GetEncoding("gb2312"));
            string sLine = "";
            for (int i = 0; i < 30 && sLine != null;)
            {
                sLine = objReader.ReadLine();

                if (sLine != null && !sLine.Equals(""))
                {
                    //string removedWhiteSpace = sLine.Replace(" ", "");
                    GetInfoFromLineStr(ref dal, sLine.Trim());
                    i++;
                }
            }
            objReader.Close();
            return true;
        }

        /// <summary>
        /// 根据字符 识别需要的信息
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="str"></param>
        private static void GetInfoFromLineStr(ref RomDal dal, string str)
        {
            int index = str.IndexOf("#", StringComparison.Ordinal);
            if (index == -1)
            {
                return;
            }
            string[] strs1 = str.Split(' ');
            if (strs1[0].Length < 5)
            {
                return;
            }

            switch (strs1[0].Substring(0, 5))
            {
                case "#TITL":
                    dal.Title = strs1[1];
                    break;
                case "#PLAY":
                    dal.Level = strs1[1];
                    break;
                case "#WAV0":
                    dal.SongName = str.Substring(str.IndexOf(' '), str.Length - str.IndexOf(' '));
                    break;
                case "#BMP0":
                    dal.ImagePath = str.Substring(str.IndexOf(' '), str.Length - str.IndexOf(' '));
                    break;
                default:
                    return;
            }
        }
    }
}