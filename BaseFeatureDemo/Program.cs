using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BaseFeatureDemo.Base;
using BaseFeatureDemo.Base.CLR;
using BaseFeatureDemo.Base.Delegate;
using BaseFeatureDemo.Base.File;
using BaseFeatureDemo.Base.Reg;
using BaseFeatureDemo.Base.ThreadDemo.ThreadBase;
using BaseFeatureDemo.Base.ThreadDemo.ThreadNew;
using BaseFeatureDemo.Encrypt;
using BaseFeatureDemo.MyGame;
using MyProject.HotelRecord.Manager;
using MyProject.TestHelper;

namespace BaseFeatureDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //QueryHotel query = new QueryHotel();
            //query.TransDataToMG();
            var souList = new List<string> {"123456","111111","Aa111111"};
             var resultList = souList.AsEnumerable().Select(Authcode.ConvertToSHA1).ToList();



          var testList=  Enumerable.Range(0, 100).Select(a => new {Index = a, name = "aa" + a});
        }
    }
}
