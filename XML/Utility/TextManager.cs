using System;

namespace Consoletest001.XML.Utility
{
    /// <summary>
    /// ʱ�䡢���ڴ�������
    /// </summary>
    public static class TextManager
    {
       
       /// <summary>
       /// ���Լ���string�ļӷ�
       /// </summary>
       /// <param name="source">ԭ�����ַ�</param>
       /// <param name="count">Ҫ���ϵ���</param>
        public static void AddForStr(ref string source ,int count )
        {
            int sourceTemp;
            if (string .IsNullOrEmpty(source))
            {
                source = "";
                return;
            }

            source = Int32.TryParse(source,out sourceTemp) ? (sourceTemp + count).ToString() : count.ToString();
        }

        /// <summary>
        /// ���Լ���string�ļӷ�
        /// </summary>
        /// <param name="source">ԭ�����ַ�</param>
        /// <param name="count">Ҫ���ϵ���</param>
        public static void AddForStr(ref string source, string count)
        {
            int sourceTemp;
            if (string.IsNullOrEmpty(source))
            {
                source = "";
                return;
            }

            source = Int32.TryParse(source, out sourceTemp) ? (sourceTemp + Int32.Parse(count)).ToString() : count.ToString();
        }

    }
}