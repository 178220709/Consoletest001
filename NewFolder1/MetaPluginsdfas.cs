using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Consoletest001.NewFolder1
{
    public  class MetaPluginsdfas
    {

        public static Dictionary<string, string> lis = new Dictionary<string, string>( );
        public static Dictionary<string, string> lisRight = new Dictionary<string, string>( );
        public static Dictionary<string, string> lisErrorC = new Dictionary<string, string>( );
        public static Dictionary<string, string> lisErrorE = new Dictionary<string, string>( );
        public static int noin = 0;
        public static void MainSdfsd()
        {
            string str1 = "大小写PpOo";
            string str2 = "大小写ppoo";
            bool b1 =  str1 ==str2;
            bool b2 = str1.ToLower() == str2.ToLower();
            string str = @"f:/Public/target/资源三/传感器/前视全色/259/BWD/ZY3S07/ZY3_01ico.jpg";
           // str = "publi";
            Regex regex = new Regex("^f:/*");
            bool b22 = regex.IsMatch(str);
            if (!File.Exists("str"))
            {
                Directory.CreateDirectory("f:/Public/target/资源三/传感器/前视全色/259/BWD/ZY3S07/ZY3");
                File.Create(str);
            }
            getFileds();
        }

        public static  void getFileds()
        {
            string configFile = System.Windows.Forms.Application.StartupPath + "\\metaFieldConfig.txt";//配置文件路径
            //_fileKey_DisPlayName.Add("", new string[] { "productOrderId" });
            string line = string.Empty;
            string strValue = string.Empty;
            if (!File.Exists(configFile)) return;

            using (StreamReader read = new StreamReader(configFile, Encoding.GetEncoding("gb2312")))
            {
                while ((line = read.ReadLine()) != null)
                {
                    if (line.Trim() == "") //跳过空行
                    {
                        continue;
                    }
                    // 将字符间的多个空格换为一个空格
               //    line = Regex.Replace(line.Trim(), @"\s+", " ");
                    
                    line = Regex.Replace(line.Trim(), @"\s{1,}", " ");
                    //解析行
                    string[] strFildsTxt = line.Split(" ".ToCharArray());
                    if (strFildsTxt.Length < 2)
                    {
                        lisRight.Add(strFildsTxt[0],"");
                        noin++;
                        continue;
                    }

                    for (int i = 1; i < strFildsTxt.Length; i++)
                    {
                        strValue = strFildsTxt[i] + ",";
                    }
                    strValue = strValue.TrimEnd(",".ToCharArray());
                /*   if (!lis.ContainsKey(strFildsTxt[0]))
                    {
                       
                        lis.Add(strFildsTxt[0], strValue);
                    }
                    else
                    {
                        lisErrorC.Add(strFildsTxt[0], strValue);
                    }*/

                    if (!lis.ContainsValue(strValue))
                    {
                        lis.Add(strFildsTxt[0], strValue);
                    }
                    else
                    {
                        lisErrorE.Add(strFildsTxt[0], strValue);
                    }
                }
            }
        }











    }
    



}