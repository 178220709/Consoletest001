using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Consoletest001.ManiaSongs
{
    class SongDal
    {
        private const string DirPath = @"F:\Mania\bmg_bak";
        /// <summary>
        /// 通过bms文件的path 分析出一个歌曲出来
        /// </summary>
        /// <returns></returns>
        public static SongInfo GetInfoFromPath(string Path)
        {
            SongInfo info = new SongInfo();
            var strs = File.ReadLines(Path, System.Text.Encoding.GetEncoding("gb2312")).Where(an => an != "").Take(15).ToList();
            foreach (var str in strs)
            {
                var lines = str.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 1)
                {
                    if (CommonDictionary.Dic.ContainsKey(lines[0]))
                    {

                        SetInfoValue(info, CommonDictionary.Dic[lines[0]], str.GetAfterSpace());
                    }
                }
            }

            if (string.IsNullOrEmpty(info.SongName))
            {

                var strs2 = strs.FirstOrDefault(an => an.Substring(0,5) == "#WAV0");

                if (
                    strs2 != null)
                    info.SongName = strs2.GetAfterSpace();
            }

            return info;
        }

        

        private static  void SetInfoValue(SongInfo info,string proName,string proValue)
        {
            Type souType = info.GetType();
            PropertyInfo[] pis = souType.GetProperties(BindingFlags.Public |BindingFlags.Instance);
            var firstOrDefault = pis.FirstOrDefault(pro => pro.Name == proName);
            if (firstOrDefault != null)
                firstOrDefault.SetValue(info, proValue,null);
        }

    }
}
