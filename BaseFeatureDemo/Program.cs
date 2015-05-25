using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BaseFeatureDemo.awaitDemo;
using BaseFeatureDemo.Base;
using BaseFeatureDemo.Base.CLR;
using BaseFeatureDemo.Base.Delegate;
using BaseFeatureDemo.Base.Disposable;
using BaseFeatureDemo.Base.File;
using BaseFeatureDemo.Base.Linq;
using BaseFeatureDemo.Base.Path;
using BaseFeatureDemo.Base.Reg;
using BaseFeatureDemo.Base.ThreadDemo.ThreadBase;
using BaseFeatureDemo.Base.ThreadDemo.ThreadNew;
using BaseFeatureDemo.Encrypt;
using BaseFeatureDemo.MyGame;
using BaseFeatureDemo.MyGame.Number;
using MyProject.Annotations;
using MyProject.HotelRecord.Manager;
using MyProject.TestHelper;

namespace BaseFeatureDemo
{
    class Program
    {
        static void Main(string[] args)
        {
           ProxyDemo.ProxyDemo.Main1().Wait();
        }
    }
}
