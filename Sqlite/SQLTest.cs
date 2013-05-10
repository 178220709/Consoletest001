using System.Data;
using System.Text;
using Consoletest001.Sqlite.DAL;
using Consoletest001.Sqlite.Entity;

namespace Consoletest001.Sqlite
{
    public class SQLTest
    {
        public static void maindf()
        {

            TableTest();

        }
          public static void TableTest()
          {
            DataTable ds =   GeneralDal.GetAlltbInfo();
            foreach (DataRow dr in ds.Rows)
            {
                DataTable dt2 = GeneralDal.GetTableFromName(dr["name"].ToString());
            }
           

          }

         public static void PhoneTest()
         {

             PhoneInfo info = new PhoneInfo("sj1111", "2013-5-9 17:14:49", "2013-5-9 17:14:54");

             PhoneDal.insertInfo(info);
             PhoneInfo newinfo = PhoneDal.GetPhoneInfo(info.Name);

             info.HomeNumber = "2013-5-9 17:17:26";
             PhoneDal.UpdatePhone(info);
             PhoneInfo newinfo2 = PhoneDal.GetPhoneInfo(info.Name);

             newinfo2.Name = "sj2222";
             PhoneDal.insertInfo(newinfo2);

             DataTable dt = PhoneDal.GetAllPhoneTable();
             PhoneDal.DeleteInfo(newinfo2);
             DataTable dt2 = PhoneDal.GetAllPhoneTable();



         }


    }


}