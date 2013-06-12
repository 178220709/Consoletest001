using System.Collections;

namespace Consoletest001.Linq
{
    public static class Helper
    {
        public static string MD5Hash(this string s)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5");
        }

        public static bool In(this object o, IEnumerable b)
        {
            foreach (object obj in b)
            {
                if (obj == o)
                    return true;
            }
            return false;
        }
    }
}
 