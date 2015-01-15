using System;
using System.IO;

namespace PriceIndex.BackService
{
    public class Globals
    {

        #region Member
        public static string Connstr
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public static int TopCount
        {
            get
            {
                try
                {
                    return int.Parse(System.Configuration.ConfigurationManager.AppSettings["TopCount"].ToString());

                }
                catch
                {
                    return 1000;
                }
            }
        }
        public static int timeForMinute
        {
            get
            {
                try
                {
                    return int.Parse(System.Configuration.ConfigurationManager.AppSettings["timeForMinute"].ToString());

                }
                catch
                {
                    return 120;
                }
            }
        }

        public static string BakPath
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["BakPath"].ToString();

                }
                catch
                {
                    return @"c:\Trade2007Category\";
                }
            }
        }

        #endregion
        #region Methods
        public static  string StrTempDate = DateTime.Now.ToString("yyyyMMddHHmmss");

        /**/
        /// <summary>
        /// 根据当前日期创建一文件夹
        /// </summary>
        /// <param name="strPreXMLPath"></param>
        /// <returns></returns>
        public static  string CreateXMLDir(string strPreXMLPath)
        {
            //strPreXMLPath += StrTempDate;
            DirectoryInfo diBak = new DirectoryInfo(strPreXMLPath);
            if (diBak.Exists)
            { return strPreXMLPath; }
            else
            {
                diBak.Create();
                return diBak.FullName;
            }
        }
        #endregion
    }
}
