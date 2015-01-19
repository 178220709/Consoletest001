using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.HotelRecord.Entity
{
   public static class Mapper
    {
       public static DateTime ParseDateTime(string str)
       {
           DateTime dt;
           DateTime.TryParse(str, out dt);
           return dt;
       }
       public static int ParseToInt(string str)
       {
           int result = 0;
           int.TryParse(str, out result);
           return result;
       }

       public static Record TransCdsgus(cdsgus model)
       {
           return new Record()
           {
               OldId = model.id,
               Address = model.Address,
               Name = model.Name,
               BirthDay = ParseToInt(model.Birthday),
               Code = model.CtfId,
               CtfTp = model.CtfTp,
               Email = model.EMail,
               Fax = model.Fax,
               Gender = model.Gender == "M",
               Mobile = model.Mobile,
               Tel = model.Tel,
               Date = ParseDateTime(model.Version)
           };
       }

    }
}
