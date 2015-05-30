#region Using namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;


#endregion

namespace MyProject.TestHelper
{

    public static class TestHelperEx
    {
        public static IntPtr GetIntPtr(this object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj);
            GCHandle handle2 = GCHandle.Alloc(obj);
            IntPtr ptr = GCHandle.ToIntPtr(handle);
            return ptr;
        } 
       
    }
}