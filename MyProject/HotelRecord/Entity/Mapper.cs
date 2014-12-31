using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.HotelRecord.Entity
{
   public static class Mapper
    {
       private static DateTime ParseDateTime(string str)
       {
           DateTime dt;
           DateTime.TryParse(str, out dt);
           return dt;
       }


       public static Record TransCdsgus(cdsgus model)
       {
           return new Record()
           {
               Address = model.Address,
               Name = model.Name,
               BirthDay = string.IsNullOrWhiteSpace(model.Birthday) ? 0 : Convert.ToInt32(model.Birthday),
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
