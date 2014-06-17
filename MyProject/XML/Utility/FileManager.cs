using System;
using System.IO;

namespace BaseFeatureDemo.XML.Utility
{
    /// <summary>
    /// 文件（xml）处理公共类
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// 检测目标XML是否存在，不存在则创建一个空的xml（包含record节点）
        /// </summary>
        /// <returns></returns>
        public static bool CheckXmlFileIsExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            //不存在，则创建一个空的xml
            try
            {
                StreamWriter sw = File.CreateText(path);
                sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sw.WriteLine(@"<records>");
                sw.WriteLine(@"/<records>");

                //清空缓冲区
                sw.Flush();

                //关闭流
                sw.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}