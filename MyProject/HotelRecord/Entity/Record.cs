using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.HotelRecord.Entity
{
   public class Record
    {
       public int Id { get; set; }
       public string Name  { get; set; }
       public string CtfTp  { get; set; }
       public string Code { get; set; }

       public bool Gender { get; set; }

       public int BirthDay { get; set; }
       public string Address { get; set; }
       public string Mobile { get; set; }
       public string Tel { get; set; }
       public string Fax { get; set; }
       public string Email { get; set; }
       public DateTime Date { get; set; }



    }
}
