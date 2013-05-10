using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Consoletest001.otherThing
{
    public class DirectoryTest6Erji
    {
        private static int totalData6 = 0;
        private static int totalData7 = 0;
        private static int totalData0 = 0;
        private static int totalData2 = 0;
        private static Dictionary<string, string> Data0 = new Dictionary<string,string>();
        private static Dictionary<string, string> Data6 = new Dictionary<string, string>();
        private static Dictionary<string, string> Data7 = new Dictionary<string, string>();
        private static Dictionary<string, string> Data2= new Dictionary<string, string>();

        private static List<string> Data = GetDataName("");

        public static void mainsdf()
        {
//            string s51 = @"HJ1A-CCD1-429-56-20090908-L20000168867";
//            string s52 = @"HJ1A-HSI-432-69-A2-20090908-L20000168919";
//           bool t1 = Data.Contains(s51);
//           bool t2 = Data.Contains(s52); 
            string path = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ-1ACCD二级产品6";
            string path1 = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ-1ACCD二级产品6\HJ-1A CCD 二级产品";
            string path2 = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ1A-CCD4\HJ1A-HSI\HJ-1A卫星 CCD";
            string[] dirs = null;
            dirs = Directory.GetDirectories(path);
            //     foreach (string dir in dirs)
            //  {
            List<string> Datas = GetDataName("");

            string[] dirs2 = null;
            dirs2 = Directory.GetDirectories(path);
            foreach (string s2 in dirs2)
            {
                string[] dirs3 = null;
                dirs3 = Directory.GetDirectories(s2);
                foreach (string s3 in dirs3)
                {
                    CheckData2(s3);
                    string ss3 = Path.GetFileName(s3);

                    if (!Datas.Contains(ss3))
                    {
                    }
                    else
                    {
                    }
                }
            }
            
         
      
     

            //  }
        }

        public static bool CheckData(string path)
        {
            string[] infos = Directory.GetFiles(path);
            if (infos.Length == 3 || infos.Length == 4)
            {
                return true;
            }
            return false;
        }

        public static void CheckData2(string path)
        {
            try
            {



                string[] infos = Directory.GetFiles(path);
                int count = infos.Length;
                if (count == 6)
                {

                    string filename = Path.GetFileName(path);
                    if (filename == @"HJ1A-CCD1-196-144-20090523-L20000117430".Trim())
                    {
                        
                    }

               //     string str = Data6[filename];
                    Data6.Add(filename, path);
                    totalData6++;
                    return;
                }
                if (count > 6)
                {
                    string filename = Path.GetFileName(path);
                    Data7.Add(filename, path);
                    totalData7++;
                    return;
                }
                if (count < 6)
                {
                    totalData0++;
                    string filename = Path.GetFileName(path);
                    Data0.Add(filename, path);
                    totalData0++;
                    return;
                }
            }
            catch(ArgumentException ex)
            {
                if (ex.Message == "已添加了具有相同键的项。")
                {
                    string str = Data6[Path.GetFileName(path)];
                    totalData2++;
                }

               
            }
        }

        public static List<string> GetDataName(string path)
        {
            //    path = @"D:\项目2012\资源三号地面应用系统数据库管理分系统\代码\CS自动归档系统1.0\Output\x86\Debug\log\201211\20121115\tongji.log";
            path = @" C:\Documents and Settings\Administrator\桌面\my dev work\project2005\Consoletest001\tongji2.log";
            string[] lines = File.ReadAllLines(path);
            List<string> lll = new List<string>();
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lll.Add(line);
                }
            }
            return lll;
        }

        public DirectoryTest6Erji()
        {
        }
    }
}