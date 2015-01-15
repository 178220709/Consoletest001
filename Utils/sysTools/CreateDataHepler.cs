using System;
using System.ComponentModel;
using System.IO;
using Omu.ValueInjecter;

namespace Suijing.Utils
{
    public  class CreateDataHepler
    {
        public static void SetProWithProName(object obj, object suffix)
        {
            var props = obj.GetProps();
            foreach (PropertyDescriptor pro in props)
            {
                var name = pro.Name;
                if (pro.PropertyType==typeof(string))
                {
                    pro.SetValue(obj, name + suffix);
                }
            }
        }

    }
}
