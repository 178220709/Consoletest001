using System;
using System.IO;

namespace BaseFeatureDemo.XML.Utility
{
    /// <summary>
    /// �ļ���xml����������
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// ���Ŀ��XML�Ƿ���ڣ��������򴴽�һ���յ�xml������record�ڵ㣩
        /// </summary>
        /// <returns></returns>
        public static bool CheckXmlFileIsExist(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }

            //�����ڣ��򴴽�һ���յ�xml
            try
            {
                StreamWriter sw = File.CreateText(path);
                sw.WriteLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
                sw.WriteLine(@"<records>");
                sw.WriteLine(@"/<records>");

                //��ջ�����
                sw.Flush();

                //�ر���
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