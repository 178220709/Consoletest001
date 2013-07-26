using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Consoletest001.ManiaSongs
{
   public static  class CommonDictionary
   {
       private  static Dictionary<string, string> _dic;

       private static void InitDic()
       {
           _dic = new  Dictionary<string, string>() ;
           _dic.Add("#TITLE", "Title");
           _dic.Add("#PLAYLEVEL", "PlayLevel");
           _dic.Add("#WAV0", "SongName");
           _dic.Add("#WAV10", "SongName");
//           _dic.Add("#WAV01", "SongName");
//           _dic.Add("#WAV02", "SongName");
//           _dic.Add("#WAV03", "SongName");
//           _dic.Add("#WAV04", "SongName");
//           _dic.Add("#WAV0A", "SongName");
           _dic.Add("#STAGEFILE", "ImageName");
           _dic.Add("#BMP00", "IconName");
           _dic.Add("#BMP01", "IconName");
       }
       public static Dictionary<string, string> Dic
       {
           get
           {
               if (_dic==null)
               {
                   InitDic();
               }
               return _dic;
           }
       }

       public static string  GetAfterSpace( this string str)
       {
           int index = str.IndexOf(" ");
           if (index>0)
           {
               return str.Substring(index + 1).Trim();
           }
           return "";
       }

       public static bool SafeDel(this DirectoryInfo dir,string name)
       {
           string fullName = dir.FullName + "\\" + name;
           if ( File.Exists(fullName))
           {
               Directory.CreateDirectory(dir.FullName + "\\Deleted");
               File.Move(fullName, dir.FullName + "\\Deleted\\" + name);

               return true;
           }
           return false;
       }

       public static void MainTest()
       {
           string path = @"F:\Mania\bmg_bak" + "\\" + @"(班德瑞) 童年的记忆.bms";

           DirectoryInfo dir = new DirectoryInfo(@"F:\Mania\bmg_bak");

           dir.SafeDel("桜音.jpg");
       }
   }
}
