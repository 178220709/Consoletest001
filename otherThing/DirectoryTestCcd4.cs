using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Consoletest001.otherThing
{
    public class DirectoryTestCcd4
    {
        private static int totalDir = 0;
        private static int totalData = 0;
        private static int totalData3 = 0;
        private static int totalData4 = 0;
        private static int totalData5 = 0;
        private static int totalData0 = 0;
        private static List<string> Data0 = new List<string>();
        private static List<string> Data3 = new List<string>();
        private static List<string> Data4 = new List<string>();
        private static List<string> Data5h = new List<string>();
        private static List<string> Data5noh = new List<string>();
        private static List<string> errorData = new List<string>();
        private static List<string> Data = GetDataName("");

        public static void mainsdf()
        {
//            string s51 = @"HJ1A-CCD1-429-56-20090908-L20000168867";
//            string s52 = @"HJ1A-HSI-432-69-A2-20090908-L20000168919";
//           bool t1 = Data.Contains(s51);
//           bool t2 = Data.Contains(s52); 
            string path = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ1A-CCD4";
            string path1 = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ1A-CCD4\HJ1A-CCD";
            string path2 = @"\\192.98.12.215\资源三号\A-环境减灾卫星遥感数据\HJ1A-CCD4\HJ1A-HSI";
            string[] dirs = null;
            errorData = new List<string>();
            dirs = Directory.GetDirectories(path);
            //     foreach (string dir in dirs)
            //  {
            List<string> Datas = GetDataName("");

            string[] dirs2 = null;
            dirs2 = Directory.GetDirectories(path2);
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
                        errorData.Add(s3);
                    }
                    else
                    {
                    }
                }
            }
            
            foreach (string ss in Data3)
            {
                string s = Path.GetFileName(ss);
                if (Data.Contains(s))
                {
                    Data.Remove(s);
                    continue;
                }
                errorData.Add(s);
            }
            foreach (string ss in Data4)
            {
                string s = Path.GetFileName(ss);
                if (Data.Contains(s))
                {
                    Data.Remove(s);
                    continue;
                }
                errorData.Add(s);
            }
            foreach (string ss in Data5h)
            {
                string s = Path.GetFileName(ss);
                if (Data.Contains(s))
                {
                    Data.Remove(s);
                    continue;
                }
                errorData.Add(s);
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
            string[] infos = Directory.GetFiles(path);
            int count = infos.Length;
            if (count == 3)
            {
                string filename = Path.GetFileName(path);
                Data3.Add(filename);
                totalData3++;
                return;
            }
            if (count == 4)
            {
                string filename = Path.GetFileName(path);
                Data4.Add(filename);
                totalData4++;
                return;
            }
            if (count > 4)
            {
                totalData5++;
                string filename = Path.GetFileName(path);
                if (Data.Contains(filename))
                {
                    Data5h.Add(path);
                }
                else
                {
                    Data5noh.Add(path);
                }
                return;
            }
            if (count == 3)
            {
                totalData3++;
                return;
            }
            Data0.Add(path);
            totalData0++;
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

        public DirectoryTestCcd4()
        {
        }
    }
}