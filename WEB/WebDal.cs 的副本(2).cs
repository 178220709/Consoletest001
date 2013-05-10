namespace Consoletest001.WEB
{

    public class WebDal
    {
        private string strFireWallIP
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["strFireWallIP"]; }
        }

        private string strFireWallPort
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["strFireWallPort"]; }
        }

        private string strUID
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["strUID"]; }
        }

        private string strPWD
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["strPWD"]; }
        }

        private string strDomain
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings["strDomain"]; }
        }


    
    }
}